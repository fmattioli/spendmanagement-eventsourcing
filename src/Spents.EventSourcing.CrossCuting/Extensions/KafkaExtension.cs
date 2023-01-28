using KafkaFlow;
using KafkaFlow.Configuration;
using KafkaFlow.Serializer;
using KafkaFlow.TypedHandler;
using Microsoft.Extensions.DependencyInjection;

using Spents.EventSourcing.Kafka.Core.Handlers;
using Spents.EventSourcing.Kafka.Core.KafkaBus;
using Spents.EventSourcing.Kafka.Core.Middlewares;
using Spents.EventSourcing.CrossCuting.Models;
using Spents.Topics;

namespace Spents.EventSourcing.CrossCuting.Extensions
{
    public static class KafkaExtension
    {
        public static IServiceCollection AddKafka(this IServiceCollection services, KafkaSettings kafkaSettings)
        {
            services.AddKafka(kafka => kafka
                .UseConsoleLog()
                .AddCluster(cluster => cluster
                    .AddBrokers(kafkaSettings)
                    .AddTelemetry()
                    .AddConsumers(kafkaSettings)
                    )
                );

            services.AddHostedService<KafkaBusHostedService>();
            return services;
        }

        private static IClusterConfigurationBuilder AddTelemetry(
           this IClusterConfigurationBuilder builder)
        {
            builder
                .EnableAdminMessages(KafkaTopics.Events.Receipt)
                .EnableTelemetry(KafkaTopics.Events.Receipt);

            return builder;
        }


        private static IClusterConfigurationBuilder AddBrokers(
            this IClusterConfigurationBuilder builder,
            KafkaSettings settings)
        {
            if (settings.Sasl_Enabled)
            {
                builder
                    .WithBrokers(settings.Sasl_Brokers)
                    .WithSecurityInformation(si =>
                    {
                        si.SecurityProtocol = KafkaFlow.Configuration.SecurityProtocol.SaslSsl;
                        si.SaslUsername = settings.Sasl_UserName;
                        si.SaslPassword = settings.Sasl_Password;
                        si.SaslMechanism = KafkaFlow.Configuration.SaslMechanism.ScramSha512;
                        si.SslCaLocation = string.Empty;
                    });
            }
            else
            {
                builder.WithBrokers(new[] { settings.Brokers });
            }

            return builder;
        }

        private static IClusterConfigurationBuilder AddConsumers(
            this IClusterConfigurationBuilder builder,
            KafkaSettings settings)
        {
            builder.AddConsumer(
                consumer => consumer
                     .Topic(Spents.Topics.KafkaTopics.Events.Receipt)
                     .WithGroupId("ReceiptEvents")
                     .WithBufferSize(settings.BufferSize)
                     .WithWorkersCount(settings.WorkerCount)
                     .WithAutoOffsetReset(KafkaFlow.AutoOffsetReset.Latest)
                     .WithInitialState((ConsumerInitialState)Enum.Parse(typeof(ConsumerInitialState), settings.ConsumerInitialState))
                     .AddMiddlewares(
                        middlewares =>
                            middlewares
                            .AddSerializer<JsonCoreSerializer>()
                            .Add<ConsumerLoggingMiddleware>()
                            .Add<ConsumerRetryMiddleware>()
                            .AddTypedHandlers(
                                h => h
                                    .WithHandlerLifetime(InstanceLifetime.Singleton)
                                    .AddHandler<ReceiptCreatedEventHandler>()
                                    )
                            )
                     ) ;

            return builder;
        }
    }
}
