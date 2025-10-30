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

        // add tarrif
        [HttpPost("add")]
        public async Task<IActionResult> AddTariff([FromBody] TariffDto dto)
        {
            // Call the TariffService to add the tariff
            var tariff = await _tariffservice.AddTariffAsync(dto);

            // Return the success response with the added tariff data
            return Ok(new { message = "Tariff added successfully", tariff });
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
