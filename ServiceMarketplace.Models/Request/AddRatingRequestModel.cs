namespace ServiceMarketplace.Models.Request;

public record AddRatingRequestModel(
    Guid ServiceId, 
    int Value,
    string? Comment);

