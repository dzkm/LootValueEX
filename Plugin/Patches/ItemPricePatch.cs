using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    internal class ItemPricePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EFT.UI.PriceTooltip).GetMethod("Show", 
            BindingFlags.Instance | BindingFlags.Public,
            null,
            [typeof(EFT.InventoryLogic.EOwnerType), typeof(string), typeof(int), typeof(string)],
            null);
        internal static bool PatchTooltip { get; private set; } = false;

        [PatchPrefix]
        internal static void EnableTooltipPatch()
        {
            if (TradingItemPatch.HoveredItem == null)
                return;

            PatchTooltip = true;
        }

        [PatchPostfix]
        internal static void DisableTooltipPatch()
        {
            PatchTooltip = false;
        }
    }
}
