using System.ComponentModel.DataAnnotations;

namespace smart_meter.Model.DTOs
{
    public class EnergyConsumtionMeasurment
    {
        [Required]
        public string meterserialno { get; set; }
        public DateTime Datefrom {  get; set; }
        public DateTime Dateto { get; set; }
    }
}
