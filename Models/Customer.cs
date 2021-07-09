namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.Customer")]
    public partial class Customer
    {
        [Key]
        public decimal customer_id { get; set; }

        [StringLength(50)]
        public string customer_email { get; set; }

        [StringLength(50)]
        public string customer_phone { get; set; }

        [StringLength(50)]
        public string customer_identity { get; set; }

        [StringLength(50)]
        public string customer_realname { get; set; }

        public decimal? customer_gender { get; set; }

        public virtual User User { get; set; }

        public virtual Landlord Landlord { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
