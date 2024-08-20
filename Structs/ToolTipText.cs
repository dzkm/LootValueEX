using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Structs
{
    internal struct ToolTipText
    {
        internal string Text { get; set; }
        internal readonly double RagfairPricePerUnit { get; }
        internal readonly double TraderPricePerUnit { get; }
        internal readonly string TraderName { get; }
        internal readonly int ItemStackCount { get; }

        internal ToolTipText(string text, double ragfairPricePerUnit, double traderPricePerUnit, string traderName, int itemStackCount)
        {
            Text = text;
            RagfairPricePerUnit = ragfairPricePerUnit;
            TraderPricePerUnit = traderPricePerUnit;
            TraderName = traderName;
            ItemStackCount = itemStackCount;
        }
    }
}
