using KajSpike.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace KajSpike.Persistence
{
    public class CalendarDbContext : DbContext
    {
        private readonly ILoggerFactory _loggerFactory;

        public CalendarDbContext(DbContextOptions<CalendarDbContext> options, ILoggerFactory loggerFactory) : base(options)
        {
            if (!Database.EnsureCreated()) 
            { 
                Database.Migrate(); 
            }
            _loggerFactory = loggerFactory; 
        }

        public DbSet<Calendar> Calendars { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CalendarEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BookingEntityTypeConfiguration());
        }
    }
    public class CalendarEntityTypeConfiguration : IEntityTypeConfiguration<Calendar>
    {
        public void Configure(EntityTypeBuilder<Calendar> builder) 
        { 
            builder.HasKey(x => x.Id);
            builder.OwnsOne(x => x.Description);
            builder.OwnsOne(x => x.MaximumBookingTimeInMinutes);
            builder.OwnsMany(x => x.Bookings);
        }
    }
    public class BookingEntityTypeConfiguration : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(x => x.BookingId);
            builder.OwnsOne(x => x.BookedBy);
            builder.OwnsOne(x => x.TimeRange);
        }
    }

}
