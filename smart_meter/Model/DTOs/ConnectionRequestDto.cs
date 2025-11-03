namespace smart_meter.Model.DTOs
{
    public class ConnectionRequestDto
    {
        public long? ConsumerId { get; set; } // null for new connection
        public string RequestType { get; set; } = null!; // "new_connection", "connect", "disconnect"
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? Remarks { get; set; }
    }
}

