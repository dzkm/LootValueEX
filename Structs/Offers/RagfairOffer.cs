namespace LootValueEX.Structs.Offers
{
    internal struct RagfairOffer
    {
        internal int Price { get; }
        internal string Currency { get; }
        internal int Count { get; }
        internal RagfairOffer(int price, string currency, int count)
        {
            Price = price;
            Currency = currency;
            Count = count;
        }
    }
}
