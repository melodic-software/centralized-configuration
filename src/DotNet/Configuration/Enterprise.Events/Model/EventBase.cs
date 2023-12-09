namespace Enterprise.Events.Model;

public abstract class EventBase : IEvent
{
    public Guid Id { get; }

    public DateTime DateTimeOccurred { get; set; }

    protected EventBase(Guid id, DateTime dateTimeOccurred)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Event ID is invalid!", nameof(id));

        Id = id;
        DateTimeOccurred = dateTimeOccurred;
    }

    protected EventBase(DateTime dateTimeOccurred) : this(Guid.NewGuid(), dateTimeOccurred)
    {
    }

    protected EventBase() : this(Guid.NewGuid(), DateTime.UtcNow)
    {
    }
}