namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.Room")]
    public partial class Room
    {
        [Key]
        [Column(Order = 0)]
        public decimal room_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal wholeRoom_id { get; set; }

        public string room_picture { get; set; }

        [StringLength(255)]
        public string room_info { get; set; }

        public virtual WholeRoom WholeRoom { get; set; }
    }
}
