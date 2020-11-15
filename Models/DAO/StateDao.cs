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
    public class StateDao
    {
        public ShoppingDbContext db;
        public StateDao()
        {
            db = new ShoppingDbContext();
        }

        public State getStateByName(string StateName)
        {
            return db.States.Where(state => state.StateName.Equals(StateName)).SingleOrDefault();
        }

        public State GetById(long? Id)
        {
            return db.States.Where(state => state.Id == Id).SingleOrDefault();
        }

        public StateModel GetJoinUserById(long? Id)
        {            
            var sql = db.States.Where(state => state.Id == Id).SingleOrDefault();
            if (sql.ModifiedBy == null)
            {
                return (from state in db.States
                        join creator in db.Users
                        on state.CreatedBy equals creator.Id
                        select new StateModel
                        {
                            Id = state.Id,
                            StateName = state.StateName,
                            StateDescription = state.StateDescription,
                            CreatedDate = state.CreatedDate,
                            Creator = creator.UserName,
                            ModifiedDate = null,
                            Modifier = null,
                            Status = state.Status
                        }).Where(state => state.Id == Id).FirstOrDefault();
            } 
            else
            {
                return (from state in db.States
                        join creator in db.Users
                        on state.CreatedBy equals creator.Id
                        join modifier in db.Users
                        on state.ModifiedBy equals modifier.Id
                        select new StateModel
                        {
                            Id = state.Id,
                            StateName = state.StateName,
                            StateDescription = state.StateDescription,
                            CreatedDate = state.CreatedDate,
                            Creator = creator.UserName,
                            ModifiedDate = state.ModifiedDate,
                            Modifier = modifier.UserName,
                            Status = state.Status
                        }).Where(state => state.Id == Id).FirstOrDefault();
            }                    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchStateName"></param>
        /// <param name="searchStatus"></param>
        /// <param name="searchCreator"></param>
        /// <param name="searchCreatedDateFrom"></param>
        /// <param name="searchCreateDateTo"></param>
        /// <param name="searchPageSize"></param>
        /// <param name="searchPage"></param>
        /// <returns></returns>
        public IEnumerable<StateModel> listAllPaging(string searchStateName, string searchCreator, 
                                                        string searchCreatedDateFrom, string searchCreateDateTo, 
                                                            string searchStatus, string sortByType, string sorting, 
                                                                int searchPageSize, int searchPage)
        {            
            var sqlLinq = from state in db.States
                          join user in db.Users
                          on state.CreatedBy equals user.Id
                          select new StateModel
                          {
                              Id = state.Id,
                              StateName = state.StateName,
                              StateDescription = state.StateDescription,
                              CreatedDate = state.CreatedDate,
                              Creator = user.UserName,
                              Status = state.Status
                          };
            if(!string.IsNullOrEmpty(searchStateName))
            {
                sqlLinq = sqlLinq.Where(state => state.StateName.Contains(searchStateName));
            }

            if(!string.IsNullOrEmpty(searchStatus))
            {
                var status = false;
                if(Int32.Parse(searchStatus) == 1)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                sqlLinq = sqlLinq.Where(state => state.Status == status);
            }

            if(!string.IsNullOrEmpty(searchCreator))
            {
                sqlLinq = sqlLinq.Where(state => state.Creator.Contains(searchCreator));
            }

            if(!string.IsNullOrEmpty(searchCreatedDateFrom))
            {
                var dateFrom = DateTime.Parse(searchCreatedDateFrom);
                sqlLinq = sqlLinq.Where(state => DateTime.Compare(dateFrom, state.CreatedDate.Value) <= 0);
            }

            if(!string.IsNullOrEmpty(searchCreateDateTo))
            {
                var dateTo = DateTime.Parse(searchCreateDateTo);
                sqlLinq = sqlLinq.Where(state => DateTime.Compare(dateTo, state.CreatedDate.Value) >= 0);
            }

            if(sortByType.Equals("stateName"))
            {
                if (sorting.Equals("asc"))
                {
                    return sqlLinq.OrderBy(state => state.StateName).ToPagedList(searchPage, searchPageSize);
                }
                else
                {
                    return sqlLinq.OrderByDescending(state => state.StateName).ToPagedList(searchPage, searchPageSize);
                }
            }

            if (sortByType.Equals("creator"))
            {
                if (sorting.Equals("asc"))
                {
                    return sqlLinq.OrderBy(state => state.Creator).ToPagedList(searchPage, searchPageSize);
                }
                else
                {
                    return sqlLinq.OrderByDescending(state => state.Creator).ToPagedList(searchPage, searchPageSize);
                }
            }

            if (sortByType.Equals("createdDate"))
            {
                if (sorting.Equals("asc"))
                {
                    return sqlLinq.OrderBy(state => state.CreatedDate).ToPagedList(searchPage, searchPageSize);
                }
                else
                {
                    return sqlLinq.OrderByDescending(state => state.CreatedDate).ToPagedList(searchPage, searchPageSize);
                }
            }

            return sqlLinq.OrderByDescending(state => state.CreatedDate).ToPagedList(searchPage, searchPageSize);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchStateName"></param>
        /// <param name="searchStatus"></param>
        /// <param name="searchCreator"></param>
        /// <param name="searchCreatedDateFrom"></param>
        /// <param name="searchCreateDateTo"></param>
        /// <param name="searchPageSize"></param>
        /// <param name="searchPage"></param>
        /// <returns></returns>
        public int totalRows(string searchStateName, string searchCreator,
                                string searchCreatedDateFrom, string searchCreateDateTo,
                                    string searchStatus)
        {
            var sqlLinq = from state in db.States
                          join user in db.Users
                          on state.CreatedBy equals user.Id
                          select new StateModel
                          {
                              Id = state.Id,
                              StateName = state.StateName,
                              StateDescription = state.StateDescription,
                              CreatedDate = state.CreatedDate,
                              Creator = user.UserName,
                              Status = state.Status
                          };
            if (!string.IsNullOrEmpty(searchStateName))
            {
                sqlLinq = sqlLinq.Where(state => state.StateName.Contains(searchStateName));
            }

            if (!string.IsNullOrEmpty(searchStatus))
            {
                var status = false;
                if (Int32.Parse(searchStatus) == 1)
                {
                    status = true;
                }
                else
                {
                    status = false;
                }

                sqlLinq = sqlLinq.Where(state => state.Status == status);
            }

            if (!string.IsNullOrEmpty(searchCreator))
            {
                sqlLinq = sqlLinq.Where(state => state.Creator.Contains(searchCreator));
            }

            if (!string.IsNullOrEmpty(searchCreatedDateFrom))
            {
                var dateFrom = DateTime.Parse(searchCreatedDateFrom);
                sqlLinq = sqlLinq.Where(state => DateTime.Compare(dateFrom, state.CreatedDate.Value) <= 0);
            }

            if (!string.IsNullOrEmpty(searchCreateDateTo))
            {
                var dateTo = DateTime.Parse(searchCreateDateTo);
                sqlLinq = sqlLinq.Where(state => DateTime.Compare(dateTo, state.CreatedDate.Value) >= 0);
            }

            return sqlLinq.ToList().Count;
        }

        public long CreateState(State state)
        {
            db.States.Add(state);
            db.SaveChanges();
            return state.Id;
        }

        public bool UpdateState(State entity)
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

        public bool ChangeStateStatus(long? id)
        {
            try
            {
                var state = db.States.Find(id);
                state.Status = !state.Status;
                db.SaveChanges();
                return (bool)state.Status;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteState(long? id)
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
