using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EFT;
using EFT.InventoryLogic;

namespace LootValueEX.Utils
{
    internal class OfferUtils
    {

        public static void SellToHighestTrader(Item item)
        {
            Structs.TraderOffer traderOffer = ItemUtils.GetBestTradingOffer(item).Result;
            TraderUtils.SellItem(item, traderOffer);
        }

        public static void QuickSellToRagfair(Item item)
        {
            double? fleaPrice = Task.Run(() => FleaPriceCache.FetchPrice(item.TemplateId)).Result;
            //We do 1 ruble underpay just so it sells quickly
            RagfairUtils.SellItem(item, fleaPrice.Value - 1); 
        }

        public static void ComparePricesAndSellItem(Item item)
        {
            Structs.TraderOffer traderOffer = Utils.ItemUtils.GetBestTradingOffer(item).Result;
            double? fleaPrice = Task.Run(() => FleaPriceCache.FetchPrice(item.TemplateId)).Result;
            if(fleaPrice.HasValue && fleaPrice.Value > traderOffer.Price)
            {
                TraderUtils.SellItem(item, traderOffer);
            }
            else
            {
                RagfairUtils.SellItem(item, fleaPrice.Value);
            }
        }
    }
}
