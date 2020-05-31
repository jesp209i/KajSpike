using KajSpike.Framework.Interfaces;
using System.Threading.Tasks;

namespace KajSpike.Persistence
{
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        private readonly CalendarDbContext _dbContext;
        public EfCoreUnitOfWork(CalendarDbContext dbContext) => _dbContext = dbContext;
        public Task Commit() => _dbContext.SaveChangesAsync();
    }
}
