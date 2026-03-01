using HarmonyLib;
using TMPro;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(ManeuverNodeEditorTabVectorInput), "Start")]
internal static class ManeuverNodeEditorTabVectorInput_Start
{
    static void Postfix(
        TMP_InputField ___proRetrogradeField,
        TMP_InputField ___normalField,
        TMP_InputField ___radialField,
        TMP_InputField ___timeField
    )
    {
        if (HUDReplacerColor.ManeuverNodeEditorTextColor is Color color)
        {
            ___proRetrogradeField.textComponent.color = color;
            ___normalField.textComponent.color = color;
            ___radialField.textComponent.color = color;
            ___timeField.textComponent.color = color;
        }
    }
}
