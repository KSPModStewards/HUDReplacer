using HarmonyLib;
using KSP.UI.Screens.Flight;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(SpeedDisplay), "Start")]
internal static class SpeedDisplay_Start
{
    static void Postfix(SpeedDisplay __instance)
    {
        if (HUDReplacerColor.SpeedDisplayColorText is Color textColor)
        {
            __instance.textTitle.color = textColor;
        }
        if (HUDReplacerColor.SpeedDisplayColorSpeed is Color speedColor)
        {
            __instance.textSpeed.color = speedColor;
        }
    }
}
