using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    /// <summary>
    /// This patch will affect the following screens: Stash, Weapon Preset Builder, Character Gear, Character Preset Selector, New Ragfair Offer, Message Items, Loot
    /// </summary>
    internal class GridItemTooltipPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(GridItemView).GetMethod("ShowTooltip", BindingFlags.Instance | BindingFlags.Public);
        internal static bool PatchTooltip { get; private set; } = false;
        internal static EFT.InventoryLogic.Item? HoveredItem {  get; private set; }

        [PatchPrefix]
        static void EnableTooltipPatch(GridItemView __instance)
        {
            PatchTooltip = true;
            HoveredItem = __instance.Item;
        }

        [PatchPostfix]
        static void DisableTooltipPatch()
        {
            PatchTooltip = false;
            HoveredItem = null;
        }
    }
}
