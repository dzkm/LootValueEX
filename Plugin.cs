using BepInEx;
using BepInEx.Logging;

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

        private void Awake()
		{
            Config.SaveOnConfigSet = true;
			Log = base.Logger;
            Utils.SettingsUtils.SetupSettings(Config);

			new Patches.TraderPatch().Enable();
			new Patches.ShowTooltipPatch().Enable();
			new Patches.GridItemOnPointerEnterPatch().Enable();
			new Patches.GridItemOnPointerExitPatch().Enable();
			new Patches.ItemViewOnClickPatch().Enable();
			new Patches.Inventory.InventoryScreenShowPostPatch().Enable();
			new Patches.Inventory.TransferItemsScreenShowPostPatch().Enable();
			new Patches.Inventory.ScavInvetoryShowPostPatch().Enable();
            new Patches.WhatScreenIsThisPatch().Enable();

			Config.SettingChanged += Utils.SettingsUtils.SettingsChangedEvent;
		}
	}
}
