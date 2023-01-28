using KafkaFlow;
using Newtonsoft.Json;

using Serilog;

using System.Diagnostics;
using System.Text;

namespace Spents.EventSourcing.Kafka.Core.Middlewares
{
    public class ConsumerLoggingMiddleware : IMessageMiddleware
    {
        private readonly ILogger log;

        public ConsumerLoggingMiddleware(ILogger log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public async Task Invoke(IMessageContext context, MiddlewareDelegate next)
        {
            var sw = Stopwatch.StartNew();

            this.log.Information(
                $"[{nameof(ConsumerLoggingMiddleware)}] - Kafka message received.",
                () => new
                {
                    context.ConsumerContext.GroupId,
                    context.ConsumerContext.Topic,
                    PartitionNumber = context.ConsumerContext.Partition,
                    PartitionKey = GetPartitionKey(context),
                    Headers = ToJsonString(context.Headers),
                    MessageType = context.Message.Value.GetType().FullName,
                    Message = JsonConvert.SerializeObject(context.Message)
                });

            try
            {
                await next(context);

                this.log.Information(
                    $"[{nameof(ConsumerLoggingMiddleware)}] - Kafka message processed.",
                    () => new
                    {
                        context.ConsumerContext.WorkerId,
                        context.ConsumerContext.GroupId,
                        context.ConsumerContext.Topic,
                        PartitionNumber = context.ConsumerContext.Partition,
                        PartitionKey = GetPartitionKey(context),
                        context.ConsumerContext.Offset,
                        Headers = ToJsonString(context.Headers),
                        MessageType = context.Message.Value.GetType().FullName,
                        Message = JsonConvert.SerializeObject(context.Message),
                        ProcessingTime = sw.ElapsedMilliseconds
                    });
            }
            catch (Exception ex)
            {
                this.log.Error(
                    $"[{nameof(ConsumerLoggingMiddleware)}] - Failed to process message.",
                    ex,
                    () => new
                    {
                        context.ConsumerContext.WorkerId,
                        context.ConsumerContext.GroupId,
                        context.ConsumerContext.Topic,
                        PartitionNumber = context.ConsumerContext.Partition,
                        PartitionKey = GetPartitionKey(context),
                        context.ConsumerContext.Offset,
                        Headers = ToJsonString(context.Headers),
                        MessageType = context.Message.Value.GetType().FullName,
                        Message = JsonConvert.SerializeObject(context.Message),
                        ProcessingTime = sw.ElapsedMilliseconds
                    });
            }
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
