﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMarketplace.Models;
using ServiceMarketplace.Models.Request;
using ServiceMarketplace.Services.Interfaces.Owner;

using static ServiceMarketplace.Common.Constants;

namespace ServiceMarketplace.Controllers.Owner;

[Authorize(Roles = OwnerRoleName)]
[Route("api/owner/business-hours")]
[ApiController]
public class BusinessHoursController : ControllerBase
{
    private readonly IBusinessHoursService _businessHoursService;

    public BusinessHoursController(IBusinessHoursService businessHoursService)
    {
        _businessHoursService = businessHoursService;
    }

    [HttpPost]
    [ProducesResponseType<ResponseContent>(200)]
    public async Task<ResponseContent> ManageAsync(ManageBusinessHoursRequestModel requestModel)
    {
        await _businessHoursService.MangeAsync(requestModel);
        return new ResponseContent();
    }

    [HttpGet("{serviceId}")]
    public async Task GetAsync([FromRoute] Guid serviceId)
    {
       var result = await _businessHoursService.GetAsync(serviceId);
    }
}
