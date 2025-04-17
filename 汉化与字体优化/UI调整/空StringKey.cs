using Assets.Scripts.Objects;
using Assets.Scripts.UI;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(InventoryWindow), nameof(InventoryWindow.Assign))]
    public class InventoryWindow_Assign_Patch
    {
        // StringKey:翻译文件所使用的Key,若Key为空,则无法找到对应的翻译词条
        // Localization.RecordThing: StringKey的作用见构造函数RecordThing(LocalizedText localizedText)和Language这个类的相关函数
        public static void Postfix(ref InventoryWindow __instance, Slot parentSlot)
        {
            // 入口点类.Log.LogInfo("成功拦截InventoryWindow_Assign_Patch");
            if (string.IsNullOrEmpty(parentSlot.StringKey))
            {
                string str = "Window ";
                DynamicThing dynamicThing = parentSlot.Get();       // 获取物品栏中的物品
                __instance.name = str + ((dynamicThing != null) ? dynamicThing.DisplayName : null);
            }
        }
    }
}