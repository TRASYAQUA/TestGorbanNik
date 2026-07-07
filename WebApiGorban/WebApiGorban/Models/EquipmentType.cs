using System;
using System.Collections.Generic;

namespace WebApiGorban.Models;

public partial class EquipmentType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public string? CreatedBy { get; set; }

    public string? UpdatedBy { get; set; }

    public virtual ICollection<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
}
