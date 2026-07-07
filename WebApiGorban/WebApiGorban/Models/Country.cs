using System;
using System.Collections.Generic;

namespace WebApiGorban.Models;

public partial class Country
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Code { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual ICollection<EquipmentItem> EquipmentItems { get; set; } = new List<EquipmentItem>();
}
