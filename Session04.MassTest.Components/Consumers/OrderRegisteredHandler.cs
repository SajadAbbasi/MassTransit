using MassTransit;
using Microsoft.Extensions.Logging;
using Session04.MassTest.Contracts;
using System;
using System.Threading.Tasks;

namespace Session04.MassTest.Components.Consumers
{
    public class OrderRegisteredHandler : IConsumer<OrderRegistered>
    {
        private readonly ILogger<OrderRegisteredHandler> _logger;
        public OrderRegisteredHandler(ILogger<OrderRegisteredHandler> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderRegistered> context)
        {
            _logger.Log(LogLevel.Debug, "OrderRegisteredHandler", context.Message.CustomerNumber);

            if (context.Message.OrderId == 2)
            {
                await context.RespondAsync(new OrderRejected
                {
                    RejectBy = "Alireza Oroumand rejector",
                    Reason = "nothing else matter",
                    RejectDate = DateTime.Now,
                    OrderId = context.Message.OrderId

                }); ;
            }
            else
            {
                await context.RespondAsync(new OrderAccepted
                {
                    AcceptBy = "Alireza Oroumand",
                    AcceptDate = DateTime.Now,
                    OrderId = context.Message.OrderId

                });
            }

        }
    }
}
