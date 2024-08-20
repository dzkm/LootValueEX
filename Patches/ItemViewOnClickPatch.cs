using System;
using System.Reflection;
using EFT.UI.DragAndDrop;
using SPT.Reflection.Patching;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace LootValueEX.Patches
{
    internal class ItemViewOnClickPatch : ModulePatch
	{
		protected override MethodBase GetTargetMethod() => typeof(GridItemView).GetMethod("OnClick", BindingFlags.Instance | BindingFlags.Public);

		private static DateTime lastFleaSellNotification = DateTime.MinValue;
		private static readonly int minSecondsBetweenNotifications = 30;

		[PatchPrefix]
		static void Prefix(GridItemView __instance, PointerEventData.InputButton button, Vector2 position, bool doubleClick)
		{
			if (__instance == null || __instance.Item == null)
			{
				if (Common.Tooltip.SimpleTooltip.Displayed)
				{
					Common.Tooltip.SimpleTooltip.Close();
					Shared.hoveredItem = null;
				}

				return;
			}
			if(!Common.Settings.EnableQuickSell.Value || Utils.RaidUtils.HasRaidStarted())
            {
                return;
            }
			try
			{
				if (Shared.IsKeyPressed(Common.Settings.QuicksellModifier.Value))
				{
					if (Common.Settings.OneButtonQuickSell.Value && button == PointerEventData.InputButton.Left)
					{
						Utils.OfferUtils.ComparePricesAndSellItem(__instance.Item);
						return;
					}
					switch (button)
					{
						case PointerEventData.InputButton.Left:
							Utils.OfferUtils.SellToHighestTrader(__instance.Item);
							break;
						case PointerEventData.InputButton.Right:
							Utils.OfferUtils.QuickSellToRagfair(__instance.Item);
							break;
					}
				}
			}
			catch (Exception ex)
			{
				Mod.Log.LogError(ex.Message);

				if (ex.InnerException != null)
				{
					Mod.Log.LogError(ex.InnerException.Message);
				}
				return;
			}
			return;
		}

	}
}
