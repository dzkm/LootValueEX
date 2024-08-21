using EFT.UI;
using HarmonyLib;
using System.Reflection;
using SPT.Reflection.Patching;
using System.Linq;
using System.Collections.Generic;
using System;
using EFT.InventoryLogic;

namespace LootValueEX.Patches.Inventory
{
    internal class InventoryScreenShowPostPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.FirstMethod(typeof(InventoryScreen), method => method.Name == nameof(InventoryScreen.Show) && method.GetParameters()[0].Name == "healthController");

        [PatchPostfix]
        public static void Postfix(InventoryControllerClass controller, LootItemClass lootItem, ItemContextAbstractClass itemContext)
        {
            controller.Inventory.GetPlayerItems(EPlayerItems.Equipment).Do(RecursiveSearch);
            if (lootItem == null)
            {
                return;
            }
            if(lootItem is EquipmentClass)
            {
                lootItem.AllSlots.Do(slots => slots.Items.Do(RecursiveSearch));
                return;
            }
            lootItem.Grids.Do(itemSlot => itemSlot.Items.Do(RecursiveSearch));
            return;
        }
        public static void RecursiveSearch(Item item)
        {
            Mod.Log.LogInfo(string.Format("Item name: {0} -> {1}", item.LocalizedName(), item.GetType()));
            item.GetAllItems().Where(queryItem => !queryItem.Equals(item)).Do(RecursiveSearch);
        }
    }
}
