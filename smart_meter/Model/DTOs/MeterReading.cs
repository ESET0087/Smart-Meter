using System;
using System.ComponentModel.DataAnnotations;

namespace smart_meter.Model.DTOs
{
    // DTO to receive meter reading data from client
    public class MeterReading
    {
        [Required]
        [MaxLength(50)]
        public string Meterserialno { get; set; } = null!; // Meter serial number

        [Required]
        public DateTime Readingdatetime { get; set; } // Date and time of the reading

        [Required]
        [Range(0, 999999)]
        public decimal Energyconsumed { get; set; } // Energy consumed in kWh

        [Required]
        [Range(0, 9999)]
        public decimal Voltage { get; set; } // Voltage recorded

        [Required]
        [Range(0, 9999)]
        public decimal Current { get; set; } // Current recorded
    }
}

