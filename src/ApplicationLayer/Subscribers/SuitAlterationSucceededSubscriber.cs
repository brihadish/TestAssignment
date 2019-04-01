using System.Threading;
using System.Threading.Tasks;
using DomainModel;
using EventFlow.Subscribers;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates;
using EventFlow.Configuration;

namespace ApplicationLayer.Subscribers
{
    /// <summary>
    /// Handles <see cref="SuitAlterationSucceeded"/> so as to send notification to customer.
    /// </summary>
    public sealed class SuitAlterationSucceededSubscriber :
        ISubscribeAsynchronousTo<SuitAlterationAggregate, SuitAlterationId, SuitAlterationSucceeded>
    {
        private readonly IResolver _resolver;

        public SuitAlterationSucceededSubscriber(IResolver resolver)
        {
            _resolver = resolver;
        }

        public Task HandleAsync(IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitAlterationSucceeded> domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
