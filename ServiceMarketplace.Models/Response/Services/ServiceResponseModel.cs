using ServiceMarketplace.Models.Response.Ratings;

namespace ServiceMarketplace.Models.Response.Services;

public record ServiceResponseModel(
        Guid Id,
        string CreatedOn,
        string NameBg,
        string NameEn,
        RatingCalculationResponseModel Rating);
