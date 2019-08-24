using ReadLater.Entities;
using System.Collections.Generic;

namespace ReadLater.Services
{
    public interface IBookmarkService
    {
        Bookmark CreateBookmark(Bookmark bookmark);
        List<Bookmark> GetBookmarks(string userId, string category);
        Bookmark GetBookmark(int id, string userId);
        List<Bookmark> GetMostPopularBookmarks();
        Bookmark GetBookmark(int id);
        void UpdateBookmark(Bookmark bookmark);
        void DeleteBookmark(Bookmark bookmark);
    }
}