using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authAPI.DTO
{
    public class CustomerValidationModel
    {
        public int RecId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AgencyName { get; set; }
        public string GuideName { get; set; }
        public string checkoutDateHotel { get; set; }
        public string  meetingDate { get; set; }
        public string meetingPlace { get; set; }
    }
}
