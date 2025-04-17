using Assets.Scripts.Objects.Electrical;
using HarmonyLib;
using UnityEngine;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(WirelessPower), nameof(WirelessPower.MovementSpeedHorizontal), MethodType.Getter)]
    public class WirelessPower_MovementSpeedHorizontal_Patch
    {
        // TODO:GetType(),根据实际类型指针判断是哪种天线,对转速微调;或后置补丁,对转速结果进行微调
        [HarmonyPostfix]
        public static void 方位轴转速调整(ref float __result) => __result = Mathf.Max(__result, 0.2f);
    }

    [HarmonyPatch(typeof(WirelessPower), nameof(WirelessPower.MovementSpeedVertical), MethodType.Getter)]
    public class WirelessPower_MovementSpeedVertical_Patch
    {
        [HarmonyPostfix]
        public static void 俯仰轴转速调整(ref float __result) => __result = Mathf.Max(__result, 0.2f);
    }
}