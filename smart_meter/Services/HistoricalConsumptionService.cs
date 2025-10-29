//using Microsoft.EntityFrameworkCore;
//using smart_meter.Data.Context;
//using smart_meter.Data.Entities;
//using smart_meter.Model.DTOs;
//using System;
//using System.Linq;
//using System.Threading.Tasks;

//namespace smart_meter.Services
//{
//    public class HistoricalConsumptionService
//    {
//        private readonly AppDbContext _context;

//        public HistoricalConsumptionService(AppDbContext context)
//        {
//            _context = context;
//        }

//        // Method to get historical consumption data based on OrgUnit and date range
//        public async Task<HistoricalConsumptionDto> GetHistoricalConsumptionAsync(int orgUnitId, DateTime startDate, DateTime endDate)
//        {
//            // Get the OrgUnit
//            var orgUnit = await _context.Orgunits
//                .Where(o => o.Orgunitid == orgUnitId)
//                .FirstOrDefaultAsync();

//            if (orgUnit == null)
//                return null; // OrgUnit not found

//            // Get the meters associated with the OrgUnit
//            var meters = await _context.Meters
//                .Where(m => m.Consumer.Orgunitid == orgUnitId)
//                .Select(m => m.Meterserialno)
//                .ToListAsync();

//            if (meters == null || meters.Count == 0)
//                return null; // No meters found for OrgUnit

//            // Fetch meter readings within the date range for those meters
//            var totalEnergyConsumed = await _context.Meterreadings
//                .Where(r => meters.Contains(r.Meterserialno)
//                            && r.Readingdatetime >= startDate
//                            && r.Readingdatetime <= endDate)
//                .SumAsync(r => r.Energyconsumed);

//            // Return the DTO with the OrgUnitName and the total energy consumed dynamically
//            return new HistoricalConsumptionDto
//            {
//                OrgUnitName = orgUnit.Name,  // OrgUnit Name
//                StartDate = startDate,       // Start Date
//                EndDate = endDate  ,          // End Date
//                TotalEnergyConsumed = totalEnergyConsumed

//            };
//        }
//    }
//}
using Microsoft.EntityFrameworkCore;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
using System;
using System.Linq;
using System.Threading.Tasks;

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

