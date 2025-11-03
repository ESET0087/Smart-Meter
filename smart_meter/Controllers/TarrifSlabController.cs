using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TariffSlabController : ControllerBase
    {
        private readonly TariffService _tariffservice;
        public TariffSlabController(TariffService tariffservice)
        {
            _tariffservice = tariffservice;
        }

        [HttpPut("{TariffSlabId}")]
        public async Task<IActionResult> UpdateTariff(int TariffSlabId, [FromBody] TariffSlabUpdateRequest request)
        {
            var (success, errorMessage) = await _tariffservice.UpdateTariffSlabAsync(TariffSlabId, request);

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
