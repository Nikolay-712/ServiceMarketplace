namespace ServiceMarketplace.Models.Request;

public record ManageSubCategoryRequestModel(
    string NameBg,
    string NameEn,
    string DescriptionBg,
    string DescriptionEn,
    Guid CategoryId);
