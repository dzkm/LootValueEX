using EFT.InventoryLogic;
using SPT.Reflection.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LootValueEX.Utils
{
    internal class ItemUtils
    {

        internal static bool IsItemExamined(Item item)
        {
            return ClientAppUtils.GetMainApp().GetClientBackEndSession().Profile.Examined(item);
        }

        internal static Item UnstackItem(Item item)
        {
            if (item.StackObjectsCount > 1 || item.UnlimitedCount)
            {
                item = item.CloneItem();
                item.StackObjectsCount = 1;
                item.UnlimitedCount = false;
            }
            return item;
        }

        internal static bool IsItemFromTraderOrRagfair(Item item)
        {
            return item.Owner?.OwnerType == EOwnerType.RagFair || item.Owner?.OwnerType == EOwnerType.Trader;
        }

        internal static int GetCellSize(Item item)
        {
            XYCellSizeStruct size = item.CalculateCellSize();
            return size.X * size.Y;
        }

        internal async static Task<Structs.TraderOffer> GetBestTradingOffer(Item item)
        {
            if (!ClientAppUtils.GetMainApp().GetClientBackEndSession().Profile.Examined(item))
            {
                return default;
            }
            if (IsItemFromTraderOrRagfair(item))
            {
                item = UnstackItem(item);
            }
            return await TraderUtils.GetItemHighestTradingOffer(item, new System.Threading.CancellationTokenSource(5000).Token);
        }

        internal async static Task<double> GetRagfairPrice(Item item)
        {
            item = UnstackItem(item);
            if (!ClientAppUtils.GetMainApp().GetClientBackEndSession().Profile.Examined(item))
            {
                return 0;
            }
            if (IsItemFromTraderOrRagfair(item))
            {
                item = UnstackItem(item);
            }

            if (Shared.hoveredItem is Weapon weapon)
            {
                double totalFleaPrice = 0;
                IEnumerable<Task<double>> tasksFetchPrice = weapon.Mods.Select(RagfairUtils.FetchPrice);

                await Task.WhenAll(tasksFetchPrice);

                tasksFetchPrice.Select(task => task.ContinueWith(completedTask => totalFleaPrice += completedTask.Result));

                if (totalFleaPrice > 0)
                    return totalFleaPrice;
            }

            return await RagfairUtils.FetchPrice(Shared.hoveredItem);
        }
    }
}
