using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("tarrifdetails")]
public partial class Tarrifdetail
{
    [Key]
    [Column("tarrifdetailid")]
    public int Tarrifdetailid { get; set; }

    [Column("tarrifid")]
    public int Tarrifid { get; set; }

    [Column("tariffslabid")]
    public int Tariffslabid { get; set; }

    [Column("todruleid")]
    public int Todruleid { get; set; }

    [ForeignKey("Tariffslabid")]
    [InverseProperty("Tarrifdetails")]
    public virtual Tariffslab Tariffslab { get; set; } = null!;

    [ForeignKey("Tarrifid")]
    [InverseProperty("Tarrifdetails")]
    public virtual Tariff Tarrif { get; set; } = null!;

    [ForeignKey("Todruleid")]
    [InverseProperty("Tarrifdetails")]
    public virtual Todrule Todrule { get; set; } = null!;
}
