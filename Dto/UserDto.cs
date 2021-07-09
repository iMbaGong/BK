using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Dto
{
    public class UserDto
    {
        public int user_id { get; set; }
        public string user_name { get; set; }
        public class AdminDto
        {
            public string invite_code { get; set; }
        }
        public AdminDto Admin;
        public class CustomerDto
        {
            public string customer_email { get; set; }
            public string customer_phone { get; set; }
            public string customer_realname { get; set; }
            public string customer_identity { get; set; }
            public decimal? customer_gender { get; set; }
            public class TenantDto
            {
                public decimal tenant_credit { get; set; }
            }
            public class LandlordDto
            {
                public decimal landlord_credit { get; set; }
                public decimal landlord_status { get; set; }
            }
            public TenantDto Tenant;
            public LandlordDto Landlord;
        }
        public CustomerDto Customer;
    }

    
}