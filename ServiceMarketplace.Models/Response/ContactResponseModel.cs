namespace ServiceMarketplace.Models.Response;

public record ContactResponseModel(
    int Id, 
    string Name, 
    string PhoneNumber, 
    string LocationUrl);

