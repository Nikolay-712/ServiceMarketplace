using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class ServiceService : IServiceService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ICategoryService _categoryService;
    private readonly ILogger<ServiceService> _logger;

    public ServiceService(
        ApplicationContext applicationContext,
        ICategoryService categoryService,
        ILogger<ServiceService> logger)
    {
        _applicationContext = applicationContext;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task CreateAsync(Guid ownerId, CreateServiceRequestModel requestModel)
    {
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
            };

            _applicationContext.Services.Add(service);
            await AddServiceTagsAsync(requestModel.Tags, requestModel.SubCategoryId, service.Id);

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
}
