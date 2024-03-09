using System;
using System.Collections.Generic;

namespace userManagerAplication.Models.Data;

public partial class UsersRole
{
    public int IdRole { get; set; }

    public string? Name { get; set; }

    public bool? Status { get; set; }

    public DateTime? CreationDate { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
