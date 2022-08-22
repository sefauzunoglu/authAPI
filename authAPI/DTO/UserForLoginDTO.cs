using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace authAPI.DTO
{
    public class UserForLoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        //[Required]
        //public string VoucherNumber { get; set; }

        //[Required]
        //public DateTime BirthDate { get; set; }
    }
}
