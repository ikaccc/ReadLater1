﻿using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class CategoriesController : Controller
    {
        private ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        // GET: Categories
        public ActionResult Index()
        {
            List<Category> model = _categoryService.GetCategories();
            return View(model);
        }

        // GET: Categories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);

        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.CreateCategory(category);
                return RedirectToAction("Index");
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateAjax(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) return Json(new { isValid = 0, errorMessage = "The Name field is required." });
            if (name.Length > 49) return Json(new { isValid = 0, errorMessage = "The field Name must be a string with a maximum length of 50." });
            var newCategory = _categoryService.CreateCategory(new Category { Name = name });

            return Json(new { isValid = 1, newCategory });
        }

        // GET: Categories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryService.UpdateCategory(category);
                return RedirectToAction("Index");
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = _categoryService.GetCategory((int)id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Category category = _categoryService.GetCategory(id);
            _categoryService.DeleteCategory(category);
            return RedirectToAction("Index");
        }
    }
}
