using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    class TradingItemPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EFT.UI.DragAndDrop.TradingItemView).GetMethod("ShowTooltip", BindingFlags.Instance | BindingFlags.Public);
        internal static EFT.InventoryLogic.Item? HoveredItem { get; private set; }

        [PatchPrefix]
        internal static void GetHoveredItem(EFT.UI.DragAndDrop.TradingItemView __instance)
        {
            HoveredItem = __instance.Item;
        }
        [PatchPostfix]
        internal static void RemoveHoveredItem()
        {
            HoveredItem = null;
        }
    }
}
