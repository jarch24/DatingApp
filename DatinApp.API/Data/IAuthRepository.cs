using System.Threading.Tasks;
using DatinApp.API.Models;

namespace DatinApp.API.Data
{
    public interface IAuthRepository
    {
         
         Task<bool> UserExists(string username);
         Task<User> Login(string username, string password);
         Task<User> Register(User user, string password);
    }
}