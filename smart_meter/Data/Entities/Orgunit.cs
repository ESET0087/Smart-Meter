using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("orgunit")]
[Index("Type", Name = "idx_orgunit_type")]
public partial class Orgunit
{
    [Key]
    [Column("orgunitid")]
    public int Orgunitid { get; set; }

    [Column("type")]
    [StringLength(20)]
    public string Type { get; set; } = null!;

    [Column("name")]
    [StringLength(100)]
    public string Name { get; set; } = null!;

    [Column("parentid")]
    public int? Parentid { get; set; }

    [InverseProperty("Orgunit")]
    public virtual ICollection<Consumer> Consumers { get; set; } = new List<Consumer>();

    [InverseProperty("Parent")]
    public virtual ICollection<Orgunit> InverseParent { get; set; } = new List<Orgunit>();

    [ForeignKey("Parentid")]
    [InverseProperty("InverseParent")]
    public virtual Orgunit? Parent { get; set; }
}
