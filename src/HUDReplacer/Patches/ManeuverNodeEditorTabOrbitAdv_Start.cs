using HarmonyLib;
using TMPro;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ManeuverNodeEditorTabOrbitAdv), "Start")]
internal static class ManeuverNodeEditorTabOrbitAdv_Start
{
    static void Postfix(
        TextMeshProUGUI ___orbitArgumentOfPeriapsis,
        TextMeshProUGUI ___orbitLongitudeOfAscendingNode,
        TextMeshProUGUI ___ejectionAngle,
        TextMeshProUGUI ___orbitEccentricity,
        TextMeshProUGUI ___orbitInclination
    )
    {
        if (HUDReplacerColor.ManeuverNodeEditorTextColor is Color color)
        {
            ___orbitArgumentOfPeriapsis.color = color;
            ___orbitLongitudeOfAscendingNode.color = color;
            ___ejectionAngle.color = color;
            ___orbitEccentricity.color = color;
            ___orbitInclination.color = color;
        }
    }
}
