using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using TradesCompany.Application.DTOs;
using TradesCompany.Domain.Entities;

namespace TradesCompany.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Quotation> Quotations { get; set; }
        public DbSet<ServiceMan> ServiceMan { get; set; }
        public DbSet<QuotationForm> QuotationForms { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<ServiceSchedule> ServiceSchedules { get; set; }
        public DbSet<ServiceType> serviceTypes { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<Notification> Notification {  get; set; }
        public DbSet<Schedule> Schedule { get; set; }

        // For Store Procedure
        public DbSet<UsersWithRole> UsersWithRole { get; set; }
        public DbSet<ServiceManByServiceType> ServiceManByServiceType { get; set; }
        public DbSet<QuotationByUser> QuotationByUser { get; set; }
        public DbSet<QuotationByServicerMan> QuotationByServicerMan { get; set; }
        public DbSet<ScheduleServiceByUser> ScheduleServiceByUser { get; set; }
        public DbSet<BookingByServiceType> BookingByServiceType { get; set; }
        public DbSet<ScheduleServiceByEmployee> ScheduleServiceByEmployee  { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UsersWithRole>().HasNoKey();
            builder .Entity<ServiceManByServiceType>().HasNoKey();
            builder.Entity<QuotationByUser>().HasNoKey();
            builder.Entity<QuotationByServicerMan>().HasNoKey();
            builder.Entity<ScheduleServiceByUser>().HasNoKey();
            builder.Entity<BookingByServiceType>().HasNoKey();
            builder.Entity<ScheduleServiceByEmployee>().HasNoKey();

            // Configure all relationships FIRST
            // Booking -> User (many-to-one)
            builder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            // Address -> User (many-to-one)
            builder.Entity<Address>()
                .HasOne(a => a.User)
                .WithMany(u => u.Addresses)
                .HasForeignKey(a => a.UserId);

            // Quotation -> Booking (many-to-one)
            builder.Entity<Quotation>()
                .HasOne(q => q.Booking)
                .WithMany(b => b.Quotations)
                .HasForeignKey(q => q.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // Quotation -> ServiceMan (many-to-one)
            builder.Entity<Quotation>()
                .HasOne(q => q.ServiceMan)
                .WithMany(sm => sm.Quotations)
                .HasForeignKey(q => q.ServiceManId)
                .OnDelete(DeleteBehavior.Restrict);


            // ServiceSchedule -> Booking
            builder.Entity<ServiceSchedule>()
                .HasOne(ss => ss.Booking)
                .WithMany(b => b.ServiceSchedules)
                .HasForeignKey(ss => ss.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceSchedule -> Quotation
            builder.Entity<ServiceSchedule>()
                .HasOne(ss => ss.Quotation)
                .WithMany()
                .HasForeignKey(ss => ss.QuotationId)
                .OnDelete(DeleteBehavior.SetNull);

            // ServiceSchedule -> ServiceMan
            builder.Entity<ServiceSchedule>()
                .HasOne(ss => ss.ServiceMan)
                .WithMany(sm => sm.ServiceSchedules)
                .HasForeignKey(ss => ss.ServiceManId)
                .OnDelete(DeleteBehavior.SetNull);

            // ServiceSchedule -> Bill (optional)
            builder.Entity<ServiceSchedule>()
                .HasOne(ss => ss.Bill)
                .WithMany()
                .HasForeignKey(ss => ss.BillId)
                .OnDelete(DeleteBehavior.SetNull);

            // Rating -> ServiceSchedule
            builder.Entity<Rating>()
                .HasOne(r => r.ServiceSchedule)
                .WithOne(ss => ss.Rating)
                .HasForeignKey<Rating>(r => r.ServiceScheduleId);  
        }
    }
}
