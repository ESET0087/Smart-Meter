using System.ComponentModel.DataAnnotations;

namespace smart_meter.Model.DTOs
{
    public class ConsumerPhotoUploadDto
    {
        [Required]
        public long ConsumerId { get; set; }

        [Required(ErrorMessage = "Please upload a photo.")]
        public IFormFile File { get; set; } = null!;
    }
}




