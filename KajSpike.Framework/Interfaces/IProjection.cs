﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.Framework.Interfaces
{
    public interface IProjection
    {
        Task Project(object @event);
    }
}
