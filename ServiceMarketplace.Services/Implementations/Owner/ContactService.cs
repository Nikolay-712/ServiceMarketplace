using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

namespace ServiceMarketplace.Services.Implementations.Owner;

public class ContactService : IContactService
{
    private const string BulgarianPhoneCode = "+359";

    private readonly ApplicationContext _applicationContext;
    private readonly ILogger<ContactService> _logger;

    public ContactService(ApplicationContext applicationContext, ILogger<ContactService> logger)
    {
        _applicationContext = applicationContext;
        _logger = logger;
    }

    public void CreateContactAsync(Guid serviceId, ManageContactRequestModel requestModel)
    {
        string phoneNumber = FormatPhoneNumber(requestModel.PhoneNumber);
        Contact contact = new()
        {
            Name = requestModel.Name,
            PhoneNumber = phoneNumber,
            LocationUrl = requestModel.LocationUrl,
            ServiceId = serviceId,
        };

        _applicationContext.Contacts.Add(contact);
    }

    public async Task UpdateContactAsync(int contactId, Guid serviceId, ManageContactRequestModel requestModel)
    {
        Contact? contact = await _applicationContext.Contacts.FirstOrDefaultAsync(x => x.Id == contactId && x.ServiceId == serviceId);
        if (contact is null)
        {
            _logger.LogError("Logger message");
            throw new NotFoundEntityException("Message");
        }

        string phoneNumber = FormatPhoneNumber(requestModel.PhoneNumber);

        contact.Name = requestModel.Name;
        contact.PhoneNumber = phoneNumber;
        contact.LocationUrl = requestModel.LocationUrl;

        await _applicationContext.SaveChangesAsync();
        _logger.LogInformation("Successfully updated a contact with ID {ContactId} for Service with ID: {ServiceId}", contactId, serviceId);
    }

    private string FormatPhoneNumber(string phoneNumber)
    {
        if (phoneNumber.StartsWith("0"))
        {
            phoneNumber = BulgarianPhoneCode + phoneNumber.Substring(1);
        }

        return phoneNumber;
    }
}
