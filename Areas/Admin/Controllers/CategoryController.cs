using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsApp.DAL;
using NewsApp.Interfaces;
using NewsApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace NewsApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(AppDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            var categories = await _unitOfWork.CategoryRepo.GetAll();
            return View(categories.ToList());

        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryID,CategoryName")] Category category)
        {

            if (ModelState.IsValid)
            {
                await _unitOfWork.CategoryRepo.Add(category);
                await _unitOfWork.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepo.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("CategoryID,CategoryName")] Category category)
        {

            if (id != category.CategoryID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.CategoryRepo.Update(category);
                    await _unitOfWork.CompleteAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(string id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var category = await _unitOfWork.CategoryRepo.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {

            var category = await _unitOfWork.CategoryRepo.GetById(id);
            await _unitOfWork.CategoryRepo.Delete(category);
            await _unitOfWork.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(string id)
        {
            return _context.Categories.Any(e => e.CategoryID == id);
        }


    }
}
