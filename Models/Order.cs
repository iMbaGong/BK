namespace BookingRoom.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("C##BK.Order")]
    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            CreditRecord = new HashSet<CreditRecord>();
        }

        [Key]
        public decimal order_id { get; set; }

        public decimal tenant_id { get; set; }

        public decimal homestay_id { get; set; }

        public DateTime start_time { get; set; }

        public DateTime expire_time { get; set; }

        public DateTime create_time { get; set; }

        public decimal payings { get; set; }

        public decimal paying_status { get; set; }

        public virtual Comment Comment { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CreditRecord> CreditRecord { get; set; }

        public virtual Homestay Homestay { get; set; }

        public virtual Tenant Tenant { get; set; }
    }
}
