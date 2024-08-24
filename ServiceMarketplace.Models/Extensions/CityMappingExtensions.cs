using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Response.Cities;

namespace ServiceMarketplace.Models.Extensions;

public static class CityMappingExtensions
{
    public static CityResponseModel ToCityResponseModel(this City city)
        => new (city.Id,
                city.NameBg,
                city.NameEn);
}
