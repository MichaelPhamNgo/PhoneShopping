using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PhoneShopping.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        // GET: Admin/Contact
        public ActionResult Index(string searchString, string sorting = "decs",
                                            int searchPageSize = 10, int searchPage = 1)
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
            ViewBag.SearchContactPage = searchPage;
            ViewBag.SearchContactPageSize = searchPageSize;
            var dao = new ContactDao();
            var model = dao.listAllPaging(searchString, sorting, searchPageSize, searchPage);
            var totalRows = dao.totalRows(searchString);
            if (totalRows == 0)
            {
                ViewBag.SearchContactPageDisplay = 0;
            }
            else
            {
                ViewBag.SearchContactPageDisplay = (searchPage - 1) * searchPageSize + 1;
            }

            var pageRange = searchPage * searchPageSize;
            if (totalRows > (pageRange))
                ViewBag.SearchContactPageSizeDisplay = pageRange;
            else
                ViewBag.SearchContactPageSizeDisplay = totalRows;
            ViewBag.TotalContactDisplay = totalRows;
            return View(model);
        }

        // GET: Admin/Contact/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Admin/Contact/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contact/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Contact/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Admin/Contact/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Admin/Contact/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Admin/Contact/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
