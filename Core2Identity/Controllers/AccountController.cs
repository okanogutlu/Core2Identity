using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core2Identity.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core2Identity.Controllers
{
    [Authorize] //Tüm methodlar için authorize gerekiyor..
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager;//Kullanıcı bilgilerini almak için
        private SignInManager<ApplicationUser> signinManager; //Kullanıcıyı sisteme authotenticate etmek için
        
        
        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signinManager)
        {
            userManager = _userManager;
            signinManager = _signinManager;
        }
        //Default Route olan AccountController/Login sayfamızı, Startup.cs üzerinden değiştirebiliriz.
        // services.configureApplicationCookie(opt =>opt.LoginPath="Member/Login");
        
        
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken] //Güvenlik için
        public async Task<IActionResult> Login(LoginModel account,string returnUrl )
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(account.Email);
                if (user!=null)
                {
                    await signinManager.SignOutAsync();
                    var result = await signinManager.PasswordSignInAsync(user, account.Password, false,true);
                    if (result.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/"); //returnURL==null ise ana sayfaya gidecek. Null'a eşit değilse nerede kaldıysa oraya gidecek
                        //(bunun ayarlanması gerek, şuan null)
                    }
                }
                ModelState.AddModelError("Email", "invalid Email or Password");
            }
          
            return View();
        }

        
        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync(); //kullanıcıyı logout yapar.
            return RedirectToAction("Index","Home");
        }
    }
}
