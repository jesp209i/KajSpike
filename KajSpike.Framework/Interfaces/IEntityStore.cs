using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Framework.Interfaces
{
    public interface IEntityStore
    {
        Task<T> Load<T>(string id);
        Task Save<T>(T entity);
    }
}
