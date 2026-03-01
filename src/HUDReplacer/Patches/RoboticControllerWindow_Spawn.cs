using System.Collections.Generic;
using Expansions.Serenity;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(RoboticControllerWindow), nameof(RoboticControllerWindow.Spawn))]
internal static class RoboticControllerWindow_Spawn
{
    static void Postfix(RoboticControllerWindow __result)
    {
        if (!__result)
            return;
        var images = __result.gameObject.GetComponentsInChildren<Image>();
        List<Texture2D> textures = [];
        foreach (Image img in images)
        {
            var texture = img.mainTexture;

            if (
                img.mainTexture.name == "app_divider_pulldown_header_over"
                && HUDReplacerColor.KALTitleBar is Color color
            )
            {
                img.color = color;
            }

            if (texture is Texture2D tex2d)
                textures.Add(tex2d);
        }

        HUDReplacer.Instance.ReplaceTextures(textures);
    }
}
