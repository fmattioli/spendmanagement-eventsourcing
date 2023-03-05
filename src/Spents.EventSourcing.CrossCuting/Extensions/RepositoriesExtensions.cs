using Microsoft.Extensions.DependencyInjection;
using Spents.EventSourcing.Domain.Interfaces;
using Spents.EventSourcing.Infra.Data.Persistence;

namespace Spents.EventSourcing.CrossCuting.Extensions
{
    public static class RepositoriesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IReceiptEventsRepository, ReceiptEventsRepository>();
            return services;
        }
    }
}
