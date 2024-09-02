using EFT.InventoryLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Structs
{
    internal struct RagfairOfferStruct
    {
        internal string ItemID { get; set; }
        internal string Currency { get; set; }
        internal int Price { get; set; }
        internal int ChanceToSell { get; set; }
        internal bool IsValid => Currency != null && Price > 0 && ChanceToSell > 0;
    }
}
