using GymManagement.Domain.Common;
using GymManagement.Infrastructure.Common.Persistence;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManagement.Infrastructure.Common.Middleware;

/// <summary>
/// Eventual consistency middleware
/// </summary>
public class EventualConsistencyMiddleware
{
    private readonly RequestDelegate _next;

    public EventualConsistencyMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IPublisher publisher, GymManagementDbContext dbContext)
    {
        // Add extra protection when we have an exception in any event handler.
        // If something went wrong, all tha changes inside the delegate will be rollback.
        var transaction = await dbContext.Database.BeginTransactionAsync();

        // Once the user recives the response, execute the delegate.
        // This is to avoid the client waits online for the response.
        context.Response.OnCompleted(async () =>
        {
            try
            {
                // Validate if we have DomainEventsQueue inside http context items
                if (context.Items.TryGetValue("DomainEventsQueue", out var value)
                    && value is Queue<IDomainEvent> domainEventsQueue)
                {
                    // Publish each domain event using mediator
                    while(domainEventsQueue!.TryDequeue(out var domainEvent))
                    {
                        await publisher.Publish(domainEvent);
                    }
                }

                // If everything goes well, save all the changes
                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                // Notify the client that even though they got a good response, the changes
                // didn't take place due to an expected error.
            }
            finally
            {
                // This will roll back the changes that we did if any unexpected error.
                await transaction.DisposeAsync();
            }
        });

        await _next(context);
    }
}
