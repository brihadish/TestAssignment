using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.ReadStores;
using EventFlow.Aggregates;

namespace ApplicationLayer.ReadModels
{
    /// <summary>
    /// Represents a read model for querying information pertaining to suit alterations.
    /// </summary>
    public sealed class SuitAlterationReadModel : IReadModel,
        IAmReadModelFor<SuitAlterationAggregate, SuitAlterationId, SuitSleeveAlterationCreated>,
        IAmReadModelFor<SuitAlterationAggregate, SuitAlterationId, SuitTrouserAlterationCreated>,
        IAmReadModelFor<SuitAlterationAggregate, SuitAlterationId, SuitAlterationPaymentReceived>,
        IAmReadModelFor<SuitAlterationAggregate, SuitAlterationId, SuitAlterationSucceeded>,
        IAmReadModelFor<SuitAlterationAggregate, SuitAlterationId, SuitAlterationFailed>
    {
        /// <summary>
        /// Gets or sets the unique identity of the suit alteration.
        /// </summary>
        public string SuitAlterationId { get; private set; }

        /// <summary>
        /// Gets or sets the unique identity of the customer whose suit is to be altered.
        /// </summary>
        public string CustomerId { get; private set; }

        /// <summary>
        /// Gets or sets the unique identity of the suit which is to be altered.
        /// </summary>
        public string SuitId { get; private set; }    

        /// <summary>
        /// Gets or sets the status of the alteration.
        /// </summary>
        public string Status { get; private set; }

        public void Apply(IReadModelContext context, IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitSleeveAlterationCreated> domainEvent)
        {
            SuitAlterationId = domainEvent.AggregateIdentity.Value;
            CustomerId = domainEvent.AggregateEvent.CustomerId.Value;
            SuitId = domainEvent.AggregateEvent.SuitId.Value;
            Status = domainEvent.AggregateEvent.Status.ToString().ToLowerInvariant();
        }

        public void Apply(IReadModelContext context, IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitTrouserAlterationCreated> domainEvent)
        {
            SuitAlterationId = domainEvent.AggregateIdentity.Value;
            CustomerId = domainEvent.AggregateEvent.CustomerId.Value;
            SuitId = domainEvent.AggregateEvent.SuitId.Value;
            Status = domainEvent.AggregateEvent.Status.ToString().ToLowerInvariant();
        }

        public void Apply(IReadModelContext context, IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitAlterationPaymentReceived> domainEvent)
        {
            Status = domainEvent.AggregateEvent.Status.ToString().ToLowerInvariant();
        }

        public void Apply(IReadModelContext context, IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitAlterationSucceeded> domainEvent)
        {
            Status = domainEvent.AggregateEvent.Status.ToString().ToLowerInvariant();
        }

        public void Apply(IReadModelContext context, IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitAlterationFailed> domainEvent)
        {
            Status = domainEvent.AggregateEvent.Status.ToString().ToLowerInvariant();
        }
    }
}
