using ServiceMarketplace.Data.Entities;

using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Models.Extensions;

public static class ContactMappingExtensions
{
    public static ContactResponseModel ToContactResponseModel(this Contact contact)
        => new(contact.Id,
               contact.Name,
               contact.PhoneNumber,
               contact.LocationUrl ?? "n/a");
}
