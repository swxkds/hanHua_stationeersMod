using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Pipes;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public static partial class 扩展方法
    {
        public static void 设置螺丝链接(this CircuitHousing IC外壳, Interactable IC外壳控件, ILogicableReference 按钮点击返回)
        {
            switch (按钮点击返回.绑定.type)
            {
                case ILogicableReference内存结构.内存结构.结构类型.原始物体:
                    {
                        var 链接物 = 按钮点击返回.绑定.原始物体结构.原始物体;
                        switch (IC外壳控件.Action)
                        {
                            case InteractableType.Button1:
                                设置链接物体(IC外壳, 链接物, 0); break;
                            case InteractableType.Button2:
                                设置链接物体(IC外壳, 链接物, 1); break;
                            case InteractableType.Button3:
                                设置链接物体(IC外壳, 链接物, 2); break;
                            case InteractableType.Button4:
                                设置链接物体(IC外壳, 链接物, 3); break;
                            case InteractableType.Button5:
                                设置链接物体(IC外壳, 链接物, 4); break;
                            case InteractableType.Button6:
                                设置链接物体(IC外壳, 链接物, 5); break;
                            default: break;
                        }
                    }
                    break;
            }
        }
        private static void 设置链接物体(CircuitHousing IC外壳, ILogicable 选择焦点, byte 螺丝编号)
        {
            // TODO:联机游戏请在此处发送数据包,目前不知道应该发送什么消息
            IC外壳.Devices[螺丝编号] = 选择焦点;
        }
        public static 链接选择面板渲染分支选择消息.消息结构.消息类型 获取渲染分支选择消息(this CircuitHousing IC外壳, Interactable IC外壳控件)
        {
            return 链接选择面板渲染分支选择消息.消息结构.消息类型.可链接物渲染分支;
        }
        public static 链接选择面板渲染分支选择消息.消息结构 获取完整渲染分支选择消息(this CircuitHousing IC外壳, Interactable IC外壳控件)
        {
            return new 链接选择面板渲染分支选择消息.消息结构
            {
                type = 链接选择面板渲染分支选择消息.消息结构.消息类型.可链接物渲染分支,
                可链接物渲染分支消息 = new 链接选择面板渲染分支选择消息.可链接物渲染分支消息 { 空数据网么 = (IC外壳.InputNetwork1DevicesSorted == null || IC外壳.InputNetwork1DevicesSorted.Count <= 1) ? true : false, 可链接物体表 = IC外壳.InputNetwork1DevicesSorted?.Where(d => d != (ILogicable)IC外壳) }
            };
        }
    }

    public class CircuitHousing_InteractWith_Patch
    {
        // 本代码没有作用,仅用于示范如何获取原始方法地址
        [HarmonyReversePatch(0)]
        [HarmonyPatch(typeof(CircuitHousing), nameof(CircuitHousing.InteractWith))]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static Thing.DelayedActionInstance CircuitHousing_InteractWith(CircuitHousing instance, Interactable interactable, Interaction interaction, bool doAction = true)
        {
            return null;
        }

    }

    [HarmonyPatch(typeof(CircuitHousing), nameof(CircuitHousing.InteractWith))]
    public class CircuitHousing_InteractWith_PrefixPatch
    {
        // 添加了光线命中的事件处理逻辑
        [HarmonyPrefix]
        public static bool 交互事件处理(CircuitHousing __instance, ref Thing.DelayedActionInstance __result, Interactable interactable, Interaction interaction, bool doAction = true)
        {
            // 游戏交互的设计是光线命中时,生成消息,此消息中 保存着玩家,玩家活动手插槽,目标被命中控件,目标物体首地址的引用
            if (interaction.SourceSlot.Get() is Labeller 贴标机)
            {
                // interactable.ContextualName: 若是控件,显示"开"关";若是物体,显示名称
                // 仅用于协程函数的时长定义,正常不使用Duration
                var 动作状态消息 = new Thing.DelayedActionInstance
                { Duration = 0, ActionMessage = interactable.ContextualName };
                __result = 链接选择面板.唤醒选择面板(__instance, interactable, interaction, 动作状态消息, 贴标机, doAction);
                return false;
            }

            // 其他情况执行游戏自带的交互逻辑
            return true;
        }
    }

}


