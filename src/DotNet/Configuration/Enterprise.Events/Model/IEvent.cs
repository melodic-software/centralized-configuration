namespace Enterprise.Events.Model;

public interface IEvent
{
    public Guid Id { get; }
    public DateTime DateTimeOccurred { get; }
}