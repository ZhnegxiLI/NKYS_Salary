using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NKYS.Account.Model;
using NKYS.Models;
using NKYS.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NKYS.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class UserController : Controller
    {
        private readonly Context _context;
        private readonly UserManager<User> _userManager;

        public UserController(Context context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult UserSearch()
        {
            var userList = (from u in _context.Users
                            select new UserSearchModel()
                            {
                                Id = u.Id,
                                Username = u.UserName,
                                Validity = u.Validity,
                                Role = (from r in _context.Roles
                                        join ur in _context.UserRoles on r.Id equals ur.RoleId
                                        where ur.UserId == u.Id
                                        select r.Label).FirstOrDefault()
                            }
                           ).ToList();

            return View(userList);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(UserUpdateOrCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var createOrUpdateUser = _context.Users.Find(model.Id);

                if (createOrUpdateUser!=null)
                {
                    createOrUpdateUser.Validity = model.Validity;

                    _context.Update(createOrUpdateUser);

                    await _context.SaveChangesAsync();

                    var role = _context.Roles.Where(p => p.Id == model.RoleId).FirstOrDefault();
                    if (role != null)
                    {
                        var previousRole = _context.UserRoles.Where(p => p.UserId == createOrUpdateUser.Id).ToList();
                        _context.UserRoles.RemoveRange(previousRole);
                        await _context.SaveChangesAsync();
                        await _userManager.AddToRoleAsync(createOrUpdateUser, role.Name);
                    }
                    return RedirectToAction(nameof(UserSearch));
                }
            
            }

            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Label", model.RoleId);
            return View(model);
        }

        public async Task<IActionResult> CreateOrEdit(long? id)
        {
            UserUpdateOrCreateModel user = new UserUpdateOrCreateModel();

            if (id == null)
            {
                ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name");
                ViewData["Action"] = "Create";
                return View(user);
            }

            var userModel = await _context.Users.FindAsync(id);


            ViewData["Action"] = "Update";
            if (userModel == null)
            {
                return NotFound();
            }
            var userRoleModel = _context.UserRoles.Where(p => p.UserId == id).FirstOrDefault();
            var roleList = _context.Roles.ToList();
            if (userRoleModel !=null)
            {
                ViewData["RoleId"] = new SelectList(roleList, "Id", "Label", userRoleModel.RoleId);
                user.RoleId = userRoleModel.RoleId;
            } 
            else
            {
                ViewData["RoleId"] = new SelectList(roleList, "Id", "Label");
            }

            user.Id = userModel.Id;
            user.Username = userModel.UserName;
            user.Validity = userModel.Validity;
    

            return View(user);

        }
    }
}
