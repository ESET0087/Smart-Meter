using smart_meter.Data.Context;
using smart_meter.Data.Entities;
using smart_meter.Model.DTOs;

namespace smart_meter.Services
{
    public class OrgunitService
    {
        private readonly AppDbContext _context;

        public OrgunitService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Orgunit> AddOrgunitAsync(OrgUnitDto dto)
        {
            var orgunit = new Orgunit
            {
                Type = dto.Type,
                Name = dto.Name,
                Parentid = dto.Parentid
            };

            // Add orgunit to the database
            _context.Orgunits.Add(orgunit);
            await _context.SaveChangesAsync();

            return orgunit; // Return the added orgunit
        }
    }
}
