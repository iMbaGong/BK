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
        [Key]
        [Column(Order = 0)]
        public decimal sender_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal reciever_id { get; set; }

        public DateTime create_time { get; set; }

        [StringLength(255)]
        public string session_detail { get; set; }

        public decimal session_status { get; set; }

        public virtual User User { get; set; }

        public virtual User User1 { get; set; }
    }
}
