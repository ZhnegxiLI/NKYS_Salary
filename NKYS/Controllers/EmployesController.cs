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
    public class EmployesController : Controller
    {
        private readonly Context _context;

        public EmployesController(Context context)
        {
            _context = context;
        }

        // GET: Employes
        public async Task<IActionResult> Index(long? id)
        {

            var context = _context.Employe.Where(p=>p.GroupsId == id).Include(e => e.Groups).Include(p=>p.EmployeDeductionConfiguration);
            return View(await context.ToListAsync());
        }

        // GET: Employes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = await _context.Employe
                .Include(e => e.Groups)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        // GET: Employes/Create
        public async Task<IActionResult> CreateOrEdit(long? id)
        {
         
            if (id == null)
            {
                ViewData["GroupsId"] = new SelectList(_context.Groups, "Id", "Name");
                ViewData["Action"] = "Create";
                return View();
            }
            var employe = await _context.Employe.FindAsync(id);
            ViewData["Action"] = "Update";
            if (employe == null)
            {
                return NotFound();
            }
            ViewData["GroupsId"] = new SelectList(_context.Groups, "Id", "Name", employe.GroupsId);
            return View(employe);

        }

        // POST: Employes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("GroupsId,Name,EntreEntrepriseDate,ExternalId,TechnicalLevel,SelfPaySocialSercurity,SelfPayHousingReserves,HasDorm,TransportFee,PositionPay,IsChefOfGroup,SeniorityPay,FixSalary,DeductionPercentage,IsTemporaryEmploye,DepartDate,Id,EmployeDeductionConfiguration")] Employe employe)
        {
            if (ModelState.IsValid)
            {
                if (employe.Id != null && employe.Id > 0)
                {
                    _context.Update(employe);
                }
                else
                {
                    employe.CreatedOn = DateTime.Now;
                    _context.Add(employe);
                }
         
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = employe.GroupsId } );
            }
            ViewData["GroupsId"] = new SelectList(_context.Groups, "Id", "Name", employe.GroupsId);
            return View(employe);
        }

   
        // GET: Employes/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employe = await _context.Employe
                .Include(e => e.Groups)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employe == null)
            {
                return NotFound();
            }

            return View(employe);
        }

        // POST: Employes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var employe = await _context.Employe.FindAsync(id);
            _context.Employe.Remove(employe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeExists(long id)
        {
            return _context.Employe.Any(e => e.Id == id);
        }
    }
}
