

using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;


namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsumerController : ControllerBase
    {
        private readonly ConsumerService _consumerService;

        public ConsumerController(ConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        // Add Consumer
        [HttpPost("add")]
        public async Task<IActionResult> AddConsumer([FromBody] ConsumerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var consumer = await _consumerService.AddConsumerAsync(dto);

            if (consumer == null)
                return BadRequest("Failed to add consumer.");

            return Ok(new
            {
                message = "Consumer added successfully",
                consumerId = consumer.Consumerid
            });
        }

        // Upload Consumer Photo
        [HttpPost("upload-photo")]
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
    }
}
