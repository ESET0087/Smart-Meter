namespace smart_meter.Model.DTOs
{
    public class TariffUpdateRequest
    {
        public string? Name { get; set; }
        public DateOnly? EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public decimal? BaseRate { get; set; }
        public decimal? TaxRate { get; set; }
    }
    public class TodRuleUpdateRequest
    {
        public string? Name { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public decimal? RatePerKwh { get; set; }
    }
    public class TariffSlabUpdateRequest
    {
        public decimal? FromKwh { get; set; }
        public decimal? ToKwh { get; set; }
        public decimal? RatePerKwh { get; set; }
    }
}
