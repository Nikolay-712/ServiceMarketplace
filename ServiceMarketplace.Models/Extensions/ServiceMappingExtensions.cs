﻿using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Models.Response.Services;
using ServiceMarketplace.Models.Response.Ratings;

namespace ServiceMarketplace.Models.Extensions;

public static class ServiceMappingExtensions
{
    public static ServiceResponseModel ToServiceResponseModel(this Service service, RatingCalculationResponseModel rating)
        => new(service.Id,
               service.CreatedOn.DateFormat(),
               service.NameBg,
               service.NameEn,
               rating);


    public static ServiceDetailsResponseModel ToServiceDetailsResponseModel(this Service service, RatingResponseModel ratings,IReadOnlyList<BusinessHoursResponseModel> businessHours)
        => new(
            service.Id,
            service.CreatedOn.DateFormat(),
            service.ModifiedOn is null ? " n/a" : service.ModifiedOn.Value.DateFormat(),
            service.NameBg,
            service.NameEn,
            service.DescriptionBg,
            service.DescriptionBg,
            service.ServiceCost.PricingType,
            service.ServiceCost.Price,
            service.SubCategory.ToSubCategoryResponseModel(),
            service.OfferedAt.ToOfferedAtResponseModel(),
            service.Cities.Select(c => c.City.ToCityResponseModel()).ToList(),
            service.SelectedTags.Select(t => t.Tag.ToTagResponseModel()).ToList(),
            service.Contacts.Select(c => c.ToContactResponseModel()).ToList(),
            ratings,
            businessHours);

    public static OfferedAtResponseModel ToOfferedAtResponseModel(this OfferedAt offeredAt)
        => new(offeredAt.Id,
               offeredAt.NameBg,
               offeredAt.NameEn);

    public static BusinessHoursResponseModel ToBusinessHoursResponseModel(this BusinessHours businessHours)
        => new BusinessHoursResponseModel(
                businessHours.DayOfWeek.ToString(),
                businessHours.StartTime.TimeFormat(),
                businessHours.EndTime.TimeFormat(),
                businessHours.IsDayOff);

}
