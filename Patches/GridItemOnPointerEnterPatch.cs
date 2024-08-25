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
			{
				Mod.Log.LogDebug("This is not an item.");
				return;
			}
			string itemTemplateId = __instance.Item.TemplateId;
			if (Common.Settings.ItemBlacklistList.Contains(itemTemplateId))
			{
				Mod.Log.LogDebug($"Item with template id {itemTemplateId} is blacklisted.");
				return;
			}
			Shared.hoveredItem = __instance.Item;
			if (Mod.TraderOfferTaskCache.taskDict.TryGetValue(__instance.Item.Id, out Structs.TimestampedTask itemTask)){
				Mod.Log.LogDebug($"Item ID {__instance.Item.Id} has been cached.");
				return;
			}
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(5000);
            itemTask = new Structs.TimestampedTask(cancellationTokenSource, Utils.TraderUtils.GetItemHighestTradingOffer(__instance.Item, cancellationTokenSource.Token), DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            Mod.TraderOfferTaskCache.taskDict.TryAdd(__instance.Item.Id, itemTask);
        }
	}
}
