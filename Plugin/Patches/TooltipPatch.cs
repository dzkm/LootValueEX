using EFT.UI;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Patches
{
    internal class TooltipPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() => typeof(SimpleTooltip).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name == "Show").ToList()[0];

        [PatchPostfix]
        public static void AlterText(SimpleTooltip __instance, string text)
        {
            StackTrace stackTrace = new StackTrace();
            Plugin.Log.LogDebug("Stacktrace of tooltip call: " + stackTrace.ToString());
            if (GridItemTooltipPatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, "<br><color=#ff0fff><b>GridItemView</b></color>"));
            }
            if (InsuranceSlotPatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, "<br><color=#00ffff><b>InsuranceItemView</b></color>"));
            }
            if (ItemPricePatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, "<br><color=#00ff00><b>PriceTooltip</b></color>"));
            }
            return;
        }
    }
}
