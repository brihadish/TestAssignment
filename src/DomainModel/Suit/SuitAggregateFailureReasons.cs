namespace DomainModel.Suit
{
    /// <summary>
    /// Provides the different failure reasons pertaining to operations on suits.
    /// </summary>
    public static class SuitAggregateFailureReasons
    {
        public const string SpecifiedAlterationAlreadyPerformed = "SpecifiedAlterationAlreadyPerformed";

        public const string CannotApplyAlterationOnLeftSleeve = "CannotApplyAlterationOnLeftSleeve";

        public const string CannotApplyAlterationOnRightSleeve = "CannotApplyAlterationOnRightSleeve";

        public const string InvalidAlterationOnLeftSleeve = "InvalidAlterationOnLeftSleeve";

        public const string InvalidAlterationOnRightSleeve = "InvalidAlterationOnRightSleeve";

        public const string SleeveAlterationDidNotYieldAnyChange = "SleeveAlterationDidNotYieldAnyChange";

        public const string CannotApplyAlterationOnLeftTrouser = "CannotApplyAlterationOnLeftTrouser";

        public const string CannotApplyAlterationOnRightTrouser = "CannotApplyAlterationOnRightTrouser";

        public const string InvalidAlterationOnLeftTrouser = "InvalidAlterationOnLeftTrouser";

        public const string InvalidAlterationOnRightTrouser = "InvalidAlterationOnRightTrouser";

        public const string TrouserAlterationDidNotYieldAnyChange = "TrouserAlterationDidNotYieldAnyChange";
    }
}
