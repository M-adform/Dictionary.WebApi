using Application.Services;
using Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static void ApplicationDependencyInjection(this IServiceCollection service)
        {
            service.AddTransient<IItemService, ItemService>();
        }
    }
}