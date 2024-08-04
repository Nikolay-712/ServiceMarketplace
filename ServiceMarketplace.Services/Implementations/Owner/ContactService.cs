using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMarketplace.Common.Exceptions.ClientExceptions;
using ServiceMarketplace.Common.Resources;
using ServiceMarketplace.Data;
using ServiceMarketplace.Data.Entities;
using ServiceMarketplace.Models.Extensions;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Models.Response.ServiceResponseModels;

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

    public async Task AddAsync(Guid serviceId, ManageContactRequestModel requestModel)
    {
        bool existsService = await _applicationContext.Services.AnyAsync(x => x.Id == serviceId);
        if (!existsService)
        {
            _logger.LogError("No service exists with this ID {ServiceId}", serviceId);
            throw new NotFoundEntityException(Messages.NotFoundService);
        }

        CreateContactAsync(serviceId, requestModel);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully added contact for service with ID: {ServiceId}", serviceId);
    }

    public async Task<IReadOnlyList<ContactResponseModel>> GetAllAsync(Guid serviceId)
    {
        IQueryable<Contact> contactsQuery = _applicationContext.Contacts.Where(x => x.ServiceId == serviceId);
        IReadOnlyList<ContactResponseModel> contacts = await contactsQuery
            .OrderBy(x => x.Id)
            .Select(x => x.ToContactResponseModel())
            .ToListAsync();

        return contacts;
    }

    public async Task UpdateAsync(int contactId, Guid serviceId, ManageContactRequestModel requestModel)
    {
        Contact contact = await GetServiceContactAsync(contactId, serviceId);

        string phoneNumber = FormatPhoneNumber(requestModel.PhoneNumber);

        contact.Name = requestModel.Name;
        contact.PhoneNumber = phoneNumber;
        contact.LocationUrl = requestModel.LocationUrl;

        await _applicationContext.SaveChangesAsync();
        _logger.LogInformation("Successfully updated a contact with ID {ContactId} for Service with ID: {ServiceId}", contactId, serviceId);
    }

    public async Task RemoveAsync(int contactId, Guid serviceId)
    {
        Contact contact = await GetServiceContactAsync(contactId, serviceId);

        IQueryable<Contact> contactsQuery = _applicationContext.Contacts.Where(x => x.ServiceId == serviceId);
        bool isLastContact = await contactsQuery.CountAsync() == 1;
        if (isLastContact)
        {
            _logger.LogError("You cannot remove all contacts. The service must have at least one contact");
            throw new RemoveAllException(Messages.CannotRemoveAllContacts);
        }

        _applicationContext.Contacts.Remove(contact);
        await _applicationContext.SaveChangesAsync();

        _logger.LogInformation("Successfully remove a contact for service with ID: {ServiceId} and Contact ID {ContactId}", serviceId, contactId);
    }

    private async Task<Contact> GetServiceContactAsync(int contactId, Guid serviceId)
    {
        Contact? contact = await _applicationContext.Contacts.FirstOrDefaultAsync(x => x.Id == contactId && x.ServiceId == serviceId);
        if (contact is null)
        {
            _logger.LogError("No contact exists with this service ID {ServiceId} and Contact ID {ContactId}", serviceId, contactId);
            throw new NotFoundEntityException(Messages.NotFoundContact);
        }
        return contact;
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
