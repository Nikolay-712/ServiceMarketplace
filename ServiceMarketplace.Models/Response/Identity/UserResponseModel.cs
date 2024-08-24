namespace ServiceMarketplace.Models.Response.Identity;

public record UserResponseModel(
    Guid Id,
    string FullName,
    string Email,
    string CreatedOn,
    IList<string> Roles);


