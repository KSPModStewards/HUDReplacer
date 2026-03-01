using HarmonyLib;
using KSP.UI.Screens;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ProtoStageIconInfo), nameof(ProtoStageIconInfo.SetMsgBgColor))]
internal static class ProtoStageIconInfo_SetMsgBgColor
{
    static bool Prefix(ref Color c)
    {
        if (HUDReplacerColor.StageEngineFuelGaugeBackgroundColor is Color fuelColor)
        {
            if (c == XKCDColors.DarkLime.A(0.6f))
            {
                c = fuelColor;
                return true;
            }
        }
        if (HUDReplacerColor.StageEngineHeatGaugeBackgroundColor is Color heatColor)
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
