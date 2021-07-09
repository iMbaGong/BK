namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.CreditRecord")]
    public partial class CreditRecord
    {
        [Key]
        [Column(Order = 0)]
        public decimal order_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal admin_id { get; set; }

        public DateTime revising_time { get; set; }

        [StringLength(255)]
        public string revising_reason { get; set; }

        public decimal landlord_credit { get; set; }

        public decimal user_credit { get; set; }

        public virtual Admin Admin { get; set; }

        public virtual Order Order { get; set; }
    }
}
