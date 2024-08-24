namespace ServiceMarketplace.Models.Response.Contacts;

public record ContactResponseModel(
        int Id,
        string Name,
        string PhoneNumber,
        string LocationUrl);
