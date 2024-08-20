using System;
using System.Reflection;
using EFT.UI;
using SPT.Reflection.Patching;
using System.Linq;
using static LootValueEX.Extensions.TooltipExtensions;

namespace LootValueEX.Patches
{
    internal class ShowTooltipPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod()
		{
			return typeof(SimpleTooltip).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name == "Show").ToList()[0];
		}

		[PatchPrefix]
		private static void Prefix(ref string text, ref float delay, SimpleTooltip __instance)
		{
			delay = 0;

			if(Shared.hoveredItem == null)
			{
                Mod.Log.LogDebug("Hovered item is null");
                return;
			}
			if(!Utils.ItemUtils.IsItemExamined(Shared.hoveredItem))
            {
                Mod.Log.LogDebug("Item is not examined");
                return;
            }
			if(!Common.Settings.ShowPrices.Value)
            {
                Mod.Log.LogDebug("Prices are not enabled");
                return;
            }

            Common.Tooltip.SimpleTooltip = __instance;

            Structs.TraderOffer bestTraderOffer = Utils.ItemUtils.GetBestTradingOffer(Shared.hoveredItem);
            SetText(ref text, new Structs.ToolTipText(
                "<br><color=#ffffff>Sell Price:</color>",
                0,
                bestTraderOffer.Price,
                bestTraderOffer.TraderName,
                Shared.hoveredItem.StackObjectsCount
                ));
        }

        private static void SetText(ref string text, Structs.ToolTipText toolTipText)
        {
            text += toolTipText.BuildToolTipText();
        }
    }
}
