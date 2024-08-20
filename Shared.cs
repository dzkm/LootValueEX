using BepInEx.Configuration;
using BepInEx.Logging;
using EFT.InventoryLogic;
using EFT.UI;
using SPT.Reflection.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LootValueEX
{
    internal class Shared
    {
        private Shared() { }
        private static Shared _instance;
        private static readonly object _lock = new object();
        public static Shared Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new Shared();
                }
            }
        }
        public static bool isStashItemHovered = false;
        public static ISession Session => ClientAppUtils.GetMainApp().GetClientBackEndSession();
        public static ManualLogSource logger { get; set; }
        public static Item hoveredItem;
        public static SimpleTooltip tooltip;
        public static List<string> blacklistedTraders = new List<string>();

        //Credit to DrakiaXYZ. Modified by me
        public static bool IsKeyPressed(KeyboardShortcut key)
        {
            if (!Input.GetKey(key.MainKey))
                return false;

            foreach (var modifier in key.Modifiers)
                if (!Input.GetKey(modifier))
                    return false;

            return true;
        }
    }
}
