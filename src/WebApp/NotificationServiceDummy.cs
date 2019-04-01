using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationLayer.External;
using ApplicationLayer.Services;

namespace WebApp
{
    public sealed class NotificationServiceDummy : INotificationService
    {
        public Task PublishAsync(Notification notification)
        {
            return Task.CompletedTask;
        }
    }
}
