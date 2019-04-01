using EventFlow.Aggregates;
using EventFlow.Aggregates.ExecutionResults;

namespace DomainModel.Suit
{
    /// <summary>
    /// Represents a suit.
    /// </summary>
    public sealed class SuitAggregate : AggregateRoot<SuitAggregate, SuitId>
    {
        private readonly SuitState _state = new SuitState();

        public SuitAggregate(SuitId id)
            : base(id)
        {
            Register(_state);
        }

        /// <summary>
        /// Gets the unique identity of the customer to whom the suit belongs to. 
        /// </summary>
        public CustomerId CustomerId => _state.CustomerId;

        /// <summary>
        /// Gets the length of left trouser.
        /// </summary>
        public Measurement LeftSleeveLength => _state.SuitSleevePair.LeftSleeveLength;

        /// <summary>
        /// Gets the length of right trouser.
        /// </summary>
        public Measurement RightSleeveLength => _state.SuitSleevePair.RightSleeveLength;

        /// <summary>
        /// Gets the length of left trouser.
        /// </summary>
        public Measurement LeftTrouserLength => _state.SuitTrouserPair.LeftTrouserLength;

        /// <summary>
        /// Gets the length of right trouser.
        /// </summary>
        public Measurement RightTrouserLength => _state.SuitTrouserPair.RightTrouserLength;

        /// <summary>
        /// Creates a new suit.
        /// </summary>
        /// <param name="suitSleevePair"></param>
        /// <param name="suitTrouserPair"></param>
        public void Create(SuitSleevePair suitSleevePair, SuitTrouserPair suitTrouserPair)
        {
            Emit(new SuitCreated(suitSleevePair, suitTrouserPair));
        }

        /// <summary>
        /// Sells the current suit to the specified customer.
        /// </summary>
        /// <param name="customerId">Unique identity of the customer to whom the suit is to be sold.</param>
        public void Sell(CustomerId customerId)
        {
            Emit(new SuitSoldEvent(customerId));
        }

        /// <summary>
        /// Alters the suit's sleeves.
        /// </summary>
        /// <param name="suitAlterationId">Unique identity of the alteration.</param>
        /// <param name="tailorId">Unique identity of the tailor who is performing the alteration.</param>
        /// <param name="suitSleeveAlterationChoice"><see cref="SuitSleeveAlterationChoice"/>.</param>
        /// <param name="alteration"><see cref="MeasurementAlteration"/> that is to be applied to the suit's sleeves.</param>
        /// <returns>Result of alteration.</returns>
        public IExecutionResult Alter(
                        SuitAlterationId suitAlterationId,
                        TailorId tailorId,
                        SuitSleeveAlterationChoice suitSleeveAlterationChoice,
                        MeasurementAlteration alteration)
        {
            if (_state.AllPerformedAlterations.Contains(suitAlterationId))
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.SpecifiedAlterationAlreadyPerformed);
            }

            var canApplyAlterationToLeftSleeve = alteration.CanApply(_state.SuitSleevePair.LeftSleeveLength);
            var canApplyAlterationToRightSleeve = alteration.CanApply(_state.SuitSleevePair.RightSleeveLength);
            var alteredLeftSleeveLength = _state.SuitSleevePair.LeftSleeveLength;
            var alteredRightSleeveLength = _state.SuitSleevePair.RightSleeveLength;

