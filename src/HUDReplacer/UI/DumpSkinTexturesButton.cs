using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class DumpSkinTexturesButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(DumpSkinTextures);
    }

    static void DumpSkinTextures()
    {
        var skin = UISkinManager.defaultSkin;
        if (skin == null)
        {
            Debug.Log("HUDReplacer: UISkinManager.defaultSkin is null.");
            return;
        }

        var entries = new List<(string source, Texture2D tex)>();

        CollectFromStyle(entries, "box", skin.box);
        CollectFromStyle(entries, "button", skin.button);
        CollectFromStyle(entries, "horizontalScrollbar", skin.horizontalScrollbar);
        CollectFromStyle(
            entries,
            "horizontalScrollbarLeftButton",
            skin.horizontalScrollbarLeftButton
        );
        CollectFromStyle(entries, "horizontalScrollbarThumb", skin.horizontalScrollbarThumb);
        CollectFromStyle(entries, "horizontalSlider", skin.horizontalSlider);
        CollectFromStyle(entries, "horizontalSliderThumb", skin.horizontalSliderThumb);
        CollectFromStyle(entries, "label", skin.label);
        CollectFromStyle(entries, "scrollView", skin.scrollView);
        CollectFromStyle(entries, "textArea", skin.textArea);
        CollectFromStyle(entries, "textField", skin.textField);
        CollectFromStyle(entries, "toggle", skin.toggle);
        CollectFromStyle(entries, "verticalScrollbar", skin.verticalScrollbar);
        CollectFromStyle(entries, "verticalScrollbarDownButton", skin.verticalScrollbarDownButton);
        CollectFromStyle(entries, "verticalScrollbarThumb", skin.verticalScrollbarThumb);
        CollectFromStyle(entries, "verticalScrollbarUpButton", skin.verticalScrollbarUpButton);
        CollectFromStyle(entries, "verticalSlider", skin.verticalSlider);
        CollectFromStyle(entries, "verticalSliderThumb", skin.verticalSliderThumb);
        CollectFromStyle(entries, "window", skin.window);

        if (skin.customStyles != null)
        {
            foreach (var style in skin.customStyles)
            {
                if (style == null)
                    continue;
                CollectFromStyle(entries, style.name ?? "unnamed", style);
            }
        }

        Debug.Log($"HUDReplacer: Dumping {entries.Count} skin texture entries...");
        foreach (var (source, tex) in entries.OrderBy(e => e.source))
            Debug.Log($"  {source}: {tex.name} ({tex.width}x{tex.height})");
        Debug.Log("HUDReplacer: Skin texture dump finished.");
    }

    static void CollectFromStyle(
        List<(string source, Texture2D tex)> entries,
        string styleName,
        UIStyle style
    )
    {
        if (style == null)
            return;

        CollectFromState(entries, $"{styleName}.normal", style.normal);
        CollectFromState(entries, $"{styleName}.highlight", style.highlight);
        CollectFromState(entries, $"{styleName}.active", style.active);
        CollectFromState(entries, $"{styleName}.disabled", style.disabled);
    }

    static void CollectFromState(
        List<(string source, Texture2D tex)> entries,
        string source,
        UIStyleState state
    )
    {
        if (state?.background == null)
            return;

        var tex = state.background.texture;
        if (tex == null)
            return;

        entries.Add((source, tex));
    }
}
