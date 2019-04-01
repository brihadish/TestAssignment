using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Commands;
using EventFlow.Aggregates.ExecutionResults;

namespace ApplicationLayer.CommandHandlers
{
    /// <summary>
    /// Handles <see cref="MarkSuitAlterationAsFailureCommand"/>.
    /// </summary>
    public sealed class MarkSuitAlterationAsFailureCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, MarkSuitAlterationAsFailureCommand>
    {
        public Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, MarkSuitAlterationAsFailureCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(aggregate.MarkAsFailed(command.TailorId));
        }
    }
}
