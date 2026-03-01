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
        Image[] image_array = __result.gameObject.GetComponentsInChildren<Image>();
        List<Texture2D> textures = new List<Texture2D>();
        foreach (Image img in image_array)
        {
            textures.Add((Texture2D)img.mainTexture);
            if (
                img.mainTexture.name == "app_divider_pulldown_header_over"
                && HUDReplacerColor.KALTitleBar is Color color
            )
            {
                img.color = color;
            }
        }
        Texture2D[] tex_array = textures.ToArray();
        if (tex_array.Length > 0)
            HUDReplacer.Instance.ReplaceTextures(tex_array);
    }
}
