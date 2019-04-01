namespace DomainModel.SuitAlteration
{
    /// <summary>
    /// Provides the different status of a suit's alteration.
    /// </summary>
    public enum SuitAlterationStatus
    {
        /// <summary>
        /// Specifies that the alteration has been created.
        /// </summary>
        Created,

        /// <summary>
        /// Specifies that the alteration has been paid for.
        /// </summary>
        Paid,

        /// <summary>
        /// Specifies that the alteration succeeded.
        /// </summary>
        Succeeded,

        /// <summary>
        /// Specifies that the alteration failed.
        /// </summary>
        Failed
    }
}
