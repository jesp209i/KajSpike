using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Serilog;
using KajSpike.Framework.Interfaces;

namespace KajSpike.ApplicationService
{
    public class RequestHandler : IRequestHandler
    {
        private static ILogger _log = Log.ForContext<RequestHandler>();
        public Task<IActionResult> HandleCommand<T>(T request, Func<T, Task> handler)
        {
            throw new NotImplementedException();
        }

        public async Task<IActionResult> HandleQuery<TModel>(Func<TModel> query)
        {
            try
            {
                return new OkObjectResult(query());
            }
            catch (Exception e)
            {
                _log.Error(e, "Error handling the query");
                return new BadRequestObjectResult(new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                });
            }
        }
    }
}
