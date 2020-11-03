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
    public class TagDao
    {
        public ShoppingDbContext db;
        public TagDao()
        {
            db = new ShoppingDbContext();
        }

        public Tag getTagByName(string TagName)
        {
            return db.Tags.Where(tag => tag.TagName.Equals(TagName)).SingleOrDefault();
        }

        public Tag GetById(long? Id)
        {
            return db.Tags.Where(tag => tag.Id == Id).SingleOrDefault();
        }

        public TagModel GetJoinUserById(long? Id)
        {            
            var sql = db.Tags.Where(tag => tag.Id == Id).SingleOrDefault();
            if (sql.ModifiedBy == null)
            {
                return (from tag in db.Tags
                        join creator in db.Users
                        on tag.CreatedBy equals creator.Id
                        select new TagModel
                        {
                            Id = tag.Id,
                            TagName = tag.TagName,
                            TagDescription = tag.TagDescription,
                            CreatedDate = tag.CreatedDate,
                            Creator = creator.UserName,
                            ModifiedDate = null,
                            Modifier = null,
                            Status = tag.Status
                        }).Where(tag => tag.Id == Id).FirstOrDefault();
            } 
            else
            {
                return (from tag in db.Tags
                        join creator in db.Users
                        on tag.CreatedBy equals creator.Id
                        join modifier in db.Users
                        on tag.ModifiedBy equals modifier.Id
                        select new TagModel
                        {
                            Id = tag.Id,
                            TagName = tag.TagName,
                            TagDescription = tag.TagDescription,
                            CreatedDate = tag.CreatedDate,
                            Creator = creator.UserName,
                            ModifiedDate = tag.ModifiedDate,
                            Modifier = modifier.UserName,
                            Status = tag.Status
                        }).Where(tag => tag.Id == Id).FirstOrDefault();
            }                    
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTagName"></param>
        /// <param name="searchStatus"></param>
        /// <param name="searchCreator"></param>
        /// <param name="searchCreatedDateFrom"></param>
        /// <param name="searchCreateDateTo"></param>
        /// <param name="searchPageSize"></param>
        /// <param name="searchPage"></param>
        /// <returns></returns>
        public IEnumerable<TagModel> listAllPaging(string searchTagName, string searchCreator, 
                                                        string searchCreatedDateFrom, string searchCreateDateTo, 
                                                            string searchStatus, string sortByType, string sorting, 
                                                                int searchPageSize, int searchPage)
        {            
            var sqlLinq = from tag in db.Tags
                          join user in db.Users
                          on tag.CreatedBy equals user.Id
                          select new TagModel
                          {
                              Id = tag.Id,
                              TagName = tag.TagName,
                              TagDescription = tag.TagDescription,
                              CreatedDate = tag.CreatedDate,
                              Creator = user.UserName,
                              Status = tag.Status
                          };
            if(!string.IsNullOrEmpty(searchTagName))
            {
                sqlLinq = sqlLinq.Where(tag => tag.TagName.Contains(searchTagName));
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

                sqlLinq = sqlLinq.Where(tag => tag.Status == status);
            }

            if(!string.IsNullOrEmpty(searchCreator))
            {
                sqlLinq = sqlLinq.Where(tag => tag.Creator.Contains(searchCreator));
            }

            if(!string.IsNullOrEmpty(searchCreatedDateFrom))
            {
                var dateFrom = DateTime.Parse(searchCreatedDateFrom);
                sqlLinq = sqlLinq.Where(tag => DateTime.Compare(dateFrom, tag.CreatedDate.Value) <= 0);
            }

            if(!string.IsNullOrEmpty(searchCreateDateTo))
            {
                var dateTo = DateTime.Parse(searchCreateDateTo);
                sqlLinq = sqlLinq.Where(tag => DateTime.Compare(dateTo, tag.CreatedDate.Value) >= 0);
            }

            if(sortByType.Equals("tagName"))
            {
                if (sorting.Equals("asc"))
                {
                    return sqlLinq.OrderBy(tag => tag.TagName).ToPagedList(searchPage, searchPageSize);
                }
                else
                {
                    return sqlLinq.OrderByDescending(tag => tag.TagName).ToPagedList(searchPage, searchPageSize);
                }
            }

            if (sortByType.Equals("creator"))
            {
                if (sorting.Equals("asc"))
                {
                    return sqlLinq.OrderBy(tag => tag.Creator).ToPagedList(searchPage, searchPageSize);
                }
                else
                {
                    return sqlLinq.OrderByDescending(tag => tag.Creator).ToPagedList(searchPage, searchPageSize);
                }
            }

            if (sortByType.Equals("createdDate"))
            {
                if (sorting.Equals("asc"))
                {
                    return sqlLinq.OrderBy(tag => tag.CreatedDate).ToPagedList(searchPage, searchPageSize);
                }
                else
                {
                    return sqlLinq.OrderByDescending(tag => tag.CreatedDate).ToPagedList(searchPage, searchPageSize);
                }
            }

            return sqlLinq.OrderByDescending(tag => tag.CreatedDate).ToPagedList(searchPage, searchPageSize);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTagName"></param>
        /// <param name="searchStatus"></param>
        /// <param name="searchCreator"></param>
        /// <param name="searchCreatedDateFrom"></param>
        /// <param name="searchCreateDateTo"></param>
        /// <param name="searchPageSize"></param>
        /// <param name="searchPage"></param>
        /// <returns></returns>
        public int totalRows(string searchTagName, string searchCreator,
                                string searchCreatedDateFrom, string searchCreateDateTo,
                                    string searchStatus)
        {
            var sqlLinq = from tag in db.Tags
                          join user in db.Users
                          on tag.CreatedBy equals user.Id
                          select new TagModel
                          {
                              Id = tag.Id,
                              TagName = tag.TagName,
                              TagDescription = tag.TagDescription,
                              CreatedDate = tag.CreatedDate,
                              Creator = user.UserName,
                              Status = tag.Status
                          };
            if (!string.IsNullOrEmpty(searchTagName))
            {
                sqlLinq = sqlLinq.Where(tag => tag.TagName.Contains(searchTagName));
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

                sqlLinq = sqlLinq.Where(tag => tag.Status == status);
            }

            if (!string.IsNullOrEmpty(searchCreator))
            {
                sqlLinq = sqlLinq.Where(tag => tag.Creator.Contains(searchCreator));
            }

            if (!string.IsNullOrEmpty(searchCreatedDateFrom))
            {
                var dateFrom = DateTime.Parse(searchCreatedDateFrom);
                sqlLinq = sqlLinq.Where(tag => DateTime.Compare(dateFrom, tag.CreatedDate.Value) <= 0);
            }

            if (!string.IsNullOrEmpty(searchCreateDateTo))
            {
                var dateTo = DateTime.Parse(searchCreateDateTo);
                sqlLinq = sqlLinq.Where(tag => DateTime.Compare(dateTo, tag.CreatedDate.Value) >= 0);
            }

            return sqlLinq.ToList().Count;
        }

        public long CreateTag(Tag tag)
        {
            db.Tags.Add(tag);
            db.SaveChanges();
            return tag.Id;
        }

        public bool UpdateTag(Tag entity)
        {
            try
            {
                var tag = db.Tags.Find(entity.Id);
                if (tag != null)
                {
                    tag.TagName = entity.TagName;
                    tag.TagDescription = entity.TagDescription;
                    tag.ModifiedDate = entity.ModifiedDate;
                    tag.ModifiedBy = entity.ModifiedBy;
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

        public bool ChangeTagStatus(long? id)
        {
            var tag = db.Tags.Find(id);
            tag.Status = !tag.Status;
            db.SaveChanges();
            return (bool)tag.Status;
        }

        public bool DeleteTag(long? id)
        {
            try
            {
                var tag = db.Tags.Find(id);
                db.Tags.Remove(tag);
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
