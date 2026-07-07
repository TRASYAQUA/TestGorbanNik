namespace GorbanWPFTest.Models
{
    public class EquipmentType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
    }
}