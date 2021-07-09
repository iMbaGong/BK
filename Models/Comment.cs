namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.Comment")]
    public partial class Comment
    {
        [Key]
        public decimal order_id { get; set; }

        public DateTime create_time { get; set; }

        public decimal? grade { get; set; }

        [StringLength(255)]
        public string detail { get; set; }

        public decimal comment_status { get; set; }

        public virtual Order Order { get; set; }
    }
}
