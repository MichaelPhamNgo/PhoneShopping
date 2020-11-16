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
        public ActionResult Index(string searchString, string sorting = "decs", int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchName = searchString;
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

        public ActionResult Detail(Guid id)
        {
            var dao = new RoleDao();
            //check if id = null
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            //check if id does not exist in database
            if (dao.GetById(id) == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                return View(dao.GetById(id));
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                var dao = new RoleDao();
                var getRole = dao.getRoleByName(role.Name);

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
                Guid id = dao.Create(entity);
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

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new RoleDao();
                var role = dao.GetById(id);
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

        [HttpPost]
        public ActionResult Edit(Role role)
        {            
            if(ModelState.IsValid)
            {
                var dao = new RoleDao();

                var checkExistRole = dao.GetById(role.Id);
                if (checkExistRole == null)
                {
                    ViewBag.EditRoleErrorMessage = "Update role failed.";
                    return View(role);
                }

                if (!role.Id.Equals(checkExistRole.Id))
                {
                    var roleDetail = dao.GetById(role.Id);

                    //Nếu tag name đã tồn tại thì báo lỗi
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
                bool update = dao.Update(entity);                
                if (update)
                {
                    ViewBag.EditRoleSuccessMessage = "Update role Successful";
                    return RedirectToAction("Edit", "Role", new RouteValueDictionary(new { id = role.Id }));
                }
                else
                {
                    ViewBag.EditRoleErrorMessage = "Update role failed";
                    return RedirectToAction("Edit", "Role", new RouteValueDictionary(new { id = role.Id }));
                }                
            }            
            return View(role);
        }

        [HttpDelete]
        public ActionResult Delete(Guid id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new RoleDao();
                var role = dao.GetById(id);
                if (role == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }
                else
                {
                    new RoleDao().Delete(id);
                    return RedirectToAction("Index");
                }
            }
        }
    }    
}
