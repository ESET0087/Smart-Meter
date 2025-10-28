using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("consumeraddress")]
[Index("Consumerid", Name = "consumeraddress_consumerid_key", IsUnique = true)]
public partial class Consumeraddress
{
    [Key]
    [Column("addressid")]
    public long Addressid { get; set; }

    [Column("consumerid")]
    public long Consumerid { get; set; }

    [Column("houseno")]
    [StringLength(100)]
    public string Houseno { get; set; } = null!;

    [Column("city")]
    [StringLength(100)]
    public string City { get; set; } = null!;

    [Column("state")]
    [StringLength(100)]
    public string State { get; set; } = null!;

    [Column("pincode")]
    [StringLength(20)]
    public string Pincode { get; set; } = null!;

    [ForeignKey("Consumerid")]
    [InverseProperty("Consumeraddress")]
    public virtual Consumer Consumer { get; set; } = null!;
}
