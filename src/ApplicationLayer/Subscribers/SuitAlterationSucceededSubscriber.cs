﻿using System.Threading;
using System.Threading.Tasks;
using DomainModel;
using EventFlow.Subscribers;
using DomainModel.SuitAlteration;
using EventFlow.Aggregates;
using EventFlow.Configuration;
using ApplicationLayer.Services;
using System;
using ApplicationLayer.External;

namespace ApplicationLayer.Subscribers
{
    /// <summary>
    /// Handles <see cref="SuitAlterationSucceeded"/> so as to send notification to customer.
    /// </summary>
    public sealed class SuitAlterationSucceededSubscriber :
        ISubscribeAsynchronousTo<SuitAlterationAggregate, SuitAlterationId, SuitAlterationSucceeded>
    {
        private readonly INotificationService _notificationService;

        public SuitAlterationSucceededSubscriber(IResolver resolver)
        {
            _notificationService = resolver.Resolve<INotificationService>();
            if (_notificationService == null)
            {
                throw new ArgumentException("INotificationService not available.");
            }
        }

        public async Task HandleAsync(
            IDomainEvent<SuitAlterationAggregate, SuitAlterationId, SuitAlterationSucceeded> domainEvent, CancellationToken cancellationToken)
        {
            var notification = new SuitAlterationFinishedNotification
            {
                SuitAlterationId = domainEvent.AggregateIdentity.Value,
                SuitAlterationStatus = domainEvent.AggregateEvent.Status.ToString().ToLowerInvariant()
            };
            await _notificationService.PublishAsync(notification);
        }
    }
}
