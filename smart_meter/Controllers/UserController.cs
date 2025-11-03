using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using smart_meter.Data.Context;
using smart_meter.Model.DTOs;
using smart_meter.Services;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace smart_meter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        public readonly AppDbContext _context;
        public readonly UserServices _userServices;

        public UserController(AppDbContext context, UserServices updateUserServices)
        {
            _context = context;
            _userServices = updateUserServices;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var user = _context.User.ToArray();
            return Ok(user) ;
        }

        // Add Consumer
        [HttpPost("addConsumer")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddConsumer([FromBody] ConsumerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdbyUsername = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var (success, consumer) = await _userServices.AddConsumerAsync(dto, createdbyUsername);

            if (!success)
                return BadRequest("Consumer already exist.");

            if (consumer == null)
                return BadRequest("Failed to add consumer");

            return Ok(new
            {
                message = "Consumer added successfully",
                consumerId = consumer.Consumerid
            });
        }

        // update password of a user
        [HttpPut("updatePassword/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDto passwordDto)
        {
            try
            {
                var update = await _userServices.UpdateUser(id, passwordDto);
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
