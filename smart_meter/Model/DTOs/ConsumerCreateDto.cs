using System.ComponentModel.DataAnnotations;

namespace smart_meter.Model.DTOs
{
    public class ConsumerCreateDto
    {
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = null!;

        //[StringLength(500)]
        //public string? Address { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [StringLength(200)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Orgunitid { get; set; }

        [Required]
        public int Tariffid { get; set; }

    }
}
