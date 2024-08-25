using System;
using System.Reflection;
using EFT.UI;
using SPT.Reflection.Patching;
using System.Linq;
using static LootValueEX.Extensions.TooltipExtensions;
using System.Threading.Tasks;
using Sirenix.Serialization;
using EFT.UI.DragAndDrop;

namespace LootValueEX.Patches
{
    // TODO: This whole class needs to go. It would be better if I patched specific methods that calls SimpleTooltip.Show() instead of the base method. This would allow for different price checking on different purposes.
    internal class ShowTooltipPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod()
		{
			return typeof(SimpleTooltip).GetMethods(BindingFlags.Instance | BindingFlags.Public).Where(x => x.Name == "Show").ToList()[0];
		}

		[PatchPrefix]
		private static void ScheduleTask(ref string text, ref float delay, SimpleTooltip __instance)
		{
			delay = 0;
            Mod.Log.LogDebug("Tooltip has been invoked");
            if (Shared.hoveredItem == null)
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
            Mod.TraderOfferTaskCache.taskDict.TryGetValue(Shared.hoveredItem.Id, out Structs.TimestampedTask timestampedTask);
            text += "<br><color=red>Fetching prices...</color>";
        }

        [PatchPostfix]
        public static void CheckTaskCompletion(string text, SimpleTooltip __instance)
        {
            if(Shared.hoveredItem == null)
                return;

            if(!Mod.TraderOfferTaskCache.taskDict.TryGetValue(Shared.hoveredItem.Id, out Structs.TimestampedTask timestampedTask))
            {
                __instance.SetText(text.Replace("Fetching prices...", "Failed to fetch price."));
                return;
            };
            timestampedTask.Task.ContinueWith((task) => {
                if (task.IsCanceled)
                {
                    Mod.Log.LogDebug("Task has been canceled.");
                    return;
                }
                Mod.Log.LogDebug($"Task has been finished with status {task.Status}. Replacing text");
                Structs.ToolTipText toolTipText = new Structs.ToolTipText(
                "<br><color=#ffffff>Sell Price:</color>",
                0,
                task.Result.Price,
                task.Result.TraderName,
                Shared.hoveredItem.StackObjectsCount
                );
                __instance.SetText(text.Replace("<br><color=red>Fetching prices...</color>", toolTipText.BuildToolTipText()));
            });
        }
    }
}
