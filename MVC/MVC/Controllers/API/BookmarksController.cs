using ReadLater.Entities;
using ReadLater.Services;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace MVC.Controllers.API
{
    [Authorize]
    public class BookmarksController : ApiController
    {
        private readonly IBookmarkService _bookmarkService;

        public BookmarksController(
            IBookmarkService bookmarkService)
        {
            _bookmarkService = bookmarkService;
        }

        // GET: api/Bookmarks
        [Route("api/{bookmarks}/{userId}")]
        public JsonResult<List<Bookmark>> GetBookmarks(string userId)
        {
            return Json(_bookmarkService.GetBookmarks(userId, string.Empty));
        }

        [Route("api/{bookmarks}/mostpopularbookmarks")]
        public JsonResult<List<Bookmark>> GetMostPopularBookmarks()
        {
            return Json(_bookmarkService.GetMostPopularBookmarks());
        }

        // GET: api/Bookmarks/5
        [Route("api/{bookmarks}/{userId}/{id}")]
        [ResponseType(typeof(Bookmark))]
        public JsonResult<Bookmark> GetBookmark(string userId, int id)
        {
            var bookmark = _bookmarkService.GetBookmark(id, userId);

            return Json(bookmark);
        }


        //the only way to delete or Update Bookmark is to get specific data for the user
        //so for that reason i dont pass the user for the rest methods

        // PUT: api/Bookmarks/5
        [ResponseType(typeof(void))]
        [Route("api/{bookmarks}")]
        public IHttpActionResult PutBookmark([FromBody]Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var bmark = _bookmarkService.GetBookmark(bookmark.ID);
            if (bmark != null)
            {
                bmark.URL = bookmark.URL;
                bmark.CategoryId = bookmark.CategoryId;
                bmark.CreateDate = bookmark.CreateDate;
                bmark.ShortDescription = bookmark.ShortDescription;

                _bookmarkService.UpdateBookmark(bmark);

                return StatusCode(HttpStatusCode.NoContent);
            }
            return NotFound();
        }

        // POST: api/Bookmarks
        [Route("api/{bookmarks}")]
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult PostBookmark([FromBody]Bookmark bookmark)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _bookmarkService.CreateBookmark(bookmark);

            return CreatedAtRoute("DefaultApi", new { id = bookmark.ID }, bookmark);
        }

        // DELETE: api/Bookmarks/5
        [Route("api/{bookmarks}/{id}")]
        [ResponseType(typeof(Bookmark))]
        public IHttpActionResult DeleteBookmark(int id)
        {
            var bookmark = _bookmarkService.GetBookmark(id);
            if (bookmark == null)
            {
                return NotFound();
            }

            _bookmarkService.DeleteBookmark(bookmark);

            return Ok(bookmark);
        }
    }
}