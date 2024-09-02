using System.Reflection;
using EFT.UI.DragAndDrop;
using LootValueEX.Extensions;
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
			if(Shared.hoveredItem != null)
			{
				Shared.hoveredItem = null;
			}
		}
	}
}
