using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    class InsuranceSlotPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EFT.UI.Insurance.InsuranceSlotItemView).GetMethod("OnPointerEnter", BindingFlags.Public | BindingFlags.Instance);
        internal static bool PatchTooltip { get; private set; } = false;
        internal static EFT.InventoryLogic.Item? HoveredItem { get; private set; }

        [PatchPrefix]
        internal static void EnableTooltipPatch(EFT.UI.Insurance.InsuranceSlotItemView __instance)
        {
            PatchTooltip = true;
            HoveredItem = __instance.Item;
        }

        [PatchPostfix]
        internal static void DisableTooltipPatch()
        {
            PatchTooltip = false;
            HoveredItem = null;
        }
    }
}
