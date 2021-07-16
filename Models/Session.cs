namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.Session")]
    public partial class Session
    {
        public decimal sender_id { get; set; }

        public decimal receiver_id { get; set; }

        public DateTime create_time { get; set; }

        [StringLength(255)]
        public string session_detail { get; set; }

        public decimal session_status { get; set; }

        [Key]
        public decimal session_id { get; set; }

        public string session_type { get; set; }

        public virtual User Sender { get; set; }

        public virtual User Receiver { get; set; }
    }
}
