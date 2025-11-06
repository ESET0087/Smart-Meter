using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("tariff")]
[Index("Effectivefrom", Name = "idx_tariff_effectivefrom")]
public partial class Tariff
{

    
    [Key]
    [Column("tariffid")]
    public int Tariffid { get; set; }

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("effectivefrom")]
    public DateOnly Effectivefrom { get; set; }

    [Column("effectiveto")]
    public DateOnly? Effectiveto { get; set; }

    [Column("baserate")]
    [Precision(18, 4)]
    public decimal Baserate { get; set; }

    [Column("taxrate")]
    [Precision(18, 4)]
    public decimal Taxrate { get; set; }

    [InverseProperty("Tariff")]
    public virtual ICollection<Consumer> Consumers { get; set; } = new List<Consumer>();

    [InverseProperty("Tariff")]
    public virtual ICollection<Tariffslab> Tariffslabs { get; set; } = new List<Tariffslab>();

    [InverseProperty("Tarrif")]
    public virtual ICollection<Tarrifdetail> Tarrifdetails { get; set; } = new List<Tarrifdetail>();

    [InverseProperty("Tariff")]
    public virtual ICollection<Todrule> Todrules { get; set; } = new List<Todrule>();
}
