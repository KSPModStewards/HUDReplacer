using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class ToggleDebugButton : MonoBehaviour
{
    public Button button;
    public TextMeshProUGUI label;

    internal static GameObject Create(Transform parent)
    {
        var go = UIBuilder.CreateButton(parent, "Debug: OFF");
        var comp = go.AddComponent<ToggleDebugButton>();
        comp.button = go.GetComponent<Button>();
        comp.label = go.GetComponentInChildren<TextMeshProUGUI>();
        return go;
    }

    void Start()
    {
        UpdateLabel();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        HUDReplacerDebug.EnableDebug = !HUDReplacerDebug.EnableDebug;
        UpdateLabel();
        HUDReplacerDebug.Instance?.UpdateToolbarTexture();
    }

    void UpdateLabel()
    {
        label.text = HUDReplacerDebug.EnableDebug ? "Debug: ON" : "Debug: OFF";
    }
}
