using Assets.Scripts.UI;
using HarmonyLib;
using UnityEngine.UI;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(SettingItem), nameof(SettingItem.Setup))]
    public class SettingItem_Setup_Patch
    {
        [HarmonyPrefix]
        public static void Setup(SettingItem __instance)
        {
            var HUD滑动条 = __instance.Selectable as Slider;
            if (HUD滑动条)
            {
                if (HUD滑动条.minValue == 25)
                {
                    HUD滑动条.minValue = 1;
                    入口类.Log.LogInfo("成功拦截SettingItem_Setup_Patch, HUD滑动条修改成功!");
                }
            }
        }
    }
}