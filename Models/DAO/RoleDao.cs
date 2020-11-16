using Models.EF;
using Models.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class RoleDao
    {
        public ShoppingDbContext db;
        public RoleDao()
        {
            db = new ShoppingDbContext();
        }

        public Role getByName(string RoleName)
        {
            return db.Roles.Where(tag => tag.Name.Equals(RoleName)).SingleOrDefault();
        }

        public Role getById(string Id)
        {
            return db.Roles.Where(role => role.Id.ToString().Equals(Id)).SingleOrDefault();
        }
        
        public IEnumerable<Role> listAllPaging(string searchRoleName, string sorting, int searchPageSize, int searchPage)
        {            
            var sqlLinq = from role in db.Roles                          
                          select role;
            if(!string.IsNullOrEmpty(searchRoleName))
            {
                sqlLinq = sqlLinq.Where(role => role.Name.Contains(searchRoleName));
            }            

            if (sorting.Equals("asc"))
            {
                return sqlLinq.OrderBy(role => role.Name).ToPagedList(searchPage, searchPageSize);
            }                            
            return sqlLinq.OrderByDescending(role => role.Name).ToPagedList(searchPage, searchPageSize);

        }
        
        public int totalRows(string searchRoleName)
        {
            var sqlLinq = from role in db.Roles                          
                          select role;
            if (!string.IsNullOrEmpty(searchRoleName))
            {
                sqlLinq = sqlLinq.Where(role => role.Name.Contains(searchRoleName));
            }
            return sqlLinq.ToList().Count;
        }

        public Guid create(Role role)
        {
            db.Roles.Add(role);
            db.SaveChanges();
            return role.Id;
        }

        public bool update(Role entity)
        {
            try
            {
                var role = db.Roles.Find(entity.Id);
                if (role != null)
                {
                    role.Name = entity.Name;                    
                    db.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool delete(string id)
        {
            try
            {
                var role = db.Roles.Find(Guid.Parse(id));
                db.Roles.Remove(role);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }        
    }
}
