using System.Reflection;
using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using UnityEngine.EventSystems;

namespace LootValueEX.Patches
{
    internal class GridItemOnPointerExitPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod() => typeof(GridItemView).GetMethod("OnPointerExit", BindingFlags.Instance | BindingFlags.Public);

		[PatchPrefix]
		static void Prefix(GridItemView __instance, PointerEventData eventData)
		{

			if (Mod.TraderOfferTaskCache.taskDict.TryGetValue(__instance.Item.Id, out Structs.TimestampedTask item))
			{
				if (!item.Task.IsCompleted)
				{
					item.CancellationTokenSource.Cancel();
					item.CancellationTokenSource.Dispose();
					Mod.TraderOfferTaskCache.taskDict.TryRemove(__instance.Item.Id, out _);
				}
			}

			Shared.isStashItemHovered = false;
			Shared.hoveredItem = null;
		}
	}
}
