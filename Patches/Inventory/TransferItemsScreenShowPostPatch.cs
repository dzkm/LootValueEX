using EFT.InventoryLogic;
using EFT.UI;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Linq;
using System.Reflection;

namespace LootValueEX.Patches.Inventory
{
    internal class TransferItemsScreenShowPostPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.FirstMethod(typeof(TransferItemsScreen), method => method.Name == nameof(TransferItemsScreen.Show) && method.GetParameters()[0].Name == "messages");

        [PatchPostfix]
        public static void Postfix(InventoryControllerClass controller, StashClass playerStash)
        {
            Mod.Log.LogInfo(controller.GetType());
            controller.ContainedItems.Do(RecursiveSearch);
            Mod.Log.LogInfo(playerStash.GetType());
            playerStash.GetAllItems().Do(RecursiveSearch);
        }
        public static void RecursiveSearch(Item item)
        {
            Mod.Log.LogInfo(string.Format("Item name: {0} -> {1}", item.LocalizedName(), item.GetType()));
            item.GetAllItems().Where(queryItem => !queryItem.Equals(item)).Do(RecursiveSearch);
        }
    }
}
