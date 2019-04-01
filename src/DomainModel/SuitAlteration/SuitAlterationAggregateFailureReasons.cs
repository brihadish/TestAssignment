namespace DomainModel.Suit
{
    /// <summary>
    /// Provides the different failure reasons pertaining to suit alterations.
    /// </summary>
    public static class SuitAlterationAggregateFailureReasons
    {
        public const string SuitAlterationAlreadyPerformed = "SuitAlterationAlreadyPerformed";

        public const string PaymentRequiredBeforeSuitAlteration = "PaymentRequiredBeforeSuitAlteration";

        public const string InvalidOperationAsPerCurrentState = "InvalidOperationAsPerCurrentState";
    }
}
