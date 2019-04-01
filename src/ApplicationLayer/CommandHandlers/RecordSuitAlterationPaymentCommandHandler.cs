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
    /// Handles <see cref="RecordSuitAlterationPaymentCommand"/>.
    /// </summary>
    public sealed class RecordSuitAlterationPaymentCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, RecordSuitAlterationPaymentCommand>
    {
        private readonly ILog _log;

        public RecordSuitAlterationPaymentCommandHandler(ILog log)
        {
            _log = log;
        }

        public Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, RecordSuitAlterationPaymentCommand command, CancellationToken cancellationToken)
        {
            if (aggregate.IsNew)
            {
                var failedResult = ExecutionResult.Failed($"SuitAlteration [Id :: {command.AggregateId}] not found.");
                _log.Error(failedResult.ToString());
                return Task.FromResult(failedResult);
            }

            var result = aggregate.RecordPayment();
            if (result.IsSuccess)
            {
                _log.Information("Successfully recorded payment for suitalteration [{0}]", command.AggregateId);
            }
            else
            {
                _log.Error(result.ToString());
            }
            return Task.FromResult(result);
        }
    }
}
