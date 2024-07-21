namespace ServiceMarketplace.Services.Interfaces.Owner;

public interface ICategoryService
{
    Task ValidateSubCategoryAsync(Guid subCategoryId);

    Task ValidateSelectedTagAsync(int tagId, Guid subCategoryId);
}
