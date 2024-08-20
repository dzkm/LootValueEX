using SPT.Reflection.Utils;
using Comfort.Common;
using EFT;
using System.Reflection;

namespace LootValueEX.Extensions
{
    internal static class TraderClassExtensions
    {
        private static ISession Session => ClientAppUtils.GetMainApp().GetClientBackEndSession();

        private static readonly FieldInfo SupplyDataField =
            typeof(TraderClass).GetField("supplyData_0", BindingFlags.NonPublic | BindingFlags.Instance);

        internal static SupplyData GetSupplyData(this TraderClass trader) =>
            SupplyDataField.GetValue(trader) as SupplyData;

        public static void SetSupplyData(this TraderClass trader, SupplyData supplyData) =>
            SupplyDataField.SetValue(trader, supplyData);

        public static async void UpdateSupplyData(this TraderClass trader)
        {
            Result<SupplyData> result = await Session.GetSupplyData(trader.Id);

            if (result.Failed)
                return;

            trader.SetSupplyData(result.Value);
        }
    }
}
