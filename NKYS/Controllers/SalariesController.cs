using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NKYS.Account.Model;
using NKYS.Models;
using NKYS.Models.ViewModel;

namespace NKYS.Controllers
{
    public class SalariesController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public SalariesController(Context context, UserManager<User> userManager)
        {
            _context = context;
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


        public async Task<IActionResult> SalariesValidation(long? DepartmentId, long? GroupsId, long? PeriodId, bool? Validity)
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
        public async Task<JsonResult> SalariesSearch(long? DepartmentId, long? GroupsId, long? CycleId, bool? Validity)
        {
            var salaries = await (from s in _context.Salary
                                  join e in _context.Employe on s.EmployeId equals e.Id
                                  join g in _context.Groups on e.GroupsId equals g.Id
                                  where (DepartmentId == null || g.DepartmentId == DepartmentId) && (GroupsId == null || g.Id == GroupsId)
                                  && (CycleId == null || s.CycleId == CycleId) && (Validity == null || s.Validity == Validity)
                                  select s).Include(s => s.Cycle).Include(s => s.Employe.Groups).ToListAsync();
            return Json(salaries);
        }

        
        [HttpPost]
        public async Task<long> SalariesValidation(long SalaryId)
        {
            var salary = _context.Salary.Where(p => p.Id == SalaryId).FirstOrDefault();

            if (salary!=null)
            {
                salary.ValidatedBy = Convert.ToInt64(_userManager.GetUserId(HttpContext.User));
                salary.ValidatedOn = DateTime.Now;
                salary.Validity = true;
                _context.Update(salary);
                await _context.SaveChangesAsync();
                return salary.Id;
            }
            return 0;
        }

        [HttpPost]
        public async Task<int> SaveSalaries(List<Salary> salariesList)
        {

            var NumberOfModifiedSalaries = 0;
            if (salariesList.Count()>0)
            {
                List<Salary> salariesToUpdate = new List<Salary>();
                foreach (var salaryToInsertOrUpdate in salariesList)
                {
                    if (salaryToInsertOrUpdate.Id>0)
                    {
                        // Update
                        salaryToInsertOrUpdate.UpdatedBy = Convert.ToInt64(_userManager.GetUserId(HttpContext.User));
                        salaryToInsertOrUpdate.Cycle = null;
                        salaryToInsertOrUpdate.Employe = null;
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


        [HttpPost]
        public JsonResult CalculSalaries(long CycleId, long? EmployeeId, bool IsUpdate)
        {

            var cycleId = new SqlParameter("CycleId", CycleId);
            var isUpdate = new SqlParameter("IsUpdate", IsUpdate);
            var employeeId = new SqlParameter("EmployeeId", DbType.Int64);
            employeeId.Value = (object)EmployeeId ?? DBNull.Value;

            var userId = new SqlParameter("UserId", Convert.ToInt64(_userManager.GetUserId(HttpContext.User)));
            //NumberOfModifiedSalaries = _context.Database.ExecuteSqlRaw("EXECUTE dbo.SP_CalculSalaries @CycleId, @IsUpdate", cycleId, IsUpdate);

            var salaryResult = _context.SalariesCalculModel.FromSqlRaw("EXECUTE dbo.SP_CalculSalaries @CycleId, @UserId, @EmployeeId, @IsUpdate", cycleId, userId, employeeId, isUpdate).ToList();
            return Json(salaryResult);
        }




        //[HttpPost]
        //public JsonResult GetSalariesValidationList(long? DepartmentId, long? GroupsId, long? CycleId)
        //{
        //    var cycleId = new SqlParameter("CycleId", CycleId);
        //    var departmentId = new SqlParameter("DepartmentId", DepartmentId);
        //    var groupId = new SqlParameter("GroupsId", GroupsId);

        //    var salaryResult = _context.SalariesCalculModel.FromSqlRaw("EXECUTE dbo.SP_CalculSalaries @CycleId, @UserId, @EmployeeId, @IsUpdate", cycleId, userId, employeeId, isUpdate).ToList();
        //    return Json(salaryResult);
        //}
        



        [HttpGet]
        public async Task<JsonResult> CheckSalaryCalculValidity(long CycleId)
        {

            // Step 1: Check all production value is inputed 
            var notInputProductionValueList = await GetNotInputedProductionValue(CycleId);
            // Step 2: Check working hours of all employee is inputed 
            var notInputSalaryList = await GetNotInputedSalary(CycleId);

            return Json(new { 
                ProductionValueList = notInputProductionValueList,
                EmployeeList = notInputSalaryList
            });
        }


        private async Task<List<string>> GetNotInputedProductionValue(long CycleId)
        {
           
            var productionValueList = await _context.ProductionValue.Where(p => p.CycleId == CycleId).ToListAsync();
            var productionValueTypeList = Enum.GetValues(typeof(ProductionValueType)).Cast<ProductionValueType>().Select(p => new { Type = p.ToString(), Value = p }).ToList();
            // Get not inputed productionValue type 
            var notInputProductionValueList = (from type in productionValueTypeList
                                               from list in productionValueList.Where(x => x.ProductionValueTypeId == type.Value).DefaultIfEmpty()
                                               where list == null
                                               select type.Type).ToList();

            return notInputProductionValueList;
        }

        private async Task<List<Employe>> GetNotInputedSalary(long CycleId)
        {

            var employeeList = await _context.Employe.Where(p => p.DepartDate == null).ToListAsync();
            var salaryList = await _context.Salary.Where(p => p.CycleId == CycleId).ToListAsync();

            // Get not inputed employee's salary 
            var notInputSalaryList = (from e in employeeList
                                      from s in salaryList.Where(x => x.EmployeId == e.Id).DefaultIfEmpty()
                                      where s == null
                                      select e).ToList();

            return notInputSalaryList;
        }

    }
}
