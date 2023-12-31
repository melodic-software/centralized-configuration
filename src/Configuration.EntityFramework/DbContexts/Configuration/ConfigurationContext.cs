﻿using Configuration.EntityFramework.DbContexts.Configuration.Extensions;
using Configuration.EntityFramework.Entities;
using Enterprise.DateTimes.Current.Abstract;
using Enterprise.DesignPatterns.UnitOfWork;
using Enterprise.DomainDrivenDesign.Entities;
using Enterprise.DomainDrivenDesign.Events.Abstract;
using Enterprise.EntityFramework.Contexts;
using Enterprise.EntityFramework.Extensions;
using Enterprise.Serialization.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Text.Json;
using static Configuration.EntityFramework.DbContexts.Configuration.Seeding.SeedService;

namespace Configuration.EntityFramework.DbContexts.Configuration;

public  sealed class ConfigurationContext : DbContextBase, IUnitOfWork
{
    private readonly ICurrentDateTimeService _currentDateTimeService;

    public ConfigurationContext(DbContextOptions<ConfigurationContext> options, ICurrentDateTimeService currentDateTimeService)
        : base(options)
    {
        _currentDateTimeService = currentDateTimeService;

        // Options can be provided at the moment the DbContext is registered.
        // See DI service registration in EntityFrameworkConfigurations.ConfigureDbContexts.
        // Contexts should be registered on the service collection (ex: "services.AddDbContext<ConfigurationContext>").

        // The alternative configuration extensibility point is to override the "OnConfiguring" method.

        // Wire up event handlers.
        SavingChanges += OnSavingChanges;
        SavedChanges += OnSavedChanges;
        SaveChangesFailed += OnSaveChangesFailed;

        // The alternative to the event handler approach would be to override the SaveChanges / SaveChangesAsync methods
        // and adding custom logic before and after a call to the base.SaveChanges().
    }

    // The base DbContext constructor ensures that the DbSets are not null after having been constructed.
    // Compiler warning "uninitialized non-nullable property" can be safely ignored with the "null-forgiving operator" (null!).

    public DbSet<ApplicationEntity> Applications { get; set; } = null!;
    public DbSet<ConfigurationEntryEntity> ConfigurationEntries { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // We can set up any instance level dependencies that are not injected in the constructor.
        // or any other properties that can be configured externally.

        if (optionsBuilder.IsConfigured)
            return;

        // IF external configuration is not setup, OR we have a public parameterless constructor
        // THEN this is where the configuration must occur.
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // NOTE: Both this method, OnConfiguring, and OnModelCreating do not call into the base
        // .These are actually empty methods in the base (also known as "no op" / "no operation" methods).
        //base.ConfigureConventions(configurationBuilder);

        // Instead of individual configurations for every string (in a separate file)
        // we can use bulk configurations registered here:

        //configurationBuilder.Properties<string>().HaveColumnType("nvarchar(450)");

        // We can define custom value conversion for all properties of a given type.
        // configurationBuilder.Properties<Color>().HaveConversion(); // uses extension method, custom value converter type
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // This can be used to manually construct the model,
        // which can be useful if the conventions based configuration is not sufficient OR if you prefer to be explicit.
        // It can also be used to seed the database.

        // This will automatically apply entity configurations in the current assembly.
        // Entity configurations use a fluent syntax, which is an alternative to decorating the entity with attributes.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConfigurationContext).Assembly);
        
        this.Map(modelBuilder);
        this.HandleProviderCustomizations(modelBuilder);

        SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void OnSavingChanges(object? sender, SavingChangesEventArgs e)
    {
        // This runs BEFORE the SaveChanges logic is performed.
        bool acceptAllChangesOnSuccess = e.AcceptAllChangesOnSuccess;

        // These are the entity entries that are tracking the entities.
        // They contain pointers to the tracked object.
        IEnumerable<EntityEntry> entries = ChangeTracker.Entries();
        List<object> entities = entries.Select(x => x.Entity).ToList();

        this.UpdateAuditShadowProperties();

        // Persist any domain events as outbox messages in the same transaction.
        AddDomainEventsAsOutboxMessagesAsync();
    }

    private void OnSavedChanges(object? sender, SavedChangesEventArgs e)
    {
        // this runs AFTER the SaveChanges logic is performed
        bool acceptAllChangesOnSuccess = e.AcceptAllChangesOnSuccess;
        int entitiesSavedCount = e.EntitiesSavedCount;
    }

    private void OnSaveChangesFailed(object? sender, SaveChangesFailedEventArgs e)
    {
        // this runs when an error occurs while executing the SaveChanges logic
        bool acceptAllChangesOnSuccess = e.AcceptAllChangesOnSuccess;
        Exception exception = e.Exception;
    }

    private void AddDomainEventsAsOutboxMessagesAsync()
    {
        List<IDomainEvent> domainEvents = ChangeTracker
            .Entries<IEntity>()
            .Select(entry => entry.Entity)
            .SelectMany(entity =>
            {
                IReadOnlyList<IDomainEvent> domainEvents = entity.GetDomainEvents();

                // Have to clear these, so they don't get reprocessed and cause issues downstream.
                // This can cause problems depending on the DI lifetime of the db context.
                entity.ClearDomainEvents();

                return domainEvents;
            }).ToList();

        JsonSerializerOptions serializerOptions = JsonSerializerOptionsService.GetDefaultOptions();

        // Projecting domain events into outbox message instances.
        List<OutboxMessage> outboxMessages = domainEvents
            .Select(domainEvent => new OutboxMessage(
                Guid.NewGuid(),
                _currentDateTimeService.GetUtcNow(),
                domainEvent.GetType().Name,
                JsonSerializer.Serialize(domainEvent, serializerOptions)
            )).ToList();

        // Add these so they will be persisted.
        AddRange(outboxMessages);
    }
}