using HarmonyLib;
using KSP.UI.Screens;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(PartCategorizer), "Setup")]
internal static class PartCategorizer_Setup
{
    static void Prefix(PartCategorizer __instance)
    {
        if (HUDReplacerColor.EditorCategoryButtonColor is Color color)
            __instance.colorFilterFunction = color;
        if (HUDReplacerColor.EditorCategoryModuleButtonColor is Color moduleColor)
            __instance.colorFilterModule = moduleColor;
        if (HUDReplacerColor.EditorCategoryResourceButtonColor is Color resourceColor)
            __instance.colorFilterResource = resourceColor;
        if (HUDReplacerColor.EditorCategoryManufacturerButtonColor is Color manufacturerColor)
            __instance.colorFilterManufacturer = manufacturerColor;
        if (HUDReplacerColor.EditorCategoryTechButtonColor is Color techColor)
            __instance.colorFilterTech = techColor;
        if (HUDReplacerColor.EditorCategoryProfileButtonColor is Color profileColor)
            __instance.colorFilterProfile = profileColor;
        if (HUDReplacerColor.EditorCategorySubassemblyButtonColor is Color subassemblyColor)
            __instance.colorSubassembly = subassemblyColor;
        if (HUDReplacerColor.EditorCategoryVariantsButtonColor is Color variantsColor)
            __instance.colorVariants = variantsColor;
        if (HUDReplacerColor.EditorCategoryCustomButtonColor is Color customColor)
            __instance.colorCategory = customColor;
    }
}
