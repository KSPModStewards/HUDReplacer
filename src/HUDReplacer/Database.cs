using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

namespace HUDReplacer;

internal static class Database
{
    internal class ReplacementInfo
    {
        public List<SizedReplacementInfo> replacements;

        public SizedReplacementInfo GetMatchingReplacement(Texture2D tex)
        {
            if (replacements.Count == 0)
                return null;

            var sized = GetSizedReplacement(tex);
            var unsized = GetUnsizedReplacement();

            if (sized is not null)
            {
                // If priorities are equal then prefer the sized texture
                if (unsized.priority <= sized.priority)
                    return sized;
            }

            return unsized;
        }

        SizedReplacementInfo GetSizedReplacement(Texture2D tex)
        {
            foreach (var info in replacements)
            {
                if (info.width == tex.width && info.height == tex.height)
                    return info;
            }

            return null;
        }

        SizedReplacementInfo GetUnsizedReplacement()
        {
            SizedReplacementInfo found = replacements[0];

            foreach (var info in replacements)
            {
                if (info.priority < found.priority)
                    break;

                // Prefer textures without a specific size if we have one
                // available.
                if (info.width == 0 && info.height == 0)
                {
                    found = info;
                    break;
                }

                // Otherwise use the biggest texture we have
                if (info.width > found.width && info.height > found.height)
                    found = info;
            }

            return found;
        }
    }

    internal class SizedReplacementInfo
    {
        public int priority;
        public int width;
        public int height;
        public string path;
        public byte[] cachedBytes;
        public Texture2D cachedTexture;
        public string basename;
    }

    internal const string FilePathConfig = "HUDReplacer";
    internal const string ColorPathConfig = "HUDReplacerRecolor";

    internal static Dictionary<string, ReplacementInfo> Images { get; private set; } = [];
    internal static Dictionary<GameScenes, Dictionary<string, ReplacementInfo>> SceneImages
    {
        get;
        private set;
    } = [];
    internal static bool Loaded { get; private set; } = false;

    internal static bool HasAnyReplacements => Images.Count != 0 || SceneImages.Count != 0;

    internal static void LoadConfigs()
    {
        Images.Clear();
        SceneImages.Clear();

        var configs = GameDatabase
            .Instance.GetConfigs(FilePathConfig)
            .OrderByDescending(
                (configFile) =>
                {
                    int priority = 0;
                    configFile.config.TryGetValue("priority", ref priority);
                    return priority;
                }
            )
            .ToArray();

        if (configs.Length == 0)
            return;

        foreach (var configFile in configs)
        {
            var config = configFile.config;
            var filePath = config.GetValue("filePath");

            string onScene = null;
            Dictionary<string, ReplacementInfo> replacements = Images;
            if (config.TryGetValue("onScene", ref onScene))
            {
                if (!Enum.TryParse(onScene, out GameScenes scene))
                {
                    Debug.LogError(
                        $"HUDReplacer: Config {configFile.url} contained invalid onScene value {onScene ?? "<null>"}"
                    );
                    continue;
                }

                if (!SceneImages.TryGetValue(scene, out replacements))
                {
                    replacements = [];
                    SceneImages.Add(scene, replacements);
                }
            }

            int priority = 0;
            if (!config.TryGetValue("priority", ref priority))
            {
                Debug.LogError(
                    $"HUDReplacer: config at {configFile.url} is missing a priority key and will not be loaded"
                );
                continue;
            }

            var basePath = KSPUtil.ApplicationRootPath;
            Debug.Log($"HUDReplacer: path {filePath} - priority: {priority}");
            string[] files = Directory.GetFiles(KSPUtil.ApplicationRootPath + filePath, "*.png");

            List<SizedReplacementInfo> infos = new(files.Length);
            foreach (string filename in files)
            {
                var relpath = filename;
                if (relpath.StartsWith(basePath))
                    relpath = relpath.Substring(basePath.Length);

                Debug.Log($"HUDReplacer: Found file {relpath}");

                int width = 0;
                int height = 0;

                string basename = Path.GetFileNameWithoutExtension(filename);
                int index = basename.LastIndexOf('#');
                if (index != -1)
                {
                    string size = basename.Substring(index + 1);
                    basename = basename.Substring(0, index);

                    index = size.IndexOf('x');
                    if (
                        index == -1
                        || !int.TryParse(size.Substring(0, index), out width)
                        || !int.TryParse(size.Substring(index + 1), out height)
                    )
                    {
                        Debug.LogError(
                            $"HUDReplacer: filename {filename} was not in the expected format. It needs to be either `name.png` or `name#<width>x<height>.png`"
                        );
                        continue;
                    }
                }

                SizedReplacementInfo info = new()
                {
                    priority = priority,
                    width = width,
                    height = height,
                    path = filename,
                    basename = basename,
                };

                if (!replacements.TryGetValue(basename, out var replacement))
                {
                    replacement = new ReplacementInfo
                    {
                        replacements = new List<SizedReplacementInfo>(1),
                    };
                    replacements.Add(basename, replacement);
                }

                // We will never select a replacement with a priority lower
                // than the highest priority, so don't bother adding it to
                // the list.
                if (replacement.replacements.Count != 0)
                {
                    if (info.priority < replacement.replacements[0].priority)
                        continue;
                }

                replacement.replacements.Add(info);
                infos.Add(info);
            }

            Parallel.ForEach(infos, info => info.cachedBytes = File.ReadAllBytes(info.path));

            if (SystemInfo.copyTextureSupport != CopyTextureSupport.None)
            {
                // We can afford to load texture async in the loading screen, but
                // when reloading we want them loaded immediately.
                if (HighLogic.LoadedScene == GameScenes.LOADING)
                {
                    foreach (var info in infos)
                        Loader.Instance.StartCoroutine(LoadTexture(info));
                }
                else
                {
                    foreach (var info in infos)
                    {
                        var tex = new Texture2D(2, 2, TextureFormat.ARGB32, false);
                        tex.LoadImage(info.cachedBytes);
                        info.cachedTexture = tex;
                    }
                }
            }

            Loaded = true;
        }
    }

    static IEnumerator LoadTexture(SizedReplacementInfo info)
    {
        using var request = UnityWebRequestTexture.GetTexture(new Uri(info.path));
        yield return request.SendWebRequest();

        if (request.isHttpError || request.isNetworkError)
            yield break;

        var texture = DownloadHandlerTexture.GetContent(request);
        if (texture == null)
            yield break;

        info.cachedTexture = texture;
    }
}
