using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Models.Response.Services;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class BusinessHoursService : IBusinessHoursService
{
    private readonly ApplicationContext _applicationContext;
    private readonly ILogger _logger;

    public BusinessHoursService(ApplicationContext applicationContext, ILogger<BusinessHoursService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public async Task MangeAsync(ManageBusinessHoursRequestModel requestModel)
    {
        bool existsService = await _applicationContext.Services.AnyAsync(x => x.Id == requestModel.ServiceId);
        if (!existsService)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", requestModel.ServiceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        BusinessHours? existsDay = await _applicationContext.BusinessHours.FirstOrDefaultAsync(x => x.ServiceId == requestModel.ServiceId && x.DayOfWeek == requestModel.DayOfWeek);
        if (existsDay is not null)
        {
            existsDay!.DayOfWeek = requestModel.DayOfWeek;
            existsDay.StartTime = requestModel.StartTime;
            existsDay.EndTime = requestModel.EndTime;
            existsDay.IsDayOff = requestModel.IsDayOff;
        }
        else
        {
            BusinessHours businessHours = new()
            {
                DayOfWeek = requestModel.DayOfWeek,
                StartTime = requestModel.StartTime,
                EndTime = requestModel.EndTime,
                IsDayOff = requestModel.IsDayOff,
                ServiceId = requestModel.ServiceId,
            };

            _applicationContext.BusinessHours.Add(businessHours);
        }

        await _applicationContext.SaveChangesAsync();
    }

    public async Task<IReadOnlyList<BusinessHoursResponseModel>> GetAsync(Guid serviceId)
    {
        IReadOnlyList<BusinessHoursResponseModel> businessHours = await _applicationContext.BusinessHours
            .Where(x => x.ServiceId == serviceId)
            .Select(x => x.ToBusinessHoursResponseModel())
            .ToListAsync();

        return businessHours;
    }
}
