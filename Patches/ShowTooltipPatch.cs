using System;
using System.Reflection;
using EFT.UI;
using SPT.Reflection.Patching;
using System.Linq;
using static LootValueEX.Extensions.TooltipExtensions;
using System.Threading.Tasks;
using Sirenix.Serialization;
using EFT.UI.DragAndDrop;
using LootValueEX.Extensions;

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
            if (Shared.hoveredItem == null)
                return;

            if (!Shared.hoveredItem.IsExamined())
                return;

			if(!Common.Settings.ShowPrices.Value)
                Mod.Log.LogDebug("Prices are not enabled");

            Common.Tooltip.SimpleTooltip = __instance;
            text += "<br><color=red>Fetching prices...</color>";
        }

        [PatchPostfix]
        public static void CheckTaskCompletion(string text, SimpleTooltip __instance)
        {
            if(Shared.hoveredItem == null)
                return;
            
            Structs.ToolTipText toolTipText = new("<br><color=red>Failed to fetch prices</color>", 0, 0, 0, "<color=red>No trader available</color>", 1);
            
            Structs.TimestampedTask<Structs.TraderOfferStruct> traderTask = Mod.TraderOfferTaskCache.GetTask(Shared.hoveredItem.GetItemID());
            Structs.TimestampedTask<Structs.RagfairOfferStruct> ragfairTask = Mod.RagfairOfferTaskCache.GetTask(Shared.hoveredItem.GetItemID());
            traderTask.Task.ContinueWith((task) => {
                if (task.IsCanceled)
                    return;

                if (task.IsFaulted)
                {
                    Mod.Log.LogError(task.Exception);
                    return;
                }

                if(!task.Result.IsValid)
                    return;

                toolTipText.TraderName = task.Result.TraderName;
                toolTipText.TraderPrice = task.Result.Price;
            });
            ragfairTask.Task.ContinueWith((task) =>
            {
                if (task.IsCanceled)
                    return;

                if (task.IsFaulted)
                {
                    Mod.Log.LogError(task.Exception);
                    return;
                }
                if (!task.Result.IsValid)
                    return;

                toolTipText.RagfairPrice = task.Result.Price;
            });
            toolTipText.Text = "<br><color=white>Sell prices:</color><br>";
            toolTipText.ItemStackCount = Shared.hoveredItem.StackObjectsCount;
            __instance.SetText(text.Replace("<br><color=red>Fetching prices...</color>", toolTipText.BuildToolTipText()));
        }
    }
}
