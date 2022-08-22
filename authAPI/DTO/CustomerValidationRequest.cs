using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authAPI.DTO
{
    public class CustomerValidationRequest
    {
        public string Voucher { get; set; }
        public string BirthDate { get; set; }
    }
}
