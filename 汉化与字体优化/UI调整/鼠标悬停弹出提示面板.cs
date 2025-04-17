using Assets.Scripts.UI;
using HarmonyLib;
using UnityEngine;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(PanelToolTip), nameof(PanelToolTip.Initialize))]
    public class PanelToolTip_Initialize_Patch
    {
        [HarmonyPostfix]
        public static void 修改字体大小(ref PanelToolTip __instance)
        {
            __instance.ToolTipItemName.修改大小与遮罩(24);
            __instance.Information.修改大小与遮罩(24);
        }
    }
}

