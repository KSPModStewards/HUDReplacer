using HarmonyLib;
using KSP.UI;
using KSP.UI.Screens.Flight;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(SASDisplay), "Start")]
internal static class SASDisplay_Start
{
    static void Postfix(SASDisplay __instance)
    {
        UIStateText.TextState[] states = __instance.stateText.states;
        if (states == null)
        {
            Debug.LogError("HUDReplacer: no states found for SASDisplay.stateText.states");
            return;
        }
        int num = states.Length;
        while (num-- > 0)
        {
            if (states[num].name == "On")
            {
                if (HUDReplacerColor.SASDisplayOnColor is Color onColor)
                {
                    states[num].textColor = onColor;
                }
            }
            if (states[num].name == "Off")
            {
                if (HUDReplacerColor.SASDisplayOffColor is Color offColor)
                {
                    states[num].textColor = offColor;
                }
            }
        }
    }
}
