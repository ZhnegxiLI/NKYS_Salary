using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using NKYS.Account.Model;
using NKYS.Models;
using NKYS.Models.ViewModel;

namespace NKYS.Controllers
{
    public class SalariesController : Controller
    {
        private readonly Context _context;
        private readonly GroupsController _GroupController;
        private readonly UserManager<User> _userManager;

        public SalariesController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _GroupController = new GroupsController(context);
            _userManager = userManager;
        }

        // GET: Salaries
        public async Task<IActionResult> SalarieSearch(long? DepartmentId, long? GroupsId, long? PeriodId)
        {
            var obj = new SalarieSearchModel();
            var departments = await _context.Department.ToListAsync();
            var periods = await _context.Cycle.ToListAsync();
            var GroupList = new List<Groups>();

            ViewData["Departments"] = new SelectList(departments, "Id", "Name", null);
            ViewData["Periods"] = new SelectList(periods, "Id", "Label", null);
            ViewData["Groups"] = new SelectList(GroupList, "Id", "Name", null);
            
            return View(obj);
        }

       
        // Api 
        [HttpGet]
        public async Task<JsonResult> SalariesSearch(long? DepartmentId, long? GroupsId, long? CycleId)
        {
            var salaries = await (from s in _context.Salary
                                  join e in _context.Employe on s.EmployeId equals e.Id
                                  join g in _context.Groups on e.GroupsId equals g.Id
                                  where (DepartmentId == null || g.DepartmentId == DepartmentId) && (GroupsId == null || g.Id == GroupsId)
                                  && (CycleId == null || s.CycleId == CycleId)
                                  select s).Include(s => s.Cycle).Include(s => s.Employe).ToListAsync();
            return Json(salaries);
        }

        [HttpPost]
        public async Task<int> SaveSalaries(List<Salary> salariesList)
        {

            var NumberOfModifiedSalaries = 0;
            if (salariesList.Count()>0)
            {
                foreach (var salaryToInsertOrUpdate in salariesList)
                {
                    if (salaryToInsertOrUpdate.Id>0)
                    {
                        // Update
                        salaryToInsertOrUpdate.UpdatedBy = Convert.ToInt64(_userManager.GetUserId(HttpContext.User));
                        _context.Update(salaryToInsertOrUpdate);
                    }
                    else
                    {
                        //Insert 
                        salaryToInsertOrUpdate.CreatedOn = DateTime.Now;
                        salaryToInsertOrUpdate.CreatedBy = Convert.ToInt64(_userManager.GetUserId(HttpContext.User));
                        await _context.AddAsync(salaryToInsertOrUpdate);
                    }
                }
                NumberOfModifiedSalaries =  await _context.SaveChangesAsync();
            }
            return NumberOfModifiedSalaries;
        }
    }
}
