using System.ComponentModel.DataAnnotations;
using userManagerAplication.Models.Data;

namespace userManagerApplication.Models
{
    public class UserModel
    {        
        public int IdUser { get; set; }
        [Required(ErrorMessage = "Name is required")]
        public string? Name { get; set; }
        [Required(ErrorMessage = "Last name is required")]
        public string? LastName { get; set; }
        
        public string? Email { get; set; }
        
        public string? Phone { get; set; }
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? StatusName { get; set; }

        public bool Status { get; set; }

        public DateTime? DateAdmision { get; set; }

        public DateTime? InactiveDate { get; set; }
        [Required(ErrorMessage = "Role is required")]
        public int? IdRole { get; set; }
        public string? RoleName { get; set; }
        public List<UsersRole> AllRoleLst { get; set; }
    }
}
