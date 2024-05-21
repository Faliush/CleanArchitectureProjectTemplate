using Domain.Core.Events;
using Infrastructure.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Infrastructure.Outbox;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        var messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);

        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(outboxMessage.Content);

            if (domainEvent is null)
            {
                continue;
            }

            await publisher.Publish(domainEvent, context.CancellationToken);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync();
    }
}
