using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class UserDao
    {
        ShoppingDbContext db;

        public UserDao()
        {
            db = new ShoppingDbContext();
        }
        
        public User getUserByEmail(string Email)
        {
            return db.Users.Where(u => u.Email.Equals(Email)).SingleOrDefault();
        }

        public User getUserByUsername(string Username)
        {
            return db.Users.Where(u => u.UserName.Equals(Username)).FirstOrDefault();
        }

        public IEnumerable<User> listAllUsers()
        {
            return db.Users.ToList();
        }

        public long CreateUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        } 
    }
}
