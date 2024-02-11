using System.Reflection;
using GymManagement.Application.Common.Interfaces;
using GymManagement.Domain.Admins;
using GymManagement.Domain.Common;
using GymManagement.Domain.Gyms;
using GymManagement.Domain.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace GymManagement.Infrastructure.Common.Persistence;

public class GymManagementDbContext : DbContext, IUnitOfWork
{
    public DbSet<Admin> Admins { get; set; } = null!;
    public DbSet<Subscription> Subscriptions { get; set; } = null!;
    public DbSet<Gym> Gyms { get; set; } = null!;

    private readonly IHttpContextAccessor _httpContextAccesor;

    public GymManagementDbContext(
        DbContextOptions options,
        IHttpContextAccessor httpContextAccesor
    ) : base(options)
    {
        _httpContextAccesor = httpContextAccesor;
    }

    public async Task CommitChangesAsync()
    {
        // Get all the domain events
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(entry => entry.Entity.PopDomainEvents())
            .SelectMany(x => x)
            .ToList();

        // Store domain events in the http context for later
        AddDomainEventsToOfflineProcessingQueue(domainEvents);

        await SaveChangesAsync();
    }

    /// <summary>
    /// This methos will store domain events into http context to use later
    /// </summary>
    /// <param name="domainEvents"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void AddDomainEventsToOfflineProcessingQueue(List<IDomainEvent> domainEvents)
    {
        // Fetch queue from http context or create a new queue if it doen't exist
        var domainEventsQueue = _httpContextAccesor.HttpContext!.Items
            .TryGetValue("DomainEventsQueue", out var value)
            && value is Queue<IDomainEvent> existingDomainEvents
                ? existingDomainEvents
                : new Queue<IDomainEvent>();

        // Add the domain events to the end of the domain events queue
        domainEvents.ForEach(domainEventsQueue.Enqueue);

        // Store the domain events queue in the http context
        _httpContextAccesor.HttpContext.Items["DomainEventsQueue"] = domainEventsQueue;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
