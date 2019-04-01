using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Commands;
using Newtonsoft.Json;

namespace ApplicationLayer.Commands
{
    /// <summary>
    /// Represents a command for marking a suit's alteration as success.
    /// </summary>
    public sealed class MarkSuitAlterationAsSuccessCommand : Command<SuitAlterationAggregate, SuitAlterationId, IExecutionResult>
    {
        [JsonConstructor]
        public MarkSuitAlterationAsSuccessCommand(SuitAlterationId aggregateId, TailorId tailorId)
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
