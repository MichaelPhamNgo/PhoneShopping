using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class ContactDao
    {
        public ShoppingDbContext db;
        public ContactDao()
        {
            db = new ShoppingDbContext();
        }        

        public Contact getById(string Id)
        {
            return db.Contacts.Where(contact => contact.Id.Equals(Id)).SingleOrDefault();
        }
    }
}
