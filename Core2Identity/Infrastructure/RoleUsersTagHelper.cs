using Core2Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2Identity.Infrastructure
{
    [HtmlTargetElement("td", Attributes="identity-role")]//Html sayfasındaki ilgili td alanının identity-role'u
    public class RoleUsersTagHelper:TagHelper
    {
        private UserManager<ApplicationUser> UserManager;//User isimlerini çekmek için
        private RoleManager<IdentityRole> RoleManager;//rolleri çekmek için

        public RoleUsersTagHelper(UserManager<ApplicationUser> _UserManager , RoleManager<IdentityRole> _RoleManager)
        {
            RoleManager = _RoleManager;
            UserManager = _UserManager;
        }
       
        [HtmlAttributeName("identity-role")] //Kullanıcı cshtml sayfasında butona basında o attribute buraya atanacak.
        public string Role { get; set; }

        public override async Task ProcessAsync (TagHelperContext context, TagHelperOutput output)
        {
            List<string> names =new List<string>();
            var role = await RoleManager.FindByIdAsync(Role);
            if (role != null)
            {
                foreach (var user in UserManager.Users)
                {
                    if (user != null && await UserManager.IsInRoleAsync(user, role.Name))
                    {
                        names.Add(user.UserName);
                    }
                }
            }
            output.Content.SetContent(names.Count == 0 ? "no user" : string.Join(", ", names));
            
        }
    }
}
