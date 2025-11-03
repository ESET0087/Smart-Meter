using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConsumerController : ControllerBase
    {
        private readonly ConsumerService _consumerService;
        public readonly BillService _billservice;

        public ConsumerController(ConsumerService consumerService, BillService billService)
        {
            _consumerService = consumerService;
            _billservice = billService;
        }

        // Upload Consumer Photo
        [HttpPost("upload-photo")]
        [Authorize(Roles = "Consumer")]
        public async Task<IActionResult> UploadPhoto([FromForm] ConsumerPhotoUploadDto dto)
        {
            var photoUrl = await _consumerService.UploadPhotoAsync(dto);

            if (string.IsNullOrEmpty(photoUrl))
                return BadRequest("Failed to upload photo. Consumer not found or invalid file.");

            return Ok(new
            {
                message = "Photo uploaded successfully",
                photoUrl = photoUrl
            });
        }

        // previous bills
        [HttpGet("PreviousBills/{consumerId}")]
        [Authorize(Roles = "User, Consumer")]
        public async Task<ActionResult<IEnumerable<object>>> GetPreviousBills(int consumerId)
        {
            var bills = await _billservice.GetPreviousBillsAsync(consumerId);
            if (bills == null ||!bills.Any())
            {
                return NotFound(new { Message = "No bills found for the specified consumer." });
            }
            return Ok(bills);
        }
    }
}