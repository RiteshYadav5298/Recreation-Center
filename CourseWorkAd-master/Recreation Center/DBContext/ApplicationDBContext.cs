using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using CourseWorkAd.Models;
using TinyHelpers.EntityFrameworkCore.Converters;
using TinyHelpers.EntityFrameworkCore.Comparers;

namespace CourseWorkAd.DBContext
{
    public class ApplicationDBContext: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Loan>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(x => x.DateOut)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });
            modelBuilder.Entity<Loan>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(x => x.DateDue)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });
            modelBuilder.Entity<Loan>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(x => x.DateReturned)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });
            modelBuilder.Entity<DVDTitle>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(x => x.DateReleased)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });
            modelBuilder.Entity<DVDCopy>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(x => x.DatePurchase)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });

            modelBuilder.Entity<Member>(builder =>
            {
                // Date is a DateOnly property and date on database
                builder.Property(x => x.MemberDateOfBirth)
                    .HasConversion<DateOnlyConverter, DateOnlyComparer>();

            });

            modelBuilder.Entity<CastMember>().HasKey(x => new { x.ActorNumber, x.DVDNumber });
        }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Actor> Actors { get; set; }
        public DbSet<CastMember> CastMember { get; set; }
        public DbSet<DVDTitle> DVDTitles { get; set; }
        public DbSet<DVDCopy> DVDCopy { get; set; }
        public DbSet<Studio> Studio { get; set; }
        public DbSet<Producer> Producers { get; set; }
        public DbSet<DVDCategory> DVDCategories { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<LoanType> LoanTypes { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<MembershipCategory> MembershipCategories { get; set; }
        public DbSet<User> Users { get; set; }



    }
}
