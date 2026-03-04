using HarmonyLib;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(KSP.UI.Screens.Tumbler), "Awake")]
internal static class Tumbler_Awake
{
    static void Prefix(KSP.UI.Screens.Tumbler __instance)
    {
        var tumbler = __instance;

        if (HUDReplacerColor.TumblerColorPositive is Color positive)
            tumbler.positiveColor = positive;
        if (HUDReplacerColor.TumblerColorNegative is Color negative)
            tumbler.negativeColor = negative;

        // Fix a stock bug
        // ===============
        // Tumbler.meshRenderers is supposed to be initialized with the mesh
        // renderers. This is set up correctly for the altitude tumblers but
        // not for the stage tumblers.
        //
        // In stock this has no visible effect since the letters are already
        // black anyway and it is not possible for the stage tumbler to go
        // negative.
        //
        // However, when we change the colors we actually need this to work.
        // As a fix, we set up the renderers manually for the stage tumblers.
        if (tumbler.tumblerRenderers?.Length == 0 && tumbler.name == "StageTumblers")
        {
            var renderers = new MeshRenderer[tumbler.tumblerTransforms.Length];
            for (int i = 0; i < tumbler.tumblerTransforms.Length; ++i)
            {
                var transform = tumbler.tumblerTransforms[i];
                renderers[i] = transform.GetComponent<MeshRenderer>();
            }

            tumbler.tumblerRenderers = renderers;
        }
    }
}
