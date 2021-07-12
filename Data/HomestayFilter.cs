using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookingRoom.Data
{
    public class HomestayFilter
    {

        public decimal? price_max { get; set; }
        public decimal? price_min { get; set; }

        public decimal? cap_max { get; set; }
        public decimal? cap_min { get; set; }

        public decimal? area_max { get; set; }
        public decimal? area_min { get; set; }

        public decimal? by_the_street { get; set; }
        public decimal? wifi { get; set; }
        public decimal? bathtub { get; set; }

        public decimal? bed_max { get; set; }
        public decimal? bed_min { get; set; }

        public decimal? type { get; set; }

        public decimal? room_max { get; set; }
        public decimal? room_min { get; set; }
    }
}