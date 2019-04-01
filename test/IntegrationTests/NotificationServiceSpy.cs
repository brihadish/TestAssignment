using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.External;
using ApplicationLayer.Services;

namespace IntegrationTests
{
    public sealed class NotificationServiceSpy : INotificationService
    {
        public List<Notification> Notifications { get; } = new List<Notification>();

        public Task PublishAsync(Notification notification)
        {
            Notifications.Add(notification);
            return Task.CompletedTask;
        }
    }
}
