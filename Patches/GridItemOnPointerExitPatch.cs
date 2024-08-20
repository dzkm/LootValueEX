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
			Shared.isStashItemHovered = false;
			Shared.hoveredItem = null;
		}
	}
}
