using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIPartActionResourcePriority), nameof(UIPartActionResourcePriority.Awake))]
internal static class UIPartActionResourcePriority_Awake
{
    static void Postfix(Button ___btnInc, Button ___btnDec, Button ___btnReset)
    {
        if (HUDReplacerColor.PAWResourcePriorityIncrease is Color inc)
            ___btnInc.GetComponent<Image>().color = inc;
        if (HUDReplacerColor.PAWResourcePriorityDecrease is Color dec)
            ___btnDec.GetComponent<Image>().color = dec;
        if (HUDReplacerColor.PAWResourcePriorityReset is Color reset)
            ___btnReset.GetComponent<Image>().color = reset;
    }
}
