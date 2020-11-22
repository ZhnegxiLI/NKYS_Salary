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
    public class GroupsController : Controller
    {
        private readonly Context _context;

        public GroupsController(Context context)
        {
            _context = context;
        }

        // GET: Groups
        public async Task<IActionResult> Index(long? id)
        {
            var context = _context.Groups.Where(p=>p.DepartmentId == id).Include(g => g.Department).Include(g=>g.Employes);
            return View(await context.ToListAsync());
        }

        // GET: Groups/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .Include(g => g.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // GET: Groups/Create
        public async Task<IActionResult> CreateOrEdit(long? id)
        {
           
            if (id == null)
            {
                ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
                ViewData["Action"] = "Create";
                return View();
            }

            var group = await _context.Groups.FindAsync(id);
            ViewData["Action"] = "Update";
            if (group == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", group.DepartmentId);
            return View(group);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("Name,DepartmentId,SharePropotion,ProductionValueTypeId,IsFixSalary,Comment,Id,CreatedBy,UpdatedBy,CreatedOn,UpdatedOn")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                if (groups.Id != null && groups.Id > 0)
                {
                    _context.Update(groups);
                }
                else
                {
                    groups.CreatedOn = DateTime.Now;
                    _context.Add(groups);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id= groups.DepartmentId});
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", groups.DepartmentId);
            return View(groups);
        }

        // GET: Groups/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = await _context.Groups
                .Include(g => g.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (groups == null)
            {
                return NotFound();
            }

            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var groups = await _context.Groups.FindAsync(id);
            _context.Groups.Remove(groups);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupsExists(long id)
        {
            return _context.Groups.Any(e => e.Id == id);
        }
    }
}
