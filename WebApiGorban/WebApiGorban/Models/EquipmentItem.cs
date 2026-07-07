using System;
using System.Collections.Generic;

namespace WebApiGorban.Models;

public partial class EquipmentItem
{
    public Guid Id { get; set; }

    public Guid ManufacturerId { get; set; }

    public Guid EquipmentTypeId { get; set; }

    public Guid CountryId { get; set; }

    public string Model { get; set; } = null!;

    public decimal Price { get; set; }

    public string? PhotoUrl { get; set; }

    public byte[]? PhotoData { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual Country Country { get; set; } = null!;

    public virtual EquipmentType EquipmentType { get; set; } = null!;

    public virtual Manufacturer Manufacturer { get; set; } = null!;
}
