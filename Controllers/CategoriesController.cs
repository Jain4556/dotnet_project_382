using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _db;
        public CategoriesController(AppDbContext db) => _db = db;

        public IActionResult Index() => View(_db.Categories.ToList());

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Edit(int id)
        {
            var cat = _db.Categories.Find(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.Update(category);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int id)
        {
            var cat = _db.Categories.Find(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var cat = _db.Categories.Find(id);
            if (cat != null)
            {
                _db.Categories.Remove(cat);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
