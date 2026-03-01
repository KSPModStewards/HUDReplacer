using Unity.Profiling;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class ReloadTexturesButton : MonoBehaviour
{
    static readonly ProfilerMarker Marker = new("HUDReplacer.ReloadTextures");

    public Button button;

    internal static GameObject Create(Transform parent)
    {
        var go = UIBuilder.CreateButton(parent, "Reload Textures");
        var comp = go.AddComponent<ReloadTexturesButton>();
        comp.button = go.GetComponent<Button>();
        return go;
    }

    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    static void OnClick()
    {
        using var scope = Marker.Auto();
        HUDReplacer.Instance?.DebugReset();
        Debug.Log("HUDReplacer: Refreshed.");
    }
}
