using BepInEx.Configuration;
using System.Collections.Generic;

namespace LootValueEX.Common
{
    public static class Settings
    {
        public static ConfigEntry<bool> UseCustomColours { get; set; }
        public static ConfigEntry<string> CustomColours { get; set; }
        public static ConfigEntry<bool> EnableQuickSell { get; set; }
        public static ConfigEntry<bool> EnableFleaQuickSell { get; set; }
        public static ConfigEntry<bool> OneButtonQuickSell { get; set; }
        public static ConfigEntry<bool> OneButtonQuickSellFlea { get; set; }
        public static ConfigEntry<bool> ShowFleaPricesInRaid { get; set; }
        public static ConfigEntry<bool> ShowPrices { get; set; }
        public static ConfigEntry<bool> OnlyShowTotalValue { get; set; }
        public static ConfigEntry<bool> ShowFleaPriceBeforeAccess { get; set; }
        public static ConfigEntry<bool> IgnoreFleaMaxOfferCount { get; set; }
        public static ConfigEntry<string> TraderBlacklist { get; set; }
        public static ConfigEntry<string> ItemBlacklist { get; set; }
        public static List<string> TraderBlacklistList { get; set; } = new List<string>();
        public static List<string> ItemBlacklistList { get; set; } = new List<string>();
        public static ConfigEntry<KeyboardShortcut> QuicksellModifier { get; set; }
    }
}
