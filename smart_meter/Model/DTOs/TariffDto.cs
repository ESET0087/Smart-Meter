namespace smart_meter.Model.DTOs
{
    public class TariffDto
    {
        public string Name { get; set; } = null!;
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public decimal BaseRate { get; set; }
        public decimal TaxRate { get; set; }

    }
}
