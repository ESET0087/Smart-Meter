using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class MeterReadingService
    {
        private readonly AppDbContext _context;

        public MeterReadingService(AppDbContext context)
        {
            _context = context;
        }

        // Method to insert a new meter reading
        public async Task<Meterreading> AddMeterReadingAsync(MeterReading dto)
        {
            // Create new entity from DTO
            var meterReading = new Meterreading
            {
                Meterserialno = dto.Meterserialno,
                Readingdatetime = dto.Readingdatetime,
                Energyconsumed = dto.Energyconsumed,
                Voltage = dto.Voltage,
                Current = dto.Current
            };

            // Add entity to DB context
            _context.Meterreadings.Add(meterReading);

            // Save changes asynchronously
            await _context.SaveChangesAsync();

            return meterReading;
        }


    }
}

