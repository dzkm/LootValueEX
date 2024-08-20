using EFT.InventoryLogic;
using System.Threading.Tasks;
using FleaRequirement = GClass1859;

namespace LootValueEX.Utils
{
    internal class RagfairUtils
    {

        public static bool IsAvailable => Shared.Session.RagFair.Available;
        public static bool HasEnoughSlots => Common.Settings.IgnoreFleaMaxOfferCount.Value || 
            (Shared.Session.RagFair.MyOffersCount < Shared.Session.RagFair.GetMaxOffersCount(Shared.Session.RagFair.MyRating));

        public static async Task<double> FetchPrice(Item item)
        {
            return (await FleaPriceCache.FetchPrice(item.TemplateId)).GetValueOrDefault(0) * item.StackObjectsCount;
        }

        public static Task SellItem(Item item, double price)
        {
            if (!IsAvailable)
            {
                return Task.CompletedTask;
            }
            if (!HasEnoughSlots)
            {
                NotificationManagerClass.DisplayWarningNotification("Maximum number of flea offers reached");
                return Task.CompletedTask;
            }
            FleaRequirement[] requirement = [new FleaRequirement() { count = price, _tpl = "5449016a4bdc2d6f028b456f" }];
            Shared.Session.RagFair.AddOffer(false, [item.Id], requirement, () => NotificationManagerClass.DisplayMessageNotification("Added fleamarket offer"));
            return Task.CompletedTask;
        }
    }
}
