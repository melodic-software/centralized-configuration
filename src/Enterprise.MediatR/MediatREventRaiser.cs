using Enterprise.Events.Model;
using Enterprise.Events.Services.Raising;
using Enterprise.MediatR.Adapters;
using MediatR;

namespace Enterprise.MediatR
{
    public class MediatREventRaiser(IMediator mediator) : IRaiseEvents
    {
        public async Task RaiseAsync(IEvent @event)
        {
            await RaiseAsync((dynamic)@event);
        }

        public async Task RaiseAsync(IEnumerable<IEvent> events)
        {
            foreach (IEvent @event in events)
                await RaiseAsync((dynamic)@event);
        }

        private async Task RaiseAsync<T>(T @event) where T : IEvent
        {
            var notification = new EventAdapter<T>(@event);
            await mediator.Publish(notification, CancellationToken.None);
        }
    }
}
