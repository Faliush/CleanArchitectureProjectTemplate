using Application.Abstractions.Events;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Persistence.Events;

internal sealed class IntegrationEventProcessorJob(
    InMemmoryMessageQueue queue,
    IPublisher publisher,
    ILogger<IntegrationEventProcessorJob> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach(IIntegrationEvent @event in queue.Reader.ReadAllAsync(stoppingToken))
        {
            logger.LogInformation("Publishing {IntegrationEventId}", @event.Id);

            await publisher.Publish(@event, stoppingToken);

            logger.LogInformation("Processed {IntegrationEventId}", @event.Id);
        }
    }
}
