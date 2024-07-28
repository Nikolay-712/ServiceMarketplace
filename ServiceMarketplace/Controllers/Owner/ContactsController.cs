using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

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

    [HttpPut("{serviceId}/{contactId}")]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> UpdateContactAsync([FromRoute] Guid serviceId, [FromRoute] int contactId, ManageContactRequestModel requestModel)
    {
        await _contactService.UpdateContactAsync(contactId, serviceId, requestModel);
        return new ResponseContent();
    }
}
