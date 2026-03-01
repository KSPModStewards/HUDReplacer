using HarmonyLib;
using KSP.UI.Screens.Flight;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(VerticalSpeedGauge), nameof(VerticalSpeedGauge.Start))]
internal static class VerticalSpeedGauge_Start
{
    static void Postfix(VerticalSpeedGauge __instance)
    {
        if (HUDReplacerColor.VerticalSpeedGaugeNeedleColor is Color color)
        {
            __instance.gauge.pointer.gameObject.GetComponentInChildren<Image>().color = color;
        }
    }
}
