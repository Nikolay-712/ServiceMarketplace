namespace ServiceMarketplace.Models.Response;

public record UserResponseModel(
    Guid Id,
    string FullName,
    string Email,
    string CreatedOn,
    IList<string> Roles);


