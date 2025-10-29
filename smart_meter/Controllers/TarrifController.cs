using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;

namespace smart_meter.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TarrifController : Controller
    {
        public readonly AppDbContext _context;
        public TarrifController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetTariffRates()
        {
            var tariffs = await _context.Tariffs
                .Select(t => new
                {
                    t.Tariffid,
                    t.Name,
                    t.Effectivefrom,
                    t.Effectiveto,
                    t.Baserate,
                    t.Taxrate,
                    TotalRate = t.Baserate + (t.Baserate * t.Taxrate / 100)
                })
                .ToListAsync();

            return Ok(tariffs);
        }

        [HttpGet("GetTariffByConsumer/{id}")]
        public async Task<ActionResult<object>> GetTariffById(int id)
        {
            // Step 1: Get the tariff ID for the given consumer
            var consumerTariff = await _context.Consumers
                .Where(c => c.Consumerid == id)
                .Select(c => c.Tariffid)
                .FirstOrDefaultAsync();

            if (consumerTariff == 0)
            {
                return NotFound(new { Message = "Consumer not found or tariff not assigned." });
            }

            // Step 2: Fetch the tariff details using the tariff ID
            var tariff = await _context.Tariffs
                .Where(t => t.Tariffid == consumerTariff)
                .Select(t => new
                {
                    t.Tariffid,
                    t.Name,
                    t.Effectivefrom,
                    t.Effectiveto,
                    t.Baserate,
                    t.Taxrate,
                    TotalRate = t.Baserate + (t.Baserate * t.Taxrate / 100)
                })
                .FirstOrDefaultAsync();

            if (tariff == null)
            {
                return NotFound(new { Message = "Tariff details not found." });
            }

            // Step 3: Return tariff details
            return Ok(tariff);
        }

      



    }
}
