using System;
using HarmonyLib;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(HighLogic), nameof(HighLogic.Skin), MethodType.Getter)]
internal static class HighLogic_Skin
{
    internal static bool ReplacedSkinTextures = false;

    static void Postfix(GUISkin __result)
    {
        if (ReplacedSkinTextures)
            return;

        ReplaceUITextures(__result);
    }

    static void ReplaceUITextures(GUISkin skin)
    {
        if (HUDReplacer.Instance == null)
            return;

        try
        {
            HUDReplacer.Instance.ReplaceUITextures(skin);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

        ReplacedSkinTextures = true;
    }
}
