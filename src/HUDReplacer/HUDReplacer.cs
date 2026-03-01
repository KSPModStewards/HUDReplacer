using System;
using System.Collections.Generic;
using System.IO;
using Cursors;
using HUDReplacer.Patches;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.SceneManagement;
using ReplacementInfo = HUDReplacer.Database.ReplacementInfo;
using SizedReplacementInfo = HUDReplacer.Database.SizedReplacementInfo;

namespace HUDReplacer;

// PSystemSpawn happens just before main menu startup, so we know that we'll be
// ready to go by the time the main menu scene is loaded.
[KSPAddon(KSPAddon.Startup.PSystemSpawn, true)]
public class HUDReplacer : MonoBehaviour
{
    internal static HUDReplacer Instance { get; private set; }

    static readonly ProfilerMarker ReplaceTexturesMarker = new("HUDReplacer.ReplaceTextures");
    static readonly ProfilerMarker ReplaceUITexturesMarker = new("HUDReplacer.ReplaceUITextures");

    static readonly Dictionary<string, ReplacementInfo> Empty = [];
    static readonly string[] CursorNames = ["basicNeutral", "basicElectricLime", "basicDisabled"];

    // These track what has already been replaced so we don't modify it again
    static readonly HashSet<int> ReplacedIds = [];

    // This is a temporary shared buffer used by everything that needs to create
    // a list of textures.
    static readonly List<Texture2D> ReplaceQueue = [];

    // Storage for configured custom cursors.
    static readonly CustomCursor[] Cursors = new CustomCursor[3];

    static bool IsCursorUpdatePending = false;
    static string ApplicationRootPath;

    void Awake()
    {
        ApplicationRootPath = KSPUtil.ApplicationRootPath;

        if (Instance != null)
        {
            enabled = false;
            DestroyImmediate(this);
        }

        Instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        if (!Database.Loaded)
            Database.LoadConfigs();
    }

