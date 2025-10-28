using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("tariffslab")]
public partial class Tariffslab
{
    [Key]
    [Column("tariffslabid")]
    public int Tariffslabid { get; set; }

    [Column("tariffid")]
    public int Tariffid { get; set; }

    [Column("fromkwh")]
    [Precision(18, 6)]
    public decimal Fromkwh { get; set; }

    [Column("tokwh")]
    [Precision(18, 6)]
    public decimal Tokwh { get; set; }

    [Column("rateperkwh")]
    [Precision(18, 6)]
    public decimal Rateperkwh { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [ForeignKey("Tariffid")]
    [InverseProperty("Tariffslabs")]
    public virtual Tariff Tariff { get; set; } = null!;

    [InverseProperty("Tariffslab")]
    public virtual ICollection<Tarrifdetail> Tarrifdetails { get; set; } = new List<Tarrifdetail>();
}
