
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
        public readonly BillService _billservice;
        public readonly RabbitMqServices _rabbitmqservice;

        public ConsumerController(ConsumerService consumerService, BillService billService, RabbitMqServices rabbitmqservice)
        {
            _consumerService = consumerService;
            _billservice = billService;
            _rabbitmqservice = rabbitmqservice;
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

        // previous bills
        [HttpGet("PreviousBills/{consumerId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPreviousBills(int consumerId)
        {
            var bills = await _billservice.GetPreviousBillsAsync(consumerId);
            if (bills == null ||!bills.Any())
            {
                return NotFound(new { Message = "No bills found for the specified consumer." });
            }
            return Ok(bills);
        }

        [HttpPost("requestconnection")]
        public async Task<IActionResult> requestconnection([FromBody] ConsumerCreateDto consumerdto)
        {
            var exchange = "Connection";
            var queue = "Requestconnection";

            //var message = new
            //{
            //    Name = consumerdto.Name,
            //    Phone= consumerdto.Phone,
            //    Email = consumerdto.Email,
            //    userId = consumerdto.userid,
            //    Address = consumerdto.Address,
            //    OrgUnit = consumerdto.Orgunitid,
            //    TarrifId = consumerdto.Tariffid

            //};
            var sent = await _rabbitmqservice.sendMesageDirectExchange(exchange, queue,"requestconnection", consumerdto);
            
            return Ok(sent);
        }

        //[HttpPost("getrequestconnection")]
        //public async Task<IActionResult> getrequestconnection()
        //{

        //    var exchange = "Connection";
        //    var queue = "Requestconnection";

        //    var channel = _rabbitmqservice._connectionFactory()
        //}
    }
}