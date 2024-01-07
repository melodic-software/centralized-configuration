using Configuration.EntityFramework.Entities;
using Configuration.Infrastructure.Outbox;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DomainDrivenDesign.Events.Abstract;
using Enterprise.Events.Services.Raising;
using Enterprise.Serialization.Json;
using Microsoft.Extensions.Options;
using Quartz;
using System.Text.Json;

namespace Configuration.API.BackgroundJobs;

// https://www.courses.milanjovanovic.tech/courses/pragmatic-clean-architecture/lectures/48236092

public class ProcessOutboxMessagesJob : IJob
{
    private readonly IRaiseEvents _eventRaiser;
    private readonly ICurrentDateTimeService _currentDateTimeService;
    private readonly IOptions<OutboxOptions> _outboxOptions;
    private readonly ILogger<ProcessOutboxMessagesJob> _logger;

    public ProcessOutboxMessagesJob(
        IRaiseEvents eventRaiser,
        ICurrentDateTimeService currentDateTimeService,
        IOptions<OutboxOptions> outboxOptions,
        ILogger<ProcessOutboxMessagesJob> logger)
    {
        _eventRaiser = eventRaiser;
        _currentDateTimeService = currentDateTimeService;
        _outboxOptions = outboxOptions;
        _logger = logger;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Beginning to process outbox messages.");

        JsonSerializerOptions jsonSerializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

        // TODO: Initialize connection and transaction.

        // TODO: Load unprocessed outbox messages.
        List<OutboxMessage> outboxMessages = new List<OutboxMessage>();

        foreach (OutboxMessage outboxMessage in outboxMessages)
        {
            Exception? exception = null;

            try
            {
                IDomainEvent? domainEvent = JsonSerializer.Deserialize<IDomainEvent>(outboxMessage.Content, jsonSerializerOptions);

                if (domainEvent == null)
                    throw new InvalidOperationException("Domain event was not properly deserialized. Cannot proceed with publication.");

                // TODO: Do we want to support cancellation tokens?
                // Would that even make sense to support?
                await _eventRaiser.RaiseAsync(domainEvent);
            }
            catch (Exception caughtException)
            {
                exception = caughtException;
            }

            // TODO: await UpdateOutboxMessagesAsync(connection, transaction, outboxMessage, exception);
        }

        // TODO: Commit transaction.

        _logger.LogInformation("Completed processing outbox messages.");
    }

}