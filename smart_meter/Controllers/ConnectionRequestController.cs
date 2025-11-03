using Microsoft.AspNetCore.Mvc;
using smart_meter.Model.DTOs;
using smart_meter.Services;
using System.Threading.Tasks;

namespace smart_meter.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConnectionRequestController : ControllerBase
    {
        private readonly ConnectionRequestService _service;

        public ConnectionRequestController(ConnectionRequestService service)
        {
            _service = service;
        }

        // POST: api/connectionrequest
        [HttpPost]
        public async Task<IActionResult> CreateRequest([FromBody] ConnectionRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.CreateRequestAsync(dto);
            return Ok(new { message = "Request submitted successfully", data = result });
        }

        // GET: api/connectionrequest
        [HttpGet]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _service.GetAllRequestsAsync();
            return Ok(requests);
        }

        // GET: api/connectionrequest/consumer/5
        [HttpGet("consumer/{consumerId}")]
        public async Task<IActionResult> GetByConsumer(long consumerId)
        {
            var requests = await _service.GetRequestsByConsumerIdAsync(consumerId);
            return Ok(requests);
        }

        // PUT: api/connectionrequest/update/1
        [HttpPut("update/{requestId}")]
        public async Task<IActionResult> UpdateStatus(long requestId, [FromQuery] string status, [FromQuery] long userId, [FromBody] string? remarks = null)
        {
            var updated = await _service.UpdateRequestStatusAsync(requestId, status, userId, remarks);
            if (!updated)
                return NotFound(new { message = "Request not found" });

            return Ok(new { message = $"Request status updated to {status}" });
        }
    }
}
