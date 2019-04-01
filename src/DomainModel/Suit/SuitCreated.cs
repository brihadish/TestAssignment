using EventFlow.Aggregates;


namespace DomainModel.Suit
{
    /// <summary>
    /// Represents that a new suit has been created.
    /// </summary>
    public sealed class SuitCreated : AggregateEvent<SuitAggregate, SuitId>
    {
        public SuitCreated(SuitSleevePair suitSleevePair, SuitTrouserPair suitTrouserPair)
        {
            SuitSleevePair = suitSleevePair;
            SuitTrouserPair = suitTrouserPair;
        }

        /// <summary>
        /// Gets the pair of sleeves pertaining to the suit.
        /// </summary>
        public SuitSleevePair SuitSleevePair { get; set; }

        /// <summary>
        /// Gets the pair of trousers pertaining to the suit.
        /// </summary>
        public SuitTrouserPair SuitTrouserPair { get; set; }
    }
}
