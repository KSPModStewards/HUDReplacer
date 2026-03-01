using System;
using System.Collections.Generic;
using HarmonyLib;
using KSP.UI.Screens.Flight;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(LinearControlGauges), "onPrecisionModeToggle")]
internal static class LinearControlGauges_onPrecisionModeToggle
{
    static void Postfix(
        LinearControlGauges __instance,
        bool precisionMode,
        List<Image> ___inputGaugeImages
    )
    {
        if (!__instance)
            return;
        if (
            HUDReplacerColor.GaugeNeedleYawPitchRollColor is null
            && HUDReplacerColor.GaugeNeedleYawPitchRollPrecisionColor is null
        )
            return;
        try
        {
            foreach (Image img in ___inputGaugeImages)
            {
                if (!precisionMode)
                {
                    if (HUDReplacerColor.GaugeNeedleYawPitchRollColor is Color color)
                    {
                        img.color = color;
                    }
                }
                else
                {
                    if (HUDReplacerColor.GaugeNeedleYawPitchRollPrecisionColor is Color precisionColor)
                    {
                        img.color = precisionColor;
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }
}
