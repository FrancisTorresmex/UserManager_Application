
using userManagerApplication.Repository.Interfaces;
using userManagerAplication.Models.Data;
using userManagerApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace userManagerApplication.Repository.Entities
{
    public class UsersRepository : GenericRepository<User>, IUsersRepository<User>
    {
        private UserManagerAplicationContext _context;
        private DbSet<User> _dbSet;

        public UsersRepository(UserManagerAplicationContext context) : base(context)
        {
            _context = context;
            _dbSet = context.Set<User>();
        }

        public UserModel GetUserAndRol(int idUser)
        {
            UserModel userModel = null;
            var user = _dbSet.Include(u => u.IdRoleNavigation).FirstOrDefault(u => u.IdUser == idUser);

            if (user != null)
            {
                userModel = new UserModel
                {
                    IdUser = user.IdUser,
                    Email = user.Email,
                    LastName = user.LastName,
                    Name = user.Name,
                    Phone = user.Phone == null ? "No phone" : user.Phone,
                    RoleName = user.IdRoleNavigation == null ? "No role" : user.IdRoleNavigation.Name,
                    StatusName = user.Status == true ? "Active" : "Inactive",
                    DateAdmision = user.DateAdmision,
                    InactiveDate = user.InactiveDate,
                };
            }            

            return userModel;
        }

        public List<UserModel> GetAllUserAndRoles()
        {
            var users = _dbSet.Select(x => new UserModel
            {
                IdUser = x.IdUser,
                Email = x.Email,
                LastName = x.LastName,
                Name = x.Name,
                Phone = x.Phone == null ? "No phone" : x.Phone,
                RoleName = x.IdRoleNavigation == null ? "No role" : x.IdRoleNavigation.Name,
                StatusName = x.Status == true ? "Active" : "Inactive",
                DateAdmision = x.DateAdmision,
                InactiveDate = x.InactiveDate
            }).ToList();

            return users;
        }

        public UserModel FindByEmail(string email)
        {
            UserModel userModel = null;
            var user = _dbSet.Include(u => u.IdRoleNavigation).FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                userModel = new UserModel
                {
                    IdUser = user.IdUser,
                    Password = user.Password,
                    Email = user.Email,
                    LastName = user.LastName,
                    Name = user.Name,
                    Phone = user.Phone == null ? "No phone" : user.Phone,
                    IdRole = user.IdRole,
                    RoleName = user.IdRoleNavigation == null ? "No role" : user.IdRoleNavigation.Name,
                    Status = (bool)(user.Status == null ? false : user.Status),
                    StatusName = user.Status == true ? "Active" : "Inactive",
                    DateAdmision = user.DateAdmision,
                    InactiveDate = user.InactiveDate,
                };
            }

            return userModel;
        }

        //activate and deactivate user
        public bool DeactivateUser(int idUser, string isActivate)
        {
            var user = _dbSet.Find(idUser);
            if (user != null)
            {
                if (isActivate == "Inactive")
                {
                    user.Status = false;
                    user.InactiveDate = DateTime.Now;

                }
                else
                    user.Status = true;

            }
            else
                throw new Exception("User not found");

            return true;
        }

    }
   
}
