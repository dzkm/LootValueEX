using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Extensions
{
    internal static class TooltipExtensions
    {
        public static string BuildToolTipText(this Structs.ToolTipText toolTipText)
        {
            if (toolTipText.RagfairPrice <= 0 && toolTipText.TraderPrice <= 0)
                return "<br><color=#ff0000>Not available for selling</color>";

            string highlightText = "<color=#FDDD45>{0}</color>";
            string ragfairTitle = "RAG FAIR".Localized();
            // "{Trader}: {Price} {Total}<br>{Ragfair}: {Price} {Total} ({SellChance}%)
            string priceFormat = "{0}: {1} Total:{2}<br>{4}: {5} Total: {6} ({7}%)".Localized();

            if (toolTipText.RagfairPrice > toolTipText.TraderPrice)
            {
                ragfairTitle = string.Format(highlightText, ragfairTitle);
                //ragfairTitle = string.Format(highlightText, toolTipText.RagfairPrice > toolTipText.TraderPrice ? "RAG FAIR".Localized() : toolTipText.TraderName);
            }
            else
            {
                toolTipText.TraderName = string.Format(highlightText, toolTipText.TraderName);
            }

            if (Common.Settings.OnlyShowTotalValue.Value)
            {
                priceFormat = "{0}: {1}<br>{4}: {5} ({6}%)";
                return toolTipText.Text += string.Format(priceFormat,
                    toolTipText.TraderName,
                    "Total: " + toolTipText.TraderPrice,
                    ragfairTitle,
                    toolTipText.RagfairPrice,
                    toolTipText.RagfairChance);
            }

            double singularTraderPrice = toolTipText.TraderPrice;
            double singularRagfairPrice = toolTipText.RagfairPrice;
            if (toolTipText.ItemStackCount > 1)
            {
                singularRagfairPrice = toolTipText.RagfairPrice / toolTipText.ItemStackCount;
                singularTraderPrice = toolTipText.TraderPrice / toolTipText.ItemStackCount;
            }
            return string.Format(priceFormat, toolTipText.TraderName, singularTraderPrice, toolTipText.TraderPrice, ragfairTitle, singularRagfairPrice, toolTipText.RagfairPrice, toolTipText.RagfairChance);
        }
    }
}
