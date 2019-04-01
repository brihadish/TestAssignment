using System;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.Suit;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Subscribers;

namespace ApplicationLayer.Subscribers
{
    /// <summary>
    /// Handles <see cref="SuitTrouserAltered"/> so as to update other parts in the system.
    /// </summary>
    public sealed class SuitTrouserAlteredSubscriber :
        ISubscribeSynchronousTo<SuitAggregate, SuitId, SuitTrouserAltered>
    {
        private readonly ICommandBus _commandBus;

        public SuitTrouserAlteredSubscriber(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public async Task HandleAsync(IDomainEvent<SuitAggregate, SuitId, SuitTrouserAltered> domainEvent, CancellationToken cancellationToken)
        {
            var command =
                new MarkSuitAlterationAsSuccessCommand(
                                    domainEvent.AggregateEvent.SuitAlterationId,
                                    domainEvent.AggregateEvent.TailorId);
            var result = await _commandBus.PublishAsync(command, cancellationToken);
            if (!result.IsSuccess)
            {
                throw new InvalidOperationException(result.ToString());
            }
        }
    }
}
