using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
using smart_meter.Services;
using System.Security;
using System.Text;

namespace smart_meter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly AppDbContext _context;
        public readonly UpdateUserServices _updateUserServices;

        public UserController(AppDbContext context, UpdateUserServices updateUserServices)
        {
            _context = context;
            _updateUserServices = updateUserServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = _context.User.ToArray();
            return Ok(user) ;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdatePasswordDto passwordDto)
        {
            try
            {
                var update = await _updateUserServices.UpdateUser(id, passwordDto);
                if (!update.result) return BadRequest(update.message);

                return Ok(update.message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }   
    }
}
