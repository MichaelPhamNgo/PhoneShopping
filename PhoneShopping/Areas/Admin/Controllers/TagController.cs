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
        public ActionResult Index(string searchTagName, string searchTagCreator,
                                    string searchTagCreatedDateFrom, string searchTagCreatedDateTo, 
                                    string searchTagStatus, string sortByType = "createdDate",
                                        string sorting = "decs", int searchPageSize = 10, int searchPage = 1)
        {
            ViewBag.SearchTagName = searchTagName;
            ViewBag.SearchTagCreator = searchTagCreator;
            ViewBag.SearchTagDateFrom = searchTagCreatedDateFrom;
            ViewBag.SearchTagDateTo = searchTagCreatedDateTo;            
            ViewBag.SearchTagStatus = searchTagStatus;
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
            var model = dao.listAllPaging(searchTagName, searchTagCreator, 
                                            searchTagCreatedDateFrom, searchTagCreatedDateTo, 
                                                searchTagStatus, sortByType, sorting, searchPageSize, searchPage);

            
            var totalRows = dao.totalRows(searchTagName, searchTagCreator,
                                            searchTagCreatedDateFrom, searchTagCreatedDateTo,
                                                searchTagStatus);

            ViewBag.SearchTagPageDisplay = (searchPage - 1) * searchPageSize + 1;

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
        /// <param name="tag"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Tag tag)
        {           
            var dao = new TagDao();
            var getTag = dao.getTagByName(tag.TagName);

            //Check if tag name exits in database
            if(getTag != null)
            {
                ModelState.AddModelError("", "Tag name exists in database.");
                TempData["CreateTagErrorMessage"] = tag.TagName + " exists in database.";
            }

            if(ModelState.IsValid)
            {
                var entity = new Tag();
                entity.TagName = tag.TagName;
                entity.TagDescription = tag.TagDescription;                
                entity.CreatedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserId;
                entity.CreatedDate = DateTime.UtcNow;
                entity.Status = true;
                long id = dao.CreateTag(entity);
                if (id > 0)
                {
                    TempData["CreateTagSuccessMessage"] = "Create " + tag.TagName + " Successful";
                }
                else
                {
                    TempData["CreateTagErrorMessage"] = "Create " + tag.TagName + " failed";
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
                TempData["EditTagErrorMessage"] = "URL does not exist!";
                return RedirectToAction("Index", "Tag");
            }
            else
            {
                var dao = new TagDao();
                var tag = dao.GetById(id);
                if (tag == null)
                {
                    TempData["EditTagErrorMessage"] = "TAG ID = " + id + " does not exist!";
                    return RedirectToAction("Index", "Tag");
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
            //Gọi lớp Models/Dao/TagDao.cs
            var dao = new TagDao();

            //Kiểm tra sự tồn tại của tag id. Do update nên id phải tồn tại.
            var checkExistTag = dao.GetById(tag.Id);
            if (checkExistTag == null)
            {
                TempData["EditTagErrorMessage"] = "Update TAG failed.";
                ModelState.AddModelError("", "Update tag failed.");
            }
            
            //Kiểm tra trường hợp nếu lưu tag name mới. Nếu đã tồi tại rồi thì thông báo
            if (!tag.TagName.Equals(checkExistTag.TagName))
            {
                //Truy xuất dữ liệu tag name
                var tagDetail = dao.getTagByName(tag.TagName);

                //Nếu tag name đã tồn tại thì báo lỗi
                if (tagDetail != null)
                {
                    TempData["EditTagErrorMessage"] = "The TAG NAME = " + tag.TagName + " exists in database.";
                    ModelState.AddModelError("", "The tag name exists in database.");
                }
            }
            

            if (ModelState.IsValid)
            {
                var entity = new Tag();
                entity.Id = tag.Id;
                entity.TagName = tag.TagName;
                entity.TagDescription = tag.TagDescription;
                entity.ModifiedDate = DateTime.UtcNow;
                //Lưu lại người tạo mới một tag
                entity.ModifiedBy = ((UserLogin)Session[CommonConstants.USER_SESSION]).UserId;
                //Lưu tag vào hệ thống
                bool update = dao.UpdateTag(entity);
                //Lưu tag vào hệ thống thành công
                if (update)
                {
                    TempData["EditTagSuccessMessage"] = "Update TAG Successful";
                    return RedirectToAction("Edit", "Tag", new RouteValueDictionary(new { id = tag.Id }));
                }
                else
                {
                    TempData["EditTagErrorMessage"] = "Update TAG failed";
                    return RedirectToAction("Edit", "Tag", new RouteValueDictionary(new { id = tag.Id }));
                }
            }
            return View(tag);
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
            var result = new TagDao().ChangeTagStatus(id);
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
            new TagDao().DeleteTag(id);                     
            return RedirectToAction("Index");
        }
    }
}
