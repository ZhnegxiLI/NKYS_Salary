using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NKYS.Models;
using NKYS.Models.ViewModel;

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

        public IActionResult EmployeSearch()
        {
            return View();
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
            var employe = await _context.Employe.Where(p=>p.Id == id).Include(p=>p.EmployeDeductionConfiguration).FirstOrDefaultAsync();
            employe.ProductionValueTypeId = new List<ProductionValueType>();
            if (employe.EmployeDeductionConfiguration.Count()>0)
            {
                foreach (var item in employe.EmployeDeductionConfiguration)
                {
                    employe.ProductionValueTypeId.Add((ProductionValueType)item.LinkedProductionValueTypeId);
                }
            }
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
        public async Task<IActionResult> CreateOrEdit(Employe employe)
        {
            if (ModelState.IsValid)
            {
                if (employe.Id > 0)
                {
                    _context.Update(employe);
                }
                else
                {
                    employe.CreatedOn = DateTime.Now;
                    _context.Add(employe);
                }
         
                await _context.SaveChangesAsync();

                var employeDeductionConfiguration = await _context.EmployeDeductionConfiguration.Where(p => p.EmployeId == employe.Id).ToListAsync();
                _context.RemoveRange(employeDeductionConfiguration);
                if (employe.ProductionValueTypeId.Count()>0)
                {
                    foreach (var item in employe.ProductionValueTypeId)
                    {
                        var DeductionConfiguration = new EmployeDeductionConfiguration();
                        DeductionConfiguration.LinkedProductionValueTypeId = item;
                        DeductionConfiguration.EmployeId = employe.Id;
                        await _context.AddAsync(DeductionConfiguration);
                    }
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


        // API
        [HttpGet]
        public async Task<JsonResult> GetEmployeList(long? DepartmentId, long? GroupsId, string Name, string ExternalId)
        {
            var employeeList = await (from e in _context.Employe
                                where (GroupsId == null || GroupsId <=0 || e.GroupsId == GroupsId) && (Name == null || e.Name.Contains(Name)) && (ExternalId== null || e.ExternalId.Contains(ExternalId))
                                select e).Include(p => p.Groups).ToListAsync();
            return Json(employeeList);
        }

        public class InsertOrUpdateEmployeCriteria: Employe
        {
            public long? UserId { get; set; }
        }

        [HttpPost]
        public async Task<long> InsertOrUpdateEmploye(InsertOrUpdateEmployeCriteria criteria)
        {
            var employeToInsertOrUpdate = new Employe();
            var employeWithSameExternalId = await CheckExternalIdNotExistsData(criteria.Id, criteria.ExternalId);

            if (employeWithSameExternalId== null)
            {
                if (criteria.Id > 0)
                {
                    employeToInsertOrUpdate = await _context.Employe.FirstOrDefaultAsync(p => p.Id == criteria.Id);
                    employeToInsertOrUpdate.UpdatedBy = criteria.UserId;
                }
                else
                {
                    employeToInsertOrUpdate.CreatedBy = criteria.UserId;
                    employeToInsertOrUpdate.CreatedOn = DateTime.Now;

                }

                if (employeToInsertOrUpdate != null)
                {
                    employeToInsertOrUpdate.Name = criteria.Name;

                    employeToInsertOrUpdate.GroupsId = criteria.GroupsId;
                    employeToInsertOrUpdate.EntreEntrepriseDate = criteria.EntreEntrepriseDate;
                    employeToInsertOrUpdate.ExternalId = criteria.ExternalId;
                    employeToInsertOrUpdate.TechnicalLevel = criteria.TechnicalLevel;
                    employeToInsertOrUpdate.SelfPaySocialSercurity = criteria.SelfPaySocialSercurity;
                    employeToInsertOrUpdate.SelfPayHousingReserves = criteria.SelfPayHousingReserves;

                    employeToInsertOrUpdate.HasDorm = criteria.HasDorm;
                    employeToInsertOrUpdate.TransportFee = criteria.TransportFee;
                    employeToInsertOrUpdate.PositionPay = criteria.PositionPay;

                    employeToInsertOrUpdate.IsChefOfGroup = criteria.IsChefOfGroup;
                    employeToInsertOrUpdate.SeniorityPay = criteria.SeniorityPay;
                    employeToInsertOrUpdate.FixSalary = criteria.FixSalary;

                    employeToInsertOrUpdate.DeductionPercentage = criteria.DeductionPercentage;
                    employeToInsertOrUpdate.IsTemporaryEmploye = criteria.IsTemporaryEmploye;
                    employeToInsertOrUpdate.DepartDate = criteria.DepartDate;

                    // todo add deductionConfiguration 
                }

                if (employeToInsertOrUpdate.Id > 0)
                {
                    _context.Update(employeToInsertOrUpdate);
                }
                else
                {
                    await _context.AddAsync(employeToInsertOrUpdate);
                }

                await _context.SaveChangesAsync();

                return employeToInsertOrUpdate.Id;
            }
            else
            {
                return 0;
            }
         
        }
        
        [HttpGet]
        public async Task<JsonResult> CheckExternalIdNotExists(long EmployeId , string ExternalId)
        {
            // Find out employee with the same externalId
            var employee = await CheckExternalIdNotExistsData(EmployeId, ExternalId);
            return Json(employee);
        }

        public async Task<Employe> CheckExternalIdNotExistsData(long EmployeId, string ExternalId)
        {
            var employee = await (from e in _context.Employe
                                  where e.Id != EmployeId && e.ExternalId == ExternalId
                                  select e).FirstOrDefaultAsync();
            return employee;
        }

    }
}
