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
        public ActionResult Index(string searchString, string sorting = "decs", 
                                            int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchString = searchString;            
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
            var model = dao.listAllPaging(searchString, sorting, searchPageSize, searchPage);            
            var totalRows = dao.totalRows(searchString);
            if (totalRows == 0)
            {
                ViewBag.SearchStatePageDisplay = 0;
            }
            else
            {
                ViewBag.SearchStatePageDisplay = (searchPage - 1) * searchPageSize + 1;
            }

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
            if (dao.getById(id) == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            } else
            {
                return View(dao.getById(id));
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
            if (ModelState.IsValid)
            {
                var dao = new StateDao();
                var getState = dao.getByName(state.StateName);

                //Check if State name exits in database
                if(getState != null)
                {
                    ModelState.AddModelError("StateName", "State name exists in database.");
                    return View(state);
                }

            
                var entity = new State();
                entity.StateName = state.StateName;
                entity.StateDescription = state.StateDescription;                
                long id = dao.create(entity);
                if (id > 0)
                {
                    ViewBag.CreateStateSuccessMessage = "Create " + state.StateName + " successful";
                }
                else
                {
                    ViewBag.CreateStateErrorMessage = "Create " + state.StateName + " failed";
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
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new StateDao();
                var state = dao.getById(id);
                if (state == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
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
            if (ModelState.IsValid)
            {
                var dao = new StateDao();
         
                var checkExistState = dao.getById(state.Id);
                if (checkExistState == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }            
            
                if (!state.StateName.Equals(checkExistState.StateName))
                {                
                    var stateDetail = dao.getByName(state.StateName);
                    if (stateDetail != null)
                    {                    
                        ModelState.AddModelError("StateName", "State " + state.StateName + " exists in database.");
                        return View(state);
                    }
                }
            
                var entity = new State();
                entity.Id = state.Id;
                entity.StateName = state.StateName;
                entity.StateDescription = state.StateDescription;                
                bool update = dao.update(entity);                
                if (update)
                {
                    ViewBag.EditStateSuccessMessage = "Update state successful";                    
                }
                else
                {
                    ViewBag.EditStateErrorMessage = "Update state failed";                    
                }                
            }
            return View(state);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new StateDao();
                var state = dao.getById(id);
                if (state == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }
                else
                {
                    new StateDao().delete(id);                     
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
