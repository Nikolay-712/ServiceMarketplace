namespace ServiceMarketplace.Models.Response;

public record CategoryResponseModel(
    Guid Id,
    string NameBg,
    string NameEn,
    string DescriptionBg, 
    string DescriptionEn);


