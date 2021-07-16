using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookingRoom.Models;

namespace BookingRoom.Dto
{
    public class SessionDto
    {
        public SessionDto(Session session)
        {
            session_id = session.session_id;
            session_status = session.session_status;
            session_type = session.session_type;
            create_time = session.create_time;
            session_detail = session.session_detail;
            Sender = new UserDto(session.Sender);
            Receiver = new UserDto(session.Receiver);
        }
        public decimal session_id { get; set; }
        public DateTime create_time { get; set; }
       
        public string session_detail { get; set; }

        public decimal session_status { get; set; }

        public string session_type { get; set; }

        public UserDto Sender { get; set; }

        public UserDto Receiver { get; set; }
    }
}