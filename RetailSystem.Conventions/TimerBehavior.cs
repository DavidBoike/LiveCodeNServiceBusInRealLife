namespace RetailSystem.Conventions;

using NServiceBus.Logging;
using NServiceBus.Pipeline
    ;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

public class TimerBehavior : Behavior<IInvokeHandlerContext>
{
    public override async Task Invoke(IInvokeHandlerContext context, Func<Task> next)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await next();
        }
        finally
        {
            stopwatch.Stop();
            log.Info($"Handling {context.MessageBeingHandled.GetType().FullName} in {context.MessageHandler.HandlerType.FullName} took {stopwatch.ElapsedMilliseconds} ms");
        }
    }

    static ILog log = LogManager.GetLogger<TimerBehavior>();
}