            switch (suitSleeveAlterationChoice)
            {
                case SuitSleeveAlterationChoice.Left:
                    if (!canApplyAlterationToLeftSleeve)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnLeftSleeve);
                    }

                    alteredLeftSleeveLength = alteration.Apply(_state.SuitSleevePair.LeftSleeveLength);
                    break;

                case SuitSleeveAlterationChoice.Right:
                    if (!canApplyAlterationToRightSleeve)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnRightSleeve);
                    }

                    alteredRightSleeveLength = alteration.Apply(_state.SuitSleevePair.RightSleeveLength);
                    break;

                case SuitSleeveAlterationChoice.Both:
                    if (!canApplyAlterationToLeftSleeve)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnLeftSleeve);
                    }

                    if (!canApplyAlterationToRightSleeve)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnRightSleeve);
                    }

                    alteredLeftSleeveLength = alteration.Apply(_state.SuitSleevePair.LeftSleeveLength);
                    alteredRightSleeveLength = alteration.Apply(_state.SuitSleevePair.RightSleeveLength);
                    break;
            }

            if (alteredLeftSleeveLength.Value <= 0)
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.InvalidAlterationOnLeftSleeve);
            }

            if (alteredRightSleeveLength.Value <= 0)
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.InvalidAlterationOnRightSleeve);
            }

            var alteredSuitSleevePair = new SuitSleevePair(alteredLeftSleeveLength, alteredRightSleeveLength);
            if (_state.SuitSleevePair.Equals(alteredSuitSleevePair))
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.SleeveAlterationDidNotYieldAnyChange);
            }

            Emit(new SuitSleeveAltered(suitAlterationId, tailorId, alteredSuitSleevePair));
            return ExecutionResult.Success();
        }

        /// <summary>
        /// Alters the suit's trousers.
        /// </summary>
        /// <param name="suitAlterationId">Unique identity of the alteration.</param>
        /// <param name="tailorId">Unique identity of the tailor who is performing the alteration.</param>
        /// <param name="suitTrouserAlterationChoice"><see cref="SuitTrouserAlterationChoice"/>.</param>
        /// <param name="alteration"><see cref="MeasurementAlteration"/> that is to be applied to the suit's trousers.</param>
        public IExecutionResult Alter(
                        SuitAlterationId suitAlterationId,
                        TailorId tailorId,
                        SuitTrouserAlterationChoice suitTrouserAlterationChoice,
                        MeasurementAlteration alteration)
        {
            if (_state.AllPerformedAlterations.Contains(suitAlterationId))
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.SpecifiedAlterationAlreadyPerformed);
            }

            var canApplyAlterationToLeftTrouser = alteration.CanApply(_state.SuitTrouserPair.LeftTrouserLength);
            var canApplyAlterationToRightTrouser = alteration.CanApply(_state.SuitTrouserPair.RightTrouserLength);
            var alteredLeftTrouserLength = _state.SuitTrouserPair.LeftTrouserLength;
            var alteredRightTrouserLength = _state.SuitTrouserPair.RightTrouserLength;

            switch (suitTrouserAlterationChoice)
            {
                case SuitTrouserAlterationChoice.Left:
                    if (!canApplyAlterationToLeftTrouser)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnLeftTrouser);
                    }

                    alteredLeftTrouserLength = alteration.Apply(_state.SuitTrouserPair.LeftTrouserLength);
                    break;

                case SuitTrouserAlterationChoice.Right:
                    if (!canApplyAlterationToRightTrouser)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnRightTrouser);
                    }

                    alteredRightTrouserLength = alteration.Apply(_state.SuitTrouserPair.RightTrouserLength);
                    break;

                case SuitTrouserAlterationChoice.Both:
                    if (!canApplyAlterationToLeftTrouser)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnLeftTrouser);
                    }

                    if (!canApplyAlterationToRightTrouser)
                    {
                        return ExecutionResult.Failed(SuitAggregateFailureReasons.CannotApplyAlterationOnRightTrouser);
                    }

                    alteredLeftTrouserLength = alteration.Apply(_state.SuitTrouserPair.LeftTrouserLength);
                    alteredRightTrouserLength = alteration.Apply(_state.SuitTrouserPair.RightTrouserLength);
                    break;
            }

            if (alteredLeftTrouserLength.Value <= 0)
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.InvalidAlterationOnLeftTrouser);
            }

            if (alteredRightTrouserLength.Value <= 0)
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.InvalidAlterationOnRightTrouser);
            }

            var alteredSuitTrouserPair = new SuitTrouserPair(alteredLeftTrouserLength, alteredRightTrouserLength);
            if (_state.SuitTrouserPair.Equals(alteredSuitTrouserPair))
            {
                return ExecutionResult.Failed(SuitAggregateFailureReasons.TrouserAlterationDidNotYieldAnyChange);
            }

            Emit(new SuitTrouserAltered(suitAlterationId, tailorId, alteredSuitTrouserPair));
            return ExecutionResult.Success();
        }
    }
}
