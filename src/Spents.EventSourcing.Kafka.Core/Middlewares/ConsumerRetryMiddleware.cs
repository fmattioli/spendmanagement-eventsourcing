namespace Spents.EventSourcing.Kafka.Core.Middlewares
{
    using System;
    using System.Text;
    using System.Threading.Tasks;
    using KafkaFlow;
    using Newtonsoft.Json;
    using Polly;

    using Serilog;

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
                                PartitionKey = GetPartitionKey(context),
                                Headers = ToJsonString(context.Headers),
                                RetryAttempt = retryAttempt,
                                Message = ex.Message
                            });
                    })
                .ExecuteAsync(() => next(context));
        }



        private string GetPartitionKey(IMessageContext context)
        {
            if (context.Message.Key is string keyString)
            {
                return keyString;
            }

            if (context.Message.Key is byte[] keyBytes)
            {
                try
                {
                    return Encoding.UTF8.GetString(keyBytes);
                }
                catch (DecoderFallbackException)
                {
                    return Convert.ToBase64String(keyBytes);
                }
            }

            return "Invalid message key";
        }

        //TODO: Generate a extension method
        private string ToJsonString(IMessageHeaders headers)
        {
            var stringifiedHeaders = headers
                .GroupBy(g => g.Key)
                .ToDictionary(
                    kv => kv.Key,
                    kv => Encoding.UTF8.GetString(kv.FirstOrDefault().Value));

            return JsonConvert.SerializeObject(stringifiedHeaders);
        }
    }
}
