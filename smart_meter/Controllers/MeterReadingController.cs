using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        public readonly EnergyMeasurementServices _meterServices;

        public MeterReadingController(AppDbContext context, EnergyMeasurementServices energyMeasurementServices)
        {
            _meterServices = energyMeasurementServices;
        }

        // POST api/meterreading/add
        [HttpPost("add")]
        public async Task<IActionResult> AddMeterReading([FromBody] MeterReading dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Return validation errors if any

            // Call service to add meter reading
            var reading = await _meterServices.AddMeterReadingAsync(dto);

            // Return success response
            return Ok(new
            {
                message = "Meter reading added successfully",
                reading
            });
        }

        // method to find energy consumption of a meter
        [HttpPost("energyConsumption")]
        public async Task<IActionResult> energyConsumption([FromBody] EnergyConsumtionMeasurment energyCon)
        {
            var energy = await _meterServices.EnergyConsumtion(energyCon);

            return Ok(energy.message);
        }
    }
}
