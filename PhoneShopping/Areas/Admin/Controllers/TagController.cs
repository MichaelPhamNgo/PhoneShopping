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
    public class TagController : BaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="searchTagName"></param>
        /// <param name="searchTagCreator"></param>
        /// <param name="searchTagCreatedDateFrom"></param>
        /// <param name="searchTagCreatedDateTo"></param>
        /// <param name="searchTagStatus"></param>
        /// <param name="sortByType"></param>
        /// <param name="sorting"></param>
        /// <param name="searchPageSize"></param>
        /// <param name="searchPage"></param>
        /// <returns></returns>
        public ActionResult Index(string searchString, string sortByType = "createdDate",
                                        string sorting = "decs", int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchString = searchString;            
            ViewBag.SortByType = sortByType;
            if(sorting.Equals("asc"))
            {
                sorting = "decs";
            } else
            {
                sorting = "asc";
            }
            ViewBag.Sorting = sorting;
            ViewBag.SearchTagPage = searchPage;
            ViewBag.SearchTagPageSize = searchPageSize;
            var dao = new TagDao();
            var model = dao.listAllPaging(searchString, sortByType, sorting, searchPageSize, searchPage);            
            var totalRows = dao.totalRows(searchString);
            if (totalRows == 0)
            {
                ViewBag.SearchRolePageDisplay = 0;
            }
            else
            {
                ViewBag.SearchRolePageDisplay = (searchPage - 1) * searchPageSize + 1;
            }            

            var pageRange = searchPage * searchPageSize;
            if (totalRows > (pageRange))
                ViewBag.SearchTagPageSizeDisplay = pageRange;            
            else
                ViewBag.SearchTagPageSizeDisplay = totalRows;
            ViewBag.TotalTagDisplay = totalRows;

            return View(model);
        }        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Detail(long? id)
        {
            var dao = new TagDao();
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
                return View(dao.getJoinUserById(id));
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
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var dao = new TagDao();
                var getTag = dao.getByName(tag.TagName);

                //Check if tag name exits in database
                if(getTag != null)
                {
                    ModelState.AddModelError("TagName", "Tag " + tag.TagName + " exists in database.");
                    return View(tag);
                }

            
                var entity = new Tag();
                entity.TagName = tag.TagName;
                entity.TagDescription = tag.TagDescription;                
                entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.Status = true;
                long id = dao.create(entity);
                if (id > 0)
                {
                    ViewBag.CreateTagSuccessMessage = "Create " + tag.TagName + " successful";
                }
                else
                {
                    ViewBag.CreateTagErrorMessage = "Create " + tag.TagName + " failed";
                }
            }            
            return View(tag);            
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
                var dao = new TagDao();
                var tag = dao.getById(id);
                if (tag == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }
                else
                {
                    return View(tag);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Tag tag)
        {
            if (ModelState.IsValid)
            {
                var dao = new TagDao();
            
                var checkExistTag = dao.getById(tag.Id);
                if (checkExistTag == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }            
            
                if (!tag.TagName.Equals(checkExistTag.TagName))
                {                    
                    var tagDetail = dao.getByName(tag.TagName);
                    if (tagDetail != null)
                    {                        
                        ModelState.AddModelError("TagName", "Tag " + tag.TagName + " exists in database.");
                        return View(tag);
                    }
                }
            
                var entity = new Tag();
                entity.Id = tag.Id;
                entity.TagName = tag.TagName;
                entity.TagDescription = tag.TagDescription;
                entity.ModifiedDate = DateTime.UtcNow;                
                entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserId;                
                bool update = dao.update(entity);                
                if (update)
                {
                    ViewBag.EditTagSuccessMessage = "Update tag successful";                    
                }
                else
                {
                    ViewBag.EditTagErrorMessage = "Update tag failed";                    
                }                
            }
            return View(tag);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ChangeStatus(long? id)
        {
            var result = new TagDao().changeStatus(id);
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
            if (id == null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            else
            {
                var dao = new TagDao();
                var role = dao.getById(id);
                if (role == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound");
                }
                else
                {
                    new TagDao().delete(id);                     
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
