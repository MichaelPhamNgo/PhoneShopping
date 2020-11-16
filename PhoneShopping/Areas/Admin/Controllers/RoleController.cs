using Models.DAO;
using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhoneShopping.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        /// <summary>
        /// Display datatable
        /// </summary>
        /// <param name="searchString">string you want to search</param>
        /// <param name="sorting">sorting type (acs or decs)</param>
        /// <param name="searchPageSize">number record per page</param>
        /// <param name="searchPage">page range</param>
        /// <returns></returns>
        public ActionResult Index(string searchString, string sorting = "decs", int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchString = searchString;
            if (sorting.Equals("asc"))
            {
                sorting = "decs";
            }
            else
            {
                sorting = "asc";
            }
            ViewBag.Sorting = sorting;
            ViewBag.SearchRolePage = searchPage;
            ViewBag.SearchRolePageSize = searchPageSize;
            var dao = new RoleDao();
            var model = dao.listAllPaging(searchString, sorting, searchPageSize, searchPage);
            var totalRows = dao.totalRows(searchString);
            if(totalRows == 0)
            {
                ViewBag.SearchRolePageDisplay = 0;
            } else
            {
                ViewBag.SearchRolePageDisplay = (searchPage - 1) * searchPageSize + 1;
            }            

            var pageRange = searchPage * searchPageSize;
            if (totalRows > (pageRange))
                ViewBag.SearchRolePageSizeDisplay = pageRange;
            else
                ViewBag.SearchRolePageSizeDisplay = totalRows;
            ViewBag.TotalRoleDisplay = totalRows;            
            return View(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(string id)
        {
            var dao = new RoleDao();
            //check if id = null
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            //check if id does not exist in database
            if (dao.getById(id.ToString()) == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                return View(dao.getById(id.ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoleDao();
                var getRole = dao.getByName(role.Name);

                //Check if new role exsits
                if (getRole != null)
                {
                    ModelState.AddModelError("Name", "Role " + role.Name + " exists in database.");
                    return View(role);
                }

                var entity = new Role();
                entity.Id = Guid.NewGuid();
                entity.Name = role.Name;
                entity.Description = role.Description;
                Guid id = dao.create(entity);
                if (id != null)
                {
                    ViewBag.CreateRoleSuccessMessage = "Create " + role.Name + " successful";
                }
                else
                {
                    ViewBag.CreateRoleErrorMessage = "Create " + role.Name + " failed";
                }
            }
            return View(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new RoleDao();
                var role = dao.getById(id);
                if (role == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }
                else
                {
                    return View(role);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Role role)
        {            
            if(ModelState.IsValid)
            {
                var dao = new RoleDao();

                var checkExistRole = dao.getById(role.Id.ToString());
                if (checkExistRole == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }

                if (!role.Id.Equals(checkExistRole.Id))
                {
                    var roleDetail = dao.getById(role.Id.ToString());
                    if (roleDetail != null)
                    {
                        ModelState.AddModelError("Name", "Role " + role.Name + " exists in database.");                        
                        return View(role);
                    }
                }

                var entity = new Role();
                entity.Id = role.Id;
                entity.Name = role.Name;
                entity.Description = role.Description;
                bool update = dao.update(entity);                
                if (update)
                {
                    ViewBag.EditRoleSuccessMessage = "Update role Successful";                    
                }
                else
                {
                    ViewBag.EditRoleErrorMessage = "Update role failed";                    
                }                
            }            
            return View(role);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new RoleDao();
                var role = dao.getById(id);
                if (role == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }
                else
                {
                    new RoleDao().delete(id);
                    return RedirectToAction("Index");
                }
            }
        }
    }    
}
