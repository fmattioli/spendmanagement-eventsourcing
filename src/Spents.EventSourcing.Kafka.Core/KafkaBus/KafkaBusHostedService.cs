using KafkaFlow;
using Microsoft.Extensions.Hosting;

namespace Spents.EventSourcing.Kafka.Core.KafkaBus
{
    public class KafkaBusHostedService : IHostedService
    {
        private readonly IKafkaBus kafkaBus;

        public KafkaBusHostedService(IServiceProvider serviceProvider)
        {
            kafkaBus = serviceProvider.CreateKafkaBus();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Kakfa started");
            await kafkaBus.StartAsync(cancellationToken);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Kakfa stoped");
            await kafkaBus.StopAsync();
        }
    }
}
