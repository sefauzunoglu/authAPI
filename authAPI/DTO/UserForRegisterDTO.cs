using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace authAPI.DTO
{
    public class UserForRegisterDTO
    {
            [Required]
            public string Name { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            //public string VoucherNumber { get; set; }
            //public DateTime BirthDate { get; set; }

    }
}
