namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.SingleRoom")]
    public partial class SingleRoom
    {
        [Key]
        [Column(Order = 0)]
        public decimal singleRoom_id { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal bed_id { get; set; }

        public decimal? bed_type { get; set; }

        public virtual Homestay Homestay { get; set; }
    }
}
