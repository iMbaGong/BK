namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.Homestay")]
    public partial class Homestay
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Homestay()
        {
            SingleRoom = new HashSet<SingleRoom>();
            Order = new HashSet<Order>();
            FavoriteBy = new HashSet<Tenant>();
        }

        [Key]
        public decimal homestay_id { get; set; }

        public decimal landlord_id { get; set; }

        [Required]
        [StringLength(255)]
        public string homestay_name { get; set; }

        public string homestay_pic { get; set; }

        [Required]
        [StringLength(255)]
        public string homestay_addr { get; set; }

        public decimal? homestay_cap { get; set; }

        public decimal? homestay_area { get; set; }

        [StringLength(500)]
        public string homestay_info { get; set; }

        public decimal? homestay_price { get; set; }

        public decimal? homestay_status { get; set; }

        public decimal? by_the_street { get; set; }

        public decimal? wifi { get; set; }

        public decimal? bathtub { get; set; }

        public decimal? bed_number { get; set; }

        public decimal? grade { get; set; }

        public virtual Landlord Landlord { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SingleRoom> SingleRoom { get; set; }

        public virtual WholeRoom WholeRoom { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Order> Order { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tenant> FavoriteBy { get; set; }

    }
}

