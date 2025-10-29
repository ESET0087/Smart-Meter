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
        public readonly AppDbContext _context;
        public readonly EnergyMeasurementServices _energyMeasurementServices;

        public MeterReadingController(AppDbContext context, EnergyMeasurementServices energyMeasurementServices)
        {
            _context = context;
            _energyMeasurementServices = energyMeasurementServices;
        }

        [HttpPost("energyConsumption")]
        public async Task<IActionResult> energyConsumption([FromBody] EnergyConsumtionMeasurment energyCon)
        {
            var energy = await _energyMeasurementServices.EnergyConsumtion(energyCon);

            return Ok(energy.message);
        }
    }
}
