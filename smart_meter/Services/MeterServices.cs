using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;
using System.Threading.Tasks;

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
    }
}

