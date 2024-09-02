using EFT.InventoryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Common
{
    internal static class Actions
    {
        public static void ProcessItemList(List<Item> items)
        {
            Offers.TraderOffer traderOffer = new(items);
            _ = traderOffer.GetTraderOffers(new System.Threading.CancellationTokenSource(5000).Token, Mod.TraderOfferTaskCache);
            if (Settings.EnableFleaQuickSell.Value)
            {
                Offers.RagfairOffer ragfairOffer = new(items);
                _ = ragfairOffer.GetOffers(new System.Threading.CancellationTokenSource(5000).Token, Mod.RagfairOfferTaskCache);
            }
        }
    }
}
