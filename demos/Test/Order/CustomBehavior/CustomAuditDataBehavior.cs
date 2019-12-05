using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

namespace Order.CustomBehavior
{
    public class CustomAuditDataBehavior : Behavior<IAuditContext>
    {
        public override Task Invoke(IAuditContext context, Func<Task> next)
        {
            context.AddAuditData("myKey", "myValue");
            return next();
        }
    }
}

