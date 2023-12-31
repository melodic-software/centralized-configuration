using Enterprise.Events.Model;

namespace Enterprise.DomainDrivenDesign.Events.Abstract;

/// <summary>
/// Represents an event that can be raised within the domain.
/// </summary>
public interface IDomainEvent : IEvent
{
    // TODO: is there a way to dynamically add a marker interface like "INotification" from Mediatr contracts library?
    // I don't want to add it here since I think it pollutes the contract by adding a dependency to a third party framework / library
    // I'd like to add some behavior in another library (Mediatr specific) that can take this interface and make it implement the marker interface.

    // Alternatively, I could see it scanning ALL assemblies for IDomainEvent and making them implement INotification
}