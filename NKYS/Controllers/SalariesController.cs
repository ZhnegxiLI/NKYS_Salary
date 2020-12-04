using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NKYS.Models;

namespace NKYS.Controllers
{
    public class SalariesController : Controller
    {
        private readonly Context _context;

        public SalariesController(Context context)
        {
            _context = context;
        }

        // GET: Salaries
        public async Task<IActionResult> Index()
        {

            int[] yearList = { 2020, 2021, 2022, 2023, 2024, 2025 };
            int[] monthList = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

            var departments = await _context.Department.ToListAsync();
            var periods = await _context.Cycle.ToListAsync();

            ViewData["Departments"] = new SelectList(departments,"Id", "Name",null);
            ViewData["Periods"] = new SelectList(periods,"Id", "Label",null);

            var context = _context.Salary.Include(s => s.Cycle).Include(s => s.Employe);
            return View(await context.ToListAsync());
        }

        // GET: Salaries/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salary
                .Include(s => s.Cycle)
                .Include(s => s.Employe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // GET: Salaries/Create
        public IActionResult Create()
        {
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label");
            ViewData["EmployeId"] = new SelectList(_context.Employe, "Id", "Id");
            return View();
        }

        // POST: Salaries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CycleId,EmployeId,WorkingHours,WorkingScore,AbsentHours,SocialSercurityFee,HousingReserves,FullPresencePay,OvertimePay,AbsentDeduct,DormFee,TransportFee,OtherFee,Comment,Id,CreatedBy,UpdatedBy,CreatedOn,UpdatedOn")] Salary salary)
        {
            if (ModelState.IsValid)
            {
                _context.Add(salary);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label", salary.CycleId);
            ViewData["EmployeId"] = new SelectList(_context.Employe, "Id", "Id", salary.EmployeId);
            return View(salary);
        }

        // GET: Salaries/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salary.FindAsync(id);
            if (salary == null)
            {
                return NotFound();
            }
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label", salary.CycleId);
            ViewData["EmployeId"] = new SelectList(_context.Employe, "Id", "Id", salary.EmployeId);
            return View(salary);
        }

        // POST: Salaries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("CycleId,EmployeId,WorkingHours,WorkingScore,AbsentHours,SocialSercurityFee,HousingReserves,FullPresencePay,OvertimePay,AbsentDeduct,DormFee,TransportFee,OtherFee,Comment,Id,CreatedBy,UpdatedBy,CreatedOn,UpdatedOn")] Salary salary)
        {
            if (id != salary.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(salary);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalaryExists(salary.Id))
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
            ViewData["CycleId"] = new SelectList(_context.Cycle, "Id", "Label", salary.CycleId);
            ViewData["EmployeId"] = new SelectList(_context.Employe, "Id", "Id", salary.EmployeId);
            return View(salary);
        }

        // GET: Salaries/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salary = await _context.Salary
                .Include(s => s.Cycle)
                .Include(s => s.Employe)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (salary == null)
            {
                return NotFound();
            }

            return View(salary);
        }

        // POST: Salaries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var salary = await _context.Salary.FindAsync(id);
            _context.Salary.Remove(salary);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalaryExists(long id)
        {
            return _context.Salary.Any(e => e.Id == id);
        }
    }
}
