using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Data.Entities;

namespace smart_meter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodruleController : Controller
    {
        public readonly AppDbContext _context;

        public TodruleController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("getTodrule")]
        public async Task<ActionResult<IEnumerable<object>>> GetTodrule()
        {
            var todrule = await _context.Todrules
                .Select(t => new
                {
                    t.Todruleid,
                    t.Name,
                    t.Starttime,
                    t.Endtime,
                    t.Rateperkwh,
                    
                })
                .ToListAsync();

            return Ok(todrule);
        }

        [HttpGet("GetTodRuleByConsumer/{id}")]
        public async Task<ActionResult<object>> GetTodRuleByConsumer(int id)
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

            // Step 2: Get TOD rule(s) associated with that tariff
            var todRule = await _context.Todrules
                .Where(t => t.Tariffid == consumerTariff && (t.Isdeleted == false || t.Isdeleted == null))
                .Select(t => new
                {
                    t.Todruleid,
                    t.Tariffid,
                    t.Name,
                    t.Starttime,
                    t.Endtime,
                    t.Rateperkwh
                   
                })
                .FirstOrDefaultAsync();

            if (todRule == null)
            {
                return NotFound(new { Message = "TOD Rule not found for this consumer." });
            }

            // Step 3: Return TOD rule details
            return Ok(todRule);
        }




    }
}
