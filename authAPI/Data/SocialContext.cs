using authAPI.models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authAPI.Data
{
    public class SocialContext : IdentityDbContext<User, Role, int>
    { 
        public SocialContext(DbContextOptions<SocialContext> options) : base(options)
        {

        }
    }
}
