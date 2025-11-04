using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MeterController : ControllerBase
    {
        private readonly MeterService _meterService;

        public MeterController(MeterService meterService)
        {
            _meterService = meterService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddMeter([FromBody] MeterCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var meter = await _meterService.AddMeterAsync(dto);

            return Ok(new
            {
                message = "Meter added successfully",
                meter
            });
        }
    }
}

