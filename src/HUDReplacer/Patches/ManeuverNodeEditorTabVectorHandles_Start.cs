using HarmonyLib;
using TMPro;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ManeuverNodeEditorTabVectorHandles), "Start")]
internal static class ManeuverNodeEditorTabVectorHandles_Start
{
    static void Postfix(TextMeshProUGUI ___sliderTimeDVString)
    {
        if (HUDReplacerColor.ManeuverNodeEditorTextColor is Color color)
        {
            ___sliderTimeDVString.color = color;
        }
    }
}
