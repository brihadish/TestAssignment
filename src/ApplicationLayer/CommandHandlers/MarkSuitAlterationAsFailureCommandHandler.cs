using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.SuitAlteration;
using EventFlow.Commands;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Logs;

namespace ApplicationLayer.CommandHandlers
{
    /// <summary>
    /// Handles <see cref="MarkSuitAlterationAsFailureCommand"/>.
    /// </summary>
    public sealed class MarkSuitAlterationAsFailureCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, MarkSuitAlterationAsFailureCommand>
    {
        private readonly ILog _log;

        public MarkSuitAlterationAsFailureCommandHandler(ILog log)
        {
            _log = log;
        }

        public Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, MarkSuitAlterationAsFailureCommand command, CancellationToken cancellationToken)
        {
            var result = aggregate.MarkAsFailed(command.TailorId);
            if (result.IsSuccess)
            {
                _log.Information("Marked suitalteration [{0}] as failed via tailor [{1}].", command.AggregateId, command.TailorId);
            }
            else
            {
                _log.Error(result.ToString());
            }

            return Task.FromResult(result);
        }
    }
}
