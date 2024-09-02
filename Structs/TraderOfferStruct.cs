using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Structs
{
    internal struct TraderOfferStruct
    {
        internal string ItemId { get; set; }
        internal string TraderId { get; set; }
        internal string TraderName { get; set; }
        internal int Price { get; set; }
        internal string Currency { get; set; }
        internal double Course { get; set; }
        internal int Stack { get; set; }
        internal bool IsValid => TraderId != null && Price > 0;
    }
}
