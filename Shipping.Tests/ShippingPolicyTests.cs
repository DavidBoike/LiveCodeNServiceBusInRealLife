using Messages;
using NServiceBus.Testing;
using NUnit.Framework;

namespace Shipping.Tests;
public class ShippingPolicyTests
{
    [Test]
    public async Task ShouldNotPublishAfterOrderPlaced()
    {
        // Arrange
        var saga = new ShippingPolicy { Data = new ShippingPolicy.SagaData() };
        var context = new TestableMessageHandlerContext();
        var orderPlaced = new OrderPlaced { OrderId = "12345" };

        // Act
        await saga.Handle(orderPlaced, context);

        // Assert
        Assert.That(context.PublishedMessages.Length, Is.EqualTo(0), "Should not publish messages");
        // Assert.That(context.SentMessages.Length, Is.EqualTo(0), "Should not send messages");
        Assert.That(context.SentMessages.Length, Is.EqualTo(1), "Timeout is sent message, should not send others");

        var timeout = context.TimeoutMessages.Single();
        Assert.That(timeout.Within, Is.EqualTo(TimeSpan.FromSeconds(5)));
    }

    [Test]
    public async Task ShouldPublishAfterBothEvents()
    {
        // Arrange
        var saga = new ShippingPolicy
        {
            Data = new ShippingPolicy.SagaData
            {
                OrderId = "12345",
                Placed = true
            }
        };
        var context = new TestableMessageHandlerContext();
        var orderBilled = new OrderBilled { OrderId = "12345" };

        // Act
        await saga.Handle(orderBilled, context);

        // Assert
        //Assert.That(context.PublishedMessages.Length, Is.EqualTo(1), "Should publish OrderShipped");
        //Assert.That(saga.Completed);
        await Verify(context);
    }
}
