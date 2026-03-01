using HarmonyLib;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(KSP.UI.Screens.Tumbler), "Awake")]
internal static class Tumbler_Awake
{
    static void Prefix(ref Color ___positiveColor, ref Color ___negativeColor)
    {
        if (HUDReplacerColor.TumblerColorPositive is Color positive)
            ___positiveColor = positive;
        if (HUDReplacerColor.TumblerColorNegative is Color negative)
            ___negativeColor = negative;
    }
}
