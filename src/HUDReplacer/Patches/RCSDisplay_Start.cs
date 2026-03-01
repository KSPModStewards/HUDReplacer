using HarmonyLib;
using KSP.UI;
using KSP.UI.Screens.Flight;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(RCSDisplay), "Start")]
internal static class RCSDisplay_Start
{
    static void Postfix(RCSDisplay __instance)
    {
        UIStateText.TextState[] states = __instance.stateText.states;
        if (states == null)
        {
            Debug.LogError("HUDReplacer: no states found for RCSDisplay.stateText.states");
            return;
        }
        int num = states.Length;
        while (num-- > 0)
        {
            if (states[num].name == "On")
            {
                if (HUDReplacerColor.RCSDisplayOnColor is Color onColor)
                {
                    states[num].textColor = onColor;
                }
            }
            if (states[num].name == "Off")
            {
                if (HUDReplacerColor.RCSDisplayOffColor is Color offColor)
                {
                    states[num].textColor = offColor;
                }
            }
        }
    }
}
