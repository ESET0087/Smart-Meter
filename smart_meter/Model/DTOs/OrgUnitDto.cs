namespace smart_meter.Model.DTOs
{
    public class OrgUnitDto
    {
        public string Type { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int? Parentid { get; set; }
    }
}
