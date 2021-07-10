using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class AdminDto
    {
        public AdminDto(Admin admin)
        {
            invite_code = admin.invite_code;
        }
        public string invite_code { get; set; }
    }
}