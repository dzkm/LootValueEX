using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using UnityEngine.EventSystems;

namespace LootValueEX.Patches
{
    internal class GridItemOnPointerEnterPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod() => typeof(GridItemView).GetMethod("OnPointerEnter", BindingFlags.Instance | BindingFlags.Public);

		[PatchPrefix]
		static void Prefix(GridItemView __instance, PointerEventData eventData)
		{
			if(__instance.Item == null)
				return;

			if (Common.Settings.ItemBlacklistList.Contains(__instance.Item.TemplateId))
				return;
			Shared.hoveredItem = __instance.Item;
			Common.Actions.ProcessItemList([__instance.Item]);
        }
	}
}
