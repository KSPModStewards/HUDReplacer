using HarmonyLib;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(StageGroup), "Awake")]
internal static class StageGroup_Awake
{
    static void Postfix(
        StageGroup __instance,
        TextMeshProUGUI ___DeltaVHeadingText,
        TextMeshProUGUI ___uiStageIndex
    )
    {
        if (!__instance)
            return;

        if (HUDReplacerColor.StageGroupDeltaVTextColor is Color textColor)
        {
            ___DeltaVHeadingText.color = textColor;
        }
        if (HUDReplacerColor.StageGroupDeltaVNumberColor is Color numberColor)
        {
            ___uiStageIndex.color = numberColor;
        }
        if (HUDReplacerColor.StageGroupDeltaVBackgroundColor is Color bgColor)
        {
            Image[] images = __instance.GetComponentsInChildren<Image>();
            foreach (Image img in images)
            {
                if (img.mainTexture.name == "StageDV")
                {
                    img.color = bgColor;
                }
            }
        }
    }
}
