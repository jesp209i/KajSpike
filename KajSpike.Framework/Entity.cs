using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace KajSpike.Framework
{
    public abstract class Entity<TId> : IInternalEventHandler where TId: ValueObject<TId>
    {
        public TId Id { get; protected set; }
        private readonly Action<object> _applier;
        protected Entity(Action<object> applier) => _applier = applier;
        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }
        protected abstract void When(object @event);
        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}
