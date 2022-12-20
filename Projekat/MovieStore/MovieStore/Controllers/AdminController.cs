using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStore.Areas.Identity.Data;
using MovieStore.Models;
using System.Collections;
using System.Data;
using System.Security.Claims;
using System.Xml.Linq;

namespace MovieStore.Controllers
{
    [Authorize(Policy = "AdminRolePolicy")]
    public class AdminController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Roles()
        {
            return View();
        }

        // DELETE method
        public async Task<IActionResult> Delete(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} can't be found";
                return View("NotFound");
            }

            await _userManager.DeleteAsync(user);
            return RedirectToAction("Users", "Admin");
        }

        [HttpGet]
        public IActionResult Users()
        {
            var users = _userManager.Users;
            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} can't be found";
                return View("NotFound");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);

            var userModel = new UserClaimsViewModel
            {
                UserId = user.Id
            };

            foreach (var claim in ClaimsStore.TotalClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type
                };

                if (userClaims.Any(x => x.Type == claim.Type))
                {
                    userClaim.isSelected = true;
                }

                userModel.UserClaims.Add(userClaim);
            }

            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} can't be found";
                return View("NotFound");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            var result = await _userManager.RemoveClaimsAsync(user, claims);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't remove user claims");
                return View(model);
            }

            result = await _userManager.AddClaimsAsync(user, model.UserClaims.Where(x => x.isSelected)
                .Select(c => new Claim(c.ClaimType, c.ClaimType)));


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Can't add selected claims to the user");
                return View(model);
            }

            return RedirectToAction("Users", "Admin");

        }

        [HttpGet]
        public async Task<IActionResult> Roles(List<UserRolesViewModel> userRolesViewModels)
        {
            var allUsers = _userManager.Users;

            foreach (var user in allUsers)
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var model = new UserRolesViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    UserRoles = userRoles.ToList<string>()
                };

                userRolesViewModels.Add(model);
            }
            return View(userRolesViewModels);
        }

        [Authorize(Policy = "AddDeleteRolePolicy")]
        public IActionResult AddRole(string id)
        {
            var model = new UserRolesViewModel
            {
                UserId = id
            };

            return View(model);
        }

        [Authorize(Policy = "AddDeleteRolePolicy")]
        [HttpPost]
        public async Task<IActionResult> AddRole(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} can't be found";
                return View("NotFound");
            }


            await _userManager.AddToRoleAsync(user, model.UserRole);
            return RedirectToAction("Roles", "Admin");
        }

        [Authorize(Policy = "AddDeleteRolePolicy")]
        public IActionResult DeleteRole (string id)
        {
            var model = new UserRolesViewModel
            {
                UserId = id
            };

            return View(model);
        }

        [Authorize(Policy = "AddDeleteRolePolicy")]
        [HttpPost]
        public async Task<IActionResult> DeleteRole (UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {model.UserId} can't be found";
                return View("NotFound");
            }

            await _userManager.RemoveFromRoleAsync(user, model.UserRole);
            return RedirectToAction("Roles", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                ViewBag.ErrorMessage = $"User with Id = {id} can't be found";
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var rolesList = roles.ToList();

            var model = new UserRolesViewModel
            {
                UserId = user.Id,
                UserRoles = rolesList
            };

            return View(model);
        }
     
    }
}