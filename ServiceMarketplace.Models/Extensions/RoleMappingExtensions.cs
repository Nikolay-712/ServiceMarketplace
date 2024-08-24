using ServiceMarketplace.Common.Extensions;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Response.Identity;

namespace ServiceMarketplace.Models.Extensions;

public static class RoleMappingExtensions
{
    public static RoleResponseModel ToRoleResponseModel(this ApplicationRole role)
        => new RoleResponseModel(
            role.Id,
            role.Name!,
            role.CreatedOn.DateFormat(),
            role.DescriptionBg,
            role.DescriptionEn);
}
