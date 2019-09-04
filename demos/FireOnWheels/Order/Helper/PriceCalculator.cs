
using Messages;

namespace Order.Helper
{
    public static class PriceCalculator
    {
        public static int GetPrice(PriceRequest priceRequest)
        {
            return priceRequest.Weight * 3;
        }
    }
}