    void OnEnable()
    {
        GameEvents.onLevelWasLoadedGUIReady.Add(OnLevelGUIReady);
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    void OnDisable()
    {
        GameEvents.onLevelWasLoadedGUIReady.Remove(OnLevelGUIReady);
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    void OnLevelGUIReady(GameScenes scene)
    {
        Reset();
        ReplaceTextures(scene);
    }

    // Clear out replace state when a scene gets unloaded so that textures
    // associated with that scene get unloaded in a timely manner.
    void OnSceneUnloaded(Scene scene)
    {
        Reset();
    }

    public static void ModuleManagerPostLoad()
    {
        SettingsManager.LoadConfig();
        HUDReplacerColor.LoadConfigs();
        Database.LoadConfigs();
    }

    internal void Reset()
    {
        ReplacedIds.Clear();
        ReplaceQueue.Clear();
        HighLogic_Skin.ReplacedSkinTextures = false;
    }

    internal void DebugReset()
    {
        Reset();
        Database.LoadConfigs();
        ReplaceTextures(HighLogic.LoadedScene);
    }

    #region ReplaceTextures
    internal void ReplaceTextures(GameScenes scene)
    {
        if (!Database.HasAnyReplacements)
            return;

        using var scope = ReplaceUITexturesMarker.Auto();

        var textures = (Texture2D[])(object)Resources.FindObjectsOfTypeAll(typeof(Texture2D));
        try
        {
            ReplaceQueue.AddRange(textures);
            ReplaceTextures(ReplaceQueue, scene);
        }
        finally
        {
            ReplaceQueue.Clear();
        }
    }

    internal void ReplaceTexture(Texture2D texture) =>
        ReplaceTexture(texture, HighLogic.LoadedScene);

    void ReplaceTexture(Texture2D texture, GameScenes scene)
    {
        if (!Database.HasAnyReplacements)
            return;
        if (texture == null)
            return;
        if (ReplacedIds.Contains(texture.GetInstanceID()))
            return;

        try
        {
            ReplaceQueue.Add(texture);
            ReplaceTextures(ReplaceQueue, scene);
        }
        finally
        {
            ReplaceQueue.Clear();
        }
    }

    internal void ReplaceTextures(params Span<Texture2D> textures)
    {
        if (!Database.HasAnyReplacements)
            return;

        try
        {
            for (int i = 0; i < textures.Length; ++i)
            {
                if (textures[i] != null)
                    ReplaceQueue.Add(textures[i]);
            }

            ReplaceTextures(ReplaceQueue, HighLogic.LoadedScene);
        }
        finally
        {
            ReplaceQueue.Clear();
        }
    }

    void ReplaceTextures(List<Texture2D> textures, GameScenes scene)
    {
        if (!Database.HasAnyReplacements)
            return;

        if (!Database.SceneImages.TryGetValue(scene, out var sceneImages))
            sceneImages = Empty;

        foreach (Texture2D tex in textures)
        {
            if (tex == null)
                continue;

            var id = tex.GetInstanceID();
            if (!ReplacedIds.Add(id))
                continue;

            string name = tex.name;
            if (string.IsNullOrEmpty(name))
                continue;

            int slashIndex = name.LastIndexOf('/');
            if (slashIndex != -1)
                name = name.Substring(slashIndex + 1);

            if (!Database.Images.TryGetValue(name, out var info))
                info = null;
            if (!sceneImages.TryGetValue(name, out var sceneInfo))
                sceneInfo = null;

            var replacement = GetMatchingReplacement(info, sceneInfo, tex);
            if (replacement is null)
                continue;

            ReplaceTexture(tex, replacement);
        }
    }

    void ReplaceTexture(Texture2D tex, SizedReplacementInfo replacement)
    {
        string name = replacement.basename;
        string basePath = ApplicationRootPath;

        if (SettingsManager.ShowDebugToolbar)
        {
            var path = replacement.path;
            if (path.StartsWith(basePath))
                path = path.Substring(basePath.Length);

            Debug.Log($"HUDReplacer: Replacing texture {name} with {path}");
        }

        // Special handling for the mouse cursor
        int cidx = CursorNames.IndexOf(name);
        if (cidx != -1)
        {
            Cursors[cidx] = CreateCursor(replacement.path);

            // Need to wait a small amount of time after scene load before you can set the cursor.
            if (!IsCursorUpdatePending)
            {
                IsCursorUpdatePending = true;
                this.Invoke(SetCursor, 1f);
            }

            return;
        }

        // NavBall GaugeGee and GaugeThrottle needs special handling as well
        if (name == "GaugeGee")
        {
            NavBall_Start.GaugeGeeFilePath = replacement.path;
        }
        else if (name == "GaugeThrottle")
        {
            NavBall_Start.GaugeThrottleFilePath = replacement.path;
        }
        else
        {
            replacement.cachedTextureBytes ??= File.ReadAllBytes(replacement.path);
            tex.LoadImage(replacement.cachedTextureBytes, tex.isReadable);
        }
    }

    internal void ReplaceUITextures(GUISkin skin)
    {
        if (!Database.HasAnyReplacements)
            return;

        using var scope = ReplaceTexturesMarker.Auto();

        try
        {
            FindUITextures(skin, ReplaceQueue);
            ReplaceTextures(ReplaceQueue, HighLogic.LoadedScene);
        }
        finally
        {
            ReplaceQueue.Clear();
        }
    }
    #endregion

    #region FindUITextures
    void FindUITextures(GUISkin skin, List<Texture2D> textures)
    {
        if (skin == null)
            return;

        FindUITextures(skin.box, textures);
        FindUITextures(skin.button, textures);
        FindUITextures(skin.horizontalScrollbar, textures);
        FindUITextures(skin.horizontalScrollbarLeftButton, textures);
        FindUITextures(skin.horizontalScrollbarThumb, textures);
        FindUITextures(skin.horizontalSlider, textures);
        FindUITextures(skin.horizontalSliderThumb, textures);
        FindUITextures(skin.label, textures);
        FindUITextures(skin.scrollView, textures);
        FindUITextures(skin.textArea, textures);
        FindUITextures(skin.textField, textures);
        FindUITextures(skin.toggle, textures);
        FindUITextures(skin.verticalScrollbar, textures);
        FindUITextures(skin.verticalScrollbarDownButton, textures);
        FindUITextures(skin.verticalScrollbarThumb, textures);
        FindUITextures(skin.verticalScrollbarUpButton, textures);
        FindUITextures(skin.verticalSlider, textures);
        FindUITextures(skin.verticalSliderThumb, textures);
        FindUITextures(skin.window, textures);

        if (skin.customStyles != null)
        {
            foreach (var style in skin.customStyles)
                FindUITextures(style, textures);
        }
    }

    void FindUITextures(GUIStyle style, List<Texture2D> textures)
    {
        if (style is null)
            return;

        FindUITextures(style.normal, textures);
        FindUITextures(style.hover, textures);
        FindUITextures(style.active, textures);
        FindUITextures(style.focused, textures);
        FindUITextures(style.onNormal, textures);
        FindUITextures(style.onHover, textures);
        FindUITextures(style.onActive, textures);
        FindUITextures(style.onFocused, textures);
    }

    void FindUITextures(GUIStyleState state, List<Texture2D> textures)
    {
        if (state is null)
            return;
        if (state.background == null)
            return;

        textures.Add(state.background);
    }
    #endregion

    // public void RunMainMenuRefreshSequence()
    // {
    //     float[] delays = { 0.5f, 1.2f, 2.0f };
    //     foreach (float delay in delays)
    //     {
    //         this.Invoke(
    //             () =>
    //             {
    //                 Debug.Log($"HUDReplacer: Performing Main Menu refresh ({delay}s).");
    //                 replacedTextureIds.Clear();
    //                 idReplacementMap.Clear();
    //                 RefreshAll();
    //             },
    //             delay
    //         );
    //     }
    // }

    // private void ForceGlobalSkin()
    // {
    //     // Legacy IMGUI Support
    //     if (HighLogic.Skin != null)
    //     {
    //         ApplySkin(HighLogic.Skin);
    //     }

    //     // Modern uGUI Support
    //     if (HighLogic.UISkin != null)
    //     {
    //         ApplyUISkinDef(HighLogic.UISkin);
    //     }

    //     if (UISkinManager.defaultSkin != null)
    //     {
    //         ApplyUISkinDef(UISkinManager.defaultSkin);
    //     }
    // }

    private static SizedReplacementInfo GetMatchingReplacement(
        ReplacementInfo info,
        ReplacementInfo sceneInfo,
        Texture2D tex
    )
    {
        if (info is null && sceneInfo is null)
            return null;

        var rep = info?.GetMatchingReplacement(tex);
        var sceneRep = sceneInfo?.GetMatchingReplacement(tex);

        if (rep != null && sceneRep != null)
        {
            if (rep.priority < sceneRep.priority)
                return sceneRep;
        }

        return rep ?? sceneRep;
    }

    private void SetCursor()
    {
        IsCursorUpdatePending = false;
        if (Cursors[0] == null)
            return;

        Cursors[1] ??= Cursors[0];
        Cursors[2] ??= Cursors[1];

        CursorController.Instance.AddCursor(
            "HUDReplacerCursor",
            Cursors[0],
            Cursors[1],
            Cursors[2]
        );
        CursorController.Instance.ChangeCursor("HUDReplacerCursor");

        if (SettingsManager.ShowDebugToolbar)
            Debug.Log("[HUDReplacer] Changed Cursor!");
    }

    private TextureCursor CreateCursor(string value)
    {
        Texture2D cursor = new(2, 2);
        cursor.LoadImage(File.ReadAllBytes(value));
        //Cursor.SetCursor(cursor, new Vector2(6,0), CursorMode.ForceSoftware);
        return new TextureCursor() { texture = cursor, hotspot = new Vector2(6, 0) };
    }
}
