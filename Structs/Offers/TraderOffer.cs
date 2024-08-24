namespace LootValueEX.Structs
{
    internal readonly struct TraderOffer
    {
        internal string TraderId { get; }
        internal string TraderName { get; }
        internal int Price { get; }
        internal string Currency { get; }
        internal double Course { get; }
        internal int Count { get; }
        internal TraderOffer(string traderId, string traderName, int price, string currency, double course, int count)
        {
            TraderId = traderId;
            TraderName = traderName;
            Price = price;
            Currency = currency;
            Course = course;
            Count = count;
        }
    }
}
