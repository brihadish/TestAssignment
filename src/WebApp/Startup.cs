using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ApplicationLayer;
using EventFlow.Configuration;
using EventFlow.Jobs;
using ApplicationLayer.Jobs;
using System.Threading;
using DomainModel.Suit;
using DomainModel;
using EventFlow.Aggregates;
using EventFlow.Core;
using EventFlow;
using ApplicationLayer.Commands;

namespace WebApp
{
    public class Startup
    {
        private Task _externalEventProcessorJobTask;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            var fakeExternalEventReceiver = new FakeExternalEventReceiver();
            var notificationServiceDummy = new NotificationServiceDummy();
            var resolver =
                EventFlowApplicationLayer
                                .Configure(
                                    fakeExternalEventReceiver,
                                    notificationServiceDummy,
                                    new SerilogLogger(EventFlow.Logs.LogLevel.Debug, string.Empty))
                                .CreateResolver();
            services.AddSingleton<IResolver>(resolver);
            services.AddSingleton<FakeExternalEventReceiver>(fakeExternalEventReceiver);
            var jobScheduler = resolver.Resolve<IJobScheduler>();
            var job = new ExternalEventProcessorJob();
            _externalEventProcessorJobTask = Task.Run(() => jobScheduler.ScheduleNowAsync(job, CancellationToken.None));

            var aggregateStore = resolver.Resolve<IAggregateStore>();
            var commandBus = resolver.Resolve<ICommandBus>();
            var suit = new SuitAggregate(SuitId.New);
            suit.Create(
                    new SuitSleevePair(
                        new Measurement(90, MeasurementUnit.Centimeter),
                        new Measurement(90, MeasurementUnit.Centimeter)),
                    new SuitTrouserPair(
                        new Measurement(120, MeasurementUnit.Centimeter),
                        new Measurement(120, MeasurementUnit.Centimeter)));
            suit.Sell(CustomerId.New);
            aggregateStore.StoreAsync<SuitAggregate, SuitId>(suit, SourceId.New, CancellationToken.None).GetAwaiter().GetResult();
            var createSleeveAlterationCommand =
                new CreateSuitSleeveAlterationCommand(
                                SuitAlterationId.New,
                                suit.Id,
                                SuitSleeveAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            commandBus.PublishAsync(createSleeveAlterationCommand, CancellationToken.None).GetAwaiter().GetResult();
            var createTrouserAlterationCommand =
                new CreateSuitTrouserAlterationCommand(
                                SuitAlterationId.New,
                                suit.Id,
                                SuitTrouserAlterationChoice.Both,
                                new MeasurementAlteration(5, MeasurementUnit.Centimeter));
            commandBus.PublishAsync(createTrouserAlterationCommand, CancellationToken.None).GetAwaiter().GetResult();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=SuitAlteration}/{action=Index}/{id?}");
            });
        }
    }
}
