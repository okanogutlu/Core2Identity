using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core2Identity.Controllers
{
    public class AdminRoleController : Controller
    {

        private RoleManager<IdentityRole> roleManager;//Kullanıcı role bilgilerini almak için
        //private SignInManager<ApplicationUser> signinManager; //Kullanıcıyı sisteme authotenticate etmek için
        public UserManager<ApplicationUser> userManager { get; set; }

        public AdminRoleController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            roleManager = _roleManager;
            userManager = _userManager;
        }


        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (ModelState.IsValid)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("",item.Description);
                    }
                }
            }
            return View(name);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (var userId in model.IdsToAdd ?? new string[] { })//Bizden string istediği için böyle yaptık
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.AddToRoleAsync(user,model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                foreach (var userId in model.IdsToDelete ?? new string[] { })//Bizden string istediği için böyle yaptık
                {
                    var user = await userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);
                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }
            }
            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit", model.RoleId);
            }
        
        }

        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);
            var members = new List<ApplicationUser>();
            var nonmembers = new List<ApplicationUser>();

            foreach (var user in userManager.Users)
            {
                var list = await userManager.IsInRoleAsync(user, role.Id) ? members : nonmembers; //Eğer ki true ise "list" değişkleni member olacak,
                //false ise nonmembers olacak ve=
                list.Add(user);//buradan o kişi o gruba eklenecek
            }
            var model = new RoleDetails()
            {
                Role = role,
                Members = members,
                NonMembers = nonmembers
            };
        return View(model);
        
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    TempData["message"] = $"{role.Name} silindi.";
                    return RedirectToAction("Index");
                }else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
               
            }
            return RedirectToAction("Index");
        }
    }
}
