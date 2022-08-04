using Ecommerce.Data;
using Ecommerce.IBL;
using Ecommerce.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.BL
{
    public class UserRepository:IUserRepository
    {
        private readonly EcommerceDbContext db;
        public UserRepository(EcommerceDbContext _db)
        {
            db = _db;
        }

        public async Task<int> AddUser(UserM user)
        {
            db.Users.Add(user);
            return await db.SaveChangesAsync();
        }

        public async Task<UserM> CheckUsernameAndPassword(UserM userM)
        {
            return await db.Users.Where(i => i.Username == userM.Username && i.Password == userM.Password && i.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<int> DeleteUser(int userId)
        {
            var user = db.Users.Where(x => x.UserId == userId).FirstOrDefault();

            if (user != null)
            {
                user.IsDeleted = true;
                db.Users.Update(user);
            }

            return await db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserM>> GetAllUsers()
        {
            return await db.Users.Where(i => i.IsDeleted == false).ToListAsync();
        }

        public async Task<UserM> GetUserByUserId(int? userId)
        {
            return await db.Users.Where(i => i.IsDeleted == false).FirstOrDefaultAsync(i=>i.UserId == userId);
        }

        public async Task<int> UpdateUser(UserM user)
        {
            db.Users.Update(user);

            return await db.SaveChangesAsync();
        }

        public bool UserExists(int id)
        {
            return (db.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
