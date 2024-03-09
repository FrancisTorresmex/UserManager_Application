
using userManagerAplication.Models.Data;
using userManagerApplication.Models;


namespace userManagerApplication.Repository.Interfaces
{
    public interface IUsersRepository<User>: IGenericRepository<User>
    {
        List<UserModel> GetAllUserAndRoles();
        UserModel GetUserAndRol(int idUser);
        UserModel FindByEmail(string email);
        bool DeactivateUser(int idUser, string isActivate);        
    }
}
