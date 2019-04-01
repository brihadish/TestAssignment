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
    /// Handles <see cref="MarkSuitAlterationAsSuccessCommand"/>.
    /// </summary>
    public sealed class MarkSuitAlterationAsSuccessCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, MarkSuitAlterationAsSuccessCommand>
    {
        public Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, MarkSuitAlterationAsSuccessCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(aggregate.MarkAsFailed(command.TailorId));
        }
    }
}
