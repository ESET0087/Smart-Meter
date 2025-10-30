
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class HistoricalConsumptionService
    {
        private readonly AppDbContext _context;

        public HistoricalConsumptionService(AppDbContext context)
        {
            _context = context;
        }

        // Method to get historical consumption data based on OrgUnit and date range
        public async Task<HistoricalConsumptionDto> GetHistoricalConsumptionAsync(int orgUnitId, DateTime startDate, DateTime endDate)
        {
            // Ensure dates are UTC to prevent PostgreSQL timestamp issues
            startDate = DateTime.SpecifyKind(startDate, DateTimeKind.Utc);
            endDate = DateTime.SpecifyKind(endDate, DateTimeKind.Utc);

            // ✅ Fetch OrgUnit details first
            var orgUnit = await _context.Orgunits
                .FirstOrDefaultAsync(o => o.Orgunitid == orgUnitId);

            if (orgUnit == null)
                return null; // OrgUnit not found

            // ✅ Fetch total energy consumed via join from OrgUnit → Consumer → Meter → MeterReading
            var totalEnergyConsumed = await (
                from ou in _context.Orgunits
                join c in _context.Consumers on ou.Orgunitid equals c.Orgunitid
                join m in _context.Meters on c.Consumerid equals m.Consumerid
                join r in _context.Meterreadings on m.Meterserialno equals r.Meterserialno
                where ou.Orgunitid == orgUnitId
                      && r.Readingdatetime >= startDate
                      && r.Readingdatetime <= endDate
                select r.Energyconsumed
            ).SumAsync();

            // ✅ Return the DTO with results
            return new HistoricalConsumptionDto
            {
                OrgUnitName = orgUnit.Name,
                StartDate = startDate,
                EndDate = endDate,
                TotalEnergyConsumed = totalEnergyConsumed
            };
        }
    }
}

