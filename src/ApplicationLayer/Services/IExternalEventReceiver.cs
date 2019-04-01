using System.Threading.Tasks;
using ApplicationLayer.External;

namespace ApplicationLayer.Services
{
    /// <summary>
    /// Listens for external events.
    /// </summary>
    public interface IExternalEventReceiver
    {
        /// <summary>
        /// Receives event. This is a waiting call and will return only after event becomes available.
        /// </summary>
        /// <returns><see cref="ExternalEvent"/>.</returns>
        Task<ExternalEvent> ReceiveEventAsync();
    }
}
