using HarmonyLib;
using KSP.UI;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIPlanetariumDateTime), "onGameUnPause")]
internal static class UIPlanetariumDateTime_onGameUnPause
{
    static void Postfix(UIPlanetariumDateTime __instance)
    {
        if (HUDReplacerColor.METDisplayColorGreen is Color color)
        {
            __instance.textDate.color = color;
        }
    }
}
