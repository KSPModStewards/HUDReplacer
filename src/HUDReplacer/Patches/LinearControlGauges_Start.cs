using System;
using System.Collections.Generic;
using HarmonyLib;
using KSP.UI.Screens.Flight;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(LinearControlGauges), nameof(LinearControlGauges.Start))]
internal static class LinearControlGauges_Start
{
    static void Postfix(LinearControlGauges __instance, List<Image> ___inputGaugeImages)
    {
        if (!__instance)
            return;
        if (HUDReplacerColor.GaugeNeedleYawPitchRollColor is not Color color)
            return;
        try
        {
            foreach (Image img in ___inputGaugeImages)
            {
                img.color = color;
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
