using Models.DAO;
using Models.EF;
using PhoneShopping.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PhoneShopping.Areas.Admin.Controllers
{    
    public class StateController : BaseController
    {        
        public ActionResult Index(string searchStateName, string searchStateCreator,
                                    string searchStateCreatedDateFrom, string searchStateCreatedDateTo, 
                                    string searchStateStatus, string sortByType = "createdDate",
                                        string sorting = "decs", int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchStateName = searchStateName;
            ViewBag.SearchStateCreator = searchStateCreator;
            ViewBag.SearchStateDateFrom = searchStateCreatedDateFrom;
            ViewBag.SearchStateDateTo = searchStateCreatedDateTo;            
            ViewBag.SearchStateStatus = searchStateStatus;
            ViewBag.SortByType = sortByType;
            if(sorting.Equals("asc"))
            {
                sorting = "decs";
            } else
            {
                sorting = "asc";
            }
            ViewBag.Sorting = sorting;
            ViewBag.SearchStatePage = searchPage;
            ViewBag.SearchStatePageSize = searchPageSize;
            var dao = new StateDao();
            var model = dao.listAllPaging(searchStateName, searchStateCreator, 
                                            searchStateCreatedDateFrom, searchStateCreatedDateTo, 
                                                searchStateStatus, sortByType, sorting, searchPageSize, searchPage);

            
            var totalRows = dao.totalRows(searchStateName, searchStateCreator,
                                            searchStateCreatedDateFrom, searchStateCreatedDateTo,
                                                searchStateStatus);

            ViewBag.SearchStatePageDisplay = (searchPage - 1) * searchPageSize + 1;

            var pageRange = searchPage * searchPageSize;
            if (totalRows > (pageRange))
                ViewBag.SearchStatePageSizeDisplay = pageRange;            
            else
                ViewBag.SearchStatePageSizeDisplay = totalRows;
            ViewBag.TotalStateDisplay = totalRows;

            return View(model);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(long? id)
        {
            var dao = new StateDao();
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
            } else
            {
                return View(dao.GetJoinUserById(id));
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
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(State state)
        {           
            var dao = new StateDao();
            var getState = dao.getStateByName(state.StateName);

            //Check if State name exits in database
            if(getState != null)
            {
                ModelState.AddModelError("", "State name exists in database.");
                TempData["CreateStateErrorMessage"] = state.StateName + " exists in database.";
            }

            if(ModelState.IsValid)
            {
                var entity = new State();
                entity.StateName = state.StateName;
                entity.StateDescription = state.StateDescription;                
                entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.Status = true;
                long id = dao.CreateState(entity);
                if (id > 0)
                {
                    TempData["CreateStateSuccessMessage"] = "Create " + state.StateName + " Successful";
                }
                else
                {
                    TempData["CreateStateErrorMessage"] = "Create " + state.StateName + " failed";
                }
            }            
            return View(state);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                TempData["EditStateErrorMessage"] = "URL does not exist!";
                return RedirectToAction("Index", "State");
            }
            else
            {
                var dao = new StateDao();
                var state = dao.GetById(id);
                if (state == null)
                {
                    TempData["EditStateErrorMessage"] = "State ID = " + id + " does not exist!";
                    return RedirectToAction("Index", "State");
                }
                else
                {
                    return View(state);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="State"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(State state)
        {
            //Gọi lớp Models/Dao/StateDao.cs
            var dao = new StateDao();

            //Kiểm tra sự tồn tại của State id. Do update nên id phải tồn tại.
            var checkExistState = dao.GetById(state.Id);
            if (checkExistState == null)
            {
                TempData["EditStateErrorMessage"] = "Update State failed.";
                ModelState.AddModelError("", "Update State failed.");
            }
            
            //Kiểm tra trường hợp nếu lưu State name mới. Nếu đã tồi tại rồi thì thông báo
            if (!state.StateName.Equals(checkExistState.StateName))
            {
                //Truy xuất dữ liệu State name
                var StateDetail = dao.getStateByName(state.StateName);

                //Nếu State name đã tồn tại thì báo lỗi
                if (StateDetail != null)
                {
                    TempData["EditStateErrorMessage"] = "The State NAME = " + state.StateName + " exists in database.";
                    ModelState.AddModelError("", "The State name exists in database.");
                }
            }
            

            if (ModelState.IsValid)
            {
                var entity = new State();
                entity.Id = state.Id;
                entity.StateName = state.StateName;
                entity.StateDescription = state.StateDescription;
                entity.ModifiedDate = DateTime.UtcNow;
                //Lưu lại người tạo mới một State
                entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserId;
                //Lưu State vào hệ thống
                bool update = dao.UpdateState(entity);
                //Lưu State vào hệ thống thành công
                if (update)
                {
                    TempData["EditStateSuccessMessage"] = "Update State Successful";
                    return RedirectToAction("Edit", "State", new RouteValueDictionary(new { id = state.Id }));
                }
                else
                {
                    TempData["EditStateErrorMessage"] = "Update State failed";
                    return RedirectToAction("Edit", "State", new RouteValueDictionary(new { id = state.Id }));
                }
            }
            return View(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetCreatorByUserName(string UserName)
        {
            using (ShoppingDbContext db = new ShoppingDbContext())
            {
                var username = (from user in db.Users
                                where user.UserName.StartsWith(UserName)
                                select new { label = user.UserName }).ToList();
                return Json(username);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeStatus(long? id)
        {
            var result = new StateDao().ChangeStateStatus(id);
            return Json(new { status = result });           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(long? id)
        {
            new StateDao().DeleteState(id);                     
            return RedirectToAction("Index");
        }
    }
}
