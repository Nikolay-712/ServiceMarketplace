using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Models.Response.ServiceResponseModels;

namespace ServiceMarketplace.Controllers.Owner;

//[Authorize(Roles = "Owner")]
[Route("api/owner/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IContactService _contactService;

    public ContactsController(IContactService contactService)
    {
        _contactService = contactService;
    }

    [HttpPost("{serviceId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> AddAsync([FromRoute] Guid serviceId, ManageContactRequestModel requestModel)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");

        await _contactService.AddAsync(serviceId, requestModel, ownerId);
        return new ResponseContent();
    }

    [HttpGet("{serviceId}")]
    [ProducesResponseType<ResponseContent<IReadOnlyList<ContactResponseModel>>>(200)]
    public async Task<ResponseContent<IReadOnlyList<ContactResponseModel>>> GetAllAsync([FromRoute] Guid serviceId)
    {
        IReadOnlyList<ContactResponseModel> contacts = await _contactService.GetAllAsync(serviceId);
        return new ResponseContent<IReadOnlyList<ContactResponseModel>>
        {
            Result = contacts,
        };
    }

    [HttpPut("{serviceId}/{contactId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateAsync([FromRoute] Guid serviceId, [FromRoute] int contactId, ManageContactRequestModel requestModel)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");

        await _contactService.UpdateAsync(contactId, serviceId, ownerId, requestModel);
        return new ResponseContent();
    }

    [HttpDelete("{serviceId}/{contactId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> RemoveAsync([FromRoute] Guid serviceId, [FromRoute] int contactId)
    {
        Guid ownerId = Guid.Parse("CEFAD0F7-678E-4769-B0C6-3943BF78A59D");

        await _contactService.RemoveAsync(contactId, serviceId, ownerId);
        return new ResponseContent();
    }
}
