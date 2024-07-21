namespace ServiceMarketplace.Models.Request;

public record CreateServiceRequestModel(
    string NameBg, 
    string NameEn,
    string DescriptionBg, 
    string DescriptionEn,
    Guid SubCategoryId,
    HashSet<Guid> Cities,
    HashSet<int> Tags);
