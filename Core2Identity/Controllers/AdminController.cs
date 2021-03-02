using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Core2Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private IPasswordHasher<ApplicationUser> passhasher; //Şifreyi değiştirmek için kullanacağımız interface.
        private IPasswordValidator<ApplicationUser> passvalidator;//Bu interface'i kendimiz oluşturmuştuk. Şifrenin koyduğumuz kurallara uygun mu
        //yoksa uygun değil mi, bunu kontrol ediyoruz.
        public AdminController(UserManager<ApplicationUser> _userManager, IPasswordHasher<ApplicationUser> _passhasher, IPasswordValidator<ApplicationUser> _passvalid)
        {
            userManager = _userManager;
            passhasher = _passhasher;
            passvalidator = _passvalid;
        }
        public IActionResult Index()
        {

            return View(userManager.Users);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser usr = new ApplicationUser();
                usr.Email = model.Email;
                usr.UserName = model.UserName;
                var result = await userManager.CreateAsync(usr, model.Password);
                if (result.Succeeded)
                {
                    RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(model);
        }
        
        public async Task<IActionResult> Delete(string Id)//Post methoduna gerek yok, çünkü değişiklik yapmayacağız,direkt sileceğiz.
        {
            var user = await userManager.FindByIdAsync(Id);//kullanıcıyı userManager aracılığıyla aldık.
            var result = await userManager.DeleteAsync(user);// kullanıcıyı sildik.
            if (user != null)
            {
                if (result.Succeeded)
                {
                    return View("Index", userManager.Users);
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "user not found");
            }
            return View("Index", userManager.Users);


        }


        [HttpGet] 
        public async Task<IActionResult> Update(string Id)
        {
            var user = await userManager.FindByIdAsync(Id);
            return View("Update", user);
        }


        [HttpPost]
        public async Task<IActionResult> Update(string Id, string Password, string Email)//cshtml formundan Id(disabled),Password ve Email alıyoruz.
        {
            var user = await userManager.FindByIdAsync(Id);//Id ile kullanıcıyı bulduk.
            if (user != null)
            {
                user.Email = Email; //Email'i değiştirdik.
                IdentityResult validPass = null; //Parolanın doğruluk kontrolünü yapacağız.
                if (!string.IsNullOrEmpty(Password))
                {
                    validPass = await passvalidator.ValidateAsync(userManager, user, Password);//Infrasucture içindeki methodumuza parametrelerle
                    //birlikte gönderdik ve kontrol sağladık.

                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = passhasher.HashPassword(user, Password);//Password hasher kullanarak şifreyi değiştiriyoruz
                    }
                    else
                    {
                        foreach (var item in validPass.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
                if (validPass.Succeeded)
                {
                    var result = await userManager.UpdateAsync(user);//Güncelleme işlemini yaptık.
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    else
                    {
                        foreach (var item in validPass.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
        }




    }
}
