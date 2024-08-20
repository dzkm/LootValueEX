namespace LootValueEX.Structs
{
    public readonly struct SellItemToTraderRequest
	{
		public string ItemID { get; }
		public string TraderID { get; }
		public int Price { get; }

		public SellItemToTraderRequest(string itemId, string traderId, int price)
		{
			this.ItemID = itemId;
			this.TraderID = traderId;
			this.Price = price;
		}
	}
}
