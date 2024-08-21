using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LootValueEX.Patches.Inventory
{
    internal class ScavInvetoryShowPostPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.FirstMethod(typeof(ScavengerInventoryScreen), method => method.Name == nameof(ScavengerInventoryScreen.Show) && method.GetParameters()[0].Name == "mainController");

        [PatchPostfix]
        public static void Postfix(ScavengerInventoryScreen __instance)
        {
            IEnumerable<Item> items;
            __instance.method_3(out items);
            items.Do(RecursiveSearch);
        }
        public static void RecursiveSearch(Item item)
        {
            Mod.Log.LogInfo(string.Format("Item name: {0} -> {1} ({2})", item.LocalizedName(), item.GetType(), item.Parent?.ContainerName));
            item.GetAllItems().Where(queryItem => !queryItem.Equals(item)).Do(RecursiveSearch);
        }
    }
}
