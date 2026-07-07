namespace GorbanWPFTest.Models
{
    public class EquipmentItem : BaseEntity
    {
        public Guid ManufacturerId { get; set; }
        public Guid EquipmentTypeId { get; set; }
        public Guid CountryId { get; set; }
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? PhotoUrl { get; set; }
        public byte[]? PhotoData { get; set; }

        public Country? Country { get; set; }
        public EquipmentType? EquipmentType { get; set; }
        public Manufacturer? Manufacturer { get; set; }
    }
}