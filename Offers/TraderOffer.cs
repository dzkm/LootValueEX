using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using LootValueEX.Common;
using LootValueEX.Extensions;
using SPT.Reflection.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LootValueEX.Offers
{
    internal class TraderOffer
    {
        internal List<Item> Items { get; }
        private List<Structs.TraderOfferStruct> TradeOffers { get; set; }
        
        internal TraderOffer(List<Item> items)
        {
            Items = items;
        }

        internal void SellItem(string itemId)
        {
            Structs.TraderOfferStruct traderOffer = TradeOffers.FirstOrDefault(offer => offer.ItemId == itemId);
            if (!traderOffer.IsValid)
            {
                Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.ErrorMessage);
                NotificationManagerClass.DisplayWarningNotification("Failed to sell item to a trader.");
                Mod.Log.LogError("Invalid trading offer when selling. Cancelling operation");
                return;
            }

            Item item = Items.FirstOrDefault(item => item.TemplateId == itemId || item.GetHashSum().ToString() == itemId);
            GClass2063.Class1765 sellTask = new();
            sellTask.source = new TaskCompletionSource<bool>();
            ClientAppUtils.GetMainApp().GetClientBackEndSession().ConfirmSell(
                traderOffer.TraderId,
                [new EFT.Trading.TradingItemReference() { Item = item, Count = item.StackObjectsCount }],
                traderOffer.Price,
                new Callback(sellTask.method_0));
            Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TradeOperationComplete);
        }

        private Structs.TraderOfferStruct GetSingleTraderOffer(Item item, TraderClass trader)
        {
            TraderClass.GStruct244? traderPrice = trader.GetUserItemPrice(item);
            if (traderPrice.Equals(null))
                return default;
            return new Structs.TraderOfferStruct
            {
                ItemId = item.GetItemID(),
                TraderId = trader.Id,
                TraderName = trader.LocalizedName,
                Price = traderPrice.Value.Amount,
                Currency = GClass2531.GetCurrencyCharById(traderPrice.Value.CurrencyId),
                Course = trader.GetSupplyData().CurrencyCourses[traderPrice.Value.CurrencyId],
                Stack = item.StackObjectsCount
            };
        }

        internal async Task<List<Structs.TraderOfferStruct>> GetTraderOffers(CancellationToken cancellationToken)
        {
            await Task.Delay(200);
            foreach(Item item in Items)
            {
                if (cancellationToken.IsCancellationRequested)
                    throw new TaskCanceledException();

                Structs.TraderOfferStruct highestOffer = new();
                foreach (TraderClass trader in ClientAppUtils.GetMainApp().GetValidTraders())
                {
                    Structs.TraderOfferStruct currentOffer = GetSingleTraderOffer(item, trader);
                    if (currentOffer.IsValid || currentOffer.Price > highestOffer.Price)
                        highestOffer = currentOffer;
                }
                TradeOffers.Append(highestOffer);
            }
            return TradeOffers;
        }

        internal async Task GetTraderOffers(CancellationToken cancellation, TaskCache<Structs.TraderOfferStruct> taskCache)
        {
            await Task.Delay(200);
            foreach(Item item in Items)
            {
                if(cancellation.IsCancellationRequested)
                    throw new TaskCanceledException();
                Task<Structs.TraderOfferStruct> cachedTask = taskCache.GetTask(item.GetItemID()).Task;
                if (!taskCache.IsTaskCacheExpired(item.GetItemID()) && cachedTask.IsCompleted)
                    continue;

                TaskCompletionSource<Structs.TraderOfferStruct> taskCompletionSource = new(TaskCreationOptions.RunContinuationsAsynchronously);
                await Task.Factory.StartNew(() =>
                {
                    Structs.TraderOfferStruct highestOffer = new();
                    foreach(TraderClass trader in ClientAppUtils.GetMainApp().GetValidTraders())
                    {
                        Structs.TraderOfferStruct currentOffer = GetSingleTraderOffer(item, trader);
                        if(currentOffer.IsValid || currentOffer.Price > highestOffer.Price)
                            highestOffer = currentOffer;
                    }
                    taskCompletionSource.SetResult(highestOffer);
                });
                taskCache.AddTask(item.GetItemID(), taskCompletionSource.Task, new CancellationTokenSource(5000));
            }
        }
    }
}
