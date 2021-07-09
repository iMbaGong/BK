using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BookingRoom.Models
{
    public partial class OracleDbContext : DbContext
    {
        public OracleDbContext()
            : base("name=OracleDbContext")
        {
        }

        public virtual DbSet<Admin> Admin { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<CreditRecord> CreditRecord { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Homestay> Homestay { get; set; }
        public virtual DbSet<Landlord> Landlord { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<SingleRoom> SingleRoom { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<WholeRoom> WholeRoom { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .Property(e => e.admin_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Admin>()
                .Property(e => e.invite_code)
                .IsUnicode(false);

            modelBuilder.Entity<Admin>()
                .HasMany(e => e.CreditRecord)
                .WithRequired(e => e.Admin)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.order_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Comment>()
                .Property(e => e.grade)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Comment>()
                .Property(e => e.detail)
                .IsUnicode(false);

            modelBuilder.Entity<Comment>()
                .Property(e => e.comment_status)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CreditRecord>()
                .Property(e => e.order_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CreditRecord>()
                .Property(e => e.admin_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CreditRecord>()
                .Property(e => e.revising_reason)
                .IsUnicode(false);

            modelBuilder.Entity<CreditRecord>()
                .Property(e => e.landlord_credit)
                .HasPrecision(38, 0);

            modelBuilder.Entity<CreditRecord>()
                .Property(e => e.user_credit)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customer_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customer_email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customer_phone)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customer_identity)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customer_realname)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.customer_gender)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Customer>()
                .HasOptional(e => e.Landlord)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Customer>()
                .HasOptional(e => e.Tenant)
                .WithRequired(e => e.Customer)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.landlord_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_name)
                .IsUnicode(false);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_pic)
                .IsUnicode(false);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_addr)
                .IsUnicode(false);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_cap)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_area)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_info)
                .IsUnicode(false);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_price)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.homestay_status)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.by_the_street)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.wifi)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.bathtub)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.bed_number)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .Property(e => e.grade)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Homestay>()
                .HasMany(e => e.SingleRoom)
                .WithRequired(e => e.Homestay)
                .HasForeignKey(e => e.singleRoom_id);

            modelBuilder.Entity<Homestay>()
                .HasOptional(e => e.WholeRoom)
                .WithRequired(e => e.Homestay)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Homestay>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.Homestay)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Landlord>()
                .Property(e => e.landlord_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Landlord>()
                .Property(e => e.landlord_credit)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Landlord>()
                .Property(e => e.landlord_status)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Landlord>()
                .HasMany(e => e.Homestay)
                .WithRequired(e => e.Landlord)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Order>()
                .Property(e => e.order_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.tenant_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.homestay_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.payings)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Order>()
                .Property(e => e.paying_status)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Order>()
                .HasOptional(e => e.Comment)
                .WithRequired(e => e.Order)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Room>()
                .Property(e => e.room_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Room>()
                .Property(e => e.wholeRoom_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Room>()
                .Property(e => e.room_picture)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.room_info)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.sender_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Session>()
                .Property(e => e.reciever_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Session>()
                .Property(e => e.session_detail)
                .IsUnicode(false);

            modelBuilder.Entity<Session>()
                .Property(e => e.session_status)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SingleRoom>()
                .Property(e => e.singleRoom_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SingleRoom>()
                .Property(e => e.bed_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<SingleRoom>()
                .Property(e => e.bed_type)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.tenant_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Tenant>()
                .Property(e => e.tenant_credit)
                .HasPrecision(38, 0);

            modelBuilder.Entity<Tenant>()
                .HasMany(e => e.Order)
                .WithRequired(e => e.Tenant)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Tenant>()
                .HasMany(e => e.Homestay)
                .WithMany(e => e.Tenant)
                .Map(m => m.ToTable("Favourite", "C##BK").MapLeftKey("tenant_id").MapRightKey("homestay_id"));

            modelBuilder.Entity<User>()
                .Property(e => e.user_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<User>()
                .Property(e => e.user_name)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Admin)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .HasOptional(e => e.Customer)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete();

            modelBuilder.Entity<User>()
                .HasMany(e => e.SessionSend)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.sender_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.SessionRecv)
                .WithRequired(e => e.User1)
                .HasForeignKey(e => e.reciever_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<WholeRoom>()
                .Property(e => e.wholeRoom_id)
                .HasPrecision(38, 0);

            modelBuilder.Entity<WholeRoom>()
                .Property(e => e.room_num)
                .HasPrecision(38, 0);
        }
    }
}
