using Assets.Scripts.Objects;
using Assets.Scripts.UI;
using HarmonyLib;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Assets.Scripts.Objects.Motherboards;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(Prefab), nameof(Prefab.LoadAll))]
    public class Prefab_LoadAll_Patch
    {
        public static void Postfix()
        {
            if (AssetsLoad.单例.内置TMP字体 && AssetsLoad.Font字体)
            {
                var 所有资源 = AssetsLoad.单例.所有资源;

                // using (StreamWriter 写 = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/所有资源.txt", append: false))
                // {
                //     int 计数 = 0;
                //     foreach (var Object in 所有资源)
                //     {
                //         try { 写.WriteLine($"序列[{++计数}] = ({Object.GetType()}) {Object.name}"); }
                //         catch (Exception e)
                //         {
                //             入口点类.Log.LogError($"错误序列:{计数}");
                //             入口点类.Log.LogError($"错误对象:{e.Source}");
                //             入口点类.Log.LogError($"错误类型:{e.Message}");
                //             入口点类.Log.LogError($"错误信息:{e.StackTrace}\n");
                //         }
                //     }
                // }

                var 节点 = Traverse.Create(Prefab.PrefabsGameObject).GetValue() as GameObject;
                // File.AppendAllText($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/3.txt", 层级.打印层级信息(节点));

                修改气体显示UI(节点);
                入口类.Log.LogInfo($"成功修改克隆母版=>气体显示");
                // 修改哈希显示UI(节点);
                // 入口类.Log.LogInfo($"成功修改克隆母版=>哈希显示");
                修改基因分析UI(节点);
                入口类.Log.LogInfo($"成功修改克隆母版=>基因分析");

                foreach (var Object in 所有资源)
                {
                    var type = Object.GetType();
                    if (type == typeof(TMP_Text))
                    {
                        var 文1 = Object as TMP_Text;
                        文1.关闭遮罩();
                        if (文1.transform.parent.name == "SPDACategory")
                        {
                            文1.修改大小与遮罩(20);
                            入口类.Log.LogInfo($"成功修改克隆母版=>SPDACategory");
                        }
                    }
                    else if (type == typeof(Text))
                    {
                        var 文2 = Object as Text;
                        文2.font = AssetsLoad.Font字体;
                        文2.关闭遮罩();
                        if (文2.transform.parent.name == "ButtonDevice")
                        {
                            文2.修改大小与遮罩(55);
                            入口类.Log.LogInfo($"成功修改克隆母版=>ButtonDevice");
                        }
                    }
                    else if (type == typeof(GameObject))
                    {
                        var obj = Object as GameObject;
                        switch (obj.name)
                        {
                            case "FilterItem":
                                {
                                    foreach (var 文 in obj.GetComponentsInChildren<Text>(includeInactive: true))
                                    { 文.font = AssetsLoad.Font字体; 文.color = Color.black; 文.修改大小与遮罩(15); }
                                    入口类.Log.LogInfo($"成功修改克隆母版=>FilterItem");
                                    break;
                                }
                            case "SorterItem":
                                {
                                    var 文 = obj.transform.GetChild(1).GetChild(0).GetComponent<Text>();
                                    文.修改大小与遮罩(28); 文.text = "分拣物品名单";
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SorterItem");
                                    break;
                                }
                            case "SPDAManufacturedBy":
                                {
                                    var 内容区 = obj.transform.GetChild(1);
                                    var DeviceName = 内容区.GetChild(0).GetComponent<TMP_Text>();
                                    DeviceName.修改大小与遮罩(20);
                                    var TextDetails = 内容区.GetChild(1).GetChild(0).GetComponent<TMP_Text>();
                                    TextDetails.修改大小与遮罩(20);
                                    var 配方抬头 = 内容区.GetChild(2).GetComponent<TMP_Text>();
                                    配方抬头.修改大小与遮罩(20);
                                    var 配方文本 = 内容区.GetChild(3).GetComponent<TMP_Text>();
                                    配方文本.修改大小与遮罩(20);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SPDAManufacturedBy");
                                    break;
                                }
                            case "SPDALogic":
                                {
                                    var 模式 = obj.transform.GetChild(0).GetComponent<TMP_Text>();
                                    模式.修改大小与遮罩(20);
                                    var 操作码 = obj.transform.GetChild(1).GetComponent<TMP_Text>();
                                    操作码.修改大小与遮罩(20);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SPDALogic");
                                    break;
                                }
                            case "SPDAVersion":
                                {
                                    obj.AddComponent<SPDAVersion_延时修改>();
                                    var 背景 = obj.GetComponent<HorizontalLayoutGroup>();
                                    背景.childForceExpandHeight = true;
                                    var 背景Rect = 背景.GetComponent<RectTransform>();
                                    背景Rect.sizeDelta = new Vector2(背景Rect.rect.width, 背景Rect.rect.height + 20);
                                    foreach (var 文本工具 in obj.GetComponentsInChildren<TMP_Text>(includeInactive: true))
                                        文本工具.修改大小与遮罩(20);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SPDAVersion");
                                    break;
                                }
                            case "SPDAGeneric":
                                {
                                    var 内容区 = obj.transform.GetChild(1);
                                    var Header = 内容区.GetChild(0).GetComponent<TMP_Text>();
                                    Header.修改大小与遮罩(24);
                                    var Text = 内容区.GetChild(1).GetComponent<TMP_Text>();
                                    Text.修改大小与遮罩(20);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SPDAGeneric");
                                    break;
                                }
                            case "SPDAFoundIn":
                                {
                                    var InfoValue = obj.transform.GetChild(0).GetComponent<TMP_Text>();
                                    InfoValue.修改大小与遮罩(20);
                                    var InfoTitle = obj.transform.GetChild(1).GetComponent<TMP_Text>();
                                    InfoTitle.修改大小与遮罩(20);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SPDAFoundIn");
                                    break;
                                }
                            case "PanelInWorldToolTip":
                                {
                                    foreach (var 文本工具 in obj.GetComponentsInChildren<TMP_Text>(includeInactive: true))
                                        文本工具.修改大小与遮罩(32);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>PanelInWorldToolTip");
                                    break;
                                }
                            case "PanelClothing":
                                {
                                    foreach (var 文本工具 in obj.GetComponentsInChildren<TMP_Text>(includeInactive: true))
                                        文本工具.修改大小与遮罩(24);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>PanelClothing");
                                    break;
                                }
                            case "StatusIcons":
                                {
                                    foreach (var 文本工具 in obj.GetComponentsInChildren<TMP_Text>(includeInactive: true))
                                        文本工具.修改大小与遮罩(24);
                                    入口类.Log.LogInfo($"成功修改克隆母版=>StatusIcons");
                                    break;
                                }
                            case "SPDAListItem":
                                {
                                    obj.AddComponent<SPDAListItem_延时修改>();
                                    入口类.Log.LogInfo($"成功修改克隆母版=>SPDAListItem");
                                    break;
                                }
                            case "MotherboardComms":
                                if (obj.GetComponent<CommsMotherboard>())
                                {
                                    var 通信主板标题 = obj.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>(); ;
                                    通信主板标题.font = AssetsLoad.单例.内置TMP字体;
                                    通信主板标题.text = "通信终端";
                                    入口类.Log.LogInfo($"成功修改克隆母版=>MotherboardComms");
                                }
                                break;
                            case "ContactItemNew":
                                {
                                    var 商人名称 = obj.transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
                                    商人名称.GetComponent<LayoutElement>().preferredWidth = 20000;
                                    // 商人名称.enableAutoSizing = true;
                                    商人名称.font = AssetsLoad.单例.内置TMP字体;

                                    var 通讯信息 = obj.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                                    通讯信息.font = AssetsLoad.单例.内置TMP字体;
                                    通讯信息.enableWordWrapping = false;

                                    入口类.Log.LogInfo($"成功修改克隆母版=>ContactItemNew");
                                    break;
                                }
                            case "ContactsTab":
                                {
                                    var 着陆平台 = obj.transform.GetChild(0).GetComponent<TMP_Text>();
                                    着陆平台.font = AssetsLoad.单例.内置TMP字体;
                                    var 天线名称 = obj.transform.GetChild(1).GetComponent<TMP_Text>();
                                    天线名称.font = AssetsLoad.单例.内置TMP字体;
                                    var 商人离开与进入日志 = obj.transform.parent.GetChild(3).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
                                    商人离开与进入日志.font = AssetsLoad.单例.内置TMP字体;
                                    break;
                                }
                            case "PrefabReference":
                                {
                                    var 生产菜单项目 = obj.transform.GetChild(0).GetComponent<TMP_Text>();
                                    生产菜单项目.font = AssetsLoad.单例.内置TMP字体;
                                    生产菜单项目.修改大小与遮罩(28);

                                    链接选择面板.选项按钮预制体 = UnityEngine.Object.Instantiate(obj);
                                    链接选择面板.选项按钮预制体.SetActive(false);
                                    var ILogicableReference = 链接选择面板.选项按钮预制体.AddComponent<ILogicableReference>();
                                    链接选择面板.选项按钮预制体.name = "数据网节点UI单元";
                                    var _ = 链接选择面板.选项按钮预制体.GetComponent<PrefabReference>();
                                    ILogicableReference.缩略图 = _.Thumbnail;
                                    ILogicableReference.描述 = _.Text;
                                    ILogicableReference.UiComponentRenderer = _.UiComponentRenderer;
                                    ILogicableReference.GameObject = _.GameObject;
                                    ILogicableReference.Transform = _.Transform;
                                    UnityEngine.Object.Destroy(_);
                                    入口类.Log.LogInfo($"成功增加数据网节点UI单元=>ILogicableReference");
                                    break;
                                }
                            case "PanelInputPrefabs":
                                {
                                    var __ = UnityEngine.Object.Instantiate(obj);
                                    __.gameObject.name = "链接选择面板";
                                    var _ = __.GetComponent<InputPrefabs>();
                                    链接选择面板.单例 = __.gameObject.AddComponent<链接选择面板>();
                                    链接选择面板.单例.面板标题 = _.TitleText;
                                    链接选择面板.单例.可链接物渲染分支 = _.GroupParents;
                                    链接选择面板.单例.面板搜索栏 = _.SearchBar;
                                    链接选择面板.单例.UiComponentRenderer = _.UiComponentRenderer;
                                    链接选择面板.单例.GameObject = _.GameObject;
                                    链接选择面板.单例.Transform = _.Transform;

                                    Utils.销毁子级节点(__.transform.GetChild(0).GetChild(7).GetChild(1).GetChild(0));
                                    UnityEngine.Object.Destroy(_);
                                    链接选择面板.单例.Initialize();
                                    // Prefab.OnPrefabsLoaded += 单例.初始化;
                                    // 注:独立面板挂在任意一个有画布组件的容器中都可
                                    链接选择面板.父级画布 = obj.transform.parent.gameObject;
                                    链接选择面板.单例.transform.SetParent(链接选择面板.父级画布.transform, false); ;

                                    入口类.Log.LogInfo($"成功增加数据网节点UI面板=>链接选择面板");
                                    break;
                                }
                        }
                    }
                    else if (type == typeof(UniversalPage))
                    {
                        var 通用页面 = Object as UniversalPage;
                        通用页面.gameObject.AddComponent<UniversalPage_延时修改>();
                        入口类.Log.LogInfo($"成功修改通用页面=>UniversalPage");
                    }
                }
            }
        }
        public static void 修改气体显示UI(GameObject 父节点)
        {
            if (父节点 == null)
            { return; }

            var 气体显示 = 父节点.transform.Find("CircuitboardGasDisplay");
            var 工作界面 = 气体显示.Find("PanelNormal");
            {
                var com = 工作界面.Find("DisplayName");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.white;
            }
            {
                var com = 工作界面.Find("Title");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.white;
            }
            {
                var com = 工作界面.Find("Text");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.white;
                文本工具.fontSize = 文本工具.fontSize - 4;
            }
            {
                var com = 工作界面.Find("Unit");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.white;
                文本工具.fontSize = 文本工具.fontSize + 4;
            }

            var 配置界面 = 气体显示.Find("PanelConfig");
            {
                var com = 配置界面.Find("Title");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.green;
            }
            {
                var com = 配置界面.Find("ButtonMode/Text");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.black;
                文本工具.修改大小与遮罩(65);
            }
        }

        public static void 修改哈希显示UI(GameObject 父节点)
        {
            if (父节点 == null)
            { return; }

            var 哈希显示 = 父节点.transform.Find("CircuitboardHashDisplay");
            var 工作界面 = 哈希显示.Find("PanelNormal");
            {
                var com = 工作界面.Find("DisplayName");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.white;
                文本工具.fontSize = 文本工具.fontSize - 4;
            }
            {
                var com = 工作界面.Find("Title");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.white;
                文本工具.修改大小与遮罩(50);
            }
            var 配置界面 = 哈希显示.Find("PanelConfig");
            {
                var com = 配置界面.Find("Title");
                var 文本工具 = com.GetComponent<Text>();
                文本工具.color = Color.green;
            }
        }

        public static void 修改基因分析UI(GameObject 父节点)
        {
            if (父节点 == null)
            { return; }

            var 基因分析 = 父节点.transform.Find("CartridgePlantAnalyser");
            foreach (var 文本 in 基因分析.GetComponentsInChildren<TMP_Text>(includeInactive: true))
            {
                bool 修改么 = false;
                var __ = 文本.text.词条匹配();
                if (__ != 文本.text)
                {
                    文本.text = __;
                    修改么 = true;
                }

                if (修改么)
                    文本.font = AssetsLoad.单例.内置TMP字体;
            }
        }

    }
}