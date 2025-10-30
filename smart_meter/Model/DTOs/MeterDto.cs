
using System.ComponentModel.DataAnnotations;

namespace smart_meter.Model.DTOs
{
    public class MeterCreateDto
    {
        [Required]
        [MaxLength(50)]
        public string Meterserialno { get; set; } = null!;

        [Required]
        [MaxLength(45)]
        public string Ipaddress { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Iccid { get; set; } = null!;

        [Required]
        [MaxLength(30)]
        public string Imsi { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Manufacturer { get; set; } = null!;

        [MaxLength(50)]
        public string? Firmware { get; set; }

        [Required]
        [MaxLength(50)]
        public string Category { get; set; } = null!;

        [Required]
        public DateTime Installtsutc { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Active";

        public long? Consumerid { get; set; }
    }
}

