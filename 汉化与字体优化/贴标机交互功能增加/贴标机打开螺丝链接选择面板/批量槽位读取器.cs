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
        public static void 设置螺丝链接(this LogicBatchSlotReader 批量槽位读取器, Interactable 批量槽位读取器控件, ILogicableReference 按钮点击返回)
        {
            switch (按钮点击返回.绑定.type)
            {
                case ILogicableReference内存结构.内存结构.结构类型.原始物体:
                    {
                        var 链接物 = 按钮点击返回.绑定.原始物体结构.原始物体;
                        switch (批量槽位读取器控件.Action)
                        { case InteractableType.Button1: 设置链接物体(批量槽位读取器, 链接物); break; }
                        break;
                    }
                case ILogicableReference内存结构.内存结构.结构类型.插槽编号:
                    {
                        var 插槽编号 = 按钮点击返回.绑定.插槽编号结构.插槽编号;
                        switch (批量槽位读取器控件.Action)
                        { case InteractableType.Button2: 设置插槽编号(批量槽位读取器, 插槽编号); break; }
                        break;
                    }
                case ILogicableReference内存结构.内存结构.结构类型.插槽类型:
                    {
                        var 插槽类型 = 按钮点击返回.绑定.插槽类型结构.插槽类型;
                        switch (批量槽位读取器控件.Action)
                        { case InteractableType.Button3: 设置插槽类型(批量槽位读取器, 插槽类型); break; }
                        break;
                    }
                case ILogicableReference内存结构.内存结构.结构类型.统计类型:
                    {
                        var 统计类型 = 按钮点击返回.绑定.统计类型结构.统计类型;
                        switch (批量槽位读取器控件.Action)
                        { case InteractableType.Button4: 设置统计类型(批量槽位读取器, 统计类型); break; }
                        break;
                    }
            }
        }
        private static void 设置统计类型(LogicBatchSlotReader 批量槽位读取器, LogicBatchMethod 统计类型)
        {
            // TODO:联机游戏请在此处发送数据包,目前不知道应该发送什么消息
            批量槽位读取器.BatchMethod = 统计类型;
            批量槽位读取器.Setting = 0;
        }
        private static void 设置链接物体(LogicBatchSlotReader 批量槽位读取器, ILogicable 选择焦点)
        {
            // TODO:联机游戏请在此处发送数据包,目前不知道应该发送什么消息
            批量槽位读取器.CurrentPrefabHash = 选择焦点.GetPrefabHash();
            批量槽位读取器.LogicSlotType = LogicSlotType.None;
            批量槽位读取器.Setting = 0;
            批量槽位读取器.SlotIndex = -1;
        }
        private static void 设置插槽编号(LogicBatchSlotReader 批量槽位读取器, int 插槽编号)
        {
            // TODO:联机游戏请在此处发送数据包,目前不知道应该发送什么消息
            if (批量槽位读取器.CurrentPrefab != null)
            {
                批量槽位读取器.SlotIndex = 插槽编号;
                批量槽位读取器.Setting = 0;
            }
        }
        private static void 设置插槽类型(LogicBatchSlotReader 批量槽位读取器, LogicSlotType 插槽类型)
        {
            // TODO:联机游戏请在此处发送数据包,目前不知道应该发送什么消息
            if (批量槽位读取器.CurrentPrefab != null && 批量槽位读取器.SlotIndex != -1)
            {
                批量槽位读取器.LogicSlotType = 插槽类型;
                批量槽位读取器.Setting = 0;
            }
        }
        public static 链接选择面板渲染分支选择消息.消息结构.消息类型 获取渲染分支选择消息(this LogicBatchSlotReader 批量槽位读取器, Interactable 批量槽位读取器控件)
        {
            switch (批量槽位读取器控件.Action)
            {
                case InteractableType.Button1: return 链接选择面板渲染分支选择消息.消息结构.消息类型.可链接物渲染分支;
                case InteractableType.Button2: return 链接选择面板渲染分支选择消息.消息结构.消息类型.插槽编号渲染分支;
                case InteractableType.Button3: return 链接选择面板渲染分支选择消息.消息结构.消息类型.插槽类型渲染分支;
                case InteractableType.Button4: return 链接选择面板渲染分支选择消息.消息结构.消息类型.统计类型渲染分支;
            }
            return 链接选择面板渲染分支选择消息.消息结构.消息类型.Null;
        }

        public static 链接选择面板渲染分支选择消息.消息结构 获取完整渲染分支选择消息(this LogicBatchSlotReader 批量槽位读取器, Interactable 批量槽位读取器控件)
        {
            switch (批量槽位读取器控件.Action)
            {
                case InteractableType.Button1:
                    return new 链接选择面板渲染分支选择消息.消息结构
                    {
                        type = 链接选择面板渲染分支选择消息.消息结构.消息类型.可链接物渲染分支,
                        可链接物渲染分支消息 = new 链接选择面板渲染分支选择消息.可链接物渲染分支消息 { 空数据网么 = (批量槽位读取器.InputNetwork1DevicesSorted == null || 批量槽位读取器.InputNetwork1DevicesSorted.Count <= 1) ? true : false, 可链接物体表 = 批量槽位读取器.InputNetwork1DevicesSorted?.Where(d => d != (ILogicable)批量槽位读取器 && d.IsLogicSlotReadable()) }
                    };
                case InteractableType.Button2:
                    return new 链接选择面板渲染分支选择消息.消息结构
                    {
                        type = 链接选择面板渲染分支选择消息.消息结构.消息类型.插槽编号渲染分支,
                        插槽编号渲染分支消息 = new 链接选择面板渲染分支选择消息.插槽编号渲染分支消息 { 已链接物体 = 批量槽位读取器.CurrentPrefab, 只读或只写 = IOCheck.Readable }
                    };
                case InteractableType.Button3:
                    return new 链接选择面板渲染分支选择消息.消息结构
                    {
                        type = 链接选择面板渲染分支选择消息.消息结构.消息类型.插槽类型渲染分支,
                        插槽类型渲染分支消息 = new 链接选择面板渲染分支选择消息.插槽类型渲染分支消息 { 已链接物体 = 批量槽位读取器.CurrentPrefab, 只读或只写 = IOCheck.Readable, 插槽编号 = 批量槽位读取器.SlotIndex }
                    };
                case InteractableType.Button4:
                    return new 链接选择面板渲染分支选择消息.消息结构
                    {
                        type = 链接选择面板渲染分支选择消息.消息结构.消息类型.统计类型渲染分支,
                        统计类型渲染分支消息 = new 链接选择面板渲染分支选择消息.统计类型渲染分支消息 { _this = 批量槽位读取器 }
                    };
            }
            return 链接选择面板渲染分支选择消息.消息结构.Null;
        }
    }

    [HarmonyPatch(typeof(LogicBatchSlotReader), nameof(LogicBatchSlotReader.InteractWith))]
    public class LogicBatchSlotReader_InteractWith_PrefixPatch
    {
        // 添加了光线命中的事件处理逻辑
        [HarmonyPrefix]
        public static bool 交互事件处理(LogicBatchSlotReader __instance, ref Thing.DelayedActionInstance __result, Interactable interactable, Interaction interaction, bool doAction = true)
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


