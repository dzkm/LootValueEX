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
        internal static bool PatchTooltip = false;

        [PatchPrefix]
        internal static void EnableTooltipPatch()
        {
            PatchTooltip = true;
        }

        [PatchPostfix]
        internal static void DisableTooltipPatch()
        {
            PatchTooltip = false;
        }
    }
}
