namespace ServiceMarketplace.Models.Response.Categories;

public record CategoryResponseModel(
        Guid Id,
        string NameBg,
        string NameEn,
        string DescriptionBg,
        string DescriptionEn);
