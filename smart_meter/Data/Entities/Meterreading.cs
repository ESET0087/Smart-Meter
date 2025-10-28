using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("meterreadings")]
public partial class Meterreading
{
    [Key]
    [Column("readingid")]
    public long Readingid { get; set; }

    [Column("meterserialno")]
    [StringLength(50)]
    public string Meterserialno { get; set; } = null!;

    [Column("readingdatetime")]
    public DateTime Readingdatetime { get; set; }

    [Column("energyconsumed")]
    [Precision(10, 3)]
    public decimal Energyconsumed { get; set; }

    [Column("voltage")]
    [Precision(7, 2)]
    public decimal Voltage { get; set; }

    [Column("current")]
    [Precision(7, 3)]
    public decimal Current { get; set; }

    [ForeignKey("Meterserialno")]
    [InverseProperty("Meterreadings")]
    public virtual Meter MeterserialnoNavigation { get; set; } = null!;
}
