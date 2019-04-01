using System;
using DomainModel.Suit;
using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;
using EventFlow.Exceptions;

namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Provides functionality for altering a suit.
    /// </summary>
    public sealed class SuitAlterationAggregate : AggregateRoot<SuitAlterationAggregate, SuitAlterationId>
    {
        private readonly SuitAlterationState _state = new SuitAlterationState();

        public SuitAlterationAggregate(SuitAlterationId id) : base(id)
        {
            Register(_state);
        }

        /// <summary>
        /// Gets the alteration value.
        /// </summary>
        internal MeasurementAlteration AlterationMeasurement => _state.AlterationMeasurement;

        public void CreateForSleeve(SuitAggregate suit, SuitSleeveAlterationChoice suitSleeveAlterationChoice, MeasurementAlteration alteration)
        {
            if (suit == null)
            {
                throw new ArgumentNullException(nameof(suit));
            }

            if (alteration == null)
            {
                throw new ArgumentNullException(nameof(alteration));
            }

            if (!IsNew)
            {
                throw DomainError.With("SleeveAlteration is already created.");
            }

            Emit(new SuitSleeveAlterationCreated(suit.CustomerId, suit.Id, suitSleeveAlterationChoice, alteration));
        }

        public void CreateForTrouser(SuitAggregate suit, SuitTrouserAlterationChoice suitTrouserAlterationChoice, MeasurementAlteration alteration)
        {
            if (suit == null)
            {
                throw new ArgumentNullException(nameof(suit));
            }

            if (alteration == null)
            {
                throw new ArgumentNullException(nameof(alteration));
            }

            if (!IsNew)
            {
                throw DomainError.With("TrouserAlteration is already created.");
            }

            Emit(new SuitTrouserAlterationCreated(suit.CustomerId, suit.Id, suitTrouserAlterationChoice, alteration));
        }

        /// <summary>
        /// Records payment for the alteration.
        /// NOTE:- This is an idempotent operation.
        /// </summary>
        public IExecutionResult RecordPayment()
        {
            if (_state.Status == SuitAlterationStatus.Succeeded ||
                _state.Status == SuitAlterationStatus.Failed)
            {
                return ExecutionResult.Failed("SuitAlterationAlreadyPerformed");
            }

            if (_state.Status == SuitAlterationStatus.Created)
            {
                Emit(new SuitAlterationPaymentReceived());
            }

            return ExecutionResult.Success();
        }

        /// <summary>
        /// Executes the alteration.
        /// </summary>
        /// <param name="suit">Suit on which alteration is to be performed.</param>
        /// <param name="tailorId">Unique identity of the tailor executing the alteration.</param>
        /// <returns></returns>
        public IExecutionResult ExecuteAlteration(SuitAggregate suit, TailorId tailorId)
        {
            if (suit.IsNew)
            {
                return ExecutionResult.Failed($"Suit[Id :: {_state.SuitId}] not found.");
            }

            switch (_state.Status)
            {
                case SuitAlterationStatus.Failed:
                case SuitAlterationStatus.Succeeded:
                    return ExecutionResult.Failed(SuitAlterationAggregateFailureReasons.SuitAlterationAlreadyPerformed);

                case SuitAlterationStatus.Created:
                    return ExecutionResult.Failed(SuitAlterationAggregateFailureReasons.PaymentRequiredBeforeSuitAlteration);
            }

            return _state.SuitAlterationStrategy.Alter(this, suit, tailorId);
        }

        /// <summary>
        /// Marks the alteration as succeeded.
        /// </summary>
        /// <returns>Result.</returns>
        public IExecutionResult MarkAsSucceeded(TailorId tailorId)
        {
            if (_state.Status != SuitAlterationStatus.Paid)
            {
                return ExecutionResult.Failed(SuitAlterationAggregateFailureReasons.InvalidOperationAsPerCurrentState);
            }

            Emit(new SuitAlterationSucceeded(_state.CustomerId, _state.SuitId, tailorId));
            return ExecutionResult.Success();
        }

        /// <summary>
        /// Marks the alteration as failed.
        /// </summary>
        /// <returns>Result.</returns>
        public IExecutionResult MarkAsFailed(TailorId tailorId)
        {
            if (_state.Status != SuitAlterationStatus.Paid)
            {
                return ExecutionResult.Failed(SuitAlterationAggregateFailureReasons.InvalidOperationAsPerCurrentState);
            }

            Emit(new SuitAlterationSucceeded(_state.CustomerId, _state.SuitId, tailorId));
            return ExecutionResult.Success();
        }
    }
}
