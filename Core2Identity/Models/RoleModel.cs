using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2Identity.Models
{
    public class RoleDetails
    {
        public IdentityRole Role { get; set; } //Detay Sayfasına bir rol gelecek.Detay sayfasında bu rolün ID ve Name'ini hidden olarak oluşturacağız.
        public IEnumerable<ApplicationUser> Members { get; set; } //İlgili roldeki kullanıcılar
        public IEnumerable<ApplicationUser> NonMembers { get; set; } //ilgili role dahil olmayan, aday kullanıcılar.
        
    }
    public class RoleEditModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string[] IdsToAdd { get; set; } //Checkbox ile seçilen kullanıcıların IDleri eklenecek
        public string[] IdsToDelete { get; set; }//Burdan da o role içerisinden kullanıcılar sillinecek.

    }
}
