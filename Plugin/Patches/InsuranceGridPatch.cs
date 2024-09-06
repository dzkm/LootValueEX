using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    class InsuranceGridPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EFT.UI.Insurance.InsuranceItemView).GetMethod("OnPointerEnter", BindingFlags.Public | BindingFlags.Instance);
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
