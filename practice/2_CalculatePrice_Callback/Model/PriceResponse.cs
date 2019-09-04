using NServiceBus;

namespace Model
{
    public class PriceResponse : IMessage
    {
        public PriceResponse(int price)
        {
            Price = price;
        }

        public int Price { get; set; }
    }
}
