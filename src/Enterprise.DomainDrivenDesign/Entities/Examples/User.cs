using Enterprise.DomainDrivenDesign.Events.Example;
using Enterprise.DomainDrivenDesign.ValueObjects.Examples.Record;

namespace Enterprise.DomainDrivenDesign.Entities.Examples;

public sealed class User : Entity
{
    private User(Guid id, FirstName firstName, LastName lastName, Email email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }

    public static User Create(Guid id, FirstName firstName, LastName lastName, Email email)
    {
        User user = new User(id, firstName, lastName, email);

        UserCreatedDomainEvent userCreated = new UserCreatedDomainEvent(user.Id);

        user.AddDomainEvent(userCreated);

        return user;
    }

    public static User Create(FirstName firstName, LastName lastName, Email email) =>
        Create(Guid.NewGuid(), firstName, lastName, email);
}