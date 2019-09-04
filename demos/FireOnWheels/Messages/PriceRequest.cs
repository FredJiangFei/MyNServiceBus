using NServiceBus;

namespace Messages
{
    public class PriceRequest : IMessage
    {
        public int Weight { get; set; }
    }
}
