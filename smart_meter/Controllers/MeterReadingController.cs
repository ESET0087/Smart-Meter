using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MeterReadingController : ControllerBase
    {
        private readonly MeterReadingService _meterReadingService;

        public MeterReadingController(MeterReadingService meterReadingService)
        {
            _meterReadingService = meterReadingService;
        }

        // POST api/meterreading/add
        [HttpPost("add")]
        public async Task<IActionResult> AddMeterReading([FromBody] MeterReading dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Return validation errors if any

            // Call service to add meter reading
            var reading = await _meterReadingService.AddMeterReadingAsync(dto);

            // Return success response
            return Ok(new
            {
                message = "Meter reading added successfully",
                reading
            });
        }
    }
}

