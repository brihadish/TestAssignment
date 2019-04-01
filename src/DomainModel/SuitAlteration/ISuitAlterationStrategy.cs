using DomainModel.Suit;
using EventFlow.Aggregates.ExecutionResults;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Provides an abstraction for representing the alteration that is to be
    /// performed on a suit.
    /// </summary>
    public interface ISuitAlterationStrategy
    {
        /// <summary>
        /// Alters the specified suit.
        /// </summary>
        /// <param name="suitAlteration"><see cref="SuitAlterationAggregate"/> which is to be performed.</param>
        /// <param name="suit"><see cref="SuitAggregate"/> which is to be altered.</param>
        /// <param name="tailorId">Unique identity of the tailor who is performing the alteration.</param>
        /// <returns>Result of alteration.</returns>
        IExecutionResult Alter(SuitAlterationAggregate suitAlteration, SuitAggregate suit, TailorId tailorId);
    }
}
