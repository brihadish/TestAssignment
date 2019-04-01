namespace ApplicationLayer.External
{
    /// <summary>
    /// Represents that payment has been made for a suit alteration.
    /// </summary>
    public sealed class OrderPaidEvent : ExternalEvent
    {
        /// <summary>
        /// Gets or sets the unique identity of the suit alteration for which payment
        /// has been made.
        /// </summary>
        public string SuitAlterationId { get; set; }
    }
}
