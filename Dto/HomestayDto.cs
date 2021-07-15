using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class HomestayDto
    {
        public HomestayDto(Homestay homestay)
        {
            homestay_name = homestay.homestay_name;
            Landlord = new UserDto(homestay.Landlord.Customer.User);
            homestay_pic = homestay.homestay_pic;
            homestay_addr = homestay.homestay_addr;
            homestay_area = homestay.homestay_area;
            homestay_cap = homestay.homestay_cap;
            homestay_info = homestay.homestay_info;
            homestay_price = homestay.homestay_price;
            homestay_status = homestay.homestay_status;
            wifi = homestay.wifi;
            by_the_street = homestay.by_the_street;
            bathtub = homestay.bathtub;
            grade = homestay.grade;
            bed_number = homestay.bed_number;
            favorite_num = homestay.FavoriteBy.Count();

            if (homestay.WholeRoom != null)
            {
                WholeRoom = new WholeRoomDto(homestay.WholeRoom);
            }
            if (homestay.SingleRoom.Count>0)
            {
                SingleRoom = new SingleRoomDto(homestay.SingleRoom.First());
            }

            Comment = new List<CommentDto>();
            foreach (var order in homestay.Order)
            {
                if(order.Comment!=null)
                    Comment.Add(new CommentDto(order.Comment));
            }

        }
        public decimal homestay_id { get; set; }

        public string homestay_name { get; set; }

        public string homestay_pic { get; set; }

        public string homestay_addr { get; set; }

        public decimal? homestay_cap { get; set; }

        public decimal? homestay_area { get; set; }

        public string homestay_info { get; set; }

        public decimal? homestay_price { get; set; }

        public decimal? homestay_status { get; set; }

        public decimal? by_the_street { get; set; }

        public decimal? wifi { get; set; }

        public decimal? bathtub { get; set; }

        public decimal? bed_number { get; set; }

        public decimal? grade { get; set; }

        public decimal? favorite_num { get; set; }

        public UserDto Landlord { get; set; }

        public WholeRoomDto WholeRoom { get; set; }

        public SingleRoomDto SingleRoom { get; set; }

        public List<CommentDto> Comment { get; set; }

    }
}