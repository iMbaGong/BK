using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class TenantDto
    {
        public TenantDto(Tenant tenant)
        {
            tenant_credit = tenant.tenant_credit;
        }
        public decimal tenant_credit { get; set; }
    }
}