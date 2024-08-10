using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class ServiceService : IServiceService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ICategoryService _categoryService;
    private readonly ICityService _cityService;
    private readonly IContactService _contactService;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        ApplicationContext applicationContext,
        ICategoryService categoryService,
        ICityService cityService,
        IContactService contactService,
        ILogger<ServiceService> logger)
    {
        _applicationContext = applicationContext;
        _categoryService = categoryService;
        _cityService = cityService;
        _contactService = contactService;
        _logger = logger;
    }

    public async Task CreateAsync(Guid ownerId, CreateServiceRequestModel requestModel)
    {
        await ValidateExistsServiceNameAsync(requestModel.NameBg, requestModel.NameEn, ownerId);
        await ValidateOfferedAtAsync(requestModel.OfferedAtId);
        await _categoryService.ValidateSubCategoryAsync(requestModel.SubCategoryId);
        using IDbContextTransaction transaction = await _applicationContext.Database.BeginTransactionAsync();

        try
        {
            Service service = new()
            {
                NameBg = requestModel.NameBg,
                NameEn = requestModel.NameEn,
                DescriptionBg = requestModel.DescriptionBg,
                DescriptionEn = requestModel.DescriptionEn,
                SubCategoryId = requestModel.SubCategoryId,
                OwnerId = ownerId,
                OfferedAtId = requestModel.OfferedAtId,
            };

            _applicationContext.Services.Add(service);

            await AddServiceTagsAsync(requestModel.Tags, requestModel.SubCategoryId, service.Id);
            await AddServiceCitiesAsync(requestModel.Cities, service.Id);
            _contactService.CreateContactAsync(service.Id, requestModel.ContactRequestModel);

            await _applicationContext.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Successfully added a service with ID: {ServiceId}", service.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Failed to add service. Transaction rolled back.");
            throw;
        }
    }

    public async Task UpdateAsync(Guid serviceId, Guid ownerId, UpdateServiceRequestModel requestModel)
    {
        Service? service = await _applicationContext.Services.Include(x => x.Cities).FirstOrDefaultAsync(x => x.Id == serviceId && x.OwnerId == ownerId);
        if (service is null)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", serviceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        await ValidateOfferedAtAsync(requestModel.OfferedAtId);
        if (service.NameEn != requestModel.NameEn || service.NameBg != requestModel.NameBg)
        {
            await ValidateExistsServiceNameAsync(requestModel.NameBg, requestModel.NameEn, ownerId);
        }

        service.NameBg = requestModel.NameBg;
        service.NameEn = requestModel.NameEn;
        service.DescriptionBg = requestModel.DescriptionEn;
        service.DescriptionEn = requestModel.DescriptionEn;
        service.OfferedAtId = requestModel.OfferedAtId;
        service.ModifiedOn = DateTime.UtcNow;

        if (requestModel.Cities is not null)
        {
            HashSet<Guid> validCities = requestModel.Cities.Where(x => !service.Cities.Select(d => d.CityId).Contains(x)).ToHashSet();
            await AddServiceCitiesAsync(validCities, serviceId);
        }

        await _applicationContext.SaveChangesAsync();
        _logger.LogInformation("Successfully updated a service with ID: {ServiceId}", serviceId);
    }

    public async Task<IReadOnlyList<ServiceResponseModel>> GetAllAsync(Guid ownerId)
    {
        IQueryable<Service> servicesQuery = _applicationContext.Services.Where(x => x.OwnerId == ownerId);
        IReadOnlyList<ServiceResponseModel> services = await servicesQuery
            .Select(x => x.ToServiceResponseModel())
            .ToListAsync();

        return services;
    }

    public async Task<ServiceDetailsResponseModel> GetDetailsAsync(Guid ownerId, Guid serviceId)
    {
        Service? service = await _applicationContext.Services
            .Include(x => x.SubCategory)
            .Include(x => x.OfferedAt)
            .Include(x => x.Cities).ThenInclude(x => x.City)
            .Include(x => x.SelectedTags).ThenInclude(x => x.Tag)
            .Include(x => x.Contacts)
            .FirstOrDefaultAsync(x => x.OwnerId == ownerId && x.Id == serviceId);

        if (service is null)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", serviceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        ServiceDetailsResponseModel serviceDetails = service.ToServiceDetailsResponseModel(null);

        return serviceDetails;
    }

    public async Task ChangeCategoryAsync(Guid serviceId, Guid ownerId, ChangeCategoryRequestModel requestModel)
    {
        Service service = await GetServiceWithTagsAsync(serviceId, ownerId);
        await _categoryService.ValidateSubCategoryAsync(requestModel.SubCategoryId);

        using IDbContextTransaction transaction = await _applicationContext.Database.BeginTransactionAsync();
        try
        {
            service.SubCategoryId = requestModel.SubCategoryId;
            service.ModifiedOn = DateTime.UtcNow;
            ICollection<ServiceTag> serviceTags = service.SelectedTags;

            _applicationContext.ServiceTags.RemoveRange(serviceTags);
            await AddServiceTagsAsync(requestModel.Tags, requestModel.SubCategoryId, serviceId);

            await _applicationContext.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Successfully change a service category Service ID: {ServiceId}", service.Id);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            _logger.LogError("Failed to change service category. Transaction rolled back.");
            throw;
        }
    }

    public async Task AddTagAsync(Guid serviceId, Guid ownerId, int tagId)
    {
        Service service = await GetServiceWithTagsAsync(serviceId, ownerId);

        bool existsTag = service.SelectedTags.Any(x => x.ServiceId == serviceId && x.TagId == tagId);
        if (existsTag)
        {
            _logger.LogError("Tag with ID: {TagId} already exists for service {ServiceId}", tagId, serviceId);
            throw new ExistsTagException(Messages.ExistsServiceTag);
        }

        await _categoryService.ValidateSelectedTagAsync(tagId, service.SubCategoryId);
        ServiceTag serviceTag = new()
        {
            ServiceId = serviceId,
            TagId = tagId
        };

        _applicationContext.ServiceTags.Add(serviceTag);
        await _applicationContext.SaveChangesAsync();
    }

    public async Task RemoveTagAsync(Guid serviceId, Guid ownerId, int tagId)
    {
        Service service = await GetServiceWithTagsAsync(serviceId, ownerId);

        bool isLastTag = service.SelectedTags.Count == 1;
        if (isLastTag)
        {
            _logger.LogError("You cannot remove all tags. The service must have at least one tag");
            throw new RemoveAllException(Messages.CannotRemoveAllTags);
        }

        ServiceTag? serviceTag = service.SelectedTags.FirstOrDefault(x => x.ServiceId == serviceId && x.TagId == tagId);
        if (serviceTag is null)
        {
            _logger.LogError("The tag you want to remove is not valid for this service: Service ID {ServiceId} - Tag ID {TagId}", serviceId, tagId);
            throw new NotFoundEntityException(Messages.NotFoundServiceTag);
        }

        _applicationContext.ServiceTags.Remove(serviceTag);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully remove a tag for service with ID: {ServiceId} and Tag ID {TagId}", serviceId, tagId);
    }

    public async Task RemoveCityAsync(Guid serviceId, Guid ownerId, Guid cityId)
    {
        Service service = await GetServiceWithCitiesAsync(serviceId, ownerId);

        bool isLastCity = service.Cities.Count == 1;
        if (isLastCity)
        {
            _logger.LogError("You cannot remove all cities. The service must have at least one city");
            throw new RemoveAllException(Messages.CannotRemoveAllTags);
        }

        ServiceCity? serviceCity = service.Cities.FirstOrDefault(x => x.ServiceId == serviceId && x.CityId == cityId);
        if (serviceCity is null)
        {
            _logger.LogError("The city you want to remove is not valid for this service: Service ID {ServiceId} - City ID {CityId}", serviceId, cityId);
            throw new NotFoundEntityException(Messages.NotFoundServiceCity);
        }

        _applicationContext.ServiceCities.Remove(serviceCity);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully remove a city for service with ID: {ServiceId} and City ID {CityId}", serviceId, cityId);
    }

    public async Task<IReadOnlyList<OfferedAtResponseModel>> GetOfferedAtOptionsAsync()
    {
        IReadOnlyList<OfferedAtResponseModel> options = await _applicationContext.OfferedAt
            .Select(x => new OfferedAtResponseModel(
                x.Id,
                x.NameBg,
                x.NameEn))
            .ToListAsync();

        return options;
    }

    private async Task AddServiceTagsAsync(HashSet<int> tags, Guid subCategoryId, Guid serviceId)
    {
        foreach (int tagId in tags)
        {
            await _categoryService.ValidateSelectedTagAsync(tagId, subCategoryId);
            ServiceTag serviceTag = new()
            {
                ServiceId = serviceId,
                TagId = tagId
            };

            _applicationContext.ServiceTags.Add(serviceTag);
        }
    }

    private async Task AddServiceCitiesAsync(HashSet<Guid> cities, Guid serviceId)
    {
        foreach (Guid cityId in cities)
        {
            await _cityService.ValidateSelectedCityAsync(cityId);
            ServiceCity serviceCity = new()
            {
                CityId = cityId,
                ServiceId = serviceId
            };

            _applicationContext.ServiceCities.Add(serviceCity);
        }
    }

    private async Task<Service> GetServiceWithTagsAsync(Guid serviceId, Guid ownerId)
    {
        Service? service = await _applicationContext.Services
           .Include(x => x.SelectedTags)
           .FirstOrDefaultAsync(x => x.Id == serviceId && x.OwnerId == ownerId);
        if (service is null)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", serviceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        return service;
    }

    private async Task<Service> GetServiceWithCitiesAsync(Guid serviceId, Guid ownerId)
    {
        Service? service = await _applicationContext.Services
           .Include(x => x.Cities)
           .FirstOrDefaultAsync(x => x.Id == serviceId && x.OwnerId == ownerId);
        if (service is null)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", serviceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        return service;
    }

    private async Task ValidateOfferedAtAsync(int offeredAtId)
    {
        bool isValidOffered = await _applicationContext.OfferedAt.AnyAsync(x => x.Id == offeredAtId);
        if (!isValidOffered)
        {
            _logger.LogError("Offer is not supported Offer ID: {OfferId}", offeredAtId);
            throw new NotFoundEntityException(Messages.OfferNotSupporte);
        }
    }

    private async Task ValidateExistsServiceNameAsync(string nameBg, string nameEn, Guid ownerId)
    {
        bool existsServiceName = await _applicationContext.Services
            .AnyAsync(x => (x.NameBg == nameBg || x.NameEn == nameEn) && x.OwnerId == ownerId);

        if (existsServiceName)
        {
            _logger.LogError("There is a service registered with this name: {ServiceNameBg}/{ServiceNameEn}",
                nameBg,
                nameEn);
            throw new ExistsServiceNameException(Messages.ExistsServiceName);
        }
    }
}
