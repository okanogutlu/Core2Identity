using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2Identity.Infrastructure
{
    //Parolaya kendi isteğimiz doğrultusunda validasyonlar koyduk.
    public class CustomPasswordValidator : IPasswordValidator<ApplicationUser> 
    {
        public Task<IdentityResult> ValidateAsync(UserManager<ApplicationUser> manager, ApplicationUser user, string password)
        {
            List<IdentityError> errors = new List<IdentityError>();
            if (password.ToLower().Contains(user.UserName.ToLower()))
            {
                errors.Add(new IdentityError()
                {
                    Code = "OKN271837",
                    Description = "kullanıcı adı ve parola aynı olamaz!"

                });
            }
            return Task.FromResult(errors.Count == 0 ?
                IdentityResult.Success :
                IdentityResult.Failed(errors.ToArray()));
        }
    }
}
