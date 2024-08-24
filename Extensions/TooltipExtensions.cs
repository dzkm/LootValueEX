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
            double bestValue = toolTipText.TraderPricePerUnit;
            string bestSeller = toolTipText.TraderName;
            if (toolTipText.RagfairPricePerUnit > toolTipText.TraderPricePerUnit)
            {
                bestValue = toolTipText.RagfairPricePerUnit;
                bestSeller = "Fleamarket";
            }

            if(bestValue <= 0)
            {
                return "<br><color=#ff0000>Not available for selling</color>";
            }
            // TODO: Fix this stinky code. Will show a wrong value when the item is on trader and stackable.
            double singularPrice = bestValue / toolTipText.ItemStackCount;
            string perSlotColor = SlotColoring.GetColorFromValuePerSlots((int)bestValue);
            string highlightText = $"<color=#FDDD45>{bestSeller}</color>";

            if (Common.Settings.OnlyShowTotalValue.Value)
            {
                return toolTipText.Text += $"<br>{highlightText}: <color={perSlotColor}>{bestValue.FormatNumber()}</color>";
            }

            toolTipText.Text += $"<br>{highlightText}: <color={perSlotColor}>{singularPrice.FormatNumber()}</color>";

            if (toolTipText.ItemStackCount > 1)
                toolTipText.Text += $" Total: {bestValue.FormatNumber()}";
            return toolTipText.Text;
        }
    }
}
