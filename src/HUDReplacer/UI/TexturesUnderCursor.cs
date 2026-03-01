using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class TexturesUnderCursor : MonoBehaviour
{
    public TextMeshProUGUI text;

    private List<RaycastResult> _results;
    private StringBuilder _sb;

    internal static GameObject Create(Transform parent)
    {
        // Label row with help button
        var labelRow = UIBuilder.CreateHorizontalLayout(
            parent,
            "LabelRow",
            padding: new RectOffset(0, 0, 0, 0),
            spacing: 4
        );
        var labelRowLayout = labelRow.GetComponent<HorizontalLayoutGroup>();
        labelRowLayout.childForceExpandHeight = false;
        labelRowLayout.childAlignment = TextAnchor.MiddleLeft;
        var labelLE = labelRow.AddOrGetComponent<LayoutElement>();
        labelLE.preferredHeight = 24;
        var labelText = UIBuilder.CreateText(labelRow.transform, "Textures Under Cursor");
        labelText.GetComponent<LayoutElement>().flexibleWidth = 1;
        UIBuilder.CreateHelpButton(
            labelRow.transform,
            "Shows textures currently under the cursor."
        );

        // Box panel that grows to fit content
        var boxGo = UIBuilder.CreateBox(parent);
        var boxLayout = boxGo.AddOrGetComponent<VerticalLayoutGroup>();
        boxLayout.padding = new RectOffset(4, 4, 4, 4);
        boxLayout.childAlignment = TextAnchor.UpperLeft;
        boxLayout.childForceExpandWidth = true;
        boxLayout.childForceExpandHeight = false;
        boxLayout.childControlWidth = true;
        boxLayout.childControlHeight = true;

        var boxLE = boxGo.AddOrGetComponent<LayoutElement>();
        boxLE.minHeight = 136; // ~8 lines at font size 12

        var boxFitter = boxGo.AddOrGetComponent<ContentSizeFitter>();
        boxFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Text element for the results
        var textGo = UIBuilder.CreateText(boxGo.transform, "", fontSize: 12);
        var tmp = textGo.GetComponent<TextMeshProUGUI>();
        tmp.alignment = TextAlignmentOptions.TopLeft;

        var comp = boxGo.AddComponent<TexturesUnderCursor>();
        comp.text = tmp;

        return boxGo;
    }

    void Awake()
    {
        _results = [];
        _sb = new StringBuilder();
    }

    void Update()
    {
        var eventData = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y),
        };

        _results.Clear();
        EventSystem.current.RaycastAll(eventData, _results);

        _sb.Clear();
        foreach (var result in _results)
        {
            var image = result.gameObject.GetComponent<Image>();
            if (image == null)
                continue;

            if (image.mainTexture is Texture2D tex)
                _sb.AppendFormat("{0} ({1}x{2})\n", tex.name, tex.width, tex.height);
        }

        text.text = _sb.ToString();
        _results.Clear();
    }
}
