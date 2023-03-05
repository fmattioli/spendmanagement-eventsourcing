namespace Spents.EventSourcing.Kafka.Core.Middlewares
{
    using KafkaFlow;
    using Polly;
    using Serilog;
    using Spents.EventSourcing.Kafka.Core.Extensions;
    using System;
    using System.Threading.Tasks;

    public sealed class ConsumerRetryMiddleware : IMessageMiddleware
    {
        private readonly ILogger log;

        private readonly int retryCount;

        private readonly TimeSpan retryInterval;

        public ConsumerRetryMiddleware(ILogger log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));

            this.retryCount = 2;
            this.retryInterval = TimeSpan.FromMilliseconds(100);
        }

        public Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            return Policy
                .Handle<Exception>(ex => ex is not InvalidOperationException)
                .WaitAndRetryAsync(
                    this.retryCount,
                    _ => this.retryInterval,
                    (ex, _, retryAttempt, __) =>
                    {
                        this.log.Warning(
                            $"[{nameof(ConsumerRetryMiddleware)}] - Failed to process message, retrying...",
                            ex,
                            () => new
                            {
                                context.ConsumerContext.Topic,
                                context.ConsumerContext.Offset,
                                PartitionNumber = context.ConsumerContext.Partition,
                                PartitionKey = context.GetPartitionKey(),
                                Headers = context.Headers.ToJsonString(),
                                RetryAttempt = retryAttempt,
                                ex.Message
                            });
                    })
                .ExecuteAsync(() => next(context));
        }
    }
}
