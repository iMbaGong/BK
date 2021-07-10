using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class SingleRoomDto
    {
        public SingleRoomDto(SingleRoom room)
        {
            bed_id = room.bed_id;
            bed_type = room.bed_type;
        }
        public decimal bed_id { get; set; }
        public decimal? bed_type { get; set; }
    }
}