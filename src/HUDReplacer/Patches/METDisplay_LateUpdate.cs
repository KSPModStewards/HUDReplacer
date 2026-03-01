using System.Collections.Generic;
using HarmonyLib;
using KSP.UI.Screens.Flight;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(METDisplay), "LateUpdate")]
internal static class METDisplay_LateUpdate
{
    public static Color METDisplayRedColorOverride()
    {
        return HUDReplacerColor.METDisplayColorRed ?? Color.red;
    }

    public static Color METDisplayYellowColorOverride()
    {
        return HUDReplacerColor.METDisplayColorYellow ?? Color.yellow;
    }

    public static Color METDisplayGreenColorOverride()
    {
        return HUDReplacerColor.METDisplayColorGreen ?? Color.green;
    }

    static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var getColorRed = AccessTools.Method(typeof(Color), "get_red");
        var getColorYellow = AccessTools.Method(typeof(Color), "get_yellow");
        var getColorGreen = AccessTools.Method(typeof(Color), "get_green");
        foreach (var instruction in instructions)
        {
            if (instruction.Calls(getColorRed))
            {
                instruction.operand = AccessTools.Method(
                    typeof(METDisplay_LateUpdate),
                    nameof(METDisplayRedColorOverride)
                );
            }
            else if (instruction.Calls(getColorYellow))
            {
                instruction.operand = AccessTools.Method(
                    typeof(METDisplay_LateUpdate),
                    nameof(METDisplayYellowColorOverride)
                );
            }
            else if (instruction.Calls(getColorGreen))
            {
                instruction.operand = AccessTools.Method(
                    typeof(METDisplay_LateUpdate),
                    nameof(METDisplayGreenColorOverride)
                );
            }
            yield return instruction;
        }
    }
}
