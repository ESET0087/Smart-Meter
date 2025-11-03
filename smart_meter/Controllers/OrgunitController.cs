
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrgunitController : ControllerBase
    {
        private readonly OrgunitService _orgunitService;

        // Inject the OrgunitService into the controller
        public OrgunitController(OrgunitService orgunitService)
        {
            _orgunitService = orgunitService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddOrgunit([FromBody] OrgUnitDto dto)
        {
            // Call the OrgunitService to add the orgunit
            var orgunit = await _orgunitService.AddOrgunitAsync(dto);

            // Return the success response with the added orgunit data
            return Ok(new { message = "Orgunit added successfully", orgunit });
        }
    }
}
