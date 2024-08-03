namespace ServiceMarketplace.Models.Request;

public record UpdateServiceRequestModel(
    string NameBg,
    string NameEn,
    string DescriptionBg,
    string DescriptionEn, 
    int OfferedAtId,
    HashSet<Guid>? Cities);
