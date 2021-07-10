using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class LandlordDto
    {
        public LandlordDto(Landlord landlord)
        {
            landlord_credit = landlord.landlord_credit;
            landlord_status = landlord.landlord_status;
        }
        public decimal landlord_credit { get; set; }
        public decimal landlord_status { get; set; }
    }
}