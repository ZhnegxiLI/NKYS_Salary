using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NKYS.Account.Model;
using NKYS.Models;
using NKYS.Models.ViewModel;

namespace NKYS.Controllers
{
    public class GroupsController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public GroupsController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Groups
        public async Task<IActionResult> Index(long? DepartmentId)
        {
            var obj = new GroupsIndex();
            if (DepartmentId == null)
            {
                ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name");
            }
            else
            {
                ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", DepartmentId);
                var list = _context.Groups.Where(p => p.DepartmentId == DepartmentId).Include(g => g.Department).Include(g => g.Employes);
                obj.Groups = await list.ToListAsync();
            }

            return View(obj);
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

            group.SharePropotion = group.SharePropotion * 100;
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", group.DepartmentId);
            return View(group);
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(Groups groups)
        {
            if (ModelState.IsValid)
            {
                groups.SharePropotion = groups.SharePropotion / 100;
                if (groups.Id != null && groups.Id > 0)
                {
                    groups.UpdatedBy = Convert.ToInt64(_userManager.GetUserId(HttpContext.User));
                    _context.Update(groups);
                }
                else
                {
                    groups.CreatedBy = Convert.ToInt64(_userManager.GetUserId(HttpContext.User));
                    groups.CreatedOn = DateTime.Now;
                    _context.Add(groups);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { DepartmentId = groups.DepartmentId});
            }
            ViewData["DepartmentId"] = new SelectList(_context.Department, "Id", "Name", groups.DepartmentId);
            return View(groups);
        }

        // API
       [HttpGet]
        public async Task<JsonResult> FindGroupList(long? DepartmentId)
        {
            return Json(await FindGroupListData(DepartmentId));
        }

        public async Task<List<Groups>> FindGroupListData(long? DepartmentId)
        {
            var groups = await(from g in _context.Groups
                               where (DepartmentId == null || g.DepartmentId == DepartmentId)
                               select g).ToListAsync();
            return groups;
        }
    }
}
