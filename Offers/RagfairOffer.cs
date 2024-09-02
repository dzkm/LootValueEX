using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using LootValueEX.Extensions;
using Newtonsoft.Json;
using SPT.Reflection.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static GClass1859;

namespace LootValueEX.Offers
{
    internal class RagfairOffer
    {
        internal List<Item> Items { get; }
        internal List<Structs.RagfairOfferStruct> RagfairOffers { get; set; }

        internal RagfairOffer(List<Item> items)
        {
            Items = items;
        }

        internal void AddOffer(string itemId)
        {
            if (!Shared.Session.RagFair.Available)
                return;
            Structs.RagfairOfferStruct ragfairOffer = RagfairOffers.FirstOrDefault(offer => offer.ItemID == itemId);
            if(!ragfairOffer.IsValid)
            {
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                NotificationManagerClass.DisplayWarningNotification("Failed to add offer to Fleamarket.");
                Mod.Log.LogError("Invalid ragfair offer when adding. Cancelling operation");
                return;
            }
            if (!Common.Settings.IgnoreFleaMaxOfferCount.Value && (Shared.Session.RagFair.MyOffersCount >= Shared.Session.RagFair.GetMaxOffersCount(Shared.Session.RagFair.MyRating)))
            {
                NotificationManagerClass.DisplayWarningNotification("ragfair/Reached maximum amount of offers".Localized());
                return;
            }
            Item itemToSell = Items.First(item => item.GetItemID().Equals(itemId));
            GClass1859[] fleaRequirement = [new GClass1859() { count = ragfairOffer.Price-1, _tpl = "5449016a4bdc2d6f028b456f" }];
            Shared.Session.RagFair.AddOffer(false, [itemToSell.Id], fleaRequirement, () => NotificationManagerClass.DisplayMessageNotification("Added fleamarket offer"));
        }

        internal async Task<List<Structs.RagfairOfferStruct>> GetOffers(CancellationToken cancellationToken)
        {
            if (!Common.Settings.EnableFleaQuickSell.Value)
                return default;

            if (!Shared.Session.RagFair.Available)
                if (!Common.Settings.ShowFleaPriceBeforeAccess.Value)
                    return default;

            string serializedRequest = JsonConvert.SerializeObject(new Structs.RagfairBackendRequest(Items.ToArray()));
            string priceRequest = await CustomRequestHandler.PostJsonAsync("/lootvalue/ragfair/highestchanceoffer", serializedRequest);
            if (priceRequest == null)
                return default;

            List<Structs.RagfairOfferStruct> offers = new();
            JsonConvert.PopulateObject(priceRequest, offers);
            return offers;
        }

        internal async Task GetOffers(CancellationToken cancellationToken, Common.TaskCache<Structs.RagfairOfferStruct> taskCache)
        {
            if (!Common.Settings.EnableFleaQuickSell.Value)
                return;
            if (!ClientAppUtils.GetMainApp().RagfairUnlocked())
                if (!Common.Settings.ShowFleaPriceBeforeAccess.Value)
                    return;

            await Task.Delay(200);
            Dictionary<string, TaskCompletionSource<Structs.RagfairOfferStruct>> dictCompletionSource = new();
            Items.ForEach(item => {
                TaskCompletionSource<Structs.RagfairOfferStruct> tsc = new(TaskCreationOptions.RunContinuationsAsynchronously);
                dictCompletionSource.Add(item.GetItemID(), tsc);
                CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
                taskCache.AddTask(item.GetItemID(), tsc.Task, cancellationTokenSource);
            });
            string serializedRequest = JsonConvert.SerializeObject(new Structs.RagfairBackendRequest(Items.ToArray()));
            string priceRequest = await CustomRequestHandler.PostJsonAsync("/lootvalue/ragfair/highestchanceoffer", serializedRequest);
            if (priceRequest == null)
            {
                dictCompletionSource.Clear();
                Items.ForEach(item => taskCache.CancelTask(item.GetItemID()));
                return;
            }
            List<Structs.RagfairOfferStruct> offers = new();
            JsonConvert.PopulateObject(priceRequest, offers);
            offers.ForEach(offer =>
            {
                dictCompletionSource.TryGetValue(offer.ItemID, out TaskCompletionSource<Structs.RagfairOfferStruct> taskCompletionSource);
                taskCompletionSource.SetResult(offer);
            });
            RagfairOffers = offers;
        }
    }
}
