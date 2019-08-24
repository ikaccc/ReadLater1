using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MVC.Models;
using Newtonsoft.Json;
using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    [Authorize]
    public class BookmarksController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IClientService _clientService;
        private ApplicationUser _user;

        public BookmarksController(
            IClientService clientService,
            ICategoryService categoryService)
        {
            _clientService = clientService;
            _categoryService = categoryService;
        }


        public new ApplicationUser User
        {
            get => _user ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindByIdAsync(System.Web.HttpContext.Current.User.Identity.GetUserId()).Result;
            private set => _user = value;
        }

        public async Task<ActionResult> Index()
        {

            var response = await _clientService.GetResponse("bookmarks/" + User.Id);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
            }

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<IEnumerable<Bookmark>>(await response.Content.ReadAsStringAsync());
                return View(model);
            }
            return HttpNotFound();
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var response = await _clientService.GetResponse(id.Value, "bookmarks/" + User.Id + "/");

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
            }

            if (response.IsSuccessStatusCode)
            {
                var result = JsonConvert.DeserializeObject<Bookmark>(await response.Content.ReadAsStringAsync());
                if (result != null)
                {
                    var categories = _categoryService.GetCategories();

                    ViewBag.categorySelectList = new SelectList(categories, "ID", "Name", result.CategoryId);
                    return View(result);
                }
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,URL,ShortDescription,CategoryId,CreateDate")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                var response = await _clientService.PutResponse(bookmark, "bookmarks/");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
                }
                return RedirectToAction("Index");
            }
            return View(bookmark);
        }

        public ActionResult Create()
        {
            var categories = _categoryService.GetCategories();

            ViewBag.categorySelectList = new SelectList(categories, "ID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,URL,ShortDescription,CategoryId,CreateDate")] Bookmark bookmark)
        {
            if (ModelState.IsValid)
            {
                bookmark.UserId = User.Id;
                var response = await _clientService.PostResponse(bookmark, "bookmarks");
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
                }
                return RedirectToAction("Index");
            }

            var categories = _categoryService.GetCategories();

            ViewBag.categorySelectList = new SelectList(categories, "ID", "Name");
            return View(bookmark);
        }

        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await _clientService.GetResponse(id.Value, "bookmarks/" + User.Id + "/");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
            }

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<Bookmark>(await response.Content.ReadAsStringAsync());
                if (model != null)
                {
                    return View(model);
                }
            }

            return HttpNotFound();
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var response = await _clientService.GetResponse(id.Value, "bookmarks/" + User.Id + "/");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
            }

            if (response.IsSuccessStatusCode)
            {
                var model = JsonConvert.DeserializeObject<Bookmark>(await response.Content.ReadAsStringAsync());
                if (model != null)
                {
                    return View(model);
                }
            }

            return HttpNotFound();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var response = await _clientService.DeleteResponse(id, "bookmarks/");
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden, "Access Denied");
            }

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            return HttpNotFound();
        }

    }
}