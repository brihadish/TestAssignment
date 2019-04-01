using System.Collections.Generic;
using EventFlow.Aggregates;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents soft state of <see cref="SuitAggregate"/>.
    /// </summary>
    public sealed class SuitState : AggregateState<SuitAggregate, SuitId, SuitState>,
        IApply<SuitCreated>,
        IApply<SuitSoldEvent>,        
        IApply<SuitSleeveAltered>,
        IApply<SuitTrouserAltered>
    {
        /// <summary>
        /// Gets or sets the unique identity of the customer to whom the suit belongs to.
        /// </summary>
        public CustomerId CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the pair of sleeves pertaining to the suit.
        /// </summary>
        public SuitSleevePair SuitSleevePair { get; set; }

        /// <summary>
        /// Gets or sets the pair of trousers pertaining to the suit.
        /// </summary>
        public SuitTrouserPair SuitTrouserPair { get; set; }

        public List<SuitAlterationId> AllPerformedAlterations { get; } = new List<SuitAlterationId>();

        /// <summary>
        /// Applies <see cref="SuitCreated"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitCreated"/>.</param>
        public void Apply(SuitCreated aggregateEvent)
        {
            SuitSleevePair = aggregateEvent.SuitSleevePair;
            SuitTrouserPair = aggregateEvent.SuitTrouserPair;
        }

        /// <summary>
        /// Applies <see cref="SuitSoldEvent"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitSoldEvent"/>.</param>
        public void Apply(SuitSoldEvent aggregateEvent)
        {
            CustomerId = aggregateEvent.CustomerId;
        }

        /// <summary>
        /// Applies <see cref="SuitSleeveAltered"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitSleeveAltered"/>.</param>
        public void Apply(SuitSleeveAltered aggregateEvent)
        {
            SuitSleevePair = aggregateEvent.SuitSleevePair;
            AllPerformedAlterations.Add(aggregateEvent.SuitAlterationId);
        }

        /// <summary>
        /// Applies <see cref="SuitTrouserAltered"/> to change state.
        /// </summary>
        /// <param name="aggregateEvent"><see cref="SuitTrouserAltered"/>.</param>
        public void Apply(SuitTrouserAltered aggregateEvent)
        {
            SuitTrouserPair = aggregateEvent.SuitTrouserPair;
            AllPerformedAlterations.Add(aggregateEvent.SuitAlterationId);
        }
    }
}
