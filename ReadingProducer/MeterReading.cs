using System;
using System.ComponentModel.DataAnnotations;

namespace Producer.Models
{
    public class MeterReading
    {
        [Required]
        [MaxLength(50)]
        public string Meterserialno { get; set; } = null!;

        [Required]
        public DateTime Readingdatetime { get; set; }

        [Required]
        public decimal Energyconsumed { get; set; }

        [Required]
        public decimal Voltage { get; set; }

        [Required]
        public decimal Current { get; set; }
    }
}