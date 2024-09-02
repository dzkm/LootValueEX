using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LootValueEX
{
	internal static class FleaPriceCache
	{
		static Dictionary<string, CachePrice> cache = new Dictionary<string, CachePrice>();

		public static async Task<double?> FetchPrice(string templateId)
		{
			bool fleaAvailable = Shared.Session.RagFair.Available || Common.Settings.ShowFleaPriceBeforeAccess.Value;

			if (!fleaAvailable || !Common.Settings.EnableFleaQuickSell.Value)
				return null;

			if (cache.ContainsKey(templateId))
			{
				double secondsSinceLastUpdate = (DateTime.Now - cache[templateId].lastUpdate).TotalSeconds;
				if (secondsSinceLastUpdate > 300)
					return await QueryAndTryUpsertPrice(templateId, true);
				else
					return cache[templateId].price;
			}
			else
				return await QueryAndTryUpsertPrice(templateId, false);
		}

		private static async Task<string> QueryPrice(string templateId)
		{
			return await CustomRequestHandler.PostJsonAsync("/LootValue/GetItemLowestFleaPrice", JsonConvert.SerializeObject(new Structs.RagfairBackendRequest(templateId)));
		}

		private static async Task<double?> QueryAndTryUpsertPrice(string templateId, bool update)
		{
			string response = await QueryPrice(templateId);

			bool hasPlayerFleaPrice = !(string.IsNullOrEmpty(response) || response == "null");

			if (hasPlayerFleaPrice)
			{
				double price = double.Parse(response);

				if (price < 0)
				{
					cache.Remove(templateId);
					return null;
				}

				if (update)
					cache[templateId].Update(price);
				else
					cache[templateId] = new CachePrice(price);

				return price;
			}

			return null;
		}
	}

	internal class CachePrice
	{
		public double price { get; private set; }
		public DateTime lastUpdate { get; private set; }

		public CachePrice(double price)
		{
			this.price = price;
			lastUpdate = DateTime.Now;
		}

		public void Update(double price)
		{
			this.price = price;
			lastUpdate = DateTime.Now;
		}
	}
}
