namespace ServiceMarketplace.Models.Request;

public record RemoveUserFromRoleRequestModel(
    Guid UserId, 
    string RoleName);

