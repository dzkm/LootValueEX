using BepInEx.Configuration;
using System.Linq;
using UnityEngine;

namespace LootValueEX.Utils
{
    internal static class SettingsUtils
    {
        public static void SetupSettings(ConfigFile config)
        {

            Common.Settings.QuicksellModifier = config.Bind(
                "Quick Sell", 
                "Modifier to enable quickselling", 
                new KeyboardShortcut(KeyCode.LeftAlt, [KeyCode.LeftShift]), 
                @"Modifier to enable quickselling.
                Two button mode: Left Mouse Button sell to trader. Right Mouse Button sell to flea. 
                One button mode: Left Mouse Button both");

            Common.Settings.OneButtonQuickSell = config.Bind(
                "Quick Sell", 
                "One button quick sell", 
                false, 
                @"Selling is done using LMB only. 
                Attempts to sell to flea and then to trader if the option is enabled");

            Common.Settings.OneButtonQuickSellFlea = config.Bind(
                "Quick Sell", 
                "Sell to trader if no flea slots left", 
                true, 
                "Does nothing if 'Ignore flea max offer count' is enabled");

            Common.Settings.EnableQuickSell = config.Bind(
                "Quick Sell", 
                "Enable quick selling", 
                true);

            Common.Settings.EnableFleaQuickSell = config.Bind(
                "Quick Sell", 
                "Enable quick selling to flea", 
                true, 
                "Does nothing if quick selling is disabled");

            Common.Settings.ShowFleaPriceBeforeAccess = config.Bind(
                "FleaMarket", 
                "Show flea price before having access to flea", 
                false);

            Common.Settings.IgnoreFleaMaxOfferCount = config.Bind(
                "FleaMarket", 
                "Ignore flea max offer count", 
                false);

            Common.Settings.UseCustomColours = config.Bind(
                "Colours", 
                "Use custom colours", 
                false);

            Common.Settings.CustomColours = config.Bind(
                "Colours", 
                "Custom colours",
                "[5000:#ff0000],[10000:#ffff00],[:#ffffff]",
                @"Custom colors have the following format: [price:#HEXCOLOR]
                The last color must hold no ruble value, for example: [5000:#ff0000],[10000:#00ff00],[:#0000ff]
                In the example above the following coloring rules will apply:
                - anything below 5000 rubles will be red, 
                - anything below 10000 rubles will be green 
                - anything above 10000 rubles will be blue.

                The price must be a whole number, no fractions.
                Must be a valid hexadecimal color.
                "
            );

            Common.Settings.ShowPrices = config.Bind("Display", "Show prices", true);
            Common.Settings.OnlyShowTotalValue = config.Bind("Display", "Only show total value", false);
            Common.Settings.ShowFleaPricesInRaid = config.Bind("Display", "Show flea prices in raid", true);

            Common.Settings.TraderBlacklist = config.Bind("Traders", "Traders to ignore", "", "Separate values by comma, must use trader's id which is usually their name. The trader Id can also be found in user/mods/%trader_name%/db/base.json");
            Common.Settings.TraderBlacklistList.AddRange(Common.Settings.TraderBlacklist.Value.ToLower().Split(',').Select((string s) => s.Trim()));

            Common.Settings.ItemBlacklist = config.Bind("Items", "Items to ignore", "5696686a4bdc2da3298b456a,5449016a4bdc2d6f028b456f,569668774bdc2da2298b4568", "Separate values by comma, must use item's id which is usually their name. The item Id can also be found in user/mods/%mod_name%/db/items.json");
            Common.Settings.ItemBlacklistList.AddRange(Common.Settings.ItemBlacklist.Value.ToLower().Split(',').Select((string s) => s.Trim()));

            if (Common.Settings.UseCustomColours.Value.Equals(true))
                SlotColoring.ReadColors(Common.Settings.CustomColours.Value);

        }
        public static void SettingsChangedEvent(object sender, SettingChangedEventArgs e)
        {
            ConfigEntryBase entry = e.ChangedSetting;

            Mod.Log.LogInfo($"Settings changed - {entry.Definition.Section}:{entry.Definition.Key}");

            if (entry.Definition.Key == "Custom colours")
            {
                if (Common.Settings.UseCustomColours.Value)
                {
                    Mod.Log.LogInfo($"Read colors");
                    SlotColoring.ReadColors(Common.Settings.CustomColours.Value);
                }
            }

            if (entry.Definition.Key == "Custom colours" || entry.Definition.Key == "Use custom colours")
            {
                if (Common.Settings.UseCustomColours.Value)
                {
                    SlotColoring.ReadColors(Common.Settings.CustomColours.Value);
                }
                else
                {
                    SlotColoring.UseDefaultColors();
                }
            }

            if (entry.Definition.Key == "Traders to ignore")
            {
                Common.Settings.TraderBlacklistList.Clear();
                Common.Settings.TraderBlacklistList.AddRange(Common.Settings.TraderBlacklist.Value.ToLower().Split(','));
            }
        }
    }
}
