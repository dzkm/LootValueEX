using EFT.UI;
using HarmonyLib;
using System.Reflection;
using SPT.Reflection.Patching;
using System.Linq;
using EFT.InventoryLogic;
using System;
using System.Collections.Generic;
using LootValueEX.Utils;
using System.Threading.Tasks;

namespace LootValueEX.Patches.Inventory
{
    internal class InventoryScreenShowPostPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.FirstMethod(typeof(InventoryScreen), method => method.Name == nameof(InventoryScreen.Show) && method.GetParameters()[0].Name == "healthController");

        [PatchPostfix]
        public static void Postfix(InventoryControllerClass controller, LootItemClass lootItem, ItemContextAbstractClass itemContext)
        {
            List<Item> itemsToProcess = new List<Item>();
            if (!RaidUtils.HasRaidStarted())
            {
                IEnumerable<Item> stashItems = controller.Inventory.GetPlayerItems(EPlayerItems.Equipment).SelectMany(item => item.GetAllItems(Mod.IsWeaponOrModPredicate));
                itemsToProcess.AddRange(stashItems);
            }
            if(lootItem is not null)
            {
                if (lootItem is EquipmentClass)
                {
                    IEnumerable<Item> deadPlayerItems = lootItem.AllSlots.SelectMany(slots => slots.Items.SelectMany(item => item.GetAllItems(Mod.IsWeaponOrModPredicate)));
                    itemsToProcess.AddRange(deadPlayerItems);
                }
                else
                {
                    IEnumerable<Item> lootItems = lootItem.Grids.SelectMany(itemSlot => itemSlot.Items.SelectMany(item => item.GetAllItems(Mod.IsWeaponOrModPredicate)));
                    itemsToProcess.AddRange(lootItems);
                }
            }
            Common.Actions.ProcessItemList(itemsToProcess);
        }
    }
}
