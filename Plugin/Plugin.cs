using BepInEx;
using BepInEx.Logging;

namespace LootValueEX
{
    [BepInPlugin("pro.kaiden.lootvalueex", "LootValueEX", "0.2.0")]
    internal class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private void Awake()
        {
            Log = base.Logger;
        }
    }
}
