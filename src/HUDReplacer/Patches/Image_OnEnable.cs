using HarmonyLib;
using UnityEngine.UI;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(Image), nameof(Image.OnEnable))]
internal static class Image_OnEnable
{
    static void Postfix(Image __instance)
    {
        if (__instance == null)
            return;
        var sprite = __instance.sprite;
        if (sprite == null)
            return;
        var texture = sprite.texture;

        HUDReplacer.Instance?.ReplaceTexture(texture);
    }
}
