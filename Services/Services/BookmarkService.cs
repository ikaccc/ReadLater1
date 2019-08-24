﻿using ReadLater.Entities;
using ReadLater.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ReadLater.Services
{
    public class BookmarkService : IBookmarkService
    {
        protected IUnitOfWork _unitOfWork;

        public BookmarkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Bookmark CreateBookmark(Bookmark bookmark)
        {
            bookmark.CreateDate = DateTime.Now;
            _unitOfWork.Repository<Bookmark>().Insert(bookmark);
            _unitOfWork.Save();
            return bookmark;
        }

        public List<Bookmark> GetBookmarks(string userId, string category)
        {
            if (string.IsNullOrEmpty(category) && !string.IsNullOrEmpty(userId))
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                        .Filter(x => x.UserId == userId)
                                                        .OrderBy(l => l.OrderByDescending(b => b.CreateDate))
                                                        .Get()
                                                        .ToList();
            }
            else
            {
                return _unitOfWork.Repository<Bookmark>().Query()
                                                            .Filter(x => x.UserId == userId)
                                                            .Filter(b => b.Category != null && b.Category.Name == category)
                                                            .Get()
                                                            .ToList();
            }
        }

        public List<Bookmark> GetMostPopularBookmarks()
        {
            return _unitOfWork.Repository<Bookmark>().Query()
                .OrderBy(l => l.OrderByDescending(b => b.Rank))
                .Get().Take(3)
                .ToList();
        }

        public Bookmark GetBookmark(int id, string userId)
        {
            var result = _unitOfWork.Repository<Bookmark>().Query()
                .Filter(c => c.ID == id && c.UserId == userId)
                .Get()
                .FirstOrDefault();
            CalculateRank(result);
            return result;
        }

        public Bookmark GetBookmark(int id)
        {
            var result = _unitOfWork.Repository<Bookmark>().Query()
                .Filter(c => c.ID == id)
                .Get()
                .FirstOrDefault();
            CalculateRank(result);
            return result;
        }

        private void CalculateRank(Bookmark bookmark)
        {
            bookmark.Rank += 1;
            UpdateBookmark(bookmark);
        }

        public void UpdateBookmark(Bookmark bookmark)
        {
            _unitOfWork.Repository<Bookmark>().Update(bookmark);
            _unitOfWork.Save();
        }

        public void DeleteBookmark(Bookmark bookmark)
        {
            _unitOfWork.Repository<Bookmark>().Delete(bookmark);
            _unitOfWork.Save();
        }
    }
}
