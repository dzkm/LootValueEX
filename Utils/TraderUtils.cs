using Comfort.Common;
using EFT.InventoryLogic;
using EFT.UI;
using LootValueEX.Extensions;
using SPT.Reflection.Utils;
using System.Threading;
using System.Threading.Tasks;
using CurrencyUtil = GClass2531;

namespace LootValueEX.Utils
{
    internal class TraderUtils
    {

        private static bool IsValidOffer(Structs.TraderOffer offer) =>
            offer.Price > 0 && offer.Count > 0;

        internal static Structs.TraderOffer GetTraderOffer(Item item, TraderClass trader)
        {
            var result = trader.GetUserItemPrice(item);
            if (result == null)
                return default;

            return new Structs.TraderOffer(
                trader.Id,
                trader.LocalizedName,
                result.Value.Amount,
                CurrencyUtil.GetCurrencyCharById(result.Value.CurrencyId),
                trader.GetSupplyData().CurrencyCourses[result.Value.CurrencyId],
                item.StackObjectsCount
            );
        }

        internal async static Task<Structs.TraderOffer> GetItemHighestTradingOffer(Item item, CancellationToken cancellationToken)
        {
            Mod.Log.LogDebug($"Task for item {item.TemplateId} has been scheduled");
            await Task.Delay(2500);
            Structs.TraderOffer highestOffer = new Structs.TraderOffer();
            foreach (TraderClass trader in ClientAppUtils.GetMainApp().GetClientBackEndSession().Traders)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Mod.Log.LogDebug($"Task for item {item.TemplateId} has been canceled.");
                    throw new TaskCanceledException();
                }

                if (Common.Settings.TraderBlacklistList.Contains(trader.Id.ToLower()))
                    continue;

                if (!trader.Info.Available || trader.Info.Disabled || !trader.Info.Unlocked)
                    continue;

                Structs.TraderOffer currentOffer = GetTraderOffer(item, trader);
                if (IsValidOffer(currentOffer) || currentOffer.Price > highestOffer.Price)
                    highestOffer = currentOffer;
            }
            Mod.Log.LogDebug($"Task for item {item.TemplateId} has been finished");
            return highestOffer;
        }

        internal static void SellItem(Item item, Structs.TraderOffer traderOffer)
        {
            TraderClass trader = Shared.Session.GetTrader(traderOffer.TraderId);
            
            // This creates an async trader sell task
            GClass2063.Class1765 sellTask = new GClass2063.Class1765();
            sellTask.source = new TaskCompletionSource<bool>();
            Shared.Session.ConfirmSell(
                trader.Id,
                [new EFT.Trading.TradingItemReference() { Item = item, Count = item.StackObjectsCount }], 
                traderOffer.Price, 
                new Callback(sellTask.method_0));
            Singleton<GUISounds>.Instance.PlayUISound(EUISoundType.TradeOperationComplete);
        }
    }
}
