using System.Collections.Generic;
using Assets.Scripts.UI.ImGuiUi;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(ImguiCreativeSpawnMenu), nameof(ImguiCreativeSpawnMenu.Draw))]
    public class ImguiCreativeSpawnMenu_Draw_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions) => instructions.修改IL代码中的字符串();
    }
}