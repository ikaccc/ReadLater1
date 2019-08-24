using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MVC.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IClientService _clientService;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get => _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            private set => _userManager = value;
        }

        public DashboardController(IClientService clientService)
        {
            _clientService = clientService;
        }
        // GET: Dashboard
        public async Task<ActionResult> Index()
        {
            return View(await UserManager.Users.ToListAsync());
        }

        public async Task<ActionResult> RecentBookmarks(string id)
        {

            var response = await _clientService.GetResponse("bookmarks/" + id);

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

        public async Task<ActionResult> MostPopularBookmarks(string id)
        {

            var response = await _clientService.GetMostPopularBookmarks();

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
    }
}