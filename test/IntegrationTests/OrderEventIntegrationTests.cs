using System.Threading;
using System.Threading.Tasks;
using ApplicationLayer;
using ApplicationLayer.Commands;
using DomainModel;
using DomainModel.Suit;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Core;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ApplicationLayer.External;
using EventFlow.Jobs;
using ApplicationLayer.Jobs;

namespace IntegrationTests
{
    [TestClass]
    public sealed class OrderEventIntegrationTests
    {
        private SuitAggregate _suit;
        private IAggregateStore _aggregateStore;
        private ICommandBus _commandBus;
        private MockExternalEventReceiver _mockExternalEventReceiver;
        private Task _externalEventProcessorJobTask;

        [TestInitialize]
        public async Task Initialize()
        {
            _mockExternalEventReceiver = new MockExternalEventReceiver();
            var resolver = EventFlowApplicationLayer.Configure(_mockExternalEventReceiver).CreateResolver();
            _aggregateStore = resolver.Resolve<IAggregateStore>();
            _commandBus = resolver.Resolve<ICommandBus>();

            _suit = new SuitAggregate(SuitId.New);
            _suit.Create(
                    new SuitSleevePair(
                        new Measurement(90, MeasurementUnit.Centimeter),
                        new Measurement(90, MeasurementUnit.Centimeter)),
                    new SuitTrouserPair(
                        new Measurement(120, MeasurementUnit.Centimeter),
                        new Measurement(120, MeasurementUnit.Centimeter)));
            _suit.Sell(CustomerId.New);
            await _aggregateStore.StoreAsync<SuitAggregate, SuitId>(_suit, SourceId.New, CancellationToken.None);
            var jobScheduler = resolver.Resolve<IJobScheduler>();
            var job = new ExternalEventProcessorJob();
            _externalEventProcessorJobTask = Task.Run(() => jobScheduler.ScheduleNowAsync(job, CancellationToken.None));
        }

        [TestMethod]
        public async Task Should_positively_alter_both_suit_sleeves_after_receiving_payment_confirmation()
        {
            // Arrange
            var suitAlterationId = SuitAlterationId.New;
            var createSleeveAlterationCommand =
                new CreateSuitSleeveAlterationCommand(
                                suitAlterationId,
                                _suit.Id,
                                SuitSleeveAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            var executeAlterationCommand = new ExecuteSuitAlterationCommand(_suit.Id, suitAlterationId, TailorId.New);

            // Act and Assert
            var result = await _commandBus.PublishAsync(createSleeveAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            await _mockExternalEventReceiver.AddEventAsync(new OrderPaidEvent
            {
                SuitAlterationId = suitAlterationId.Value,
                Type = "orderpaid"
            });

            await Task.Delay(5000);

            result = await _commandBus.PublishAsync(executeAlterationCommand, CancellationToken.None);
            result.IsSuccess.Should().BeTrue();

            var updatedSuit = await _aggregateStore.LoadAsync<SuitAggregate, SuitId>(_suit.Id, CancellationToken.None);
            updatedSuit.LeftSleeveLength.Equals(new Measurement(95, MeasurementUnit.Centimeter)).Should().BeTrue();
            updatedSuit.RightSleeveLength.Equals(new Measurement(95, MeasurementUnit.Centimeter)).Should().BeTrue();           
        }
    }
}
