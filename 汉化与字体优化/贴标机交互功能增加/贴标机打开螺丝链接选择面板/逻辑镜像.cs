using System.Linq;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public static partial class 扩展方法
    {
        public static void 设置螺丝链接(this LogicMirror 逻辑镜像, Interactable 逻辑镜像控件, ILogicableReference 按钮点击返回)
        {
            switch (按钮点击返回.绑定.type)
            {
                case ILogicableReference内存结构.内存结构.结构类型.原始物体:
                    {
                        var 链接物 = 按钮点击返回.绑定.原始物体结构.原始物体;
                        switch (逻辑镜像控件.Action)
                        { case InteractableType.Button1: 设置链接物体(逻辑镜像, 链接物); break; }
                        break;
                    }
            }
        }
        private static void 设置链接物体(LogicMirror 逻辑镜像, ILogicable 选择焦点)
        {
            // TODO:联机游戏请在此处发送数据包,目前不知道应该发送什么消息
            逻辑镜像.CurrentDevice = (Device)选择焦点;

            if (逻辑镜像.CurrentDevice == null || 逻辑镜像.InputNetwork1 == null || !逻辑镜像.InputNetwork1.DataDeviceList.Contains(逻辑镜像.CurrentDevice))
            {
                if (逻辑镜像.Error == 0)
                {
                    OnServer.Interact(逻辑镜像.InteractError, 1, false);
                }
                return;
            }
            if (逻辑镜像.Error == 1)
            {
                OnServer.Interact(逻辑镜像.InteractError, 0, false);
            }
            return;
        }
        public static 链接选择面板渲染分支选择消息.消息结构.消息类型 获取渲染分支选择消息(this LogicMirror 逻辑镜像, Interactable 逻辑镜像控件)
        {
            switch (逻辑镜像控件.Action)
            {
                case InteractableType.Button1: return 链接选择面板渲染分支选择消息.消息结构.消息类型.可链接物渲染分支;
            }
            return 链接选择面板渲染分支选择消息.消息结构.消息类型.Null;
        }

        public static 链接选择面板渲染分支选择消息.消息结构 获取完整渲染分支选择消息(this LogicMirror 逻辑镜像, Interactable 逻辑镜像控件)
        {
            switch (逻辑镜像控件.Action)
            {
                case InteractableType.Button1:
                    return new 链接选择面板渲染分支选择消息.消息结构
                    {
                        type = 链接选择面板渲染分支选择消息.消息结构.消息类型.可链接物渲染分支,
                        可链接物渲染分支消息 = new 链接选择面板渲染分支选择消息.可链接物渲染分支消息 { 空数据网么 = (逻辑镜像.InputNetwork1DevicesSorted == null || 逻辑镜像.InputNetwork1DevicesSorted.Count <= 1) ? true : false, 可链接物体表 = 逻辑镜像.InputNetwork1DevicesSorted?.Where(d => d != (ILogicable)逻辑镜像 && d.IsLogicReadable()) }
                    };
            }
            return 链接选择面板渲染分支选择消息.消息结构.Null;
        }
    }

    [HarmonyPatch(typeof(LogicMirror), nameof(LogicMirror.InteractWith))]
    public class LogicMirror_InteractWith_PrefixPatch
    {
        // 添加了光线命中的事件处理逻辑
        [HarmonyPrefix]
        public static bool 交互事件处理(LogicMirror __instance, ref Thing.DelayedActionInstance __result, Interactable interactable, Interaction interaction, bool doAction = true)
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


