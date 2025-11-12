using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace smart_meter.Data.Entities;

[Table("consumer")]
public partial class Consumer
{
    [Key]
    [Column("consumerid")]
    public long Consumerid { get; set; }

    [Column("name")]
    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column("photo")]
    [StringLength(300)]
    public string? Photo { get; set; }


    [Column("address")]
    [StringLength(500)]
    public string? Address { get; set; }

    [Column("phone")]
    [StringLength(30)]

    public string? Phone { get; set; }



    [Column("email")]
    [StringLength(200)]
    public string? Email { get; set; }

    [Column("orgunitid")]
    public int Orgunitid { get; set; }

    [Column("tariffid")]
    public int Tariffid { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("createdat")]
    public DateTime Createdat { get; set; }

    [Column("createdby")]
    [StringLength(100)]
    public string Createdby { get; set; } = null!;

    [Column("updatedat")]
    public DateTime? Updatedat { get; set; }

    [Column("updatedby")]
    [StringLength(100)]
    public string? Updatedby { get; set; }

    [Column("isdeleted")]
    public bool? Isdeleted { get; set; }

    [InverseProperty("Consumer")]
    public virtual ICollection<Arrear> Arrears { get; set; } = new List<Arrear>();

    [InverseProperty("Consumer")]
    public virtual ICollection<Bill> Bills { get; set; } = new List<Bill>();

    [InverseProperty("Consumer")]
    public virtual Consumeraddress? Consumeraddress { get; set; }

    [InverseProperty("Consumer")]
    public virtual ICollection<Meter> Meters { get; set; } = new List<Meter>();

    [ForeignKey("Orgunitid")]
    [InverseProperty("Consumers")]
    public virtual Orgunit Orgunit { get; set; } = null!;

    [ForeignKey("Tariffid")]
    [InverseProperty("Consumers")]
    public virtual Tariff Tariff { get; set; } = null!;

   
}