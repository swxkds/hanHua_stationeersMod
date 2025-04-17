using HarmonyLib;
using meanran_xuexi_mods_xiaoyouhua;

namespace meanran_xuexi_mods
{
    [HarmonyPatch(typeof(Assets.Scripts.CursorManager), nameof(Assets.Scripts.CursorManager.SetCursorTarget))]
    public class 鼠标事件钩子类
    {
        static void Postfix(ref Assets.Scripts.CursorManager __instance)
        {
            无线输电面板类.单例?.更新焦点物体(__instance.FoundThing);
        }
    }
}
