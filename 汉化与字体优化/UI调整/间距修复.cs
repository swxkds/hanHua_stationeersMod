using Assets.Scripts.UI;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(InventoryWindow), nameof(InventoryWindow.SetVisible))]
    public class InventoryWindow_SetVisible_Patch
    {
        // 背包窗口不这样做,最底下会空一格,且间距不一样
        public static void Prefix(ref InventoryWindow __instance, bool isVisble)
        {
            __instance.RectTransform.gameObject.SetActive(isVisble);
        }
    }
}