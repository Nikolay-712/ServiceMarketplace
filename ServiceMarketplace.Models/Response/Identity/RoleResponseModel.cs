namespace ServiceMarketplace.Models.Response.Identity;

public record RoleResponseModel(
    Guid Id,
    string Name,
    string CreatedOn,
    string DescriptionBg,
    string DescriptionEn);


