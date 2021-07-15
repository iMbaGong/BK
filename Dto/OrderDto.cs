using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class OrderDto
    {
        public OrderDto(Order order)
        {
            order_id = order.order_id;
            Tenant = new UserDto(order.Tenant.Customer.User);
            Homestay = new HomestayDto(order.Homestay);
            start_time = order.start_time;
            expire_time = order.expire_time;
            create_time = order.create_time;
            payings = order.payings;
            if(order.Comment!=null)
                Comment = new CommentDto(order.Comment);
        }

        public decimal order_id { get; set; }

        public UserDto Tenant;

        public HomestayDto Homestay;

        public DateTime start_time { get; set; }

        public DateTime expire_time { get; set; }

        public DateTime create_time { get; set; }

        public decimal payings { get; set; }

        public decimal paying_status { get; set; }

        public CommentDto Comment { get; set; }

    }
}