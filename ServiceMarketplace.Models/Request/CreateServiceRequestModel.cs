namespace ServiceMarketplace.Models.Request;

public record CreateServiceRequestModel(
    string NameBg,
    string NameEn,
    string DescriptionBg,
    string DescriptionEn,
    Guid SubCategoryId,
    int OfferedAtId,
    HashSet<Guid> Cities,
    HashSet<int> Tags,
    ManageContactRequestModel ContactRequestModel);
