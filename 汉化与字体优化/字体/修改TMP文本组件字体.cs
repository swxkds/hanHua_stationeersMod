using System.Reflection;
using HarmonyLib;
using TMPro;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(TMP_Settings), "defaultFontAsset", MethodType.Getter)]
    public class TMP_Settings_defaultFontAsset_Patch
    {
        // TMP组件若在构造时没有指定使用的字符图集,则默认使用defaultFontAsset这个字符图集
        // 注:LocalizedFont组件会在每帧渲染时替换回官方内置中文字体,因此此补丁的作用很小,仅仅用于小部分默认使用了英文字体的文本组件
        //    恰恰也正是这部分文本组件,会显示口口
        public static bool Prefix(ref TMP_FontAsset __result)
        {
            if (AssetsLoad.单例.内置TMP字体)
            {
                __result = AssetsLoad.单例.内置TMP字体;
                // 入口点类.Log.LogInfo("成功拦截TMP_Settings_defaultFontAsset_Patch");
                return false;
            }
            else
            {
                入口类.Log.LogWarning("拦截失败TMP_Settings_defaultFontAsset_Patch");
                return true;
            }
        }
    }

    [HarmonyPatch(typeof(TextMeshProUGUI), "Awake")]
    public class TextMeshProUGUI_Awake_Patch
    {
        public static void Postfix(ref TextMeshProUGUI __instance)
        {
            if (AssetsLoad.单例.内置TMP字体)
                __instance.font = AssetsLoad.单例.内置TMP字体;
            // else
            //     入口点类.Log.LogWarning("拦截失败TextMeshProUGUI_Awake_Patch");
        }
    }
}