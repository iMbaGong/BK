using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class WholeRoomDto
    {
        public WholeRoomDto()
        {
            Room = new HashSet<RoomDto>();
        }

        public WholeRoomDto(WholeRoom wholeRoom)
        {
            room_num = wholeRoom.room_num;
            Room = new HashSet<RoomDto>();
            foreach(var room in wholeRoom.Room)
            {
                Room.Add(new RoomDto(room));
            }
        }

        public decimal? room_num { get; set; }

        public ICollection<RoomDto> Room { get; set; }

        
    }
}