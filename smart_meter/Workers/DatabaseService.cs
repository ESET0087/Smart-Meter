using System;
using System.Threading.Tasks;
using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;

namespace smart_meter.Worker
{
    internal class DatabaseService
    {
        private readonly AppDbContext _context;
        public DatabaseService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task InsertMeterReadingAsync(MeterReading dto)
        {
            if (dto is null) throw new ArgumentNullException(nameof(dto));

            var entity = new Meterreading
            {
                Meterserialno = dto.Meterserialno,
                Readingdatetime = DateTime.SpecifyKind(dto.Readingdatetime, DateTimeKind.Utc),
                Energyconsumed = dto.Energyconsumed,
                Voltage = dto.Voltage,
                Current = dto.Current
            };

            _context.Meterreadings.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}