namespace GorbanWPFTest.Models
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Code { get; set; }

        public List<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }
}