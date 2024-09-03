using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    class InsuranceSlotPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(EFT.UI.Insurance.InsuranceSlotItemView).GetMethod("OnPointerEnter", BindingFlags.Public | BindingFlags.Instance);
        internal static bool PatchTooltip = false;

        [PatchPrefix]
        internal static void EnableTooltipPatch(EFT.UI.Insurance.InsuranceSlotItemView __instance)
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
