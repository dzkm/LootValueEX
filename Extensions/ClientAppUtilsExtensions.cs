using SPT.Reflection.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LootValueEX.Extensions
{
    internal static class TarkovApplicationExtensions
    {
        internal static IEnumerable<TraderClass> GetValidTraders(this EFT.TarkovApplication client) => client.GetClientBackEndSession().DisplayableTraders.Where(trader => !Common.Settings.TraderBlacklistList.Contains(trader.Id.ToLower()));
        internal static bool RagfairUnlocked(this EFT.TarkovApplication client) => client.Session.RagFair.Available;
    }
}
