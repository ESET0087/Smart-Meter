namespace smart_meter.Model.DTOs
{
    public class BillDto
    {
        public int Billid { get; set; }
        public long Consumerid { get; set; }
        public string Meterserialno { get; set; } = string.Empty;
        public DateOnly Billingperiodstart { get; set; }
        public DateOnly Billingperiodend { get; set; }
        public decimal Totalunitsconsumed { get; set; }
        public decimal Baseamount { get; set; }
        public decimal Taxamount { get; set; }
        public decimal Totalamount { get; set; }
        public DateTime Generatedat { get; set; }
        public DateOnly Duedate { get; set; }
        public bool? Ispaid { get; set; }
    }
}
