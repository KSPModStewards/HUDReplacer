using HarmonyLib;
using TMPro;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ManeuverNodeEditorTabOrbitBasic), "Start")]
internal static class ManeuverNodeEditorTabOrbitBasic_Start
{
    static void Postfix(
        TextMeshProUGUI ___apoapsisAltitude,
        TextMeshProUGUI ___apoapsisTime,
        TextMeshProUGUI ___periapsisAltitude,
        TextMeshProUGUI ___periapsisTime,
        TextMeshProUGUI ___orbitPeriod
    )
    {
        if (HUDReplacerColor.ManeuverNodeEditorTextColor is Color color)
        {
            ___apoapsisAltitude.color = color;
            ___apoapsisTime.color = color;
            ___periapsisAltitude.color = color;
            ___periapsisTime.color = color;
            ___orbitPeriod.color = color;
        }
    }
}
