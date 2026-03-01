using HarmonyLib;
using KSP.UI;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIButtonToggle), nameof(UIButtonToggle.Awake))]
internal static class UIButtonToggle_Awake
{
    static void Postfix(UIButtonToggle __instance)
    {
        if (HUDReplacerColor.PAWBlueButtonToggle is not Color color)
            return;

        if (__instance.toggleImage.mainTexture.name.Contains("Blue_Btn"))
        {
            __instance.toggleImage.color = color;
        }
    }
}
