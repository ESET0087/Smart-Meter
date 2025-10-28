using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("arrears")]
[Index("Billid", Name = "arrears_billid_key", IsUnique = true)]
public partial class Arrear
{
    [Key]
    [Column("arrearid")]
    public long Arrearid { get; set; }

    [Column("consumerid")]
    public long Consumerid { get; set; }

    [Column("billid")]
    public long Billid { get; set; }

    [Column("arreartype")]
    [StringLength(20)]
    public string? Arreartype { get; set; }

    [Column("paidstatus")]
    [StringLength(20)]
    public string Paidstatus { get; set; } = null!;

    [ForeignKey("Billid")]
    [InverseProperty("Arrear")]
    public virtual Bill Bill { get; set; } = null!;

    [ForeignKey("Consumerid")]
    [InverseProperty("Arrears")]
    public virtual Consumer Consumer { get; set; } = null!;
}
