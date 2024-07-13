namespace ServiceMarketplace.Models.Request;

public record CreateTagRequestModel(
    string NameBg,
    string NameEn,
    Guid SubCategoryId);
