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

			if (Mod.TaskCache.taskDict.TryGetValue(__instance.Item.TemplateId, out Structs.TimestampedTask item))
			{
				if (!item.Task.IsCompleted)
				{
					item.CancellationTokenSource.Cancel();
					item.CancellationTokenSource.Dispose();
					Mod.TaskCache.taskDict.TryRemove(__instance.Item.TemplateId, out _);
				}
			}

			Shared.isStashItemHovered = false;
			Shared.hoveredItem = null;
		}
	}
}
