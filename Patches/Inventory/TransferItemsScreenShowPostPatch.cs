using EFT.InventoryLogic;
using EFT.UI;
using EFT.UI.DragAndDrop;
using HarmonyLib;
using SPT.Reflection.Patching;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LootValueEX.Patches.Inventory
{
    internal class TransferItemsScreenShowPostPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => AccessTools.FirstMethod(typeof(TransferItemsScreen), method => method.Name == nameof(TransferItemsScreen.Show) && method.GetParameters()[0].Name == "messages");

        [PatchPostfix]
        public static void Postfix(IEnumerable<ChatMessageClass> messages)
        {
            /*
             * This function will get all the items contained in the messages.
             * 
             * Class2719.class2719_0 is a helper class for the items contained in the messages.
             * method_0 is the one that actually get the items
             * method_1 returns true if the item is not an quest item
             */
            // I know we are doing double the work here, since this is already done in TransferItemsScreen.Show(), but it's the only way to get the items.
            IEnumerable<Item> itemArray = messages.SelectMany(TransferItemsScreen.Class2719.class2719_0.method_0).Where(TransferItemsScreen.Class2719.class2719_0.method_1);
            itemArray.Do(RecursiveSearch);
        }
        public static void RecursiveSearch(Item item)
        {
            Mod.Log.LogInfo(string.Format("Item name: {0} -> {1} ({2})", item.LocalizedName(), item.GetType(), item.Parent?.ContainerName));
            item.GetAllItems().Where(queryItem => !queryItem.Equals(item)).Do(RecursiveSearch);
        }
    }
}
