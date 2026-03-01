using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace HUDReplacer;

[KSPAddon(KSPAddon.Startup.Instantly, true)]
internal class Loader : MonoBehaviour
{
    internal static Loader Instance { get; private set; }

    void Awake()
    {
        Instance = this;

        var harmony = new Harmony("UltraJohn.Mods.HUDReplacer");
        harmony.PatchAll(Assembly.GetExecutingAssembly());
    }

    void OnDestroy()
    {
        Instance = null;
    }
}
