using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Framework.Interfaces
{
    public interface IHandleCommand<in T>
    {
        public Task Handle(T command);
    }
}
