using EFT.UI;
using EFT;
using HarmonyLib;
using SPT.Reflection.Patching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using EFT.UI.Screens;

namespace LootValueEX.Patches
{
    internal class WhatScreenIsThisPatch : ModulePatch
    {
        public static EEftScreenType CurrentScreen;

        protected override MethodBase GetTargetMethod()
        {
            return AccessTools.FirstMethod(typeof(MenuTaskBar),
                x => x.Name == nameof(MenuTaskBar.OnScreenChanged));
        }

        [PatchPostfix]
        public static void PatchPostfix(EEftScreenType eftScreenType)
        {
            CurrentScreen = eftScreenType;

            Mod.Log.LogInfo($"Current screen: {eftScreenType}");
        }
    }
}
