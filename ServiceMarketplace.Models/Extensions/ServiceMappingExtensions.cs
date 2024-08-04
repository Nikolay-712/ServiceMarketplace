using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Response;
using ServiceMarketplace.Common.Extensions;

namespace ServiceMarketplace.Models.Extensions;

public static class ServiceMappingExtensions
{
    public static ServiceResponseModel ToServiceResponseModel(this Service service)
        => new(service.Id,
               service.CreatedOn.DateFormat(),
               service.NameBg,
               service.NameEn);

    public static ServiceDetailsResponseModel ToServiceDetailsResponseModel(this Service service)
        => new(
            service.Id,
            service.CreatedOn.DateFormat(),
            service.ModifiedOn is null ? " n/a" : service.ModifiedOn.Value.DateFormat(),
            service.NameBg,
            service.NameEn,
            service.DescriptionBg,
            service.DescriptionBg,
            service.SubCategory.ToSubCategoryResponseModel(),
            service.OfferedAt.ToOfferedAtResponseModel(),
            service.Cities.Select(c => c.City.ToCityResponseModel()).ToList(),
            service.SelectedTags.Select(t => t.Tag.ToTagResponseModel()).ToList(),
            service.Contacts.Select(c => c.ToContactResponseModel()).ToList());

    public static OfferedAtResponseModel ToOfferedAtResponseModel(this OfferedAt offeredAt)
        => new(offeredAt.Id,
               offeredAt.NameBg,
               offeredAt.NameEn);

}
