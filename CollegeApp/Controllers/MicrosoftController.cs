﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [EnableCors(PolicyName = "AllowOnlyMicrosoft")]
    [Authorize(AuthenticationSchemes = "LoginForMicrosoftUsers", Roles = "Superadmin,Admin")]
    public class MicrosoftController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult Get()
        {
            return Ok("This is microsoft");
        }
    }
}