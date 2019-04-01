using DomainModel.Suit;
using EventFlow.Aggregates;
using System;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Represents soft state of <see cref="SuitAlterationAggregate"/>.
    /// </summary>
    public sealed class SuitAlterationState : AggregateState<SuitAlterationAggregate, SuitAlterationId, SuitAlterationState>,
        IApply<SuitSleeveAlterationCreated>,
        IApply<SuitTrouserAlterationCreated>,
        IApply<SuitAlterationSucceeded>,
        IApply<SuitAlterationFailed>,
        IApply<SuitAlterationPaymentReceived>
    {
        /// <summary>
        /// Gets or sets the unique identity of the customer to whom the suit belongs to.
        /// </summary>
        public CustomerId CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identity of the suit on which alteration is to be performed.
        /// </summary>
        public SuitId SuitId { get; set; }

        /// <summary>
        /// Gets or sets the alteration strategy.
        /// </summary>
        public ISuitAlterationStrategy SuitAlterationStrategy { get; set; }

        /// <summary>
        /// Gets or sets the alteration value.
        /// </summary>
        public MeasurementAlteration AlterationMeasurement { get; set; }

        /// <summary>
        /// Gets or sets status of the alteration.
        /// </summary>
        public SuitAlterationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the timestamp of last modification.
        /// </summary>
        public DateTime LastModifiedUtc { get; set; }

        /// <summary>
        /// Applies <see cref="SuitSleeveAlterationCreated"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitSleeveAlterationCreated"/>.</param>
        public void Apply(SuitSleeveAlterationCreated aggregateEvent)
        {
            CustomerId = aggregateEvent.CustomerId;
            SuitId = aggregateEvent.SuitId;
            SuitAlterationStrategy = SuitAlterationStrategies.GetStrategy(aggregateEvent.SuitSleeveAlterationChoice);
            AlterationMeasurement = aggregateEvent.Alteration;
            Status = aggregateEvent.Status;
        }

        /// <summary>
        /// Applies <see cref="SuitTrouserAlterationCreated"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitTrouserAlterationCreated"/>.</param>
        public void Apply(SuitTrouserAlterationCreated aggregateEvent)
        {
            CustomerId = aggregateEvent.CustomerId;
            SuitId = aggregateEvent.SuitId;
            SuitAlterationStrategy = SuitAlterationStrategies.GetStrategy(aggregateEvent.SuitTrouserAlterationChoice);
            AlterationMeasurement = aggregateEvent.Alteration;
            Status = aggregateEvent.Status;
        }

        /// <summary>
        /// Applies <see cref="SuitAlterationPaymentReceived"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitAlterationPaymentReceived"/>.</param>
        public void Apply(SuitAlterationPaymentReceived aggregateEvent)
        {
            Status = aggregateEvent.Status;
        }

        /// <summary>
        /// Applies <see cref="SuitAlterationSucceeded"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitAlterationSucceeded"/>.</param>
        public void Apply(SuitAlterationSucceeded aggregateEvent)
        {
            Status = aggregateEvent.Status;
        }

        /// <summary>
        /// Applies <see cref="SuitAlterationFailed"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitAlterationFailed"/>.</param>
        public void Apply(SuitAlterationFailed aggregateEvent)
        {
            Status = aggregateEvent.Status;
        }
    }
}
