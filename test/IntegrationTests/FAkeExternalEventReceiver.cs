using System;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using ApplicationLayer.External;
using ApplicationLayer.Services;

namespace IntegrationTests
{
    public sealed class FakeExternalEventReceiver : IExternalEventReceiver
    {
        private readonly BufferBlock<ExternalEvent> _bufferBlock = new BufferBlock<ExternalEvent>(new DataflowBlockOptions
        {
            BoundedCapacity = 1,
            EnsureOrdered = true
        });

        public async Task AddEventAsync(ExternalEvent externalEvent)
        {
            await _bufferBlock.SendAsync(externalEvent);
        }

        /// <summary>
        /// Receives event. This is a waiting call and will return only after event becomes available.
        /// </summary>
        /// <returns><see cref="ExternalEvent"/>.</returns>
        public async Task<ExternalEvent> ReceiveEventAsync()
        {
            while (await _bufferBlock.OutputAvailableAsync())
            {
                return await _bufferBlock.ReceiveAsync();
            }

            return null;
        }
    }
}
