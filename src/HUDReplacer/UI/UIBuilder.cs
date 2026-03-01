using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal static class UIBuilder
{
    static GameObject Prefab(string name) => UISkinManager.GetPrefab(name);

    internal static GameObject CreateWindow(
        Transform parent,
        string name,
        Vector2 position,
        Vector2 size
    )
    {
        var skin = UISkinManager.defaultSkin;

        var go = Object.Instantiate(Prefab("UIBoxPrefab"), parent);
        go.name = name;

        var rect = go.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0, 1);
        rect.anchorMax = new Vector2(0, 1);
        rect.pivot = new Vector2(0, 1);
        rect.anchoredPosition = position;
        rect.sizeDelta = size;

        var image = go.GetComponent<Image>();
        if (skin.window.normal?.background != null)
            image.sprite = skin.window.normal.background;
        image.type = Image.Type.Sliced;

        go.AddComponent<CanvasGroup>();
        go.AddComponent<DragPanel>();

        var inputLock = go.AddComponent<DialogMouseEnterControlLock>();
        inputLock.lockName = $"HUDReplacer_{name}";

        var layout = go.AddComponent<VerticalLayoutGroup>();
        layout.padding = new RectOffset(8, 8, 8, 8);
        layout.spacing = 4;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        layout.childControlWidth = true;
        layout.childControlHeight = true;

        return go;
    }

    internal static GameObject CreateTitleBar(Transform parent, string title)
    {
        var skin = UISkinManager.defaultSkin;

        var go = Object.Instantiate(Prefab("UIHorizontalLayoutPrefab"), parent);
        go.SetActive(true);
        go.name = "TitleBar";

        var layout = go.GetComponent<HorizontalLayoutGroup>();
        layout.padding = new RectOffset(4, 4, 2, 2);
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = true;
        layout.childControlHeight = true;

        var le = go.AddComponent<LayoutElement>();
        le.preferredHeight = 28;
        le.flexibleHeight = 0;
        le.flexibleWidth = 1;

        var titleGo = CreateText(go.transform, title);
        var titleText = titleGo.GetComponent<TextMeshProUGUI>();
        titleText.fontStyle = FontStyles.Bold;
        titleText.color = skin.window.normal.textColor;
        titleGo.GetComponent<LayoutElement>().flexibleWidth = 1;

        // TMP's Awake() calls LoadDefaultSettings() which runs
        // ConvertTextAlignmentEnumValues and resets non-legacy values to TopLeft.
        // Defer the alignment assignment to Start() to work around this.
        var align = titleGo.AddComponent<TextAlignment>();
        align.text = titleText;
        align.alignment = TextAlignmentOptions.Center;

        return go;
    }

    internal static GameObject CreateText(Transform parent, string text, float fontSize = 14)
    {
        var go = Object.Instantiate(Prefab("UITextPrefab"), parent);
        go.SetActive(true);
        go.name = text;

        var tmp = go.GetComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;

        return go;
    }

    internal static GameObject CreateButton(Transform parent, string text, float fontSize = 14)
    {
        var go = Object.Instantiate(Prefab("UIButtonPrefab"), parent);
        go.SetActive(true);
        go.name = text;

        var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;

        // The prefab's Text child has zero size by default; stretch it to fill the button
        var textRect = tmp.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        // Ensure the button has a reasonable minimum height in layouts
        var le = go.AddOrGetComponent<LayoutElement>();
        le.preferredHeight = fontSize + 16;

        return go;
    }

    internal static GameObject CreateVerticalLayout(
        Transform parent,
        string name = "VerticalLayout",
        RectOffset padding = null,
        float spacing = 4
    )
    {
        var go = Object.Instantiate(Prefab("UIVerticalLayoutPrefab"), parent);
        go.SetActive(true);
        go.name = name;

        var layout = go.GetComponent<VerticalLayoutGroup>();
        layout.padding = padding ?? new RectOffset(4, 4, 4, 4);
        layout.spacing = spacing;
        layout.childForceExpandWidth = true;
        layout.childForceExpandHeight = false;
        layout.childControlWidth = true;
        layout.childControlHeight = true;

        return go;
    }

    internal static GameObject CreateHorizontalLayout(
        Transform parent,
        string name = "HorizontalLayout",
        RectOffset padding = null,
        float spacing = 4
    )
    {
        var go = Object.Instantiate(Prefab("UIHorizontalLayoutPrefab"), parent);
        go.SetActive(true);
        go.name = name;

        var layout = go.GetComponent<HorizontalLayoutGroup>();
        layout.padding = padding ?? new RectOffset(4, 4, 4, 4);
        layout.spacing = spacing;
        layout.childForceExpandWidth = false;
        layout.childForceExpandHeight = true;
        layout.childControlWidth = true;
        layout.childControlHeight = true;

        return go;
    }

    internal static GameObject CreateScrollView(Transform parent, string name = "ScrollView")
    {
        var go = Object.Instantiate(Prefab("UIScrollViewPrefab"), parent);
        go.SetActive(true);
        go.name = name;
        return go;
    }

    internal static GameObject CreateToggle(Transform parent, string label)
    {
        var go = Object.Instantiate(Prefab("UITogglePrefab"), parent);
        go.SetActive(true);
        go.name = label;

        var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
        if (tmp != null)
            tmp.text = label;

        return go;
    }

    internal static GameObject CreateSlider(Transform parent, string name = "Slider")
    {
        var go = Object.Instantiate(Prefab("UISliderPrefab"), parent);
        go.SetActive(true);
        go.name = name;
        return go;
    }

    internal static GameObject CreateTextInput(
        Transform parent,
        string name = "TextInput",
        string placeholder = ""
    )
    {
        var go = Object.Instantiate(Prefab("UITextInputPrefab"), parent);
        go.SetActive(true);
        go.name = name;

        if (!string.IsNullOrEmpty(placeholder))
        {
            var input = go.GetComponent<TMP_InputField>();
            if (input.placeholder is TextMeshProUGUI tmp)
                tmp.text = placeholder;
        }

        return go;
    }

    internal static GameObject CreateBox(Transform parent, string name = "Box")
    {
        var go = Object.Instantiate(Prefab("UIBoxPrefab"), parent);
        go.SetActive(true);
        go.name = name;
        return go;
    }

    internal static GameObject CreateImage(Transform parent, string name = "Image")
    {
        var go = Object.Instantiate(Prefab("UIImagePrefab"), parent);
        go.SetActive(true);
        go.name = name;
        return go;
    }

    internal static GameObject CreateRawImage(Transform parent, string name = "RawImage")
    {
        var go = Object.Instantiate(Prefab("UIRawImagePrefab"), parent);
        go.SetActive(true);
        go.name = name;
        return go;
    }

    internal static GameObject CreateHelpButton(Transform parent, string tooltipText)
    {
        var buttonGo = CreateButton(parent, "?", fontSize: 12);
        var buttonLE = buttonGo.AddOrGetComponent<LayoutElement>();
        buttonLE.preferredWidth = 24;
        buttonLE.preferredHeight = 24;
        buttonLE.flexibleWidth = 0;

        // Use KSP's built-in tooltip system
        var tooltipPrefab = Prefab("UISliderPrefab")
            .GetComponent<KSP.UI.TooltipTypes.TooltipController_Text>()
            .prefab;

        var controller = buttonGo.AddComponent<KSP.UI.TooltipTypes.TooltipController_Text>();
        controller.prefab = tooltipPrefab;
        controller.textString = tooltipText;

        return buttonGo;
    }
}
