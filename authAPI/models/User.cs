using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authAPI.models
{
    public class User : IdentityUser <int> // her eklenen kullanıcı için bunlar 1 2 3 diye gelsin.
    {
        public string Name { get; set; }
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

    }
}
