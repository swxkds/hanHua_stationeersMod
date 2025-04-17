using Assets.Scripts.UI;
using HarmonyLib;
using TMPro;
using Assets.Scripts.Inventory;
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(SlotDisplay), "Initialise")]
    public class SlotDisplay_Initialise_Patch
    {
        public static void Postfix(ref SlotDisplay __instance)
        {
            // 每个物品栏(SlotDisplayButton)预留四个文本组件引用:物品栏按钮名称/物品栏栏位名称/物品栏物品名称/物品栏物品数量
            __instance.SlotDisplayButton.gameObject.AddComponent<SlotDisplay_延时修改>();
        }
    }
}