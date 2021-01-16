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
    public class DepartmentsController : Controller
    {
        private readonly Context _context;

        public DepartmentsController(Context context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            return View(await _context.Department.Include(p=>p.Groups).ToListAsync());
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        public async Task<IActionResult> CreateOrEdit(long? id)
        {
            Department department = null;
            if (id == null)
            {
                ViewData["Action"] = "Create";
                return View();
            }

            department = await _context.Department.FindAsync(id);
            ViewData["Action"] = "Update";
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit([Bind("Name,Comment,Id")] Department department)
        {
            if (ModelState.IsValid)
            {
                if (department.Id >0)
                {
                    _context.Update(department);
                }
                else
                {
                    department.CreatedOn = DateTime.Now;
                    _context.Add(department);
                }
              
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }


        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var department = await _context.Department
                .FirstOrDefaultAsync(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var department = await _context.Department.FindAsync(id);
            _context.Department.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(long id)
        {
            return _context.Department.Any(e => e.Id == id);
        }


        // API
        [HttpGet]
        public async Task<JsonResult> FindDepartmentList()
        {
            return Json(await FindDepartmentListData());
        }
        public async Task<List<Department>> FindDepartmentListData()
        {
            var departments = await (from d in _context.Department
                                select d).ToListAsync();
            return departments;
        }

    }
}
