using DomainModel;
using DomainModel.Suit;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Newtonsoft.Json;

namespace ApplicationLayer.Commands
{
    /// <summary>
    /// Represents a command for executing a suit's alteration.
    /// </summary>
    public sealed class ExecuteSuitAlterationCommand : Command<SuitAggregate, SuitId, IExecutionResult>
    {
        [JsonConstructor]
        public ExecuteSuitAlterationCommand(SuitId aggregateId, SuitAlterationId suitAlterationId, TailorId tailorId)
            : base(aggregateId)
        {
            SuitAlterationId = suitAlterationId;
            TailorId = tailorId;
        }

        /// <summary>
        /// Gets or sets the unique identity of the suit alteration which is to be executed.
        /// </summary>
        public SuitAlterationId SuitAlterationId { get; set; }

        /// <summary>
        /// Gets or sets the unique identity of the tailor who is executing the alteration.
        /// </summary>
        public TailorId TailorId { get; set; }
    }
}
