namespace ServiceMarketplace.Models.Request;

public record ChangeCategoryRequestModel(Guid SubCategoryId, HashSet<int> Tags);
