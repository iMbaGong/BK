using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class UserDto
    {
        public UserDto(User user)
        { 
            user_id = (int)user.user_id;
            user_name = user.user_name;
                
            if (user.Admin != null)
            {
                Admin = new AdminDto(user.Admin);
            }

            Customer = new CustomerDto(user.Customer);
        }
        public int user_id { get; set; }
        public string user_name { get; set; }
        
        public AdminDto Admin;
        
        public CustomerDto Customer;
    }

    
}