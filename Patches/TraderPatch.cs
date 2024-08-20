using LootValueEX.Extensions;
using SPT.Reflection.Patching;
using System.Reflection;

namespace LootValueEX.Patches
{
    internal class TraderPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(TraderClass).GetConstructors()[0];

        [PatchPostfix]
        private static void PatchPostfix(ref TraderClass __instance)
        {
            __instance.UpdateSupplyData();
        }
    }
}
