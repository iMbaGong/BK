using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class RoomDto
    {
        public RoomDto(Room room)
        {
            room_picture = room.room_picture;
            room_info = room.room_info;
        }
        public string room_picture { get; set; }

        public string room_info { get; set; }
    }
}