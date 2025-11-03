using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Npgsql;
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

        // add tariff
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

        // method for user to update Tarrif
        public async Task<(bool Success, string? ErrorMessage)> UpdateTariffAsync(int id, TariffUpdateRequest request)
        {
            var tariff = await _context.Tariffs.FindAsync(id);

            if (tariff == null)
            {
                return (false, "Tariff not found.");
            }

            if (request.Name != null)
            {
                tariff.Name = request.Name;
            }
            if (request.EffectiveFrom.HasValue)
            {
                tariff.Effectivefrom = request.EffectiveFrom.Value;
            }
            if (request.EffectiveTo.HasValue)
            {
                tariff.Effectiveto = request.EffectiveTo;
            }
            if (request.BaseRate.HasValue)
            {
                if (request.BaseRate.Value <= 0)
                {
                    return (false, "BaseRate must be greater than 0.");
                }
                tariff.Baserate = request.BaseRate.Value;
            }
            if (request.TaxRate.HasValue)
            {
                tariff.Taxrate = request.TaxRate.Value;
            }

            if (tariff.Effectiveto.HasValue && tariff.Effectiveto <= tariff.Effectivefrom)
            {
                return (false, "EffectiveTo must be after EffectiveFrom.");
            }

            await _context.SaveChangesAsync();
            return (true, null); // Success
        }

        // method for user to update Tod Rule
        public async Task<(bool Success, string? ErrorMessage)> UpdateTodRuleAsync(int id, TodRuleUpdateRequest request)
        {
            var todRule = await _context.Todrules.FindAsync(id);

            if (todRule == null)
            {
                return (false, "TOD Rule not found.");
            }

            if (request.Name != null)
            {
                todRule.Name = request.Name;
            }
            if (request.StartTime.HasValue)
            {
                todRule.Starttime = request.StartTime.Value;
            }
            if (request.EndTime.HasValue)
            {
                todRule.Endtime = request.EndTime.Value;
            }
            if (request.RatePerKwh.HasValue)
            {
                todRule.Rateperkwh = request.RatePerKwh.Value;
            }

            if (todRule.Starttime >= todRule.Endtime)
            {
                return (false, "StartTime must be before EndTime.");
            }

            await _context.SaveChangesAsync();
            return (true, null); // Success
        }

        // method for user to update Tarrif Slab
        public async Task<(bool Success, string? ErrorMessage)> UpdateTariffSlabAsync(int id, TariffSlabUpdateRequest request)
        {
            var slab = await _context.Tariffslabs.FindAsync(id);

            if (slab == null)
            {
                return (false, "Tariff slab not found.");
            }

            if (request.FromKwh.HasValue)
            {
                slab.Fromkwh = request.FromKwh.Value;
            }
            if (request.ToKwh.HasValue)
            {
                slab.Tokwh = request.ToKwh.Value;
            }
            if (request.RatePerKwh.HasValue)
            {
                if (request.RatePerKwh.Value < 0)
                {
                    return (false, "RatePerKwh must be >= 0.");
                }
                slab.Rateperkwh = request.RatePerKwh.Value;
            }

            if (slab.Fromkwh < 0 || slab.Tokwh <= slab.Fromkwh)
            {
                return (false, "Invalid slab range: FromKwh must be >= 0 and ToKwh must be > FromKwh.");
            }

            try
            {
                await _context.SaveChangesAsync();
                return (true, null); // Success
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx && pgEx.SqlState == "23P01")
            {
                // 23P01 is the SQLSTATE for an exclusion constraint violation.
                // This catches the non-overlapping range check.
                return (false, "Tariff slabs for the same tariff cannot overlap.");
            }
            catch (Exception ex)
            {
                return (false, $"An error occurred while saving: {ex.Message}");
            }
        }

        // Gets a list of all tariffs with calculated total rates.
        public async Task<IEnumerable<object>> GetAllTariffsAsync()
        {
            return await _context.Tariffs
                .Select(t => new
                {
                    t.Tariffid,
                    t.Name,
                    t.Effectivefrom,
                    t.Effectiveto,
                    t.Baserate,
                    t.Taxrate,
                    TotalRate = t.Baserate + t.Taxrate
                })
                .ToListAsync();
        }

        // Gets a specific consumer's assigned tariff details.
        public async Task<object> GetTariffByConsumerIdAsync(int consumerId)
        {
            var tariffDto = await _context.Consumers
                .Where(c => c.Consumerid == consumerId)
                .Select(c => c.Tariff) 
                .Select(t => new 
                {
                    t.Tariffid,
                    t.Name,
                    t.Effectivefrom,
                    t.Effectiveto,
                    t.Baserate,
                    t.Taxrate,
                    TotalRate = t.Baserate + t.Taxrate
                })
                .FirstOrDefaultAsync();

            return tariffDto; // This will be null if the consumer or tariff is not found
        }
    }
}