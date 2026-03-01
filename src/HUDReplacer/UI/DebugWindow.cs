using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HUDReplacer.UI;

internal class DebugWindow : MonoBehaviour
{
    private const string WindowTitle = "HUDReplacer";

    private static GameObject _prefab;
    private static DebugWindow _instance;

    internal static DebugWindow Instance => _instance;

    internal static void Toggle()
    {
        if (_instance == null)
        {
            if (_prefab == null)
                _prefab = BuildPrefab();

            var go = Object.Instantiate(_prefab, MainCanvasUtil.MainCanvas.transform);
            _instance = go.GetComponent<DebugWindow>();
            go.SetActive(true);
        }
        else
        {
            _instance.gameObject.SetActive(!_instance.gameObject.activeSelf);
        }
    }

    private static GameObject BuildPrefab()
    {
        // Root window
        var windowGo = UIBuilder.CreateWindow(
            null,
            "HUDReplacerDebugWindow",
            new Vector2(100, -100),
            new Vector2(400, 300)
        );

        // Let the window grow vertically to fit content
        var windowFitter = windowGo.AddOrGetComponent<ContentSizeFitter>();
        windowFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        // Title bar with drag support
        var titleBarGo = UIBuilder.CreateTitleBar(windowGo.transform, WindowTitle);

        // Close button
        var closeGo = UIBuilder.CreateButton(titleBarGo.transform, "X", fontSize: 12);
        var closeLE = closeGo.AddOrGetComponent<LayoutElement>();
        closeLE.preferredWidth = 24;
        closeLE.preferredHeight = 24;
        closeLE.flexibleWidth = 0;

        // Content area
        var contentGo = UIBuilder.CreateVerticalLayout(windowGo.transform, "Content");
        var contentLE = contentGo.AddComponent<LayoutElement>();
        contentLE.flexibleHeight = 1;
        contentLE.flexibleWidth = 1;

        // Toggle Debug button
        var toggleRow = CreateButtonRow(contentGo.transform);
        var toggleGo = ToggleDebugButton.Create(toggleRow.transform);
        toggleGo.AddOrGetComponent<LayoutElement>().flexibleWidth = 1;
        UIBuilder.CreateHelpButton(
            toggleRow.transform,
            "Enable debug keyboard shortcuts:\n"
                + "D - Dump textures under cursor\n"
                + "E - Dump all loaded textures\n"
                + "Q - Reload textures"
        );

        // Reload Textures button
        var reloadRow = CreateButtonRow(contentGo.transform);
        var reloadGo = ReloadTexturesButton.Create(reloadRow.transform);
        reloadGo.AddOrGetComponent<LayoutElement>().flexibleWidth = 1;
        UIBuilder.CreateHelpButton(
            reloadRow.transform,
            "Reload all replacement textures and re-apply them."
        );

        // Dump Textures button
        var dumpRow = CreateButtonRow(contentGo.transform);
        var dumpTexturesGo = UIBuilder.CreateButton(dumpRow.transform, "Dump Loaded Textures");
        dumpTexturesGo.AddOrGetComponent<LayoutElement>().flexibleWidth = 1;
        var dumpTexturesBtn = dumpTexturesGo.AddComponent<DumpTexturesButton>();
        dumpTexturesBtn.button = dumpTexturesGo.GetComponent<Button>();
        UIBuilder.CreateHelpButton(
            dumpRow.transform,
            "Dump the names and sizes of all loaded textures to KSP.log."
        );

        // Dump Skin Textures button
        var skinRow = CreateButtonRow(contentGo.transform);
        var skinTexturesGo = UIBuilder.CreateButton(skinRow.transform, "Dump IMGUI Skin Textures");
        skinTexturesGo.AddOrGetComponent<LayoutElement>().flexibleWidth = 1;
        var skinTexturesBtn = skinTexturesGo.AddComponent<DumpSkinTexturesButton>();
        skinTexturesBtn.button = skinTexturesGo.GetComponent<Button>();
        UIBuilder.CreateHelpButton(
            skinRow.transform,
            "Dump names and sizes of all textures used by the KSP IMGUI skin.\nOverride these to style <i>all</i> IMGUI UIs created by mods or KSP."
        );

        // Textures under cursor panel
        TexturesUnderCursor.Create(contentGo.transform);

        // Attach the DebugWindow component
        windowGo.AddComponent<DebugWindow>();

        // Wire close button to destroy the window
        var closeButton = closeGo.AddComponent<CloseButton>();
        closeButton.button = closeGo.GetComponent<Button>();
        closeButton.target = windowGo;

        return windowGo;
    }

