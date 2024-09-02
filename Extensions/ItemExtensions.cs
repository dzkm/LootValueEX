using EFT.InventoryLogic;
using LootValueEX.Offers;
using LootValueEX.Utils;
using SPT.Reflection.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LootValueEX.Extensions
{
    internal static class ItemExtensions
    {
        internal static bool IsExamined(this Item item) => ClientAppUtils.GetMainApp().GetClientBackEndSession().Profile.Examined(item);
        internal static bool IsFromTraderOrRagfair(this Item item) => item.Owner?.OwnerType == EOwnerType.RagFair || item.Owner?.OwnerType == EOwnerType.Trader;
        internal static bool HasNestedItems(this Item item) => item.GetAllItems().Where(nestedItem => !item.Equals(nestedItem)).Count() > 0;
        internal static string GetItemID(this Item item) => item.IsContainer ? item.GetHashSum().ToString() : item.TemplateId;
        internal static long FullCellSize(this Item item)
        {
            XYCellSizeStruct size = item.CalculateCellSize();
            return size.X * size.Y;
        }
        internal static Item UnstackItem(this Item item)
        {
            if (item.StackObjectsCount > 1 || item.UnlimitedCount)
            {
                item = item.CloneItem();
                item.StackObjectsCount = 1;
                item.UnlimitedCount = false;
            }
            return item;
        }
        internal async static Task<TraderOffer> GetBestTradingOffer(this Item item)
        {
            if (!item.IsExamined())
                return default;

            if (item.IsFromTraderOrRagfair())
                item = item.UnstackItem();

            Structs.TimestampedTask<TraderOffer> tradingOfferTask = Mod.TraderOfferTaskCache.GetTask(item.GetItemID());
            if (tradingOfferTask.Equals(default(Structs.TimestampedTask<Structs.TraderOfferStruct>)) || Mod.TraderOfferTaskCache.IsTaskCacheExpired(item.GetItemID()))
            {
                CancellationTokenSource cancellationTokenSource = new(5000);
                TraderOffer traderOffer = new([item]);
                tradingOfferTask = new Structs.TimestampedTask<TraderOffer>(
                    cancellationTokenSource,
                    traderOffer.GetTraderOffers(cancellationTokenSource.Token),
                    DateTimeOffset.Now.ToUnixTimeSeconds()
                    );
                Mod.TraderOfferTaskCache.AddTask(item.GetItemID(), tradingOfferTask);
            }
            return await tradingOfferTask.Task;
        }

        public static async Task<double> FetchRagfairPrice(this Item item)
        {
            return (await FleaPriceCache.FetchPrice(item.TemplateId)).GetValueOrDefault(0) * item.StackObjectsCount;
        }

        internal async static Task<double> GetRagfairPrice(this Item item)
        {
            item = UnstackItem(item);
            if (!item.IsExamined())
            {
                return 0;
            }

            if (item is not ContainerCollection containerCollection)
            {
                return await item.FetchRagfairPrice();
            }

            IEnumerable<Task<double>> tasksFetchPrice = containerCollection.Containers.SelectMany(container => container.Items).Select(item => item.FetchRagfairPrice());
            return (await Task.WhenAll(tasksFetchPrice)).Sum();
        }

        //TODO: See the possibility of removing this. Apparently the game has something for this already using a Predicate<T> on Item.GetAllItems().
        internal async static Task<List<Item>> GetNestedItems(this Item item)
        {
            List<Item> items = new List<Item>();
            foreach (var queryItem in item.GetAllItems())
            {
                if (!queryItem.Equals(item))
                {
                    items.Add(queryItem);
                    var nestedItems = await GetNestedItems(queryItem);
                    items.AddRange(nestedItems);
                }
            }
            return items;
        }
    }
}
