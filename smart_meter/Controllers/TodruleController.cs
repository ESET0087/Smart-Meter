using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodRuleController : ControllerBase
    {
        private readonly TariffService _tariffservice;
        public TodRuleController(TariffService tariffservice)
        {
            _tariffservice = tariffservice;
        }

        [HttpPut("{todRuleId}")]
        public async Task<IActionResult> UpdateTodRule(int todRuleId, [FromBody] TodRuleUpdateRequest request)
        {
            var (success, errorMessage) = await _tariffservice.UpdateTodRuleAsync(todRuleId, request);

            if (!success)
            {
                if (errorMessage == "TOD Rule not found.")
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage });
            }

            return NoContent();
        }
    }
}
