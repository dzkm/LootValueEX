using EFT.UI;
using SPT.Reflection.Patching;
using SPT.Reflection.Utils;
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
            Plugin.Log.LogDebug("Stacktrace of tooltip call: \n" + stackTrace.ToString());
            if (GridItemTooltipPatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, $"<br>TemplateID: {GridItemTooltipPatch.HoveredItem?.TemplateId}<br>Item hashsum: {GridItemTooltipPatch.HoveredItem?.GetHashSum()}<br><color=#ff0fff><b>GridItemView</b></color>"));
            }
            if (InsuranceSlotPatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, $"<br>TemplateID: {InsuranceSlotPatch.HoveredItem?.TemplateId}<br><color=#00ffff><b>InsuranceSlotItemView</b></color>"));
            }
            if (ItemPricePatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, $"<br>TemplateID: {TradingItemPatch.HoveredItem?.TemplateId}<br><color=#00ff00><b>PriceTooltip</b></color>"));
            }
            if (HandbookPatching.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, $"<br>TemplateID: {HandbookPatching.HoveredItem?.TemplateId}<br><color=#0000ff><b>EntityIcon</b></color>"));
            }
            if (InsuranceGridPatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, $"<br>TemplateID: {InsuranceGridPatch.HoveredItem?.TemplateId}<br><color=#21f788><b>InsuranceItemView</b></color>"));
            }
            if (BarterItemPatch.PatchTooltip)
            {
                __instance.SetText(string.Concat(text, $"<br>TemplateID: {BarterItemPatch.HoveredItem?.TemplateId}<br><color=#ff6521><b>TradingRequisitePanel</b></color>"));
            }
            return;
        }
    }
}
