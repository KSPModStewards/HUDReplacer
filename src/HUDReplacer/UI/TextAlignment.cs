using TMPro;
using UnityEngine;

namespace HUDReplacer.UI;

/// <summary>
/// Sets TMP text alignment in Start() to work around a TMP bug where
/// <see cref="TMP_Compatibility.ConvertTextAlignmentEnumValues"/> resets
/// non-legacy alignment values to TopLeft during Awake().
/// </summary>
internal class TextAlignment : MonoBehaviour
{
    public TextMeshProUGUI text;
    public TextAlignmentOptions alignment;

    void Start()
    {
        text.alignment = alignment;
    }
}
