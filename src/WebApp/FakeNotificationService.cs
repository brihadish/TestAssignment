using System.Threading.Tasks;
using ApplicationLayer.External;
using ApplicationLayer.Services;
using System;
using Newtonsoft.Json;

namespace WebApp
{
    public sealed class FakeNotificationService : INotificationService
    {
        public Task PublishAsync(Notification notification)
        {
            Console.WriteLine("Sending notification :- \n {0}", JsonConvert.SerializeObject(notification));
            return Task.CompletedTask;
        }
    }
}
