using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace smart_meter.Data.Entities;

[Table("bill")]
public partial class Bill
{
    [Key]
    [Column("billid")]
    public long Billid { get; set; }

    [Column("consumerid")]
    public long Consumerid { get; set; }

    [Column("meterserialno")]
    [StringLength(50)]
    public string Meterserialno { get; set; } = null!;

    [Column("billingperiodstart")]
    public DateOnly Billingperiodstart { get; set; }

    [Column("billingperiodend")]
    public DateOnly Billingperiodend { get; set; }

    [Column("totalunitsconsumed")]
    [Precision(18, 6)]
    public decimal Totalunitsconsumed { get; set; }

    [Column("baseamount")]
    [Precision(18, 4)]
    public decimal Baseamount { get; set; }

    [Column("taxamount")]
    [Precision(18, 4)]
    public decimal Taxamount { get; set; }

    [Column("totalamount")]
    [Precision(18, 4)]
    public decimal? Totalamount { get; set; }

    [Column("generatedat")]
    public DateTime Generatedat { get; set; }

    [Column("paymentdate")]
    public DateOnly? Paymentdate { get; set; }

    [Column("duedate")]
    public DateOnly Duedate { get; set; }

    [Column("ispaid")]
    public bool? Ispaid { get; set; }

    [Column("discdate")]
    public DateTime? Discdate { get; set; }

    [InverseProperty("Bill")]
    public virtual Arrear? Arrear { get; set; }

    [ForeignKey("Consumerid")]
    [InverseProperty("Bills")]
    public virtual Consumer Consumer { get; set; } = null!;

    [ForeignKey("Meterserialno")]
    [InverseProperty("Bills")]
    public virtual Meter MeterserialnoNavigation { get; set; } = null!;
}
