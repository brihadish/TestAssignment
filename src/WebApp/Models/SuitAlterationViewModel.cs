namespace WebApp.Models
{
    public sealed class SuitAlterationViewModel
    {
        /// <summary>
        /// Gets or sets the unique identity of the suit alteration.
        /// </summary>
        public string SuitAlterationId { get; set; }

        /// <summary>
        /// Gets or sets the unique identity of the customer whose suit is to be altered.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the unique identity of the suit which is to be altered.
        /// </summary>
        public string SuitId { get; set; }

        /// <summary>
        /// Gets or sets the status of the alteration.
        /// </summary>
        public string Status { get; set; }
    }
}
