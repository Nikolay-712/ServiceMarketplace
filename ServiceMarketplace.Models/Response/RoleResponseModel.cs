namespace ServiceMarketplace.Models.Response;

public record RoleResponseModel(
    Guid Id, 
    string Name, 
    string CreatedOn,
    string DescriptionBg,
    string DescriptionEn);


