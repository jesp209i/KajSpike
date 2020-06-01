using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KajSpike.Framework.Interfaces
{
    public interface IRequestHandler
    {
        Task<IActionResult> HandleCommand<T>(T request, Func<T, Task> handler);
        Task<IActionResult> HandleQuery<TModel>(Func<TModel> query);
    }
}
