using NServiceBus;

namespace Model
{
    [TimeToBeReceived("00:01:00")]
    public class PriceRequest : IMessage
    {
        public PriceRequest(int weight)
        {
            Weight = weight;
        }

        public int Weight { get; set; }
    }
}
