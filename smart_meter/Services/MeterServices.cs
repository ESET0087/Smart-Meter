using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class MeterService
    {
        private readonly AppDbContext _context;

        public MeterService(AppDbContext context)
        {
            _context = context;
        }
        // add meter
        public async Task<Meter> AddMeterAsync(MeterCreateDto dto)
        {
            var meter = new Meter
            {
                Meterserialno = dto.Meterserialno,
                Ipaddress = dto.Ipaddress,
                Iccid = dto.Iccid,
                Imsi = dto.Imsi,
                Manufacturer = dto.Manufacturer,
                Firmware = dto.Firmware,
                Category = dto.Category,
                Installtsutc = dto.Installtsutc,
                Status = dto.Status,
                Consumerid = dto.Consumerid
            };

            _context.Meters.Add(meter);
            await _context.SaveChangesAsync();

            return meter;
        }
        public async Task InsertMeterData(IEnumerable<MeterReading> readingdto)
        {
            var reading = readingdto.Select(r => new Meterreading
            {
                Meterserialno = r.Meterserialno,
                Energyconsumed = r.Energyconsumed,
                Readingdatetime = r.Readingdatetime,
                Voltage = r.Voltage,
                Current = r.Current
            }).ToList();
            

            await _context.Meterreadings.AddRangeAsync(reading);
            await _context.SaveChangesAsync();
            //return Task.CompletedTask;
        }
    }
}

