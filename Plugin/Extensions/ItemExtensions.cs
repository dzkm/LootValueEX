using EFT.InventoryLogic;
using SPT.Reflection.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Extensions
{
    internal static class ItemExtensions
    {
        internal static bool IsExamined(this Item? item) => item != null && ClientAppUtils.GetMainApp().GetClientBackEndSession().Profile.Examined(item);
    }
}
