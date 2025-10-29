using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using smart_meter.Data.Context;
using smart_meter.Data.Entities;

namespace smart_meter.Services
{
    public class BillService
    {
        public readonly AppDbContext _context;

        public BillService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Bill>> GetPreviousBillsAsync(int consumerId)
        {
            return await _context.Bills
                .Where(b => b.Consumerid == consumerId)
                .OrderByDescending(b => b.Generatedat)
                .ToListAsync();
        }

        public async Task<Bill> GenerateBillAsync(string meterserialno)
        {
            var onemonthago = DateTime.Now.AddMonths(-1);
            var meterReading = await _context.Meterreadings.Where(m => m.Meterserialno == meterserialno && m.Readingdatetime >= onemonthago)
                                                            .OrderBy(m => m.Readingdatetime.TimeOfDay)
                                                            .ToListAsync();
            var tarriff = await _context.Meters.Where(m => m.Meterserialno == meterserialno)
                            .Select(x => x.Consumer.Tariffid)
                          .FirstOrDefaultAsync();
            var todrule = await _context.Todrules.Where(m => m.Tariffid == tarriff).OrderBy(t => t.Endtime).ToListAsync();
            
            decimal energyconsumed = 0;
            decimal price = 0;
            foreach (var rule in todrule)
            {
                foreach(var reading in meterReading)
                {
                    decimal ruleEnergyConsume = 0;
                    while (reading.Readingdatetime.TimeOfDay >= rule.Starttime.ToTimeSpan() && reading.Readingdatetime.TimeOfDay < rule.Endtime.ToTimeSpan())
                    {
                        ruleEnergyConsume += reading.Energyconsumed;
                    }
                    energyconsumed = ruleEnergyConsume;
                    price += ruleEnergyConsume * rule.Rateperkwh;
                }
            }

        }
    }
}
