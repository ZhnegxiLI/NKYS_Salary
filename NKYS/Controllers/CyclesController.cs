using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NKYS.Models;

namespace NKYS.Controllers
{
    [Authorize]
    public class CyclesController : Controller
    {
        private readonly Context _context;

        public CyclesController(Context context)
        {
            _context = context;
        }

        // GET: Cycles
        public async Task<IActionResult> Index()
        {
            return View(await _context.Cycle.ToListAsync());
        }

        // GET: Cycles/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cycle = await _context.Cycle
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cycle == null)
            {
                return NotFound();
            }

            return View(cycle);
        }

        // GET: Cycles/Create
        public async Task<IActionResult> CreateOrEdit(long? id)
        {
            Cycle cycle = null;
            if (id == null)
            {
                ViewData["Action"] = "Create";
                return View();
            }

            cycle = await _context.Cycle.FindAsync(id);
            ViewData["Action"] = "Update";
            if (cycle == null)
            {
                return NotFound();
            }
            return View(cycle);

        }

        // POST: Cycles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("FromDate,ToDate,Validity,Label,StandardWorkingHours,Comment,Id")] Cycle cycle)
        {
            if (cycle.FromDate >= cycle.ToDate)
            {
                ModelState.AddModelError(string.Empty, "结束日期必须大于起始日期");
            }

            if (ModelState.IsValid)
            {

                cycle.Year = cycle.FromDate.Year;// place into sql trigger 
                cycle.Month = cycle.FromDate.Month; // place into sql trigger 

                if (cycle.Id!=null && cycle.Id > 0)
                {
                    _context.Update(cycle);
                }
                else
                {
                    cycle.CreatedOn = DateTime.Now;
                    _context.Add(cycle);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                
            }
            return View(cycle);
        }


        // POST: Cycles/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var cycle = await _context.Cycle.FindAsync(id);
            _context.Cycle.Remove(cycle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
