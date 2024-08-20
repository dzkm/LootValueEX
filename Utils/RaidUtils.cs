using Comfort.Common;
using EFT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Utils
{
    internal class RaidUtils
    {
        public static bool HasRaidStarted()
        {
            bool? inRaid = Singleton<AbstractGame>.Instance?.InRaid;
            return inRaid.HasValue && inRaid.Value;
        }
        public static bool HasRaidStartedAndCanShowRagfairInRaid()
        {
            return HasRaidStarted() && Common.Settings.ShowFleaPricesInRaid.Value;
        }
    }
}
