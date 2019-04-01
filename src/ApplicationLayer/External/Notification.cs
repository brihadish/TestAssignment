namespace ApplicationLayer.External
{
    /// <summary>
    /// Provides an abstraction to represent notifications that are to be sent to an
    /// external messaging endpoint for the sake of integration with other applications.
    /// </summary>
    public abstract class Notification
    {
        protected Notification()
        {
            Type = GetType().Name.ToLowerInvariant().Replace("Notification", string.Empty);
        }

        public string Type { get; }
    }
}