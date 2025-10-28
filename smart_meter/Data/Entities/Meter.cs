using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("meter")]
public partial class Meter
{
    [Key]
    [Column("meterserialno")]
    [StringLength(50)]
    public string Meterserialno { get; set; } = null!;

    [Column("ipaddress")]
    [StringLength(45)]
    public string Ipaddress { get; set; } = null!;

    [Column("iccid")]
    [StringLength(30)]
    public string Iccid { get; set; } = null!;

    [Column("imsi")]
    [StringLength(30)]
    public string Imsi { get; set; } = null!;

    [Column("manufacturer")]
    [StringLength(100)]
    public string Manufacturer { get; set; } = null!;

    [Column("firmware")]
    [StringLength(50)]
    public string? Firmware { get; set; }

    [Column("category")]
    [StringLength(50)]
    public string Category { get; set; } = null!;

    [Column("installtsutc")]
    public DateTime Installtsutc { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("consumerid")]
    public long? Consumerid { get; set; }

    [InverseProperty("MeterserialnoNavigation")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [ForeignKey("Consumerid")]
    [InverseProperty("Meters")]
    public virtual Consumer? Consumer { get; set; }

    [InverseProperty("MeterserialnoNavigation")]
    public virtual ICollection<Meterreading> Meterreadings { get; set; } = new List<Meterreading>();
}
