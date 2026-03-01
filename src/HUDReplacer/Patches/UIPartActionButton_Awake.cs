using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIPartActionButton), nameof(UIPartActionButton.Awake))]
internal static class UIPartActionButton_Awake
{
    static void Postfix(UIPartActionButton __instance)
    {
        if (HUDReplacerColor.PAWBlueButton is not Color color)
            return;

        __instance.button.GetComponent<Image>()?.color = color;
    }
}
