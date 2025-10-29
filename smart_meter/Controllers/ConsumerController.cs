using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsumerController : ControllerBase
    {
        public readonly AppDbContext _context;
        public readonly BillService _billservice;

        public ConsumerController(AppDbContext context,BillService billService)
        {
            _context = context;
            _billservice = billService;
        }

        [HttpGet("PreviousBills/{consumerId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPreviousBills(int consumerId)
        {
            var bills = await _billservice.GetPreviousBillsAsync(consumerId);
            if (bills == null ||!bills.Any())
            {
                return NotFound(new { Message = "No bills found for the specified consumer." });
            }
            return Ok(bills);
        }

        



    }
    }
