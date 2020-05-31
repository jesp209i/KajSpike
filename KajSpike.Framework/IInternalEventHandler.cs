using System;
using System.Collections.Generic;
using System.Text;

namespace KajSpike.Framework
{
    public interface IInternalEventHandler
    {
        void Handle(object @event);
    }
}
