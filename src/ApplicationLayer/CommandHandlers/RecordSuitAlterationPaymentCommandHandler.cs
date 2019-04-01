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
    /// Handles <see cref="RecordSuitAlterationPaymentCommand"/>.
    /// </summary>
    public sealed class CRecordSuitAlterationPaymentCommandHandler : ICommandHandler<SuitAlterationAggregate, SuitAlterationId, IExecutionResult, RecordSuitAlterationPaymentCommand>
    {
        public Task<IExecutionResult> ExecuteCommandAsync(
            SuitAlterationAggregate aggregate, RecordSuitAlterationPaymentCommand command, CancellationToken cancellationToken)
        {
            if (aggregate.IsNew)
            {
                return Task.FromResult(ExecutionResult.Failed($"SuitAlteration [Id :: {command.AggregateId}] not found."));
            }

            var result = aggregate.RecordPayment();
            return Task.FromResult(result);
        }
    }
}
