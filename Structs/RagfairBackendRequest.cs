using System;

namespace LootValueEX.Structs
{
    internal readonly struct RagfairBackendRequest
	{
		internal long Timestamp { get; }
		internal EFT.InventoryLogic.Item[] Items { get; }

		public RagfairBackendRequest(EFT.InventoryLogic.Item[] items)
		{
			Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			Items = items;
		}
	}
}
