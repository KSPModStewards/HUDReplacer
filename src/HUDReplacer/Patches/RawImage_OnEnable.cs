using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(MaskableGraphic), nameof(MaskableGraphic.OnEnable))]
internal static class RawImage_OnEnable
{
    static void Postfix(MaskableGraphic __instance)
    {
        if (__instance is not RawImage image)
            return;

        if (image.texture != null)
        {
            HUDReplacer.Instance?.ReplaceTexture(image.texture as Texture2D);
        }
        else
        {
            HUDReplacer.Instance?.ReplaceTexture(image.mainTexture as Texture2D);
        }
    }
}
