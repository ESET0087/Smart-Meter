namespace smart_meter.Model.DTOs
{
    public class RegisterRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
