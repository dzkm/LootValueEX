using EFT.UI;
using EFT.UI.DragAndDrop;
using LootValueEX.Extensions;
using SPT.Reflection.Patching;
using SPT.Reflection.Utils;
using System.Reflection;

namespace LootValueEX.Patches
{
    /// <summary>
    /// This patch will affect the following screens: Stash, Weapon Preset Builder, Character Gear, Character Preset Selector, New Ragfair Offer, Message Items, Loot
    /// </summary>
    internal class BarterItemPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(TradingRequisitePanel).GetMethod("method_1", BindingFlags.Instance | BindingFlags.Public);
        internal static bool PatchTooltip { get; private set; } = false;
        internal static EFT.InventoryLogic.Item? HoveredItem { get; private set; }

        [PatchPrefix]
        static void EnableTooltipPatch(GClass2064 ___gclass2064_0)
        {
            //This does not check if the item has been examined since the game also show what the item is on the barter regardless
            PatchTooltip = true;
            HoveredItem = ___gclass2064_0.RequiredItem;
        }

        [PatchPostfix]
        static void DisableTooltipPatch()
        {
            PatchTooltip = false;
            HoveredItem = null;
        }
    }
}
