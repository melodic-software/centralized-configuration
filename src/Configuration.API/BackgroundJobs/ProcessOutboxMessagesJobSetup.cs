using Configuration.Infrastructure.Outbox;
using Microsoft.Extensions.Options;
using Quartz;

namespace Configuration.API.BackgroundJobs
{
    public class ProcessOutboxMessagesJobSetup : IConfigureOptions<QuartzOptions>
    {
        private readonly OutboxOptions _outboxOptions;

        public ProcessOutboxMessagesJobSetup(IOptions<OutboxOptions> outboxOptions)
        {
            _outboxOptions = outboxOptions.Value;
        }

        public void Configure(QuartzOptions options)
        {
            const string jobName = nameof(ProcessOutboxMessagesJob);

            options.AddJob<ProcessOutboxMessagesJob>(builder => builder.WithIdentity(jobName))
                .AddTrigger(configure =>
                    configure.ForJob(jobName)
                        .WithSimpleSchedule(builder =>
                            builder.WithIntervalInSeconds(_outboxOptions.IntervalInSeconds).RepeatForever()
                        )
                );
        }
    }
}
