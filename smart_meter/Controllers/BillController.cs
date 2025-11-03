using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
using smart_meter.Services;

namespace smart_meter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : Controller
    {

        public readonly AppDbContext _context;
        public readonly BillService _billService;
        public BillController(AppDbContext context,BillService billService)
        {
            _context = context;
            _billService = billService;
        }

        // Generate Bill
        [HttpGet("GenerateBill")]
        public async Task<IActionResult> GenerateBill(int consumerId)
        {
            var meterno = await _context.Meters
                .Where(m => m.Consumerid == consumerId)
                .Select(m => m.Meterserialno)
                .FirstOrDefaultAsync();

            //Console.WriteLine("Meter Numbers:" + meterno.ToString()+"\n\n\n\n");
            var bill = await _billService.GenerateBillAsync(meterno.ToString());

            var dto = new
            {                
                Consumerid = bill.Consumerid,
                Meterserialno = bill.Meterserialno,
                Billingperiodstart = bill.Billingperiodstart,
                Billingperiodend = bill.Billingperiodend,
                Totalunitsconsumed = bill.Totalunitsconsumed,
                Baseamount = bill.Baseamount,
                Taxamount = bill.Taxamount,
                Totalamount = bill.Totalamount ?? 0,
                Generatedat = bill.Generatedat,
                Duedate = bill.Duedate,
                Ispaid = bill.Ispaid
            };
            return Ok(dto);

        }
       }
}
