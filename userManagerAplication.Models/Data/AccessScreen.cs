using System;
using System.Collections.Generic;

namespace userManagerAplication.Models.Data;

public partial class AccessScreen
{
    public int? IdUser { get; set; }

    public int? IdScreen { get; set; }

    public int Id { get; set; }

    public virtual Screen? IdScreenNavigation { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
