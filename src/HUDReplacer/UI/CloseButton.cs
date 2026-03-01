using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class CloseButton : MonoBehaviour
{
    public Button button;
    public GameObject target;

    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        HUDReplacerDebug.Instance?.OnWindowClosed();
        Destroy(target);
    }
}
