using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("todrule")]
[Index("Name", Name = "idx_tod_name")]
public partial class Todrule
{
    [Key]
    [Column("todruleid")]
    public int Todruleid { get; set; }

    [Column("tariffid")]
    public int Tariffid { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    [Column("starttime")]
    [Precision(0, 0)]
    public TimeOnly Starttime { get; set; }

    [Column("endtime")]
    [Precision(0, 0)]
    public TimeOnly Endtime { get; set; }

    [Column("rateperkwh")]
    [Precision(18, 6)]
    public decimal Rateperkwh { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Tariffid")]
    [InverseProperty("Todrules")]
    public virtual Tariff Tariff { get; set; } = null!;

    [InverseProperty("Todrule")]
    public virtual ICollection<Tarrifdetail> Tarrifdetails { get; set; } = new List<Tarrifdetail>();
}
