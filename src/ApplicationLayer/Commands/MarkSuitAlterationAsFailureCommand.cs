using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Newtonsoft.Json;


namespace ApplicationLayer.Commands
{
    /// <summary>
    /// Represents a command for marking a suit's alteration as failure.
    /// </summary>
    public sealed class MarkSuitAlterationAsFailureCommand : Command<SuitAlterationAggregate, SuitAlterationId, IExecutionResult>
    {
        [JsonConstructor]
        public MarkSuitAlterationAsFailureCommand(SuitAlterationId aggregateId, TailorId tailorId)
            : base(aggregateId)
        {
            TailorId = tailorId;
        }

        /// <summary>
        /// Gets or sets the unique identity of the tailor who did the alteration.
        /// </summary>
        public TailorId TailorId { get; set; }
    }
}
