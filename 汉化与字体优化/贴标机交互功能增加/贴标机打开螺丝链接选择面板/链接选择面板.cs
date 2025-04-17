using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Assets.Scripts;
using Assets.Scripts.Inventory;
using Assets.Scripts.Localization2;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Objects.Pipes;
using Assets.Scripts.UI;
using Assets.Scripts.Util;
using HarmonyLib;
using Reagents;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static meanran_xuexi_mods_xiaoyouhua.链接选择面板渲染分支选择消息;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class ILogicableReference内存结构
    {
        public class 原始试剂结构
        {
            public Reagent 原始试剂;
            public 原始试剂结构(Reagent 原始试剂)
            {
                this.原始试剂 = 原始试剂;
            }
        }
        public class 逻辑类型结构
        {
            public LogicType 逻辑类型;
            public 逻辑类型结构(LogicType 支持么)
            {
                this.逻辑类型 = 支持么;
            }
        }
        public class 插槽类型结构
        {
            public LogicSlotType 插槽类型;
            public 插槽类型结构(LogicSlotType 支持么)
            {
                this.插槽类型 = 支持么;
            }
        }
        public class 插槽编号结构
        {
            public ILogicable 原始物体;
            public int 插槽编号;
            public 插槽编号结构(ILogicable obj, int 插槽编号)
            {
                this.原始物体 = obj;
                this.插槽编号 = 插槽编号;
            }
        }
        public class 试剂模式结构
        {
            public LogicReagentMode 试剂模式;
            public 试剂模式结构(LogicReagentMode 试剂模式)
            {
                this.试剂模式 = 试剂模式;
            }
        }
        public class 基本数学类型结构
        {
            public MathOperators 基本数学类型;
            public 基本数学类型结构(MathOperators 基本数学类型)
            {
                this.基本数学类型 = 基本数学类型;
            }
        }
        public class 高级数学类型结构
        {
            public MathOperatorsUnary 高级数学类型;
            public 高级数学类型结构(MathOperatorsUnary 高级数学类型)
            {
                this.高级数学类型 = 高级数学类型;
            }
        }
        public class 比较类型结构
        {
            public ConditionOperation 比较类型;
            public 比较类型结构(ConditionOperation 比较类型)
            {
                this.比较类型 = 比较类型;
            }
        }
        public class 统计类型结构
        {
            public LogicBatchMethod 统计类型;
            public 统计类型结构(LogicBatchMethod type)
            {
                this.统计类型 = type;
            }
        }
        public class 原始物体结构
        {
            public ILogicable 原始物体;
            public 原始物体结构(ILogicable obj)
            {
                this.原始物体 = obj;
            }
        }
        public class 逻辑门类型结构
        {
            public GateOperators 逻辑门类型;
            public 逻辑门类型结构(GateOperators obj)
            {
                this.逻辑门类型 = obj;
            }
        }
        public class 最大最小值类型结构
        {
            public enum _ComparisonOperation { Greater = 1, Less }
            public _ComparisonOperation 最大最小值类型;
            public 最大最小值类型结构(_ComparisonOperation obj) { this.最大最小值类型 = obj; }
        }
        public class 无线模式结构
        {
            public enum 无线模式 { 联网设备数据转发或者点对点发送 = 0, 点对点监听 = 1 }
            public 无线模式 工作模式;
            public 无线模式结构(无线模式 obj)
            {
                this.工作模式 = obj;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        public class 内存结构
        {
            public 内存结构(结构类型 type, 原始物体结构 obj)
            {
                this.type = type;
                this.原始物体结构 = obj;
            }
            public 内存结构(结构类型 type, 逻辑类型结构 obj)
            {
                this.type = type;
                this.逻辑类型结构 = obj;
            }
            public 内存结构(结构类型 type, 统计类型结构 obj)
            {
                this.type = type;
                this.统计类型结构 = obj;
            }
            public 内存结构(结构类型 type, 插槽类型结构 obj)
            {
                this.type = type;
                this.插槽类型结构 = obj;
            }
            public 内存结构(结构类型 type, 插槽编号结构 obj)
            {
                this.type = type;
                this.插槽编号结构 = obj;
            }
            public 内存结构(结构类型 type, 基本数学类型结构 obj)
            {
                this.type = type;
                this.基本数学类型结构 = obj;
            }
            public 内存结构(结构类型 type, 原始试剂结构 obj)
            {
                this.type = type;
                this.原始试剂结构 = obj;
            }
            public 内存结构(结构类型 type, 试剂模式结构 obj)
            {
                this.type = type;
                this.试剂模式结构 = obj;
            }
            public 内存结构(结构类型 type, 比较类型结构 obj)
            {
                this.type = type;
                this.比较类型结构 = obj;
            }
            public 内存结构(结构类型 type, 高级数学类型结构 obj)
            {
                this.type = type;
                this.高级数学类型结构 = obj;
            }
            public 内存结构(结构类型 type, 逻辑门类型结构 obj)
            {
                this.type = type;
                this.逻辑门类型结构 = obj;
            }
            public 内存结构(结构类型 type, 最大最小值类型结构 obj)
            {
                this.type = type;
                this.最大最小值类型结构 = obj;
            }
            public 内存结构(结构类型 type, 无线模式结构 obj)
            {
                this.type = type;
                this.无线模式结构 = obj;
            }

            public enum 结构类型 : byte
            {
                原始试剂 = 0, 逻辑类型 = 1, 插槽类型 = 2, 插槽编号 = 3, 试剂模式 = 4,
                基本数学类型 = 5, 高级数学类型 = 6, 比较类型 = 7, 统计类型 = 8, 原始物体 = 9,
                逻辑门类型 = 10, 最大最小值类型 = 11, 无线模式 = 12
            }

            [FieldOffset(0)]
            public 结构类型 type;
            [FieldOffset(8)]
            public 原始试剂结构 原始试剂结构;
            [FieldOffset(8)]
            public 逻辑类型结构 逻辑类型结构;
            [FieldOffset(8)]
            public 插槽类型结构 插槽类型结构;
            [FieldOffset(8)]
            public 插槽编号结构 插槽编号结构;
            [FieldOffset(8)]
            public 试剂模式结构 试剂模式结构;
            [FieldOffset(8)]
            public 基本数学类型结构 基本数学类型结构;
            [FieldOffset(8)]
            public 高级数学类型结构 高级数学类型结构;
            [FieldOffset(8)]
            public 比较类型结构 比较类型结构;
            [FieldOffset(8)]
            public 统计类型结构 统计类型结构;
            [FieldOffset(8)]
            public 原始物体结构 原始物体结构;
            [FieldOffset(8)]
            public 逻辑门类型结构 逻辑门类型结构;
            [FieldOffset(8)]
            public 最大最小值类型结构 最大最小值类型结构;
            [FieldOffset(8)]
            public 无线模式结构 无线模式结构;

        }
    }
    public class ILogicableReference : UserInterfaceAnimated, IScreenSpaceTooltip
    {
        public ILogicableReference内存结构.内存结构 绑定;

        // 游戏物体的DisplayName随时可能变更,而其它类型之类是固定的显示名称
        public string 显示名称 => 绑定.type == ILogicableReference内存结构.内存结构.结构类型.原始物体 ? 绑定.原始物体结构.原始物体.DisplayName : 描述.text;
        public bool TooltipIsVisible => IsVisible;
        public Image 缩略图;
        public TextMeshProUGUI 描述;
        public void 回调_按钮点击()
        {
            链接选择面板.当前焦点 = this;
            链接选择面板.Submit();
        }
        public void 设置(ILogicableReference内存结构.内存结构 绑定, Sprite 可选自定义缩略图 = null)
        {
            this.绑定 = 绑定;
            if (绑定.type == ILogicableReference内存结构.内存结构.结构类型.原始物体)
            {
                缩略图.sprite = ((Thing)绑定.原始物体结构.原始物体).Thumbnail;
                回调_RefreshString();
            }
            else
            {
                switch (绑定.type)
                {
                    case ILogicableReference内存结构.内存结构.结构类型.原始试剂:
                        描述.text = 绑定.原始试剂结构.原始试剂.DisplayName + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.逻辑类型:
                        描述.text = 绑定.逻辑类型结构.逻辑类型.GetName() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.插槽类型:
                        描述.text = EnumCollections.LogicSlotTypes.GetName(绑定.插槽类型结构.插槽类型, false) + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.插槽编号:
                        描述.text = 绑定.插槽编号结构.原始物体.GetSlot(绑定.插槽编号结构.插槽编号).DisplayName + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.试剂模式:
                        描述.text = 绑定.试剂模式结构.试剂模式.ToString() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.基本数学类型:
                        描述.text = LogicMath.EnumOperators.GetName(绑定.基本数学类型结构.基本数学类型, false) + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.高级数学类型:
                        描述.text = LogicMathUnary.EnumOperators.GetNameFromValue((int)绑定.高级数学类型结构.高级数学类型, false) + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.比较类型:
                        描述.text = 绑定.比较类型结构.比较类型.ToString() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.统计类型:
                        描述.text = 绑定.统计类型结构.统计类型.ToString() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.逻辑门类型:
                        描述.text = 绑定.逻辑门类型结构.逻辑门类型.ToString() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.最大最小值类型:
                        描述.text = 绑定.最大最小值类型结构.最大最小值类型.ToString() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                    case ILogicableReference内存结构.内存结构.结构类型.无线模式:
                        描述.text = 绑定.无线模式结构.工作模式.ToString() + "\n" + Localization.GetSlotTooltip(Slot.Class.None); break;
                }

                if (可选自定义缩略图 != null)
                    缩略图.sprite = 可选自定义缩略图;
            }
        }

        public void 回调_RefreshString()
        {
            // 只有描述.text = 原始物体的显示名称时才刷新文本(贴标机改名),其他文本不会变更
            if (绑定.原始物体结构.原始物体 is Thing)
            {
                描述.text = 绑定.原始物体结构.原始物体.ToTooltip() + "\n" + Localization.GetSlotTooltip(Slot.Class.None);
            }
        }

        private string 工具提示详情()
        {
            return Localization.GetSlotTooltip(Slot.Class.None) + "\n模组制作真好玩";
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            // 此函数由Unity引擎自动调用,当光标进入感应区域时调用
            base.OnPointerEnter(eventData);
            PanelToolTip.Instance.SetUpToolTip(显示名称, 工具提示详情(), this);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            // 此函数由Unity引擎自动调用,当光标离开感应区域时调用
            base.OnPointerExit(eventData);
            PanelToolTip.Instance.ClearToolTip();
        }

        public void DoUpdate()
        {
            // 此函数由PanelToolTip.Instance在内部调用
            PanelToolTip.Instance.SetInfoText(工具提示详情());
        }
    }
    public struct 链接选择面板渲染分支选择消息
    {
        public static Dictionary<Type, (Func<LogicUnitBase, Interactable, 消息结构.消息类型>, Func<LogicUnitBase, Interactable, 消息结构>)> 函数地址缓冲区 =
        new Dictionary<Type, (Func<LogicUnitBase, Interactable, 消息结构.消息类型>, Func<LogicUnitBase, Interactable, 消息结构>)>(64);
        public struct 可链接物渲染分支消息 { public IEnumerable<ILogicable> 可链接物体表; public bool 空数据网么; }   // 扫描得到的最新的数据网节点物体表
        public struct 无线可链接物渲染分支消息 { public int 无线模式; public IEnumerable<ILogicable> 可链接物体表; }   // 扫描得到的最新的数据网节点物体表
        public struct 逻辑类型渲染分支消息 { public ILogicable 已链接物体; public IOCheck 只读或只写; }   // 逻辑组件要么读取器选择只读类型,要么写入器选择只写类型
        public struct 插槽编号渲染分支消息 { public ILogicable 已链接物体; public IOCheck 只读或只写; }
        public struct 插槽类型渲染分支消息 { public ILogicable 已链接物体; public IOCheck 只读或只写; public int 插槽编号; }
        public struct 试剂模式渲染分支消息 { public ILogicable 已链接物体; }
        public struct 原始试剂渲染分支消息 { public ILogicable 已链接物体; public LogicReagentMode 试剂模式; }
        public struct 比较类型渲染分支消息 { public ILogicable _this; }
        public struct 统计类型渲染分支消息 { public ILogicable _this; }
        public struct 基本数学类型渲染分支消息 { public ILogicable _this; }
        public struct 高级数学类型渲染分支消息 { public ILogicable _this; }
        public struct 逻辑门类型渲染分支消息 { public ILogicable _this; }
        public struct 最大最小值类型渲染分支消息 { public ILogicable _this; }
        public struct 无线模式渲染分支消息 { public ILogicable _this; public int 无线模式; }

        [StructLayout(LayoutKind.Explicit)]
        public struct 消息结构
        {
            public static 消息结构 Null = new 消息结构 { type = 消息类型.Null };
            public enum 消息类型 : byte
            {
                原始试剂渲染分支, 逻辑类型渲染分支, 插槽类型渲染分支, 插槽编号渲染分支, 试剂模式渲染分支,
                基本数学类型渲染分支, 高级数学类型渲染分支, 比较类型渲染分支, 统计类型渲染分支, 可链接物渲染分支,
                无线可链接物渲染分支, 逻辑门类型渲染分支, 最大最小值类型渲染分支, 无线模式渲染分支, Null
            }

            [FieldOffset(0)]
            public 消息类型 type;
            [FieldOffset(8)]
            public 可链接物渲染分支消息 可链接物渲染分支消息;
            [FieldOffset(8)]
            public 逻辑类型渲染分支消息 逻辑类型渲染分支消息;
            [FieldOffset(8)]
            public 插槽编号渲染分支消息 插槽编号渲染分支消息;
            [FieldOffset(8)]
            public 插槽类型渲染分支消息 插槽类型渲染分支消息;
            [FieldOffset(8)]
            public 试剂模式渲染分支消息 试剂模式渲染分支消息;
            [FieldOffset(8)]
            public 原始试剂渲染分支消息 原始试剂渲染分支消息;
            [FieldOffset(8)]
            public 统计类型渲染分支消息 统计类型渲染分支消息;
            [FieldOffset(8)]
            public 基本数学类型渲染分支消息 基本数学类型渲染分支消息;
            [FieldOffset(8)]
            public 比较类型渲染分支消息 比较类型渲染分支消息;
            [FieldOffset(8)]
            public 高级数学类型渲染分支消息 高级数学类型渲染分支消息;
            [FieldOffset(8)]
            public 逻辑门类型渲染分支消息 逻辑门类型渲染分支消息;
            [FieldOffset(8)]
            public 最大最小值类型渲染分支消息 最大最小值类型渲染分支消息;
            [FieldOffset(8)]
            public 无线模式渲染分支消息 无线模式渲染分支消息;
            [FieldOffset(8)]
            public 无线可链接物渲染分支消息 无线可链接物渲染分支消息;
        }
    }
    public class 链接选择面板 : InputWindowBase
    {
        // Interactable: 控件,注意是控件,而不是物体
        // Interaction: 交互双方. 玩家/玩家活动手插槽/视线交互焦点所在的控件或物体或另一个玩家
        // doAction: 玩家当前是在执行动作还是单纯的就看看(拿着工具不操作也算无动作)

        // AttackWith是操作事件,是对整个物体的相关状态变量进行修改
        // InteractWith是交互事件,是对物体内部的某个控件相关状态变量进行修改
        // 管道没有控件(例开关/按钮/刻度盘),因此是AttackWith,管道也没有
        // AttackWith和InteractWith这两个函数由游戏的事件总线每帧调用,会传递交互双方信息,我们只需要根据这些信息的组合,返回对应消息即可
        public static Thing.DelayedActionInstance 唤醒选择面板(LogicUnitBase 光线命中的设备, Interactable 光线命中的控件, Interaction 交互双方, Thing.DelayedActionInstance 默认动作状态消息, Labeller 贴标机, bool 玩家有动作么 = true)
        {
            // 重要!重要!玩家有动作么只有在协程内部才会写入true,在正常时永远是false
            // 当调用默认动作状态消息.Succeed()给IsDisabled写入false时,表示允许开启协程,当单击左键时,创建动作协程
            // 当调用默认动作状态消息.Fail()给IsDisabled写入true,表示禁止开启协程,当单击左键时,直接忽略
            // 在协程函数内部,判断若鼠标左键弹起或者视线乱唤导致光线命中的焦点物体改变了,直接结束协程,不产生任何AttackWith消息
            // 在协程函数内部,若长按时长>=默认动作状态消息.Duration,产生一个玩家有动作么=true的AttackWith消息

            Thing.DelayedActionInstance 动作状态消息 = null;
            // 仅用于正常函数的视线焦点物体工具提示,左上角
            默认动作状态消息.ActionMessage = ActionStrings.Set;

            // 贴标机电源不是开启时,仅显示工具提示面板:电源未打开,无法交互
            if (!贴标机.OnOff)
            {
                动作状态消息 = 默认动作状态消息.Fail(GameStrings.DeviceNotOn);
            }
            // 贴标机电池未安装或者没有电量,仅显示工具提示面板:未通电,无法交互
            else if (!贴标机.IsOperable)
            {
                动作状态消息 = 默认动作状态消息.Fail(GameStrings.DeviceNoPower);
            }

            // 贴标机电源打开且有电量
            if (动作状态消息 == null)
            {
                // 光线命中的设备.GetType() 和 typeof(Interactable) 的区别是一个是读取预设构造指令写入实例内存的类型对象指针,一个是编译完成后加载内存时将IL代码替换成类型对象指针
                // C#为每个类生成类型对象.   nameof()的作用是编译完成后将最终生成的方法名称替换此处
                const BindingFlags 标志 = BindingFlags.Public | BindingFlags.Static;
                var 类型对象指针 = 光线命中的设备.GetType();
                消息结构.消息类型 简易消息;
                消息结构 完整消息 = default;
                (Func<LogicUnitBase, Interactable, 消息结构.消息类型>, Func<LogicUnitBase, Interactable, 消息结构>) 地址;

                if (函数地址缓冲区.TryGetValue(类型对象指针, out 地址))
                {
                    简易消息 = 地址.Item1(光线命中的设备, 光线命中的控件);
                }
                else
                {
                    函数地址缓冲区[typeof(LogicReader)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicReader)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicReader)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicBatchReader)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicBatchReader)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicBatchReader)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(CircuitHousing)] = ((LogicUnitBase obj, Interactable obj控件) => ((CircuitHousing)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((CircuitHousing)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicWriter)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicWriter)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicWriter)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicBatchWriter)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicBatchWriter)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicBatchWriter)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicSlotReader)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicSlotReader)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicSlotReader)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicBatchSlotReader)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicBatchSlotReader)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicBatchSlotReader)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicMath)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicMath)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicMath)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(ReagentReader)] = ((LogicUnitBase obj, Interactable obj控件) => ((ReagentReader)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((ReagentReader)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicWriterSwitch)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicWriterSwitch)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicWriterSwitch)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicMirror)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicMirror)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicMirror)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicCompare)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicCompare)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicCompare)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicSelect)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicSelect)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicSelect)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicMathUnary)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicMathUnary)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicMathUnary)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicGate)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicGate)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicGate)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicMinMax)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicMinMax)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicMinMax)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(LogicTransmitter)] = ((LogicUnitBase obj, Interactable obj控件) => ((LogicTransmitter)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((LogicTransmitter)obj).获取完整渲染分支选择消息(obj控件));
                    函数地址缓冲区[typeof(RocketCircuitHousing)] = ((LogicUnitBase obj, Interactable obj控件) => ((CircuitHousing)obj).获取渲染分支选择消息(obj控件), (LogicUnitBase obj, Interactable obj控件) => ((CircuitHousing)obj).获取完整渲染分支选择消息(obj控件));

                    函数地址缓冲区.TryGetValue(类型对象指针, out 地址);
                    简易消息 = 地址.Item1(光线命中的设备, 光线命中的控件);
                }

                switch (简易消息)
                {
                    case 消息结构.消息类型.Null:
                        {
                            // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Fail("请等待交互实装");
                            return 动作状态消息;
                        }
                    case 消息结构.消息类型.原始试剂渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.原始试剂渲染分支消息.已链接物体 == null)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("请先设置螺丝链接物体");
                                return 动作状态消息;
                            }
                            else if (完整消息.原始试剂渲染分支消息.试剂模式 == LogicReagentMode.TotalContents)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("试剂总质量模式(TotalContents),不需要选择试剂");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开试剂选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.可链接物渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.可链接物渲染分支消息.空数据网么)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("当前数据网未检测到可链接物体");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开链接节点选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.基本数学类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开基本数学类型选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                    case 消息结构.消息类型.插槽类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.插槽类型渲染分支消息.已链接物体 == null)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("请先设置螺丝链接物体");
                                return 动作状态消息;
                            }
                            else if (完整消息.插槽类型渲染分支消息.插槽编号 == -1)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("请先设置插槽");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开插槽类型选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.插槽编号渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.插槽编号渲染分支消息.已链接物体 == null)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("请先设置螺丝链接物体");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开插槽编号选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.比较类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开比较类型选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                    case 消息结构.消息类型.统计类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开统计类型选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                    case 消息结构.消息类型.试剂模式渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.试剂模式渲染分支消息.已链接物体 == null)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("请先设置螺丝链接物体");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开试剂模式选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.逻辑类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.逻辑类型渲染分支消息.已链接物体 == null)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("请先设置螺丝链接物体");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开逻辑类型选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.高级数学类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开高级数学类型选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                    case 消息结构.消息类型.逻辑门类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开逻辑门类型选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                    case 消息结构.消息类型.最大最小值类型渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开最大最小值类型选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                    case 消息结构.消息类型.无线可链接物渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            if (完整消息.无线可链接物渲染分支消息.无线模式 == 1)
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail(GameStrings.ThingModeDoesNotSupportLinking);
                                return 动作状态消息;
                            }
                            else if (!完整消息.无线可链接物渲染分支消息.可链接物体表.Any())
                            {
                                // ActionMessage:禁用时设置是红色字,不允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Fail("当前数据网未检测到可链接物体");
                                return 动作状态消息;
                            }
                            else
                            {
                                默认动作状态消息.AppendStateMessage("单击打开链接节点选择面板");
                                // ActionMessage:启用时设置是绿色字,允许创建动作协程
                                动作状态消息 = 默认动作状态消息.Succeed();
                            }
                            break;
                        }
                    case 消息结构.消息类型.无线模式渲染分支:
                        {
                            完整消息 = 地址.Item2(光线命中的设备, 光线命中的控件);
                            默认动作状态消息.AppendStateMessage("单击打开无线模式选择面板");
                            // ActionMessage:启用时设置是绿色字,允许创建动作协程
                            动作状态消息 = 默认动作状态消息.Succeed();
                            break;
                        }
                }

                // 协程函数完美执行结束后,发送一条玩家有动作么=true的AttackWith消息,播放角磨机音效,并调用管道高压炸开协程函数
                if (玩家有动作么)
                {
                    // 播放音效
                    光线命中的设备.PlayPooledAudioSound(Defines.Sounds.ScrewdriverSound, Vector3.zero);

                    if (交互双方.SourceThing is Human 玩家 && 玩家.State == EntityState.Alive && 玩家.OrganBrain != null && 玩家.OrganBrain.LocalControl)
                    {
                        if (绘制链接选择面板("链接选择面板", 完整消息))
                        {
                            var 设置螺丝链接 = typeof(扩展方法).GetMethod(nameof(扩展方法.设置螺丝链接), 标志, null, new[] { 类型对象指针, typeof(Interactable), typeof(ILogicableReference) }, null);

                            // 当选择某个物品后,执行什么事件
                            按钮点击事件 += 按钮点击返回 =>
                            {
                                // 当选择某个物品后,执行什么事件
                                设置螺丝链接.Invoke(null, new object[] { 光线命中的设备, 光线命中的控件, 按钮点击返回 });
                            };
                        }
                    }
                }
            }

            return 动作状态消息;
        }
        private delegate ILogicableReference内存结构.内存结构 通用渲染分支表设置回调<T1>(T1 参数);
        public static GameObject 选项按钮预制体;    // 按钮的层级和控制结构,增加显示物体时就构造此结构
        public static GameObject 渲染分支预制体;    // 原始面板的垂直布局组,用于对所有显示的按钮进行排版,有一个区域适配组件(设置了高度适配) 有一个垂直布局组
        public static GameObject 父级画布;
        public delegate void 链接选择面板事件(ILogicableReference 按钮点击返回_当前焦点);
        public static event 链接选择面板事件 按钮点击事件;
        public static 链接选择面板 单例;
        public static InputPanelState 链接选择面板状态 = InputPanelState.None;
        public static ILogicableReference 当前焦点; // 每个按钮保存了一个可链接物体的引用,点击时触发时间将引用赋给当前焦点
        public TextMeshProUGUI 面板标题;
        public static 消息结构.消息类型 当前消息类型;         // 用于区分使用哪个渲染分支
        public Transform 渲染分支父级;
        public ScrollRect 滚动组件;
        public TMP_InputField 面板搜索栏;
        public Button 面板取消按钮;
        // -----------------------------------------------------------------------------------------------------------
        // 重要! 重要! 每一个按钮的点击事件都绑定了一个数据网引用组件,当触发事件时,将数据网引用组件中的数据网物体/逻辑类型/统计类型赋给链接选择面板的当前焦点
        // 重要! 重要! 数据网物体引用组件和按钮是同一个GameObject,激活时两个同时激活,失活时两个同时失活
        // 搜索功能: 对面板节点表的的按钮进行临时的激活或者失活    搜索栏每次文本变更时,遍历活跃面板节点表,对每个数据网物体引用组件里的物体显示名称进行匹配,若匹配上,则激活,否则失活
        public RectTransform 可链接物渲染分支;
        private Dictionary<long, ILogicable> 已发现节点表 = new Dictionary<long, ILogicable>(32);
        private Dictionary<long, ILogicable> 已显示节点表 = new Dictionary<long, ILogicable>(32);
        private Dictionary<long, ILogicable> 已失效节点表 = new Dictionary<long, ILogicable>(32);
        private Dictionary<long, ILogicableReference> 活跃面板节点表 = new Dictionary<long, ILogicableReference>(32);
        private Queue<ILogicableReference> 休眠面板节点表 = new Queue<ILogicableReference>(32);
        // -----------------------------------------------------------------------------------------------------------
        private Dictionary<消息结构.消息类型, (RectTransform, List<ILogicableReference>)> 通用渲染分支表 = new Dictionary<消息结构.消息类型, (RectTransform, List<ILogicableReference>)>();
        private List<ILogicableReference> 通用活跃面板节点表 = new List<ILogicableReference>();
        private Dictionary<(int PrefabHash, IOCheck 读或写), (RectTransform 分支节点, List<ILogicableReference> 面板节点表)> 已构造逻辑类型分支表
        = new Dictionary<(int, IOCheck), (RectTransform, List<ILogicableReference>)>();
        private Dictionary<(int PrefabHash, IOCheck 读或写, int 插槽编号), (RectTransform 分支节点, List<ILogicableReference> 面板节点表)> 已构造插槽类型分支表
        = new Dictionary<(int, IOCheck, int), (RectTransform, List<ILogicableReference>)>();
        private Dictionary<(int PrefabHash, IOCheck 读或写), (RectTransform 分支节点, List<ILogicableReference> 面板节点表)> 已构造插槽编号分支表
        = new Dictionary<(int, IOCheck), (RectTransform, List<ILogicableReference>)>();
        private static ILogicableReference内存结构.无线模式结构.无线模式[] 无线模式表 = (ILogicableReference内存结构.无线模式结构.无线模式[])Enum.GetValues(typeof(ILogicableReference内存结构.无线模式结构.无线模式));
        private static ILogicableReference内存结构.最大最小值类型结构._ComparisonOperation[] 最大最小值类型表 = (ILogicableReference内存结构.最大最小值类型结构._ComparisonOperation[])Enum.GetValues(typeof(ILogicableReference内存结构.最大最小值类型结构._ComparisonOperation));
        private static GateOperators[] 逻辑门类型表 = (GateOperators[])Enum.GetValues(typeof(GateOperators));
        private static MathOperatorsUnary[] 高级数学类型表 = (MathOperatorsUnary[])Enum.GetValues(typeof(MathOperatorsUnary));
        private static ConditionOperation[] 比较类型表 = (ConditionOperation[])Enum.GetValues(typeof(ConditionOperation));
        private static LogicReagentMode[] 试剂模式表 = (LogicReagentMode[])Enum.GetValues(typeof(LogicReagentMode));
        private static Reagent[] 原始试剂表 = null;
        private static MathOperators[] 基本数学类型表 = (MathOperators[])Enum.GetValues(typeof(MathOperators));
        private static LogicBatchMethod[] 统计类型表 = (LogicBatchMethod[])Enum.GetValues(typeof(LogicBatchMethod));
        public override void Initialize()
        {
            base.Initialize();
            SetVisible(isVisble: false);        // 初始失活面板

            Localization.OnLanguageChanged += 回调_语言设置变更;  // 当显示逻辑类型滚动区域时,请确保不要刷新文本

            面板标题.font = AssetsLoad.单例.内置TMP字体;

            面板取消按钮 = this.transform.GetChild(0).GetChild(4).GetComponent<Button>();
            面板取消按钮.onClick.AddListener(CancelInput);

            面板搜索栏.onSubmit.AddListener(回调_搜索栏查询操作);       // 搜索栏输入时按回车键触发此事件,请根据当前面板类型,作出不同的搜索条件
            面板搜索栏.onValueChanged.AddListener(回调_搜索栏查询操作); // 搜索栏文本变化时触发此事件,请根据当前面板类型,作出不同的搜索条件

            渲染分支预制体 = UnityEngine.Object.Instantiate(可链接物渲染分支.gameObject, null);
            渲染分支预制体.gameObject.SetActive(false);      // 预制体保持失活,不要让Unity引擎运行它

            渲染分支父级 = 可链接物渲染分支.transform.parent;

            设置激活状态(可链接物渲染分支, false);
            通用渲染分支表[消息结构.消息类型.可链接物渲染分支] = (可链接物渲染分支, null);

            滚动组件 = transform.GetChild(0).GetChild(7).GetComponent<ScrollRectNoDrag>();

        }
        private static void 设置激活状态(Transform obj, bool 状态)
        {
            if (obj && obj.gameObject.activeSelf != 状态)
            {
                obj.gameObject.SetActive(状态);
            }
        }
        private static void 回调_语言设置变更()
        {
            if (当前消息类型 == 消息结构.消息类型.可链接物渲染分支)
            {
                foreach (var value in 单例.活跃面板节点表.Values)
                {
                    value.回调_RefreshString();
                }
            }
        }
        public static bool 绘制链接选择面板(string 面板标题, 消息结构 完整消息)
        {
            // 面板当前正在显示中时不可以重复开关面板;未实装的消息Null不可以打开面板
            if (链接选择面板状态 != 0 || 完整消息.type == 消息结构.消息类型.Null)
            {
                return false;
            }

            // 单例.面板标题.text = 面板标题;
            当前消息类型 = 完整消息.type;
            CursorManager.SetCursor(isLocked: false);       // 通知光标管理器不要锁定光标位移
            链接选择面板状态 = InputPanelState.Waiting;      // 请在人物镜头管理器相关函数中插入判断此状态,当状态不为0时,锁定镜头移动
            EventSystem.current.SetSelectedGameObject(单例.面板搜索栏.gameObject);  // 通知Unity引擎监控搜索栏操作

            foreach ((RectTransform 分支, List<ILogicableReference> 分支节点表) __ in 单例.通用渲染分支表.Values)
            {
                设置激活状态(__.分支, false);
            }

            switch (当前消息类型)
            {
                case 消息结构.消息类型.原始试剂渲染分支:
                    单例.更新原始试剂滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.可链接物渲染分支:
                    单例.更新可链接物滚动区域(完整消息.可链接物渲染分支消息.可链接物体表);
                    break;
                case 消息结构.消息类型.无线可链接物渲染分支:
                    单例.更新可链接物滚动区域(完整消息.无线可链接物渲染分支消息.可链接物体表);
                    break;
                case 消息结构.消息类型.基本数学类型渲染分支:
                    单例.更新基本数学类型滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.插槽类型渲染分支:
                    单例.更新插槽类型滚动区域(完整消息.插槽类型渲染分支消息);
                    break;
                case 消息结构.消息类型.插槽编号渲染分支:
                    单例.更新插槽编号滚动区域(完整消息.插槽编号渲染分支消息);
                    break;
                case 消息结构.消息类型.比较类型渲染分支:
                    单例.更新比较类型滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.统计类型渲染分支:
                    单例.更新统计类型滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.试剂模式渲染分支:
                    单例.更新试剂模式滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.逻辑类型渲染分支:
                    单例.更新逻辑类型滚动区域(完整消息.逻辑类型渲染分支消息);
                    break;
                case 消息结构.消息类型.高级数学类型渲染分支:
                    单例.更新高级数学类型滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.逻辑门类型渲染分支:
                    单例.更新逻辑门类型滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.最大最小值类型渲染分支:
                    单例.更新最大最小值类型滚动区域(完整消息);
                    break;
                case 消息结构.消息类型.无线模式渲染分支:
                    单例.更新无线模式滚动区域(完整消息);
                    break;
            }

            单例.回调_搜索栏查询操作(单例.面板搜索栏.text);
            单例.SetVisible(isVisble: true);                    // 激活(显示)面板

            // LayoutRebuilder.ForceRebuildLayoutImmediate(单例.RectTransform);
            return true;
        }
        private List<ILogicableReference> 更新或者构造滚动区域<T1>(消息结构.消息类型 消息类型, Array 参数表, 通用渲染分支表设置回调<T1> 回调, Sprite 缩略图 = null)
        {
            if (通用渲染分支表.TryGetValue(消息类型, out (RectTransform 分支, List<ILogicableReference> 分支节点表) __))
            {
                if (__.分支.parent != 渲染分支父级)
                    __.分支.SetParent(渲染分支父级, false);
                单例.滚动组件.content = __.分支;
                设置激活状态(__.分支, true);

                foreach (var 节点 in __.分支节点表)
                {
                    if (缩略图 != null) { 节点.缩略图.sprite = 缩略图; }
                    设置激活状态(节点.transform, true);
                }

                return __.分支节点表;
            }
            else
            {
                var 分支 = UnityEngine.Object.Instantiate(渲染分支预制体).GetComponent<RectTransform>();
                var 分支节点表 = new List<ILogicableReference>();

                foreach (T1 参数 in 参数表)
                {
                    ILogicableReference 节点 = UnityEngine.Object.Instantiate(选项按钮预制体, 分支).GetComponent<ILogicableReference>();
                    节点.GetComponent<Button>().onClick.AddListener(节点.回调_按钮点击);
                    节点.设置(回调(参数), 缩略图);
                    分支节点表.Add(节点);
                }

                通用渲染分支表[消息类型] = (分支, 分支节点表);
                LayoutRebuilder.ForceRebuildLayoutImmediate(分支);
                return 更新或者构造滚动区域<T1>(消息类型, 参数表, 回调, 缩略图);
            }
        }
        private void 更新无线模式滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<ILogicableReference内存结构.无线模式结构.无线模式>
            (
                完整消息.type,
                无线模式表,
                (无线模式) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.无线模式, new ILogicableReference内存结构.无线模式结构(无线模式)),
                缩略图: ((Thing)完整消息.无线模式渲染分支消息._this).Thumbnail
            );
        }
        private void 更新最大最小值类型滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<ILogicableReference内存结构.最大最小值类型结构._ComparisonOperation>
            (
                完整消息.type,
                最大最小值类型表,
                (最大最小值类型) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.最大最小值类型, new ILogicableReference内存结构.最大最小值类型结构(最大最小值类型)),
                 缩略图: ((Thing)完整消息.最大最小值类型渲染分支消息._this).Thumbnail
            );
        }
        private void 更新逻辑门类型滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<GateOperators>
            (
                完整消息.type,
                逻辑门类型表,
                (逻辑门类型) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.逻辑门类型, new ILogicableReference内存结构.逻辑门类型结构(逻辑门类型)),
                 缩略图: ((Thing)完整消息.逻辑门类型渲染分支消息._this).Thumbnail
            );
        }
        private void 更新高级数学类型滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<MathOperatorsUnary>
            (
                完整消息.type,
                高级数学类型表,
                (高级数学类型) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.高级数学类型, new ILogicableReference内存结构.高级数学类型结构(高级数学类型)),
                缩略图: ((Thing)完整消息.高级数学类型渲染分支消息._this).Thumbnail
            );
        }
        private void 更新比较类型滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<ConditionOperation>
            (
                完整消息.type,
                比较类型表,
                (比较类型) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.比较类型, new ILogicableReference内存结构.比较类型结构(比较类型)),
                缩略图: ((Thing)完整消息.比较类型渲染分支消息._this).Thumbnail
            );
        }
        private void 更新试剂模式滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<LogicReagentMode>
            (
                完整消息.type,
                试剂模式表,
                (试剂模式) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.试剂模式, new ILogicableReference内存结构.试剂模式结构(试剂模式)),
                 缩略图: ((Thing)完整消息.试剂模式渲染分支消息.已链接物体).Thumbnail
            );
        }
        private void 更新原始试剂滚动区域(消息结构 完整消息)
        {
            if (原始试剂表 == null) { 原始试剂表 = Reagent.AllReagentsSorted.ToArray(); }
            通用活跃面板节点表 = 更新或者构造滚动区域<Reagent>
            (
                完整消息.type,
                原始试剂表,
                (试剂) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.原始试剂, new ILogicableReference内存结构.原始试剂结构(试剂)),
                 缩略图: ((Thing)完整消息.原始试剂渲染分支消息.已链接物体).Thumbnail
            );
        }
        private void 更新基本数学类型滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<MathOperators>
            (
                完整消息.type,
                基本数学类型表,
                (基本数学类型) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.基本数学类型, new ILogicableReference内存结构.基本数学类型结构(基本数学类型)),
                缩略图: ((Thing)完整消息.基本数学类型渲染分支消息._this).Thumbnail
            );
        }
        private void 更新统计类型滚动区域(消息结构 完整消息)
        {
            通用活跃面板节点表 = 更新或者构造滚动区域<LogicBatchMethod>
            (
                完整消息.type,
                统计类型表,
                (统计类型) => new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.统计类型, new ILogicableReference内存结构.统计类型结构(统计类型)),
                缩略图: ((Thing)完整消息.统计类型渲染分支消息._this).Thumbnail
            );
        }
        public void 更新插槽类型滚动区域(插槽类型渲染分支消息 消息)
        {
            var 只读或只写 = 消息.只读或只写;
            var 插槽编号 = 消息.插槽编号;

            if (已构造插槽类型分支表.TryGetValue((((Thing)消息.已链接物体).PrefabHash, 只读或只写, 插槽编号), out (RectTransform 分支节点, List<ILogicableReference> 面板节点表) __))
            {
                通用渲染分支表[消息结构.消息类型.插槽类型渲染分支] = __;
                通用活跃面板节点表 = __.面板节点表;
                if (__.分支节点.parent != 渲染分支父级)
                    __.分支节点.SetParent(渲染分支父级, false);
                单例.滚动组件.content = __.分支节点;
                设置激活状态(__.分支节点, true);

                foreach (var 节点 in 通用活跃面板节点表)
                {
                    节点.缩略图.sprite = ((Thing)消息.已链接物体).Thumbnail;
                    设置激活状态(节点.transform, true);
                }
            }
            else
            {
                // 入口类.Log.LogMessage(((Thing)___.参数物体).DisplayName + "\t" + ___.读或写);
                var 分支父级 = UnityEngine.Object.Instantiate(渲染分支预制体);

                var 面板节点表 = new List<ILogicableReference>();
                foreach (var 支持么 in Logicable.LogicSlotTypes)
                {
                    // 当前是逻辑读取器或其他逻辑读取组件,因此获取所有可读逻辑类型,并为所有类型创建按钮,ILogicableReference持有相关引用
                    if (只读或只写 == IOCheck.Readable && 消息.已链接物体.CanLogicRead(支持么, 插槽编号))
                    {
                        ILogicableReference 面板节点;
                        面板节点 = UnityEngine.Object.Instantiate(选项按钮预制体, 分支父级.transform).GetComponent<ILogicableReference>();
                        面板节点.GetComponent<Button>().onClick.AddListener(面板节点.回调_按钮点击);
                        面板节点.设置(new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.插槽类型, new ILogicableReference内存结构.插槽类型结构(支持么)));
                        面板节点表.Add(面板节点);
                    }
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(分支父级.GetComponent<RectTransform>());
                已构造插槽类型分支表.Add((((Thing)消息.已链接物体).PrefabHash, 只读或只写, 插槽编号), (分支父级.GetComponent<RectTransform>(), 面板节点表));
                更新插槽类型滚动区域(消息);
            }
        }
        public void 更新插槽编号滚动区域(插槽编号渲染分支消息 消息)
        {
            var 只读或只写 = 消息.只读或只写;

            if (已构造插槽编号分支表.TryGetValue((((Thing)消息.已链接物体).PrefabHash, 只读或只写), out (RectTransform 分支节点, List<ILogicableReference> 面板节点表) __))
            {
                通用渲染分支表[消息结构.消息类型.插槽编号渲染分支] = __;
                通用活跃面板节点表 = __.面板节点表;
                if (__.分支节点.parent != 渲染分支父级)
                    __.分支节点.SetParent(渲染分支父级, false);
                单例.滚动组件.content = __.分支节点;
                设置激活状态(__.分支节点, true);

                foreach (var 节点 in 通用活跃面板节点表)
                {
                    节点.缩略图.sprite = ((Thing)消息.已链接物体).Thumbnail;
                    设置激活状态(节点.transform, true);
                }
            }
            else
            {
                // 入口类.Log.LogMessage(((Thing)___.参数物体).DisplayName + "\t" + ___.读或写);
                var 分支父级 = UnityEngine.Object.Instantiate(渲染分支预制体);

                var 面板节点表 = new List<ILogicableReference>();
                for (var i = 0; i < ((Thing)消息.已链接物体).Slots.Count; i++)
                {
                    // 当前是逻辑读取器或其他逻辑读取组件,因此获取所有可读逻辑类型,并为所有类型创建按钮,ILogicableReference持有相关引用
                    if (只读或只写 == IOCheck.Readable)
                    {
                        ILogicableReference 面板节点;
                        面板节点 = UnityEngine.Object.Instantiate(选项按钮预制体, 分支父级.transform).GetComponent<ILogicableReference>();
                        面板节点.GetComponent<Button>().onClick.AddListener(面板节点.回调_按钮点击);
                        面板节点.设置(new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.插槽编号, new ILogicableReference内存结构.插槽编号结构(消息.已链接物体, i)));
                        面板节点表.Add(面板节点);
                    }
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(分支父级.GetComponent<RectTransform>());
                已构造插槽编号分支表.Add((((Thing)消息.已链接物体).PrefabHash, 只读或只写), (分支父级.GetComponent<RectTransform>(), 面板节点表));
                更新插槽编号滚动区域(消息);
            }
        }

        public void 更新逻辑类型滚动区域(逻辑类型渲染分支消息 消息)
        {
            var 只读或只写 = 消息.只读或只写;

            if (已构造逻辑类型分支表.TryGetValue((((Thing)消息.已链接物体).PrefabHash, 只读或只写), out (RectTransform 分支节点, List<ILogicableReference> 面板节点表) __))
            {
                通用渲染分支表[消息结构.消息类型.逻辑类型渲染分支] = __;
                通用活跃面板节点表 = __.面板节点表;
                if (__.分支节点.parent != 渲染分支父级)
                    __.分支节点.SetParent(渲染分支父级, false);
                单例.滚动组件.content = __.分支节点;
                设置激活状态(__.分支节点, true);

                foreach (var 节点 in 通用活跃面板节点表)
                {
                    节点.缩略图.sprite = ((Thing)消息.已链接物体).Thumbnail;
                    设置激活状态(节点.transform, true);
                }
            }
            else
            {
                // 入口类.Log.LogMessage(((Thing)___.参数物体).DisplayName + "\t" + ___.读或写);
                var 分支父级 = UnityEngine.Object.Instantiate(渲染分支预制体);

                var 面板节点表 = new List<ILogicableReference>();
                foreach (var 支持么 in Logicable.LogicTypes)
                {
                    // 当前是逻辑读取器或其他逻辑读取组件,因此获取所有可读逻辑类型,并为所有类型创建按钮,ILogicableReference持有相关引用
                    if (只读或只写 == IOCheck.Readable && 消息.已链接物体.CanLogicRead(支持么))
                    {
                        ILogicableReference 面板节点;
                        面板节点 = UnityEngine.Object.Instantiate(选项按钮预制体, 分支父级.transform).GetComponent<ILogicableReference>();
                        面板节点.GetComponent<Button>().onClick.AddListener(面板节点.回调_按钮点击);
                        面板节点.设置(new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.逻辑类型, new ILogicableReference内存结构.逻辑类型结构(支持么)));
                        面板节点表.Add(面板节点);
                    }
                    // 当前是逻辑写入器或其他逻辑写入组件,因此获取所有写入逻辑类型,并为所有类型创建按钮,ILogicableReference持有相关引用
                    else if (只读或只写 == IOCheck.Writable && 消息.已链接物体.CanLogicWrite(支持么))
                    {
                        ILogicableReference 面板节点;
                        面板节点 = UnityEngine.Object.Instantiate(选项按钮预制体, 分支父级.transform).GetComponent<ILogicableReference>();
                        面板节点.GetComponent<Button>().onClick.AddListener(面板节点.回调_按钮点击);
                        面板节点.设置(new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.逻辑类型, new ILogicableReference内存结构.逻辑类型结构(支持么)));
                        面板节点表.Add(面板节点);
                    }
                }

                LayoutRebuilder.ForceRebuildLayoutImmediate(分支父级.GetComponent<RectTransform>());
                已构造逻辑类型分支表.Add((((Thing)消息.已链接物体).PrefabHash, 只读或只写), (分支父级.GetComponent<RectTransform>(), 面板节点表));
                更新逻辑类型滚动区域(消息);
            }
        }
        public void 回调_搜索栏查询操作(string str)
        {
            // 统一转换为小写并去除前后空格
            string 条件 = str?.Trim().ToLower() ?? "";

            switch (当前消息类型)
            {
                case 消息结构.消息类型.可链接物渲染分支:
                    foreach (var 节点 in 活跃面板节点表.Values)
                    {
                        string 节点名称 = 节点.显示名称?.Trim().ToLower() ?? "";
                        bool 是否显示 = string.IsNullOrEmpty(条件) || 节点名称.Contains(条件);
                        节点.SetVisible(isVisble: 是否显示);
                    }
                    break;
                default:
                    foreach (var 节点 in 通用活跃面板节点表)
                    {
                        string 节点名称 = 节点.描述.text.Trim().ToLower() ?? "";
                        bool 是否显示 = string.IsNullOrEmpty(条件) || 节点名称.Contains(条件);
                        节点.SetVisible(isVisble: 是否显示);
                    }
                    break;
            }

            SetInputKeyState(!string.IsNullOrWhiteSpace(面板搜索栏.text));
        }
        public void 更新可链接物滚动区域(IEnumerable<ILogicable> 已发现表)
        {
            单例.通用渲染分支表[消息结构.消息类型.可链接物渲染分支] = (单例.可链接物渲染分支, null);
            单例.滚动组件.content = 单例.可链接物渲染分支;
            设置激活状态(单例.可链接物渲染分支, true);

            // 根据数据网节点物体数量实时更新面板内容
            // 注: 字典特别适合比较操作

            已发现节点表.Clear();
            foreach (var d in 已发现表)
            {
                已发现节点表.Add(((Thing)d).ReferenceId, d);
            }

            foreach (var 节点 in 已发现节点表)
            {
                if (!已显示节点表.ContainsKey(节点.Key))
                {
                    已显示节点表.Add(节点.Key, 节点.Value);
                    构造或唤醒面板节点(节点.Key, 节点.Value);
                }
            }

            已失效节点表.Clear();

            // 哈希表算法会创建很多占位用的数组格子,不可以用 已显示节点表[下标++] 来访问
            foreach (var 节点 in 已显示节点表)
            {
                if (!已发现节点表.ContainsKey(节点.Key))
                { 已失效节点表[节点.Key] = 节点.Value; }
            }

            foreach (var 节点 in 已失效节点表)
            {
                已显示节点表.Remove(节点.Key);
                休眠面板节点(节点.Key);
            }

            foreach (var 节点 in 活跃面板节点表.Values)
            {
                设置激活状态(节点.transform, true);
            }

            回调_语言设置变更();                // 立刻刷新一次显示文本
        }
        private void 构造或唤醒面板节点(long id, ILogicable device)
        {
            // 不同的物体也就是缩略图引用,物体引用和物体描述不同,改的东西不多,可以复用
            // 优先从隐藏池中获取物体信息按钮

            ILogicableReference 面板节点;

            if (休眠面板节点表.Count > 0)
            {
                面板节点 = 休眠面板节点表.Dequeue();
            }
            else
            {
                面板节点 = UnityEngine.Object.Instantiate(选项按钮预制体, 可链接物渲染分支).GetComponent<ILogicableReference>();
                面板节点.GetComponent<Button>().onClick.AddListener(面板节点.回调_按钮点击);
            }

            面板节点.设置(new ILogicableReference内存结构.内存结构(ILogicableReference内存结构.内存结构.结构类型.原始物体, new ILogicableReference内存结构.原始物体结构(device)));
            活跃面板节点表.Add(id, 面板节点);
            设置激活状态(面板节点.transform, true);
        }
        private void 休眠面板节点(long id)
        {
            // 将不使用的物体信息按钮存放到隐藏池中
            if (!活跃面板节点表.TryGetValue(id, out var 面板节点)) { return; }
            设置激活状态(面板节点.transform, false);
            活跃面板节点表.Remove(id);
            // 面板节点.设置目标(null, Recipe.INVALID);
            休眠面板节点表.Enqueue(面板节点);
        }
        public static void CancelInput()
        {
            // 回调函数,当点击了关闭面板按钮时调用,然后重置面板为初始状态,并关闭面板
            // 开启或者关闭鼠标控制屏幕光标移动
            CursorManager.SetCursor(isLocked: true);
            // 请在人物移动组件代码中加入此面板状态判断,在面板工作时屏蔽掉鼠标移动导致的人物视线乱晃
            链接选择面板状态 = InputPanelState.Cancelled;

            单例.SetVisible(isVisble: false);
            单例.面板搜索栏.text = string.Empty;

            当前焦点 = null;
            按钮点击事件 = null;

            链接选择面板状态 = InputPanelState.None;
        }

        public static void Submit()
        {
            // 回调函数,当点击了物体信息按钮时调用,将该物体引用赋给螺丝
            // 然后重置面板为初始状态,并关闭面板
            if (按钮点击事件 != null)
            {
                // 请在调用 绘制链接选择面板 返回true时,对此事件写入回调
                按钮点击事件(当前焦点);
            }

            CursorManager.SetCursor(isLocked: true);
            链接选择面板状态 = InputPanelState.Submitted;

            单例.SetVisible(isVisble: false);
            单例.面板搜索栏.text = string.Empty;

            当前焦点 = null;
            按钮点击事件 = null;

            链接选择面板状态 = InputPanelState.None;
        }
    }

    [HarmonyPatch(typeof(InputWindowBase), nameof(InputWindowBase.IsInputWindow), MethodType.Getter)]
    public class InputWindowBase_IsInputWindow_PrefixPatch
    {
        [HarmonyPrefix]
        public static bool IsInputWindow(ref bool __result)
        {
            // 增加链接选择面板的状态判断,当返回true时,鼠标位移不会导致人物视线位移
            __result = 链接选择面板.链接选择面板状态 == InputPanelState.Waiting || InputSourceCode.InputState == InputPanelState.Waiting || InputWindow.InputState == InputPanelState.Waiting || InputPrefabs.InputState == InputPanelState.Waiting;
            return false;
        }
    }

    [HarmonyPatch(typeof(InputWindowBase), nameof(InputWindowBase.Cancel))]
    public class InputWindowBase_Cancel_PrefixPatch
    {
        [HarmonyPrefix]
        public static bool Cancel()
        {
            // 当游戏处于暂停状态时,不要显示链接选择面板
            if (KeyManager.InputState == KeyInputState.Paused && InputSourceCode.InputState == InputPanelState.Waiting)
            {
                return false;
            }

            InputWindow.CancelInput();
            InputSourceCode.CancelInput();
            InputPrefabs.CancelInput();
            链接选择面板.CancelInput();

            return false;
        }
    }
}