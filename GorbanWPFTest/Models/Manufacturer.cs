namespace GorbanWPFTest.Models
{
    public class Manufacturer : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }
}