    private static GameObject CreateButtonRow(Transform parent)
    {
        var row = UIBuilder.CreateHorizontalLayout(
            parent,
            "ButtonRow",
            padding: new RectOffset(0, 0, 0, 0),
            spacing: 4
        );
        var layout = row.GetComponent<HorizontalLayoutGroup>();
        layout.childForceExpandHeight = false;
        layout.childAlignment = TextAnchor.MiddleLeft;
        return row;
    }

    internal static void DumpWindow()
    {
        if (_instance == null)
        {
            Debug.Log("HUDReplacer: No active debug window to dump.");
            return;
        }

        var sb = new StringBuilder();
        DumpGameObject(sb, _instance.gameObject, 0);

        var path = Path.Combine(KSPUtil.ApplicationRootPath, "Logs/HUDReplacer");
        Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "DebugWindow.txt"), sb.ToString());
        Debug.Log("HUDReplacer: Debug window dump written to Logs/HUDReplacer/DebugWindow.txt");
    }

    internal static void DumpPopupDialogBase()
    {
        var prefab = PopupDialogController.Instance?.popupDialogBase;
        if (prefab == null)
        {
            Debug.Log("HUDReplacer: PopupDialogController not available.");
            return;
        }

        var sb = new StringBuilder();
        DumpGameObject(sb, prefab.gameObject, 0);

        var path = Path.Combine(KSPUtil.ApplicationRootPath, "Logs/HUDReplacer");
        Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "PopupDialogBase.txt"), sb.ToString());
        Debug.Log(
            "HUDReplacer: PopupDialogBase dump written to Logs/HUDReplacer/PopupDialogBase.txt"
        );
    }

    internal static void DumpPrefabs()
    {
        var sb = new StringBuilder();

        foreach (var prefab in UISkinManager.fetch.prefabs)
        {
            if (prefab == null)
                continue;
            DumpGameObject(sb, prefab, 0);
            sb.AppendLine();
        }

        var path = Path.Combine(KSPUtil.ApplicationRootPath, "Logs/HUDReplacer");
        Directory.CreateDirectory(path);
        File.WriteAllText(Path.Combine(path, "Prefabs.txt"), sb.ToString());
        Debug.Log($"HUDReplacer: Prefab dump written to Logs/HUDReplacer/Prefabs.txt");
    }

    private static void DumpGameObject(StringBuilder sb, GameObject go, int depth)
    {
        var indent = new string(' ', depth * 2);
        var rect = go.GetComponent<RectTransform>();
        var rectInfo = "";
        if (rect != null)
        {
            var sd = rect.sizeDelta;
            var amin = rect.anchorMin;
            var amax = rect.anchorMax;
            var piv = rect.pivot;
            var ap = rect.anchoredPosition;
            rectInfo =
                $" [size={sd.x}x{sd.y} anchor=({amin.x},{amin.y})-({amax.x},{amax.y}) pivot=({piv.x},{piv.y}) pos=({ap.x},{ap.y})]";
        }

        sb.AppendLine($"{indent}{go.name} (active={go.activeSelf}){rectInfo}");

        foreach (var comp in go.GetComponents<Component>())
        {
            if (comp == null)
                continue;
            if (comp is Transform or RectTransform)
                continue;
            var compInfo = "";
            if (comp is TextMeshProUGUI tmp)
                compInfo =
                    $" alignment={tmp.alignment} overflowMode={tmp.overflowMode} enableWordWrapping={tmp.enableWordWrapping}";
            sb.AppendLine($"{indent}  + {comp.GetType().Name}{compInfo}");
        }

        for (int i = 0; i < go.transform.childCount; i++)
            DumpGameObject(sb, go.transform.GetChild(i).gameObject, depth + 1);
    }
}
