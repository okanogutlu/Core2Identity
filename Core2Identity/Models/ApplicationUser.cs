﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core2Identity.Models
{
    public class ApplicationUser:IdentityUser //ApplicationUser, IdentityUser'den kalıtım aldığı için username,id,password gibi özellikleri aldı.
    {
    }
}
