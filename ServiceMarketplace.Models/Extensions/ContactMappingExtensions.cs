using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Response;

namespace ServiceMarketplace.Models.Extensions;

public static class ContactMappingExtensions
{
    public static ContactResponseModel ToContactResponseModel(this Contact contact)
        => new(contact.Id,
               contact.Name,
               contact.PhoneNumber,
               contact.LocationUrl ?? "n/a");
}
