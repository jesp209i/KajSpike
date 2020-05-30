using KajSpike.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace KajSpike.ApplicationService
{
    public class RetryingCommandHandler<T> : IHandleCommand<T>
    {
        static RetryPolicy _policy = _policy.Handle<InvalidOperationException>().Retry();

        private IHandleCommand<T> _next;

        public RetryingCommandHandler(IHandleCommand<T> next) => _next = next;

        public Task Handle(T command) => _policy.ExecuteAsync(() => _next.Handle(command));
    }
}
