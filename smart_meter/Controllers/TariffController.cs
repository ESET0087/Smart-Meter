using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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

        //Update tariff by id
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

        // get all tarrif
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTariffRates()
        {
            var tariffs = await _tariffservice.GetAllTariffsAsync();
            return Ok(tariffs);
        }

        // get tariff of a consumer
        [HttpGet("GetTariffByConsumer/{id}")]
        public async Task<ActionResult<object>> GetTariffById(int id)
        {
            var tariff = await _tariffservice.GetTariffByConsumerIdAsync(id);

            if (tariff == null)
            {
                return NotFound(new { Message = "Consumer not found or tariff not assigned." });
            }

            return Ok(tariff);
        }
    }
}
