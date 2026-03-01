using HarmonyLib;
using KSP.UI.Screens;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(StageManager), "Awake")]
internal static class StageManager_Awake
{
    static void Postfix(StageManager __instance)
    {
        if (__instance && HUDReplacerColor.StageTotalDeltaVColor is Color color)
        {
            __instance.deltaVTotalText.color = color;
        }
    }
}
