using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;
using System.Linq;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly AppDbContext _db;
        public ExpensesController(AppDbContext db) => _db = db;

        // --------------------- INDEX ---------------------
        // Show all expenses (with search)
        public async Task<IActionResult> Index(string? search)
        {
            var q = _db.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
                q = q.Where(e => e.Description.Contains(search));

            return View(await q.ToListAsync());
        }

        // --------------------- CREATE ---------------------
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Expense expense)
        {
            if (ModelState.IsValid)
            {
                _db.Add(expense);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        // --------------------- EDIT ---------------------
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var expense = await _db.Expenses.FindAsync(id);
            if (expense == null) return NotFound();

            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Expense expense)
        {
            if (id != expense.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _db.Update(expense);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_db.Expenses.Any(e => e.Id == expense.Id))
                        return NotFound();
                    else
                        throw;
                }
            }

            ViewBag.Categories = new SelectList(_db.Categories, "Id", "Name", expense.CategoryId);
            return View(expense);
        }

        // --------------------- DELETE ---------------------
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var expense = await _db.Expenses
                .Include(e => e.Category)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (expense == null) return NotFound();
            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _db.Expenses.FindAsync(id);
            if (expense != null)
            {
                _db.Expenses.Remove(expense);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // --------------------- SUMMARY PAGE ---------------------
        // This will show a summary list and total
        public async Task<IActionResult> Summary()
        {
            var expenses = await _db.Expenses
                .Include(e => e.Category)
                .OrderByDescending(e => e.Date)
                .ToListAsync();

            var categorySummary = expenses
                .GroupBy(e => e.Category!.Name)
                .Select(g => new
                {
                    Category = g.Key,
                    Total = g.Sum(e => e.Amount)
                })
                .ToList();

            ViewBag.CategorySummary = categorySummary;
            ViewBag.TotalExpense = expenses.Sum(e => e.Amount);

            return View(expenses);
        }

        // --------------------- API: SUMMARY BY CATEGORY ---------------------
        [HttpGet]
        public async Task<IActionResult> SummaryByCategory()
        {
            var data = await _db.Expenses
                .Include(e => e.Category)
                .GroupBy(e => e.Category!.Name)
                .Select(g => new { Category = g.Key, Total = g.Sum(x => x.Amount) })
                .ToListAsync();

            return Json(data);
        }

        // --------------------- API: MONTHLY TOTALS ---------------------
        [HttpGet]
        public async Task<IActionResult> MonthlyTotals(int months = 6)
        {
            var cutoff = DateTime.Today.AddMonths(-months + 1);

            var data = await _db.Expenses
                .Where(e => e.Date >= cutoff)
                .GroupBy(e => new { e.Date.Year, e.Date.Month })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(x => x.Year).ThenBy(x => x.Month)
                .ToListAsync();

            return Json(data);
        }
    }
}
