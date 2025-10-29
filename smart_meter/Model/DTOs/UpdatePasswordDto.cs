namespace smart_meter.Model.DTOs
{
    public class UpdatePasswordDto
    {
        public string oldPassword {  get; set; }
        public string NewPassword { get; set; }
        public string ConfirmePassword { get; set; }
    }
}
