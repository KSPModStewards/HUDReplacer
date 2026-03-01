using System.IO;
using HarmonyLib;
using KSP.UI.Screens.Flight;
using UnityEngine;

namespace HUDReplacer.Patches;

[HarmonyPatch(typeof(NavBall), "Start")]
internal static class NavBall_Start
{
    internal static string GaugeGeeFilePath = "";
    internal static string GaugeThrottleFilePath = "";

    static void Postfix(NavBall __instance)
    {
        if (HUDReplacerColor.NavBallHeadingColor is Color color)
        {
            __instance.headingText.color = color;
        }

        if (GaugeGeeFilePath != "")
        {
            Texture2D tex = (Texture2D)__instance.sideGaugeGee.mainTexture;
            ImageConversion.LoadImage(tex, File.ReadAllBytes(GaugeGeeFilePath));
        }
        if (GaugeThrottleFilePath != "")
        {
            Texture2D tex = (Texture2D)__instance.sideGaugeThrottle.mainTexture;
            ImageConversion.LoadImage(tex, File.ReadAllBytes(GaugeThrottleFilePath));
        }
    }
}
