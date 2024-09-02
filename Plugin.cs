using BepInEx;
using BepInEx.Logging;
using EFT.InventoryLogic;
using LootValueEX.Offers;
using System;

namespace LootValueEX
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Mod : BaseUnityPlugin
    {
        // BepinEx
        public const string pluginGuid = "dev.dfms.LootValueEX";
        public const string pluginName = "LootValueEX";
        public const string pluginVersion = "0.2.0";
		public static ManualLogSource Log;
		internal static Common.TaskCache<Structs.TraderOfferStruct> TraderOfferTaskCache;
		internal static Common.TaskCache<Structs.RagfairOfferStruct> RagfairOfferTaskCache;
		internal static Predicate<ContainerCollection> IsWeaponOrModPredicate;

        private void Awake()
		{
            Config.SaveOnConfigSet = true;
			Log = base.Logger;
			TraderOfferTaskCache = new Common.TaskCache<Structs.TraderOfferStruct>(-1); // -1 means only manual cleaning.
			RagfairOfferTaskCache = new Common.TaskCache<Structs.RagfairOfferStruct>(600);
            IsWeaponOrModPredicate = (ContainerCollection container) => container is Weapon or EFT.InventoryLogic.Mod;
            Utils.SettingsUtils.SetupSettings(Config);

			new Patches.TraderPatch().Enable();
			new Patches.ShowTooltipPatch().Enable();
			new Patches.GridItemOnPointerEnterPatch().Enable();
			new Patches.GridItemOnPointerExitPatch().Enable();
			new Patches.ItemViewOnClickPatch().Enable();
			new Patches.Inventory.InventoryScreenShowPostPatch().Enable();
			new Patches.Inventory.TransferItemsScreenShowPostPatch().Enable();
			new Patches.Inventory.ScavInventoryShowPostPatch().Enable();
            new Patches.WhatScreenIsThisPatch().Enable();

            Config.SettingChanged += Utils.SettingsUtils.SettingsChangedEvent;
		}
	}
}
