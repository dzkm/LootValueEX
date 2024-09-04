using BepInEx;
using BepInEx.Logging;

namespace LootValueEX
{
    [BepInPlugin("pro.kaiden.lootvalueex", "LootValueEX", "0.1.0")]
    internal class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = base.Logger;

            new Patches.GridItemTooltipPatch().Enable();
            new Patches.TooltipPatch().Enable();
            new Patches.InsuranceGridPatch().Enable();
            new Patches.InsuranceSlotPatch().Enable();
            new Patches.ItemPricePatch().Enable();
            new Patches.HandbookPatching().Enable();
        }
    }
}
