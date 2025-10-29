using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class TariffService
    {
        private readonly AppDbContext _context;

        public TariffService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Tariff> AddTariffAsync(TariffDto dto)
        {
            var tariff = new Tariff
            {
                Name = dto.Name,
                Effectivefrom = dto.EffectiveFrom,
                Effectiveto = dto.EffectiveTo,
                Baserate = dto.BaseRate,
                Taxrate = dto.TaxRate
            };

            // Add tariff to the database
            _context.Tariffs.Add(tariff);
            await _context.SaveChangesAsync();

            return tariff; // Return the added tariff
        }
    }
}

