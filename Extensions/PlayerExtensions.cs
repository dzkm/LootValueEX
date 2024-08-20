using EFT;
using System.Reflection;

namespace LootValueEX.Extensions
{
    internal static class PlayerExtensions
    {
        private static readonly FieldInfo InventoryControllerField =
            typeof(Player).GetField("_inventoryController", BindingFlags.NonPublic | BindingFlags.Instance);

        public static InventoryControllerClass GetInventoryController(this Player player) =>
            InventoryControllerField.GetValue(player) as InventoryControllerClass;
    }
}
