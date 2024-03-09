using System;
using System.Collections.Generic;

namespace userManagerAplication.Models.Data;

public partial class User
{
    public int IdUser { get; set; }

    public string? Name { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Password { get; set; }

    public bool? Status { get; set; }

    public DateTime? DateAdmision { get; set; }

    public DateTime? InactiveDate { get; set; }

    public int? IdRole { get; set; }

    public virtual ICollection<AccessScreen> AccessScreens { get; set; } = new List<AccessScreen>();

    public virtual UsersRole? IdRoleNavigation { get; set; }
}
