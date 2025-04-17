using Assets.Scripts.Inventory;
using Assets.Scripts.Localization2;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.UI;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(LogicHashGen), nameof(LogicHashGen.InteractWith))]
    public class LogicHashGen_InteractWith_PrefixPatch
    {
        // 添加了光线命中的事件处理逻辑
        [HarmonyPrefix]
        public static bool 交互事件处理(LogicHashGen __instance, ref Thing.DelayedActionInstance __result, Interactable interactable, Interaction interaction, bool doAction = true)
        {
            // 游戏交互的设计是光线命中时,生成消息,此消息中 保存着玩家,玩家活动手插槽,目标被命中控件,目标物体首地址的引用
            if (interaction.SourceSlot.Get() is Labeller 贴标机 && interactable.Action == InteractableType.Button1)
            {
                // interactable.ContextualName: 若是控件,显示"开"关";若是物体,显示名称
                // 仅用于协程函数的时长定义,正常不使用Duration
                var 动作状态消息 = new Thing.DelayedActionInstance
                { Duration = 0, ActionMessage = interactable.ContextualName };

                // 贴标机电源不是开启时,仅显示工具提示面板:电源未打开,无法交互
                if (!贴标机.OnOff)
                {
                    __result = 动作状态消息.Fail(GameStrings.DeviceNotOn); return false;
                }
                // 贴标机电池未安装或者没有电量,仅显示工具提示面板:未通电,无法交互
                else if (!贴标机.IsOperable)
                {
                    __result = 动作状态消息.Fail(GameStrings.DeviceNoPower); return false;
                }

                // 贴标机电源打开且有电量
                动作状态消息.AppendStateMessage("单击打开哈希物体选择面板");
                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                __result = 动作状态消息.Succeed();

                if (doAction)
                {
                    __instance.ScrewSound();
                    if (InventoryManager.ParentHuman.OrganBrain.ClientId == (interaction.SourceThing as Entity)?.OrganBrain?.ClientId && InputPrefabs.ShowInputPanelAllDynamicThings("Select Thing"))
                    {
                        InputPrefabs.OnSubmit += __instance.InputFinished;
                    }
                }

                return false;
            }

            // 其他情况执行游戏自带的交互逻辑
            return true;
        }
    }

}


