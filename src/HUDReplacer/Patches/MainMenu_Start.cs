using HarmonyLib;

namespace HUDReplacer.Patches;

// [HarmonyPatch(typeof(MainMenu), "Start")]
// internal static class MainMenu_Start
// {
//     static void Postfix()
//     {
//         if (HUDReplacer.Instance != null)
//         {
//             HUDReplacer.Instance.RunMainMenuRefreshSequence();
//         }
//     }
// }
