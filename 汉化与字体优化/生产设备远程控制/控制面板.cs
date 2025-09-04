using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Inventory;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using meanran_xuexi_mods_xiaoyouhua;
using UnityEngine;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class 控制面板 : MonoBehaviour
    {
        public static 控制面板 单例;
        public static Dictionary<long, SimpleFabricatorBase> 生产设备表 = new();
        int m_主窗口ID;
        bool m_主窗口开关;
        Rect m_主窗口区域 = new Rect(0f, 0f, 300f, 300f);
        Vector2 当前滚动区左上角 = Vector2.zero;
        int 样式加载了么 = 0;
        public static void 构造()
        {
            if (单例)
                单例.销毁();

            单例 = Utils.构造节点<控制面板>("控制面板");
            单例.初始化();
            入口类.Log.LogMessage($"成功构造控制面板");
        }
        private void 销毁()
        {
            Utils.销毁节点(单例);
            单例 = null;
            入口类.Log.LogMessage($"成功销毁控制面板");
        }
        private void 初始化()
        {
            // KeyWrap的构造函数中自动将引用添加到游戏的事件轮询表中
            var keyWrap = new InputSystem.KeyWrap(KeyCode.Home, null);
            keyWrap.KeyUp += () => m_主窗口开关 = !m_主窗口开关; ;

            m_主窗口ID = GetInstanceID();
            Utils.唤醒节点(this);
        }

        private void OnGUI()
        {
            if (!m_主窗口开关) { return; }

            if (样式加载了么 == 0)
            {
                样式加载了么 = 1;
                // 添加自定义样式组, 滚动条使用样式组(样式组命名规范请自行查询)
                var __ = new List<GUIStyle>{
            UI样式管理器.水平滚动条样式,
            UI样式管理器.水平滚动条样式Leftbutton,
            UI样式管理器.水平滚动条样式Rightbutton,
            UI样式管理器.水平滚动条样式thumb,
            UI样式管理器.垂直滚动条样式,
            UI样式管理器.垂直滚动条样式upbutton,
            UI样式管理器.垂直滚动条样式downbutton,
            UI样式管理器.垂直滚动条样式thumb};

                var current = (GUISkin)Traverse.Create(typeof(GUISkin)).Field("current").GetValue();
                __.AddRange(current.customStyles);
                current.customStyles = __.ToArray();

                入口类.Log.LogMessage($"成功添加自定义滚动样式");
            }

            m_主窗口区域 = GUILayout.Window(m_主窗口ID, m_主窗口区域, 主窗口内容, "", UI样式管理器.窗口样式);
        }

        private void 主窗口内容(int windowID_)
        {
            // GUI所有控件函数都是状态机代码, 根据阶段跳转到对应段落;  GUI布局系统使用两两相对坐标系, 下级位置 = 上级位置 + 相对偏移
            // GUILayout的所有函数都不是直接绘制, 而是按照调用顺序(自增控件ID)生成 <位置/尺寸/内容/UI状态/UI事件处理函数回调/样式> UI布局树
            // 刷新布局阶段 => 执行GUILayout.Window, 按照调用顺序生成UI布局树, 然后UI布局树从尾节点开始读取样式配置并结合UI控件内容计算UI控件尺寸并向上递归, 
            //                然后遇到垂直布局或者水平布局, 就按 当前元素位置 = 上一个元素位置 + 上一个元素尺寸 + 样式元素间距 计算各个子元素位置
            //                垂直布局或者水平布局计算完所有子元素得到区域尺寸后, 读取样式内容区内缩宽度对区域尺寸进行修饰, 得到本级UI尺寸
            // 事件处理阶段 => 执行GUILayout.Window, 按照调用顺序(自增控件ID)将鼠标和键盘消息传入控件函数, 以自增控件ID作为Key修改UI布局树中的指定控件
            // 渲染阶段 => 执行GUILayout.Window, 按照调用顺序(自增控件ID)执行控件渲染代码, 以自增控件ID作为Key从UI布局树中获取渲染位置与尺寸、内容
            // 合成阶段(内部操作) => 根据窗口深度值, 对生成的渲染图进行排序, 然后混叠后输出到屏幕

            // 标题栏
            GUILayout.BeginHorizontal(UI样式管理器.浅背景中对齐样式);
            {
                GUILayout.Label(" 生产设备远程控制", UI样式管理器.无背景中对齐样式);
                if (GUILayout.Button("×", UI样式管理器.按钮样式, GUILayout.Width(20)))
                {
                    m_主窗口开关 = false;
                }
            }
            GUILayout.EndHorizontal();

            // 滚动条的尺寸必须显式在GUIStyle指定
            当前滚动区左上角 = GUILayout.BeginScrollView(当前滚动区左上角, false, true, UI样式管理器.水平滚动条样式, UI样式管理器.垂直滚动条样式, UI样式管理器.浅背景中对齐样式);
            {
                GUILayout.BeginVertical();
                {
                    foreach (var v in 生产设备表.Values)
                    {
                        // Interactable: 控件,注意是控件,而不是物体
                        // Interaction: 交互双方. 玩家/玩家活动手插槽/视线交互焦点所在的控件或物体或另一个玩家
                        // doAction: 玩家当前是在执行动作还是单纯的就看看(拿着工具不操作也算无动作)
                        // InteractWith是交互事件,是对物体内部的某个控件相关状态变量进行修改

                        if (!v.OnOff || !v.Powered)
                        { continue; }

                        GUILayout.Label($"设备名称: {(v.DisplayName.Length > 15 ? v.DisplayName.Substring(0, 15) + "..." : v.DisplayName)}", UI样式管理器.无背景左对齐样式);

                        GUILayout.BeginHorizontal(UI样式管理器.浅背景中对齐样式);
                        {
                            if (GUILayout.Button("打开生产配方", UI样式管理器.按钮样式))
                            {
                                var 控件 = v.Interactables.FirstOrDefault(t => t.Action == InteractableType.Button3);
                                var 交互双方 = new Interaction(InventoryManager.Parent, InventoryManager.ActiveHandSlot, v, KeyManager.GetButton(KeyMap.QuantityModifier));
                                v.InteractWith(控件, 交互双方, true);
                            }
                            if (GUILayout.Button("激活生产", UI样式管理器.按钮样式))
                            {
                                var 控件 = v.Interactables.FirstOrDefault(t => t.Action == InteractableType.Activate);
                                var 交互双方 = new Interaction(InventoryManager.Parent, InventoryManager.ActiveHandSlot, v, KeyManager.GetButton(KeyMap.QuantityModifier));
                                v.InteractWith(控件, 交互双方, true);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndScrollView();

            GUI.DragWindow();     // 此拖动窗口UI消息最后入消息树因此最先处理, 若存在鼠标拖动则以上帧的UI消息树为事件处理基准变更本帧的面板根节点位置
        }
    }
}

public class UI样式管理器
{
    public static GUIStyle m_窗口样式 = null;
    public static GUIStyle 窗口样式
    {
        get
        {
            if (m_窗口样式 == null)
            {
                var 主体颜色 = new Color(0.3f, 0.3f, 0.3f, 0.8f);  // 此临时变量会自动销毁
                var 正常边框颜色 = Color.black;
                var 悬停边框颜色 = Color.clear;
                var 按下边框颜色 = new Color(1f, 1f, 1f, 0.8f);     // 此临时变量会自动销毁
                var 键盘焦点边框颜色 = Color.clear;
                m_窗口样式 = Utils.创建GUIStyle(("窗口样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 4, 排版间距: 2, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                m_窗口样式.alignment = TextAnchor.MiddleCenter;
                m_窗口样式.imagePosition = ImagePosition.ImageOnly;
                // 窗口只有两种状态=>1.正常渲染的Normal贴图 2.点击获取焦点后的onNormal贴图
                m_窗口样式.onNormal.background = m_窗口样式.active.background;    // 资源转移, 不需要释放内存
                m_窗口样式.active.background = null;
                m_窗口样式.onActive.background = null;
            }
            return m_窗口样式;
        }
    }
    public static GUIStyle m_输入框体样式 = null;
    public static GUIStyle 输入框体样式
    {
        get
        {
            if (m_输入框体样式 == null)
            {
                var 主体颜色 = new Color(0.3f, 0.3f, 0.3f, 0.8f);  // 此临时变量会自动销毁
                var 正常边框颜色 = Color.black;
                var 悬停边框颜色 = Color.white;
                var 按下边框颜色 = Color.black;
                var 键盘焦点边框颜色 = Color.white;
                m_输入框体样式 = Utils.创建GUIStyle(("输入框体样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 4, 排版间距: 2, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                m_输入框体样式.alignment = TextAnchor.MiddleLeft;
            }
            return m_输入框体样式;
        }
    }
    public static GUIStyle m_输入框头样式 = null;
    public static GUIStyle 输入框头样式
    {
        get
        {
            if (m_输入框头样式 == null)
            {
                var 主体颜色 = Color.clear;
                var 正常边框颜色 = Color.clear;
                var 悬停边框颜色 = Color.clear;
                var 按下边框颜色 = Color.clear;
                var 键盘焦点边框颜色 = Color.clear;
                m_输入框头样式 = Utils.创建GUIStyle(("输入框头样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 4, 排版间距: 2, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                m_输入框头样式.alignment = TextAnchor.MiddleLeft;
                m_输入框头样式.stretchWidth = false;
            }
            return m_输入框头样式;
        }
    }
    public static GUIStyle m_按钮样式 = null;
    public static GUIStyle 按钮样式
    {
        get
        {
            if (m_按钮样式 == null)
            {
                var 主体颜色 = new Color(0.3f, 0.3f, 0.3f, 0.8f);  // 此临时变量会自动销毁
                var 正常边框颜色 = Color.black;
                var 悬停边框颜色 = Color.white;
                var 按下边框颜色 = Color.black;
                var 键盘焦点边框颜色 = Color.white;
                m_按钮样式 = Utils.创建GUIStyle(("按钮样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 4, 排版间距: 2, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                m_按钮样式.alignment = TextAnchor.MiddleCenter;
            }
            return m_按钮样式;
        }
    }
    public static GUIStyle m_垂直滚动条样式 = null;
    public static GUIStyle 垂直滚动条样式
    {
        get
        {
            {
                if (m_垂直滚动条样式 == null)
                {
                    var 主体颜色 = new Color(0.4f, 0.4f, 0.4f, 0.8f);  // 此临时变量会自动销毁
                    var 正常边框颜色 = Color.black;
                    var 悬停边框颜色 = Color.clear;
                    var 按下边框颜色 = Color.clear;
                    var 键盘焦点边框颜色 = Color.clear;
                    m_垂直滚动条样式 = Utils.创建GUIStyle(("垂直滚动条样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 1, 排版间距: 0, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                    m_垂直滚动条样式.imagePosition = ImagePosition.ImageOnly;
                    m_垂直滚动条样式.stretchHeight = false;
                    m_垂直滚动条样式.stretchWidth = true;
                    m_垂直滚动条样式.fixedHeight = 0;
                    m_垂直滚动条样式.fixedWidth = 15f;
                }
                return m_垂直滚动条样式;
            }
        }
    }
    public static GUIStyle m_垂直滚动条样式thumb = null;
    public static GUIStyle 垂直滚动条样式thumb
    {
        get
        {
            {
                if (m_垂直滚动条样式thumb == null)
                {
                    var 主体颜色 = new Color(0.2f, 0.2f, 0.2f, 0.8f);  // 此临时变量会自动销毁
                    var 正常边框颜色 = Color.black;
                    var 悬停边框颜色 = Color.clear;
                    var 按下边框颜色 = Color.clear;
                    var 键盘焦点边框颜色 = Color.clear;
                    m_垂直滚动条样式thumb = Utils.创建GUIStyle(("垂直滚动条样式thumb", 宽: 64, 高: 64, 边框宽度: 0, 内容区内缩宽度: 0, 排版间距: 0, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                    m_垂直滚动条样式thumb.imagePosition = ImagePosition.ImageOnly;
                    m_垂直滚动条样式thumb.stretchHeight = true;
                    m_垂直滚动条样式thumb.stretchWidth = false;
                    m_垂直滚动条样式thumb.fixedHeight = 0;
                    m_垂直滚动条样式thumb.fixedWidth = 13f;
                }
                return m_垂直滚动条样式thumb;
            }
        }
    }
    public static GUIStyle m_垂直滚动条样式upbutton = null;
    public static GUIStyle 垂直滚动条样式upbutton
    {
        get
        {
            {
                if (m_垂直滚动条样式upbutton == null)
                {
                    m_垂直滚动条样式upbutton = new GUIStyle(垂直滚动条样式);
                    m_垂直滚动条样式upbutton.name = "垂直滚动条样式upbutton";
                    m_垂直滚动条样式upbutton.stretchHeight = false;
                    m_垂直滚动条样式upbutton.stretchWidth = true;
                    m_垂直滚动条样式upbutton.fixedHeight = 0;
                    m_垂直滚动条样式upbutton.fixedWidth = 0;
                }
                return m_垂直滚动条样式upbutton;
            }
        }
    }
    public static GUIStyle m_垂直滚动条样式downbutton = null;
    public static GUIStyle 垂直滚动条样式downbutton
    {
        get
        {
            {
                if (m_垂直滚动条样式downbutton == null)
                {
                    m_垂直滚动条样式downbutton = new GUIStyle(垂直滚动条样式);
                    m_垂直滚动条样式downbutton.name = "垂直滚动条样式downbutton";
                    m_垂直滚动条样式downbutton.stretchHeight = false;
                    m_垂直滚动条样式downbutton.stretchWidth = true;
                    m_垂直滚动条样式downbutton.fixedHeight = 0;
                    m_垂直滚动条样式downbutton.fixedWidth = 0;
                }
                return m_垂直滚动条样式downbutton;
            }
        }
    }


    public static GUIStyle m_水平滚动条样式 = null;
    public static GUIStyle 水平滚动条样式
    {
        get
        {
            {
                // 在Unity的GUISkin中, 滚动条通常由以下样式组成, 只需传入XX, Unity自动获取XX.name+预设字符拼接出其它样式名并查找
                // XX: 水平滚动条; XXThumb: 水平滚动条的滑块; XXLeftButton: 水平滚动条左按钮; XXRightButton: 水平滚动条右按钮
                // XX 垂直滚动条; XXThumb: 垂直滚动条的滑块; XXUpButton: 垂直滚动条上按钮; XXDownButton: 垂直滚动条下按钮

                if (m_水平滚动条样式 == null)
                {
                    var 主体颜色 = new Color(0.4f, 0.4f, 0.4f, 0.8f);  // 此临时变量会自动销毁
                    var 正常边框颜色 = Color.black;
                    var 悬停边框颜色 = Color.clear;
                    var 按下边框颜色 = Color.clear;
                    var 键盘焦点边框颜色 = Color.clear;
                    m_水平滚动条样式 = Utils.创建GUIStyle(("水平滚动条样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 1, 排版间距: 0, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                    m_水平滚动条样式.imagePosition = ImagePosition.ImageOnly;
                    m_水平滚动条样式.stretchHeight = false;
                    m_水平滚动条样式.stretchWidth = true;
                    m_水平滚动条样式.fixedHeight = 15f;
                    m_水平滚动条样式.fixedWidth = 0;
                }
                return m_水平滚动条样式;
            }
        }
    }
    public static GUIStyle m_水平滚动条样式thumb = null;
    public static GUIStyle 水平滚动条样式thumb
    {
        get
        {
            {
                if (m_水平滚动条样式thumb == null)
                {
                    var 主体颜色 = new Color(0.2f, 0.2f, 0.2f, 0.8f);  // 此临时变量会自动销毁
                    var 正常边框颜色 = Color.black;
                    var 悬停边框颜色 = Color.clear;
                    var 按下边框颜色 = Color.clear;
                    var 键盘焦点边框颜色 = Color.clear;
                    m_水平滚动条样式thumb = Utils.创建GUIStyle(("水平滚动条样式thumb", 宽: 64, 高: 64, 边框宽度: 0, 内容区内缩宽度: 0, 排版间距: 0, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                    m_水平滚动条样式thumb.imagePosition = ImagePosition.ImageOnly;
                    m_水平滚动条样式thumb.stretchHeight = false;
                    m_水平滚动条样式thumb.stretchWidth = true;
                    m_水平滚动条样式thumb.fixedHeight = 13f;
                    m_水平滚动条样式thumb.fixedWidth = 0;
                }
                return m_水平滚动条样式thumb;
            }
        }
    }
    public static GUIStyle m_水平滚动条样式Leftbutton = null;
    public static GUIStyle 水平滚动条样式Leftbutton
    {
        get
        {
            {
                if (m_水平滚动条样式Leftbutton == null)
                {
                    m_水平滚动条样式Leftbutton = new GUIStyle(水平滚动条样式);
                    m_水平滚动条样式Leftbutton.name = "水平滚动条样式Leftbutton";
                    m_水平滚动条样式Leftbutton.stretchHeight = false;
                    m_水平滚动条样式Leftbutton.stretchWidth = true;
                    m_水平滚动条样式Leftbutton.fixedHeight = 0;
                    m_水平滚动条样式Leftbutton.fixedWidth = 0;
                }
                return m_水平滚动条样式Leftbutton;
            }
        }
    }
    public static GUIStyle m_水平滚动条样式Rightbutton = null;
    public static GUIStyle 水平滚动条样式Rightbutton
    {
        get
        {
            {
                if (m_水平滚动条样式Rightbutton == null)
                {
                    m_水平滚动条样式Rightbutton = new GUIStyle(水平滚动条样式);
                    m_水平滚动条样式Rightbutton.name = "水平滚动条样式Rightbutton";
                    m_水平滚动条样式Rightbutton.stretchHeight = false;
                    m_水平滚动条样式Rightbutton.stretchWidth = true;
                    m_水平滚动条样式Rightbutton.fixedHeight = 0;
                    m_水平滚动条样式Rightbutton.fixedWidth = 0;
                }
                return m_水平滚动条样式Rightbutton;
            }
        }
    }
    public static GUIStyle m_浅背景中对齐样式 = null;
    public static GUIStyle 浅背景中对齐样式
    {
        get
        {
            if (m_浅背景中对齐样式 == null)
            {
                var 主体颜色 = new Color(0.4f, 0.4f, 0.4f, 0.8f);  // 此临时变量会自动销毁
                var 正常边框颜色 = Color.black;
                var 悬停边框颜色 = Color.clear;
                var 按下边框颜色 = Color.clear;
                var 键盘焦点边框颜色 = Color.clear;
                m_浅背景中对齐样式 = Utils.创建GUIStyle(("浅背景样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 4, 排版间距: 2, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                m_浅背景中对齐样式.alignment = TextAnchor.MiddleCenter;
            }
            return m_浅背景中对齐样式;
        }
    }
    public static GUIStyle m_无背景左对齐样式 = null;
    public static GUIStyle 无背景左对齐样式
    {
        get
        {
            if (m_无背景左对齐样式 == null)
            {
                var 主体颜色 = Color.clear;
                var 正常边框颜色 = Color.clear;
                var 悬停边框颜色 = Color.clear;
                var 按下边框颜色 = Color.clear;
                var 键盘焦点边框颜色 = Color.clear;
                m_无背景左对齐样式 = Utils.创建GUIStyle(("无背景样式", 宽: 64, 高: 64, 边框宽度: 1, 内容区内缩宽度: 4, 排版间距: 2, 主体颜色: 主体颜色, 正常边框颜色, 悬停边框颜色, 按下边框颜色, 键盘焦点边框颜色));
                m_无背景左对齐样式.alignment = TextAnchor.MiddleLeft;
            }
            return m_无背景左对齐样式;
        }
    }
    public static GUIStyle m_无背景中对齐样式 = null;
    public static GUIStyle 无背景中对齐样式
    {
        get
        {
            if (m_无背景中对齐样式 == null)
            {
                m_无背景中对齐样式 = new(无背景左对齐样式);
                m_无背景中对齐样式.alignment = TextAnchor.MiddleCenter;
            }
            return m_无背景中对齐样式;
        }
    }
    public static GUIStyle m_无背景右对齐样式 = null;
    public static GUIStyle 无背景右对齐样式
    {
        get
        {
            if (m_无背景右对齐样式 == null)
            {
                m_无背景右对齐样式 = new(无背景左对齐样式);
                m_无背景右对齐样式.alignment = TextAnchor.MiddleRight;
            }
            return m_无背景右对齐样式;
        }
    }
}






