using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using NewsApp.Areas.Admin.Models;
using NewsApp.DAL;
using NewsApp.Interfaces;
using NewsApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace NewsApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ArticlesController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public ArticlesController(AppDbContext context, IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _unitOfWork = unitOfWork;
            webHostEnvironment = hostEnvironment;
        }

        /// <summary>
        /// Get all category to viewbag
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        private async Task PopulateArticleCategory(ArticleViewsModel article)
        {
            var allCategories = await _unitOfWork.CategoryRepo.GetAll();
            var newsCates = new HashSet<string>(article.NewsCategories.Select(i => i.CategoryID));
            var viewmodel = new List<AssignedCategoryData>();
            foreach (var item in allCategories)
            {
                viewmodel.Add(new AssignedCategoryData
                {
                    CategoryID = item.CategoryID,
                    CategoryName = item.CategoryName,
                    IsAssigned = newsCates.Contains(item.CategoryID)
                });
            }

            ViewBag.Categories = viewmodel;
        }
        private async Task UpdateArticleCategories(string[] selectedCategories, Article articleToUpdate)
        {
            if (selectedCategories == null)
            {
                articleToUpdate.NewsCategories = new List<NewsCategory>();
                return;
            }
            var allCategories = await _unitOfWork.CategoryRepo.GetAll();
            var selectedCates = new HashSet<string>(selectedCategories);
            var alreadySeleted = new HashSet<string>(articleToUpdate.NewsCategories.Select(i => i.CategoryID));
            foreach (var item in allCategories)
            {
                if (selectedCates.Contains(item.CategoryID))
                {
                    if (!alreadySeleted.Contains(item.CategoryID))
                    {
                        articleToUpdate.NewsCategories.Add(new NewsCategory(
                                            articleToUpdate.ArticleID, item.CategoryID, articleToUpdate, item
                                            ));
                    }
                }
                else
                {
                    if (alreadySeleted.Contains(item.CategoryID))
                    {
                        articleToUpdate.NewsCategories.Remove(await _unitOfWork.NewsCategoryRepo.GetById(articleToUpdate.ArticleID, item.CategoryID));
                    }
                }
            }
        }
        /// <summary>
        ///  get file and copy to wwwroot/Images/{id}
        /// </summary>
        /// <param name="item"></param>
        /// <returns>path string to wwwroot</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<string> UploadedFileAsync(ArticleViewsModel item)
        {
            string path = Path.Combine(webHostEnvironment.WebRootPath, "Resources", item.ArticleID, "Images", item.Image.FileName);
            if (System.IO.File.Exists(path))
            {
                return path;
            }

            try
            {
                if (item.Image != null)
                {
                    string fileName = Guid.NewGuid().ToString() + '-' + item.Image.FileName;
                    string upFolder = Path.Combine(webHostEnvironment.WebRootPath, "Resources", item.ArticleID, "Images");
                    if (!Directory.Exists(upFolder))
                    {
                        Directory.CreateDirectory(upFolder);
                    }
                    using (var stream = new FileStream(Path.Combine(upFolder, fileName), FileMode.Create))
                    {
                        await item.Image.CopyToAsync(stream);
                    }
                    path = Path.Combine(upFolder, fileName);


                    return path;
                }

            }
            catch (Exception)
            {
                ViewData["Template"] = "no image";
            }

            return path;
        }


        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParm = String.IsNullOrEmpty(sortOrder) ? "date_asc" : "";
            ViewBag.ViewsSortParm = sortOrder == "view_asc" ? "view_desc" : "view_asc";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            var articles = await _unitOfWork.ArticleRepo.GetAll();


            if (!String.IsNullOrEmpty(searchString))
            {
                articles = articles.Where(s => s.ArticleTitle.ToUpper().Contains(searchString.ToUpper())
                                       //|| s.AuthorName.ToUpper().Contains(searchString.ToUpper())
                                       );
            }
            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "date_asc":
                    articles = articles.OrderBy(s => s.PublishTime);
                    break;
                case "view_desc":
                    articles = articles.OrderByDescending(s => s.ViewsCount);
                    break;
                case "view_asc":
                    articles = articles.OrderBy(s => s.ViewsCount);
                    break;
                default:
                    articles = articles.OrderByDescending(s => s.PublishTime);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);


            return View(articles.ToPagedList(pageNumber, pageSize));
        }
        // GET: Admin/Articles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _unitOfWork.ArticleRepo.GetById(id);
            if (article == null)
            {
                return NotFound();
            }
            if (article.ImageUrl != null)
            {
                ViewBag.Image = article.ImageUrl.Replace(webHostEnvironment.WebRootPath, "");
            }
            return View(article);
        }

        // GET: Admin/Articles/Create
        public async Task<IActionResult> CreateAsync()
        {

            var vm = new ArticleViewsModel();

            vm.ArticleID = _unitOfWork.ArticleRepo.GenerateID();
            vm.NewsCategories = new List<NewsCategory>();
            await PopulateArticleCategory(vm);
            return View(vm);



        }

        // POST: Admin/Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [IgnoreAntiforgeryToken]
        [RequestSizeLimit(20000000)]
        public async Task<IActionResult> Create(ArticleViewsModel postItem, string[] selectedCategories)
        {
            var old = (await _unitOfWork.ArticleRepo.GetAll()).FirstOrDefault();

            var name = HttpContext.Request.Cookies["Name"];
            var user = await _unitOfWork.AccountRepo.GetById(name);


            if (ModelState.IsValid)
            {
                string imagePath = await UploadedFileAsync(postItem);

                var article = new Article();
                article.NewsCategories = new List<NewsCategory>();
                article.ArticleID = postItem.ArticleID;
                article.ArticleContent = postItem.ArticleContent;
                article.ArticleDescription = postItem.ArticleDescription;
                article.ArticleTitle = postItem.ArticleTitle;

                article.ViewsCount = 0;
                article.PublishBy = user;
                article.PublishTime = DateTime.Now;
                article.AuthorName = postItem.AuthorName;
                article.ImageUrl = imagePath;


                if (selectedCategories != null)
                {
                    foreach (string category in selectedCategories)
                    {
                        var cateToAdd = await _unitOfWork.CategoryRepo.GetById(category);
                        var cateOfNews = new NewsCategory(article.ArticleID, cateToAdd.CategoryID, article, cateToAdd);
                        article.NewsCategories.Add(cateOfNews);
                    }
                }
                await _unitOfWork.ArticleRepo.Add(article);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(postItem);
        }

        // GET: Admin/Articles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _unitOfWork.ArticleRepo.GetById(id);

            if (article == null)
            {
                return NotFound();
            }

            var vm = new ArticleViewsModel
            {
                ArticleID = article.ArticleID,
                ArticleContent = article.ArticleContent,
                ArticleDescription = article.ArticleDescription,
                ArticleTitle = article.ArticleTitle,
                NewsCategories = article.NewsCategories,
                ViewsCount = article.ViewsCount,
                PublishTime = article.PublishTime,
                AuthorName = article.AuthorName,
                ImageUrl = article.ImageUrl,

            };


            if (article.ImageUrl != null)
            {
                ViewBag.Image = article.ImageUrl.Replace(webHostEnvironment.WebRootPath, "");
            }

            await PopulateArticleCategory(vm);
            return View(vm);
        }

        // POST: Admin/Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestSizeLimit(20000000)]
        public async Task<IActionResult> Edit(ArticleViewsModel postItem, string[] selectedCategories)
        {

            var article = await _unitOfWork.ArticleRepo.GetById(postItem.ArticleID);

            var name = HttpContext.Request.Cookies["Name"];
            var user = await _unitOfWork.AccountRepo.GetById(name);

            string imagePath = await UploadedFileAsync(postItem);

            article.ArticleID = postItem.ArticleID;
            article.ArticleContent = postItem.ArticleContent;
            article.ArticleDescription = postItem.ArticleDescription;
            article.ArticleTitle = postItem.ArticleTitle;
            article.ViewsCount = postItem.ViewsCount;
            article.PublishTime = postItem.PublishTime;
            article.AuthorName = postItem.AuthorName;
            article.PublishBy = user;
            article.ImageUrl = imagePath;

            try
            {
                await UpdateArticleCategories(selectedCategories, article);
                await _unitOfWork.ArticleRepo.Update(article);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (RetryLimitExceededException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }



            await PopulateArticleCategory(postItem);

            return View(postItem);
        }

        // GET: Admin/Articles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _unitOfWork.ArticleRepo.GetById(id);

            if (article == null)
            {
                return NotFound();
            }
            if (article.ImageUrl != null)
            {
                ViewBag.Image = article.ImageUrl.Replace(webHostEnvironment.WebRootPath, "");
            }
            return View(article);
        }

        // POST: Admin/Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var article = await _unitOfWork.ArticleRepo.GetById(id);

            string imFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images", article.ArticleID);
            if (Directory.Exists(imFolder))
            {
                Directory.Delete(imFolder, true);
            }
            string reFolder = Path.Combine(webHostEnvironment.WebRootPath, "Resources", article.ArticleID);
            if (Directory.Exists(reFolder))
            {
                Directory.Delete(reFolder, true);
            }
            await _unitOfWork.ArticleRepo.Delete(article);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult UploadFile(IFormFile upload, string id)
        {
            if (upload.Length <= 0) return null;
            if (upload == null)
            {

                return Json(new { uploaded = false, V = "No file uploaded" });
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Resources", id, "Others");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Resources", id, "Others", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);
            }

            var url = $"{this.Request.Scheme}://{this.Request.Host}{"/Resources/"}{id + "/Others/"}{fileName}";
            return Json(new { uploaded = true, url });
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult UploadImage(IFormFile upload, string id)
        {
            if (upload.Length <= 0) return null;
            if (upload == null)
            {

                return Json(new { uploaded = false, V = "No  file uploaded" });
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(upload.FileName).ToLower();

            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Resources", id, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = Path.Combine(
                Directory.GetCurrentDirectory(), "wwwroot/Resources", id, "Images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                upload.CopyTo(stream);

            }

            var url = $"{this.Request.Scheme}://{this.Request.Host}{"/Resources/"}{id + "/Images/"}{fileName}";
            return Json(new { uploaded = true, url });
        }
    }
}
