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
    public class RoleController : Controller
    {        
        public ActionResult Index(string searchRoleName, string sorting = "decs", int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchRoleName = searchRoleName;
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
            var model = dao.listAllPaging(searchRoleName, sorting, searchPageSize, searchPage);
            var totalRows = dao.totalRows(searchRoleName);

            ViewBag.SearchRolePageDisplay = (searchPage - 1) * searchPageSize + 1;

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
            var dao = new RoleDao();
            var getRole = dao.GetById(role.Id);

            //Check if tag name exits in database
            if (getRole != null)
            {
                ModelState.AddModelError("", "ROLE ID exists in database.");
                TempData["CreateRoleErrorMessage"] = role.Id + " exists in database.";
            }

            if (ModelState.IsValid)
            {
                var entity = new Role();
                entity.Id = role.Id;               
                Guid id = dao.CreateRole(entity);
                if (id != null)
                {
                    TempData["CreateRoleSuccessMessage"] = "Create " + role.Name + " successful";
                }
                else
                {
                    TempData["CreateRoleErrorMessage"] = "Create " + role.Name + " failed";
                }
            }
            return View(role);
        }

        public ActionResult Edit(Guid id)
        {
            if (id == null)
            {
                TempData["EditRoleErrorMessage"] = "URL does not exist!";
                return RedirectToAction("Index", "Role");
            }
            else
            {
                var dao = new RoleDao();
                var role = dao.GetById(id);
                if (role == null)
                {
                    TempData["EditTagErrorMessage"] = "ROLE ID = " + id + " does not exist!";
                    return RedirectToAction("Index", "Role");
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
            var dao = new RoleDao();
                        
            var checkExistRole = dao.GetById(role.Id);
            if (checkExistRole == null)
            {
                TempData["EditRoleErrorMessage"] = "Update ROLE failed.";
                ModelState.AddModelError("", "Update role failed.");
            }
            
            if (!role.Id.Equals(checkExistRole.Id))
            {
                var roleDetail = dao.GetById(role.Id);

                //Nếu tag name đã tồn tại thì báo lỗi
                if (roleDetail != null)
                {
                    TempData["EditRoleErrorMessage"] = "The ROLE ID = " + role.Id + " exists in database.";
                    ModelState.AddModelError("", "The role id exists in database.");
                }
            }


            if (ModelState.IsValid)
            {
                var entity = new Role();
                entity.Id = role.Id;
                entity.Name = role.Name;                
                bool update = dao.UpdateRole(entity);
                //Lưu tag vào hệ thống thành công
                if (update)
                {
                    TempData["EditRoleSuccessMessage"] = "Update ROLE Successful";
                    return RedirectToAction("Edit", "Role", new RouteValueDictionary(new { id = role.Id }));
                }
                else
                {
                    TempData["EditRoleErrorMessage"] = "Update ROLE failed";
                    return RedirectToAction("Edit", "Role", new RouteValueDictionary(new { id = role.Id }));
                }
            }
            return View(role);
        }

        [HttpDelete]
        public ActionResult Delete(string id)
        {
            new RoleDao().DeleteRole(id);
            return RedirectToAction("Index");
        }
    }    
}
