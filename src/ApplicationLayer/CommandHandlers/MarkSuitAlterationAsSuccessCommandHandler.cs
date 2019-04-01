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
    /// Handles <see cref="MarkSuitAlterationAsSuccessCommand"/>.
    /// </summary>
    public sealed class MarkSuitAlterationAsSuccessCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, MarkSuitAlterationAsSuccessCommand>
    {
        private readonly ILog _log;

        public MarkSuitAlterationAsSuccessCommandHandler(ILog log)
        {
            _log = log;
        }

        public Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, MarkSuitAlterationAsSuccessCommand command, CancellationToken cancellationToken)
        {
            var result = aggregate.MarkAsSucceeded(command.TailorId);
            if (result.IsSuccess)
            {
                _log.Information("Marked suitalteration [{0}] as succeeded via tailor [{1}].", command.AggregateId, command.TailorId);
            }
            else
            {
                _log.Error(result.ToString());
            }

            return Task.FromResult(result);
        }
    }
}
