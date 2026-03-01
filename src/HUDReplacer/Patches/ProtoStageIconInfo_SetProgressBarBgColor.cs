using HarmonyLib;
using KSP.UI.Screens;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ProtoStageIconInfo), nameof(ProtoStageIconInfo.SetProgressBarBgColor))]
internal static class ProtoStageIconInfo_SetProgressBarBgColor
{
    static bool Prefix(ref Color c)
    {
        if (HUDReplacerColor.StageEngineFuelGaugeFillBackgroundColor is Color fuelColor)
        {
            if (c == XKCDColors.DarkLime.A(0.6f))
            {
                c = fuelColor;
                return true;
            }
        }
        if (HUDReplacerColor.StageEngineHeatGaugeFillBackgroundColor is Color heatColor)
        {
            if (c == XKCDColors.DarkRed.A(0.6f))
            {
                c = heatColor;
                return true;
            }
        }
        return true;
    }
}
