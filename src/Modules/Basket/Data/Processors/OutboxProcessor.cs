using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EShop.Basket.Data.Processors;

public class OutboxProcessor
    (IServiceProvider serviceProvider,
     IBus bus,
     ILogger<OutboxProcessor> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<BasketDbContext>();
                    var outboxMessages = await dbContext.OutboxMessages
                        .Where(m => m.ProcessedOn == null)
                        .ToListAsync(stoppingToken);

                    foreach (var message in outboxMessages)
                    {
                        var eventType = Type.GetType(message.Type);
                        if (eventType is null)
                        {
                            logger.LogWarning("Could not find type {Type} for outbox message {MessageId}",
                                message.Type, message.Id);
                            continue;
                        }

                        var eventMessage = JsonSerializer.Deserialize(message.Content, eventType);
                        if (eventMessage is null)
                        {
                            logger.LogWarning("Could not deserialize outbox message {MessageId} to type {Type}",
                                message.Id, message.Type);
                            continue;
                        }

                        await bus.Publish(eventMessage, stoppingToken);

                        message.ProcessedOn = DateTime.UtcNow;

                        logger.LogInformation("Processed outbox message {MessageId} of type {Type}",
                            message.Id, message.Type);
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.LogError(ex, "Error processing outbox messages");
            }

            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
