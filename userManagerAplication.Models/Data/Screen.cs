using System;
using System.Collections.Generic;

namespace userManagerAplication.Models.Data;

public partial class Screen
{
    public int IdScreen { get; set; }

    public string? Name { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreationDate { get; set; }

    public string? Url { get; set; }

    public virtual ICollection<AccessScreen> AccessScreens { get; set; } = new List<AccessScreen>();
}
