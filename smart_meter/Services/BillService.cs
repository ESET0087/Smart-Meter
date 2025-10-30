using Microsoft.AspNetCore.Http.HttpResults;
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
            try
            {
                var oneMonthAgo = DateTime.UtcNow.AddMonths(-1);

                // 1️⃣ Fetch all readings for this meter (past month)
                var meterReadings = await _context.Meterreadings
                    .Where(m => m.Meterserialno == meterserialno && m.Readingdatetime > oneMonthAgo)
                    .OrderBy(m => m.Readingdatetime)
                    .ToListAsync();

                if (meterReadings.Count < 2)
                    return null; // Not enough data

                // 2️⃣ Fetch tariff rules
                var tariffid = await _context.Meters
                    .Where(m => m.Meterserialno == meterserialno)
                    .Select(x => new  { 
                                    tariffid = x.Consumer.Tariffid,
                                    consumerid = x.Consumerid
                    })
                    .FirstOrDefaultAsync();

                var tariff = await _context.Tariffs.FirstOrDefaultAsync(x => x.Tariffid == tariffid.tariffid);

                var todRules = await _context.Todrules
                    .Where(r => r.Tariffid == tariffid.tariffid)
                    .OrderBy(r => r.Starttime)
                    .ToListAsync();

                var tariffslab = await _context.Tariffslabs
                    .Where(s => s.Tariffid == tariffid.tariffid)
                    .OrderBy(s => s.Fromkwh)
                    .ToListAsync();

                // 3️⃣ Initialize totals
                decimal totalEnergy = 0;
                decimal totalPrice = 0; 

                // 4️⃣ Calculate energy for each 30-min interval
                for (int i = 1; i < meterReadings.Count; i++)
                {
                    var prev = meterReadings[i - 1];
                    var curr = meterReadings[i];

                    decimal intervalEnergy = curr.Energyconsumed - prev.Energyconsumed;
                    if (intervalEnergy < 0) continue; // skip if reset or invalid

                    var time = curr.Readingdatetime.TimeOfDay;

                    // 5️⃣ Find which TOD rule this interval belongs to
                    var matchingRule = todRules.FirstOrDefault(rule =>
                        (rule.Starttime < rule.Endtime &&
                            time >= rule.Starttime.ToTimeSpan() && time < rule.Endtime.ToTimeSpan()) ||
                        (rule.Starttime > rule.Endtime &&
                            (time >= rule.Starttime.ToTimeSpan() || time < rule.Endtime.ToTimeSpan()))
                    );

                    if (matchingRule == null)
                    {
                        totalEnergy += intervalEnergy;
                        totalPrice += intervalEnergy * tariff.Baserate;
                    }

                    // 6️⃣ Apply rate
                    if (matchingRule != null)
                    {
                        totalEnergy += intervalEnergy;
                        totalPrice += intervalEnergy * matchingRule.Rateperkwh;
                    }
                }
                var slabEnergy = totalEnergy;
                while(slabEnergy > 0)
                {
                    foreach (var slab in tariffslab)
                    {
                        if (slabEnergy > slab.Fromkwh && slabEnergy <= slab.Tokwh)
                        {
                            totalPrice += slab.Rateperkwh * slabEnergy - slab.Fromkwh;
                            slabEnergy -= (slabEnergy - slab.Fromkwh);
                  
                        }
                    }
                    
                }

                Console.WriteLine("Total Energy : " + totalEnergy);
                Console.WriteLine("Total Price : " + (double)totalPrice);

                // 7️⃣ Return bill
                var newbill =  new Bill
                {
                    Generatedat = DateTime.UtcNow,
                    Meterserialno = meterserialno,
                    Billingperiodstart = DateOnly.FromDateTime(oneMonthAgo),
                    Billingperiodend = DateOnly.FromDateTime(DateTime.UtcNow),
                    Totalunitsconsumed = totalEnergy,
                    Baseamount = totalPrice,
                    Taxamount = totalPrice * tariff.Taxrate / 100,
                    Totalamount = totalPrice + (totalPrice * tariff.Taxrate / 100),
                    Duedate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(15)),
                    Ispaid = false,
                    Discdate = DateTime.UtcNow.AddMonths(3),
                    Consumerid =(long)tariffid.consumerid

                };

                _context.Bills.Add(newbill);
                await _context.SaveChangesAsync();
                return newbill;

            }
            catch (Exception EX)
            {
                return new Bill
                {
                    Generatedat = DateTime.Now,
                    Meterserialno = EX.Message
                };
            }

        }
    }
}
