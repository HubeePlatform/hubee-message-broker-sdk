using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hubee.MessageBroker.Sdk.Extensions;
using SubscriberWorker.Handles;
using Events.Order;
using Events.Payment;

namespace SubscriberWorker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddEventBus(Configuration, o =>
            {
                o.AddConsumer<PaymentHandle>();
                o.AddConsumer<PaymentFaultHandle>();
                o.AddConsumer<NotificationHandle>();
                o.AddConsumer<DeliveryHandle>();
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseEventBus(o =>
            {
                o.Subscribe<OrderCreatedEvent, PaymentHandle>(1, 1);
                o.Subscribe<PaymentAuthorizedEvent, NotificationHandle>();
                o.Subscribe<PaymentAuthorizedEvent, DeliveryHandle>();
                o.SubscribeFault<OrderCreatedEvent, PaymentFaultHandle>();
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}