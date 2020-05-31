using KajSpike.Domain;
using KajSpike.Domain.Interfaces;
using KajSpike.Domain.ValueObjects;
using System.Threading.Tasks;

namespace KajSpike.Persistence
{
    public class CalendarRepository : ICalendarRepository
    {
        private readonly CalendarDbContext _dbContext;

        public CalendarRepository(CalendarDbContext dbContext) => _dbContext = dbContext;
        public async Task Add(Calendar entity) => await _dbContext.Calendars.AddAsync(entity);

        public async Task<bool> Exists(CalendarId entityId) => await _dbContext.Calendars.FindAsync(entityId) != null;

        public async Task<Calendar> Load(CalendarId id) => await _dbContext.Calendars.FindAsync(id);
    }
}
