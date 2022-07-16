using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NewsApp.Interfaces;
using NewsApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NewsApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;

        }



        public async Task<IActionResult> Index()
        {
            var articles = await _unitOfWork.ArticleRepo.GetAll();
            var display = articles.Take(10).ToList();
            if (articles != null)
            {
                TempData["Newest"] = articles.Take(5).ToList();
            }

            Dictionary<Category, IList<Article>> dic = new Dictionary<Category, IList<Article>>();
            dic.Add(await _unitOfWork.CategoryRepo.GetById("C0002"), (await _unitOfWork.ArticleRepo.GetAllByCategory("C0002")).Take(3).ToList());
            dic.Add(await _unitOfWork.CategoryRepo.GetById("C0003"), (await _unitOfWork.ArticleRepo.GetAllByCategory("C0003")).Take(3).ToList());
            dic.Add(await _unitOfWork.CategoryRepo.GetById("C0004"), (await _unitOfWork.ArticleRepo.GetAllByCategory("C0004")).Take(3).ToList());
            ViewBag.SideCol = dic;
            return View(display);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> ReadArticle(string? id)
        {
            if (id == null)
            {
                return NotFound();

            }
            Article article = await _unitOfWork.ArticleRepo.GetById(id);
            if (article == null)
            {
                return NotFound();
            }
            article.ViewsCount += 1;
            await _unitOfWork.ArticleRepo.Update(article);
            await _unitOfWork.CompleteAsync();

            Category tag = await _unitOfWork.ArticleRepo.GetArticleTag(id);
            ViewBag.Cate = tag;
            if (tag != null)
            {
                ViewBag.MostView = (await _unitOfWork.ArticleRepo.GetMostRead(tag.CategoryID)).Take(5).ToList();
                ViewBag.Related = (await _unitOfWork.ArticleRepo.GetRelatedArticle(article.ArticleID)).Take(6).ToList();

            }


            return View(article);
        }
        public async Task<IActionResult> FindArticleByCate(string? cateid, int? page)
        {
            if (cateid == null)
            {
                return NotFound();

            }
            var articles = await _unitOfWork.ArticleRepo.GetAllByCategory(cateid);
            Category tag = await _unitOfWork.CategoryRepo.GetById(cateid);
            ViewBag.Cate = tag;
            if (tag != null)
            {
                ViewBag.MostView = (await _unitOfWork.ArticleRepo.GetMostRead(cateid)).Take(5).ToList();
            }
            TempData["Newest"] = (await _unitOfWork.ArticleRepo.GetAll()).Take(5).ToList();

            int pageSize = 10;
            int pageNumber = (page ?? 1);


            return View(articles.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string? searchString, int? page)
        {
            string name = Request.Form["Name"];
            var articles = await _unitOfWork.ArticleRepo.GetAll();

            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s =>
                                            s.ArticleTitle.ToUpper().Contains(searchString.ToUpper())
                                       );
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);


            return View(articles.ToPagedList(pageNumber, pageSize));
        }

    }
}