using Assets.Scripts.Objects.Items;
using HarmonyLib;
using Objects.Items;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(MiningDrill), nameof(MiningDrill.ModeStrings), MethodType.Getter)]
    public class MiningDrill_ModeStrings_Patch
    {
        public static string[] 开采模式 = new string[] { "默认模式", "平地模式" };
        public static bool Prefix(ref string[] __result)
        {
            __result = 开采模式;
            return false;
        }
    }

    [HarmonyPatch(typeof(PneumaticMiningDrill), nameof(PneumaticMiningDrill.ModeStrings), MethodType.Getter)]
    public class PneumaticMiningDrill_ModeStrings_Patch
    {
        public static bool Prefix(ref string[] __result)
        {
            __result = MiningDrill_ModeStrings_Patch.开采模式;
            return false;
        }
    }
}



















// using HarmonyLib;
// using System;

// namespace meanran_xuexi_mods_xiaoyouhua
// {
//     [HarmonyPatch(typeof(Enum), nameof(Enum.GetNames), typeof(Type))]
//     public class Enum_GetNames_Patch
//     {
//         public static void Postfix(ref string[] __result)
//         {
//             try
//             {
//                 for (var i = 0; i < __result.Length; i++)
//                 {
//                     // 入口点类.Log.LogInfo($"枚举转换的词条: {__result[i]}");
//                     __result[i] = __result[i].词条匹配();
//                 }
//             }
//             catch (Exception e)
//             {
//                 入口类.Log.LogError($"错误对象:{e.Source}");
//                 入口类.Log.LogError($"错误类型:{e.Message}");
//                 入口类.Log.LogError($"错误信息:{e.StackTrace}\n");
//             }
//         }
//     }
// }