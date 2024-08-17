namespace ServiceMarketplace.Models.Request;

public record AssignUserToRoleRequestModel(
    Guid UserId, 
    Guid RoleId);

