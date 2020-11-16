using Models.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DAO
{
    public class StateDao
    {
        public ShoppingDbContext db;
        public StateDao()
        {
            db = new ShoppingDbContext();
        }

        public State getByName(string StateName)
        {
            return db.States.Where(state => state.StateName.Equals(StateName)).SingleOrDefault();
        }

        public State getById(long? Id)
        {
            return db.States.Where(state => state.Id == Id).SingleOrDefault();
        }        
        
        public IEnumerable<State> listAllPaging(string searchStateName, string sorting, 
                                                                int searchPageSize, int searchPage)
        {
            var sqlLinq = from state in db.States
                          select state;
            if(!string.IsNullOrEmpty(searchStateName))
            {
                sqlLinq = sqlLinq.Where(state => state.StateName.Contains(searchStateName));
            }            

            if (sorting.Equals("asc"))
            {
                return sqlLinq.OrderBy(state => state.StateName).ToPagedList(searchPage, searchPageSize);
            }

            return sqlLinq.OrderByDescending(state => state.StateName).ToPagedList(searchPage, searchPageSize);
        }

        public int totalRows(string searchStateName)
        {
            var sqlLinq = from state in db.States
                          select state;
            if (!string.IsNullOrEmpty(searchStateName))
            {
                sqlLinq = sqlLinq.Where(state => state.StateName.Contains(searchStateName));
            }

            return sqlLinq.ToList().Count;
        }

        public long create(State state)
        {
            db.States.Add(state);
            db.SaveChanges();
            return state.Id;
        }

        public bool update(State entity)
        {
            try
            {
                var state = db.States.Find(entity.Id);
                if (state != null)
                {
                    state.StateName = entity.StateName;
                    state.StateDescription = entity.StateDescription;
                    state.ModifiedDate = entity.ModifiedDate;
                    state.ModifiedBy = entity.ModifiedBy;
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

        public bool delete(long? id)
        {
            try
            {
                var state = db.States.Find(id);
                db.States.Remove(state);
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
