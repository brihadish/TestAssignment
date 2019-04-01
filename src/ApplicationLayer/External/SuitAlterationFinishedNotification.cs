namespace ApplicationLayer.External
{
    public sealed class SuitAlterationFinishedNotification : Notification
    {
        /// <summary>
        /// Gets or sets the unique identity of the alteration.
        /// </summary>
        public string SuitAlterationId { get; set; }

        /// <summary>
        /// Gets or sets the status of the alteration.
        /// </summary>
        public string SuitAlterationStatus { get; set; }

        /// <summary>
        /// Gets or sets the summary pertaining to alteration.
        /// This will contain useful information like failure reason if in case
        ///  the alteration failed.
        /// </summary>
        public string SuitAlterationSummary { get; set; }
    }
}
