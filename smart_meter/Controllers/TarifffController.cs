
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;
using System.Threading.Tasks;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TariffController : ControllerBase
    {
        private readonly TariffService _tariffService;

        // Inject the TariffService into the controller
        public TariffController(TariffService tariffService)
        {
            _tariffService = tariffService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddTariff([FromBody] TariffDto dto)
        {
            // Call the TariffService to add the tariff
            var tariff = await _tariffService.AddTariffAsync(dto);

            // Return the success response with the added tariff data
            return Ok(new { message = "Tariff added successfully", tariff });
        }
    }
}

