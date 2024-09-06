using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Patches
{
    internal class HandbookPatching : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EFT.HandBook.EntityIcon).GetMethod("method_1", BindingFlags.Public | BindingFlags.Instance);
        internal static bool PatchTooltip { get; private set; } = false;
        internal static EFT.InventoryLogic.Item? HoveredItem { get; private set; }

        [PatchPrefix]
        internal static void EnableTooltipPatch(ref EFT.InventoryLogic.Item ___item_0)
        {
            PatchTooltip = true;
            HoveredItem = ___item_0;
        }
        [PatchPostfix]
        internal static void DisableTooltipPatch()
        {
            PatchTooltip = false;
            HoveredItem = null;
        }
    }
}
