using Ecommerce.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.IBL
{
    public interface IUserRepository
    {
        public Task<IEnumerable<UserM>> GetAllUsers();

        public Task<UserM> GetUserByUserId(int? userId);
        public Task<UserM> CheckUsernameAndPassword(UserM userM);
        public Task<int> AddUser(UserM user);
        public Task<int> UpdateUser(UserM user);
        public Task<int> DeleteUser(int userId);
        public bool UserExists(int id);
    }
}
