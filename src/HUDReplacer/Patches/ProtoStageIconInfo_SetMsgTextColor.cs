using HarmonyLib;
using KSP.UI.Screens;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ProtoStageIconInfo), nameof(ProtoStageIconInfo.SetMsgTextColor))]
internal static class ProtoStageIconInfo_SetMsgTextColor
{
    static bool Prefix(ref Color c)
    {
        if (HUDReplacerColor.StageEngineFuelGaugeTextColor is Color fuelColor)
        {
            if (c == XKCDColors.ElectricLime.A(0.6f))
            {
                c = fuelColor;
                return true;
            }
        }
        if (HUDReplacerColor.StageEngineHeatGaugeTextColor is Color heatColor)
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
