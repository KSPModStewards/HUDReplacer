using System;
using Assets._UI5.Rendering.Scripts;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(UIPartActionController), nameof(UIPartActionController.CreatePartUI))]
internal static class UIPartActionController_CreatePartUI
{
    static void Postfix(UIPartActionWindow __result)
    {
        if (HUDReplacerColor.PAWTitleBar is not Color color)
            return;

        try
        {
            var images = __result.gameObject.GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                if (image.mainTexture.name == "app_divider_pulldown_header_over")
                    image.color = color;
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
