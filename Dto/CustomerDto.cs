using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class CustomerDto
    {
        public CustomerDto(Customer customer)
        {
            customer_email = customer.customer_email;
            customer_phone = customer.customer_phone;
            customer_identity = customer.customer_identity;
            customer_realname = customer.customer_realname;
            customer_gender = customer.customer_gender;
            Tenant = new TenantDto(customer.Tenant);
            if (customer.Landlord != null)
            {
                Landlord = new LandlordDto(customer.Landlord);
            }
        }
        public string customer_email { get; set; }
        public string customer_phone { get; set; }
        public string customer_realname { get; set; }
        public string customer_identity { get; set; }
        public decimal? customer_gender { get; set; }
        
        public TenantDto Tenant;

        public LandlordDto Landlord;
    }
}