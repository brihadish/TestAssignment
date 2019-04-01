namespace WebApp.Models
{
    public sealed class NewSuitAlterationViewModel
    {
        /// <summary>
        /// Gets or sets the name of the customer whose suit is to be altered.
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// Gets or sets name of the suit which is to be altered.
        /// </summary>
        public string SuitName { get; set; }

        /// <summary>
        /// Gets or sets the alteration type.
        /// </summary>
        public NewSuitAlterationType AlterationType { get; set; }
    }
}
