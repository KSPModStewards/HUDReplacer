using HarmonyLib;
using TMPro;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIPartActionResourceEditor), nameof(UIPartActionResourceEditor.Setup))]
internal static class UIPartActionResourceEditor_Setup
{
    static void Postfix(UIPartActionResourceEditor __instance)
    {
        if (HUDReplacerColor.PAWFuelSliderColor is Color sliderColor)
        {
            __instance.slider.image.color = sliderColor;
        }
        if (HUDReplacerColor.PAWFuelSliderTextColor is Color textColor)
        {
            __instance.resourceName.color = textColor;
            __instance.resourceAmnt.color = textColor;
            __instance.resourceMax.color = textColor;
            foreach (Transform child in __instance.sliderContainer.transform)
            {
                if (child.name == "Slash")
                {
                    child.GetComponent<TextMeshProUGUI>().color = textColor;
                }
            }
        }
    }
}
