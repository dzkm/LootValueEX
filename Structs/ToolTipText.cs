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
        internal double RagfairPrice { get; set; }
        internal double RagfairChance { get; set; }
        internal double TraderPrice { get; set; }
        internal string TraderName { get; set; }
        internal int ItemStackCount { get; set; }

        internal ToolTipText(string text, double ragfairPrice, double ragfairChance, double traderPrice, string traderName, int itemStackCount)
        {
            Text = text;
            RagfairPrice = ragfairPrice;
            RagfairChance = ragfairChance;
            TraderPrice = traderPrice;
            TraderName = traderName;
            ItemStackCount = itemStackCount;
        }
    }
}
