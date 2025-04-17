using System;
using System.Collections.Generic;
using System.Text;
using Assets.Scripts.Localization2;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    // 若该词条不存在翻译文件,且无法自己创建翻译文件的Key,则需要在IL代码中修改静态构造函数中的加载字符串操作码
    // 所有静态变量的赋值语句会被编译器自动挪到静态构造函数中
    // [HarmonyPatch(typeof(GameStrings), MethodType.StaticConstructor)]
    // public class GameStrings_StaticConstructor_Patch
    // {
    //     public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    // }

    [HarmonyPatch(typeof(NewWorldDifficulty), nameof(NewWorldDifficulty.SetDifficulty), typeof(DifficultySetting))]
    public class NewWorldDifficulty_SetDifficulty_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }

    [HarmonyPatch(typeof(NewWorldDifficulty), "DisplaySmartStat", new Type[] { typeof(float), typeof(StringBuilder), typeof(string), typeof(float), typeof(bool), typeof(string), typeof(string), typeof(GameString) })]
    public class NewWorldDifficulty_DisplaySmartStat_1_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }

    [HarmonyPatch(typeof(NewWorldDifficulty), "DisplaySmartStat", new Type[] { typeof(float), typeof(StringBuilder), typeof(string), typeof(string), typeof(float), typeof(bool), typeof(string), typeof(string), typeof(GameString) })]
    public class NewWorldDifficulty_DisplaySmartStat_2_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }

    [HarmonyPatch(typeof(NewWorldDifficulty), "DisplayAlways")]
    public class NewWorldDifficulty_DisplayAlways_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }
    [HarmonyPatch(typeof(NewWorldDifficulty), "DisplayOnlyIf")]
    public class NewWorldDifficulty_DisplayOnlyIf_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }

    [HarmonyPatch(typeof(NewWorldDifficulty), "DisplayPercentStat")]
    public class NewWorldDifficulty_DisplayPercentStat_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }
}