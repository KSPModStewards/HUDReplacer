using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class DumpTexturesButton : MonoBehaviour
{
    static readonly ProfilerMarker Marker = new("HUDReplacer.DumpTextures");

    public Button button;

    void Start()
    {
        button.onClick.AddListener(DumpAllTextures);
    }

    internal static void DumpAllTextures()
    {
        using var scope = Marker.Auto();
        Debug.Log("HUDReplacer: Dumping list of loaded texture2D objects...");
        var textures = Resources.FindObjectsOfTypeAll<Texture2D>();
        foreach (var tex in textures)
            Debug.Log($"{tex.name} - WxH={tex.width}x{tex.height}");
        Debug.Log("HUDReplacer: Dumping finished.");
    }
}
