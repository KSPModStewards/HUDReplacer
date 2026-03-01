using HarmonyLib;
using KSP.UI.Screens;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ProtoStageIconInfo), nameof(ProtoStageIconInfo.SetProgressBarColor))]
internal static class ProtoStageIconInfo_SetProgressBarColor
{
    static bool Prefix(ref Color c)
    {
        if (HUDReplacerColor.StageEngineFuelGaugeFillColor is Color fuelColor)
        {
            if (c == XKCDColors.Yellow.A(0.6f))
            {
                c = fuelColor;
                return true;
            }
        }
        if (HUDReplacerColor.StageEngineHeatGaugeFillColor is Color heatColor)
        {
            if (c == XKCDColors.OrangeYellow.A(0.6f))
            {
                c = heatColor;
                return true;
            }
        }
        return true;
    }
}
