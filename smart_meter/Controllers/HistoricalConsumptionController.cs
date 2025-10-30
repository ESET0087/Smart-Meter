using Microsoft.AspNetCore.Mvc;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricalConsumptionController : ControllerBase
    {
        private readonly HistoricalConsumptionService _historicalConsumptionService;

        // Constructor to inject the service
        public HistoricalConsumptionController(HistoricalConsumptionService historicalConsumptionService)
        {
            _historicalConsumptionService = historicalConsumptionService;
        }

        // GET api/historicalconsumption/total-consumption
        [HttpGet("total-consumption")]
        public async Task<IActionResult> GetTotalEnergyConsumed([FromQuery] int orgUnitId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {

            startDate = startDate.ToUniversalTime();
            endDate = endDate.ToUniversalTime();
            // Validate the date range
            if (startDate > endDate)
                return BadRequest("Start date cannot be greater than end date.");

            // Get the historical consumption data from the service
            var historicalData = await _historicalConsumptionService.GetHistoricalConsumptionAsync(orgUnitId, startDate, endDate);

            // Check if no data is found for the provided OrgUnit and date range
            if (historicalData == null)
                return NotFound("No data found for the given OrgUnit and date range.");

            // Return the result
            return Ok(historicalData);
        }
    }
}
