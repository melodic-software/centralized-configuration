﻿using Enterprise.DomainDrivenDesign.Events.Abstract;

namespace Configuration.Domain.Applications.Events;

public class ApplicationNotFound : DomainEvent
{
    public Guid Id { get; }

    public ApplicationNotFound(Guid id)
    {
        Id = id;
    }
}