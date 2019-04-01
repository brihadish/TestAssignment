using System.Threading.Tasks;
using ApplicationLayer.External;

namespace ApplicationLayer.Services
{
    /// <summary>
    /// Provides abstraction for publishing notifications to external messaging endpoints for
    /// the sake of integration with other applications.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Publishes the specified notification.
        /// </summary>
        /// <param name="notification"><see cref="Notification"/> which is to be published.</param>
        /// <returns></returns>
        Task PublishAsync(Notification notification);
    }
}
