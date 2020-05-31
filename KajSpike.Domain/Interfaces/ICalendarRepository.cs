using KajSpike.Domain.ValueObjects;
using System.Threading.Tasks;

namespace KajSpike.Domain.Interfaces
{
    public interface ICalendarRepository
    {
        Task<Calendar> Load(CalendarId id);
        Task Add(Calendar entity);
        Task<bool> Exists(CalendarId entityId);
    }
}
