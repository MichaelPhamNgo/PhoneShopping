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

        public Guid createUser(User user)
        {
            db.Users.Add(user);
            db.SaveChanges();
            return user.Id;
        }

        public void resetCountAttemptedLogin(Guid UserId, int countAttempt, DateTimeOffset nextDateTime, bool lockAttempt)
        {
            try
            {
                var user = db.Users.Find(UserId);
                if (user != null)
                {                    
                    user.AccessFailedCount = countAttempt;
                    user.LockoutEnd = nextDateTime;
                    user.LockoutEnabled = lockAttempt;                    
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }


        public void countUserAttempt(Guid UserId, int numberAttempt)
        {
            try
            {
                var user = db.Users.Find(UserId);
                if (user != null)
                {
                    user.AccessFailedCount = numberAttempt;                    
                    db.SaveChanges();                    
                }                
            }
            catch (Exception ex)
            {
                
            }
        }

        public void disableAccount(Guid UserId, int numberAttempt, bool disable = false)
        {
            try
            {
                var user = db.Users.Find(UserId);
                if (user != null)
                {
                    user.AccessFailedCount = numberAttempt;
                    user.LockoutEnabled = disable;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
