using System;
using System.IO;
using System.Reflection;
using Assets.Scripts.UI;
using HarmonyLib;
using ImGuiNET;
using ImGuiNET.Unity;
using UnityEngine;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(ImGuiManager), "Awake")]
    public class ImGuiManager_Awake_Patch
    {
        private static string ImGui字体路径_主要 = Path.Combine(Directory.GetParent(System.AppDomain.CurrentDomain.BaseDirectory).Parent.FullName, "workshop/content/544550/2968021662/Game_Data/微软雅黑.ttc");
        private static string ImGui字体路径_次要 = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "BepInEx/plugins/微软雅黑.ttc");
        private static FieldInfo 字体引用偏移 = typeof(ImGuiManager).GetField("_fontAtlasConfiguration", BindingFlags.Instance | BindingFlags.NonPublic);
        public static void Postfix(ref ImGuiManager __instance)
        {
            // var 游戏程序目录 = System.AppDomain.CurrentDomain.BaseDirectory;     

            // 创建一个字体定义添加到配置文件中
            入口类.Log.LogInfo($"ImGui字体路径_主要: {ImGui字体路径_主要}");
            入口类.Log.LogInfo($"ImGui字体路径_次要: {ImGui字体路径_次要}");

            var _fontAtlasConfiguration = 字体引用偏移.GetValue(__instance) as FontAtlasConfigAsset;
            var Fonts = _fontAtlasConfiguration.Fonts;
            var _ = new FontDefinition[Fonts.Length + 1];

            ImGuiNET.Unity.FontDefinition 字体定义;
            if (File.Exists(ImGui字体路径_主要))
            {
                扩展方法.创建ImGui字体定义(out 字体定义, ImGui字体路径_主要);
            }
            else
            {
                扩展方法.创建ImGui字体定义(out 字体定义, ImGui字体路径_次要);
            }

            _[0] = Fonts[0];
            _[1] = Fonts[1];
            _[2] = 字体定义;
            for (var i = 3; i < _.Length; i++)
                _[i] = Fonts[i - 1];
            _fontAtlasConfiguration.Fonts = _;
            // TODO: 是否需要销毁Fonts数组,防止内存泄露
        }
    }

    [HarmonyPatch(typeof(TextureManager), nameof(TextureManager.BuildFontAtlas))]
    public class TextureManager_BuildFontAtlas_Patch
    {
        private static MethodInfo AllocateGlyphRangeArray = typeof(TextureManager).GetMethod("AllocateGlyphRangeArray", BindingFlags.Instance | BindingFlags.NonPublic);
        public static bool Prefix(ref TextureManager __instance, ImGuiIOPtr io, in FontAtlasConfigAsset settings)
        {
            if (io.Fonts.IsBuilt())
            {
                __instance.DestroyFontAtlas(io);
            }

            if (!io.MouseDrawCursor)
            {
                io.Fonts.Flags |= ImFontAtlasFlags.NoMouseCursors;
            }

            if (settings == null)
            {
                io.Fonts.AddFontDefault();
                io.Fonts.Build();
                return false;
            }

            FontDefinition[] fonts = settings.Fonts;

            for (int i = 0; i < 2; i++)
            {
                FontDefinition fontDefinition = fonts[i];
                string text = Path.Combine(Application.streamingAssetsPath, fontDefinition.FontPath);
                if (!File.Exists(text))
                {
                    Debug.Log("字体路径不存在,生成图集失败: " + text);
                    continue;
                }

                ImFontConfig fontConfig = default(ImFontConfig);
                ImFontConfigPtr imFontConfigPtr = new ImFontConfigPtr(ref fontConfig);
                FontConfig config = fontDefinition.Config;
                config.ApplyTo(imFontConfigPtr);
                imFontConfigPtr.GlyphRanges = (IntPtr)AllocateGlyphRangeArray.Invoke(__instance, new object[] { fontDefinition.Config });
                io.Fonts.AddFontFromFileTTF(text, fontDefinition.Config.SizeInPixels, imFontConfigPtr);
            }

            for (int i = 2; i < 3; i++)
            {
                FontDefinition fontDefinition = fonts[i];
                string text = fontDefinition.FontPath;
                if (!File.Exists(text))
                {
                    Debug.Log("字体路径不存在,生成图集失败: " + text);
                    continue;
                }

                ImFontConfig fontConfig = default(ImFontConfig);
                ImFontConfigPtr imFontConfigPtr = new ImFontConfigPtr(ref fontConfig);
                FontConfig config = fontDefinition.Config;
                config.ApplyTo(imFontConfigPtr);
                imFontConfigPtr.GlyphRanges = (IntPtr)AllocateGlyphRangeArray.Invoke(__instance, new object[] { fontDefinition.Config });
                io.Fonts.AddFontFromFileTTF(text, fontDefinition.Config.SizeInPixels, imFontConfigPtr);
            }

            for (int i = 3; i < fonts.Length; i++)
            {
                FontDefinition fontDefinition = fonts[i];
                string text = Path.Combine(Application.streamingAssetsPath, fontDefinition.FontPath);
                if (!File.Exists(text))
                {
                    Debug.Log("字体路径不存在,生成图集失败: " + text);
                    continue;
                }

                ImFontConfig fontConfig = default(ImFontConfig);
                ImFontConfigPtr imFontConfigPtr = new ImFontConfigPtr(ref fontConfig);
                FontConfig config = fontDefinition.Config;
                config.ApplyTo(imFontConfigPtr);
                imFontConfigPtr.GlyphRanges = (IntPtr)AllocateGlyphRangeArray.Invoke(__instance, new object[] { fontDefinition.Config });
                io.Fonts.AddFontFromFileTTF(text, fontDefinition.Config.SizeInPixels, imFontConfigPtr);
            }

            if (io.Fonts.Fonts.Size == 0)
            {
                io.Fonts.AddFontDefault();
            }

            if (settings.Rasterizer == FontRasterizerType.StbTrueType)
            {
                io.Fonts.Build();
                return false;
            }

            Debug.LogWarning($"{settings.Rasterizer:G} rasterizer not available, using {FontRasterizerType.StbTrueType:G}. Check if feature is enabled (PluginFeatures.cs).");
            io.Fonts.Build();

            return false;
        }
    }
}