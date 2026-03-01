using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIPartActionVariantSelector), nameof(UIPartActionVariantSelector.Setup))]
internal static class UIPartActionVariantSelector_Setup
{
    static void Postfix(UIPartActionVariantSelector __instance)
    {
        if (HUDReplacerColor.PAWVariantSelectorNext is Color nextColor)
            __instance.buttonNext.GetComponent<Image>().color = nextColor;
        if (HUDReplacerColor.PAWVariantSelectorPrevious is Color prevColor)
            __instance.buttonNext.GetComponent<Image>().color = prevColor;
    }
}
