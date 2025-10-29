using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(Roles = "User")]
    public class TariffController : ControllerBase
    {
        private readonly TariffService _tariffservice;
        public TariffController(TariffService tariffservice)
        {
            _tariffservice = tariffservice;
        }

        [HttpPut("{TariffId}")]
        public async Task<IActionResult> UpdateTariff(int TariffId, [FromBody] TariffUpdateRequest request)
        {
            var (success, errorMessage) = await _tariffservice.UpdateTariffAsync(TariffId, request);

            if (!success)
            {
                if (errorMessage == "Tariff not found.")
                {
                    return NotFound(new { message = errorMessage });
                }
                return BadRequest(new { message = errorMessage });
            }

            return NoContent();
        }
    }
}
