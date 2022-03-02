using Messages;
using NServiceBus;
using NServiceBus.Logging;

namespace Sales
{
    public class PlaceOrderHandler : IHandleMessages<PlaceOrder>
    {
        public async Task Handle(PlaceOrder message, IMessageHandlerContext context)
        {
            //if (random.Next(0, 100) > 0)
            //{
            //    throw new Exception("BOOM");
            //}

            log.Info($"Received PlaceOrder, OrderId = {message.OrderId}");

            await context.Publish(new OrderPlaced { OrderId = message.OrderId });
        }

        static ILog log = LogManager.GetLogger<PlaceOrderHandler>();
        static Random random = new Random();
    }
}
