using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Framework.Interfaces
{
    public interface IApplicationService
    {
        public Task Handle(object command);
    }
}
