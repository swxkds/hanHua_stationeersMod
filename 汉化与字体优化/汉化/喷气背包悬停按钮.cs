using Assets.Scripts.Objects;
using HarmonyLib;
using Assets.Scripts.Objects.Items;
using System.Collections.Generic;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(Jetpack), nameof(Jetpack.GetContextualName), typeof(Interactable))]
    public class Jetpack_GetContextualName_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }
}