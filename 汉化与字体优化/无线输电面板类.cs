using System.IO;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class 无线输电面板类 : MonoBehaviour
    {
        public static 无线输电面板类 单例;
        private Canvas 根画布;
        private RectTransform 根画布区域;
        private PowerTransmitter 焦点发射器引用;
        private PowerReceiver 焦点接收器引用;
        private TMP_Text 焦点发射器名称;
        private TMP_Text 焦点接收器名称;
        private TMP_Text 焦点发射器朝向文本;
        private TMP_Text 焦点接收器朝向文本;
        private double 发射器朝向;
        private double 接收器朝向;
        private bool 校正么;
        private UnityEngine.UI.Button 面板开关;
        enum 设备类型
        {
            发射器, 接收器,
        }
        public static void 构造()
        {
            if (单例)
                单例.销毁();

            单例 = Utils.构造节点<无线输电面板类>("无线输电面板");
            单例.初始化();
            入口类.Log.LogMessage($"成功构造无线输电面板");
        }
        private void 销毁()
        {
            根画布.transform.SetParent(null, false);
            Utils.销毁节点(根画布);
            单例 = null;
            Utils.销毁节点(this);
            入口类.Log.LogMessage($"成功销毁无线输电面板");
        }
        private void 初始化()
        {
            构造UI();
            Utils.唤醒节点(this);
        }
        private float 计时器;
        private void Update()
        {
            if (WorldManager.IsGamePaused) { return; }
            计时器 += Time.deltaTime;
            if (计时器 < 0.25f) { return; }
            计时器 = 0;

            if (!校正么 || 焦点发射器引用 == null || 焦点接收器引用 == null) { return; }
            更新无线输电();
        }
        private void 更新无线输电()
        {
            // 入口类.Log.LogInfo($"({焦点发射器引用.GetType()}) {焦点发射器引用.DisplayName} {发射器朝向}");
            // 入口类.Log.LogInfo($"({焦点接收器引用.GetType()}) {焦点接收器引用.DisplayName} {接收器朝向}");

            // 物体被销毁了,但引用还在时,需要捕获一下空引用
            try
            {
                var 发射器X = 焦点发射器引用.GetLogicValue(LogicType.PositionX);
                var 发射器Y = 焦点发射器引用.GetLogicValue(LogicType.PositionY);
                var 发射器Z = 焦点发射器引用.GetLogicValue(LogicType.PositionZ);

                var 接收器X = 焦点接收器引用.GetLogicValue(LogicType.PositionX);
                var 接收器Y = 焦点接收器引用.GetLogicValue(LogicType.PositionY);
                var 接收器Z = 焦点接收器引用.GetLogicValue(LogicType.PositionZ);

                var 相对 = new Vector3((float)(接收器X - 发射器X), (float)(接收器Y - 发射器Y), (float)(接收器Z - 发射器Z));

                var 方位夹角 = Mathf.Atan2(相对.x, 相对.z) * Mathf.Rad2Deg;
                var 俯仰夹角 = Mathf.Atan(相对.y / Mathf.Sqrt(相对.x * 相对.x + 相对.z * 相对.z)) * Mathf.Rad2Deg;
                俯仰夹角 = Mathf.Abs(俯仰夹角);

                计算朝向修饰(焦点发射器引用.Transform.forward, 设备类型.发射器, ref 发射器朝向);
                计算朝向修饰(焦点接收器引用.Transform.forward, 设备类型.接收器, ref 接收器朝向);

                焦点发射器引用.SetLogicValue(LogicType.Horizontal, 发射器朝向 + 方位夹角);
                焦点发射器引用.SetLogicValue(LogicType.Vertical, 90 + (相对.y >= 0 ? 俯仰夹角 : -俯仰夹角));

                焦点接收器引用.SetLogicValue(LogicType.Horizontal, 接收器朝向 + 方位夹角 + 180);
                焦点接收器引用.SetLogicValue(LogicType.Vertical, 90 + (相对.y <= 0 ? 俯仰夹角 : -俯仰夹角));
            }
            catch (System.Exception e)
            {
                // 关闭面板,清空引用
                入口类.Log.LogWarning($"空引用,焦点发射器或焦点接收器被销毁 {e}");
                面板开关.onClick.Invoke();
                return;
            }
        }
        private void 计算朝向修饰(Vector3 正前, 设备类型 类型, ref double 朝向)
        {
            // 使用点乘计算夹角,然后用固定的静态向量进行方向比较,避免不同Vector3对象构造时的浮点数误差导致比较相等失败

            Vector3 近似方向;
            float 近似夹角;

            近似夹角 = Vector3.Dot(正前, Vector3.forward);
            近似方向 = Vector3.forward;

            var bd = Vector3.Dot(正前, Vector3.back);
            if (bd > 近似夹角)
            {
                近似夹角 = bd;
                近似方向 = Vector3.back;
            }
            var ld = Vector3.Dot(正前, Vector3.left);
            if (ld > 近似夹角)
            {
                近似夹角 = ld;
                近似方向 = Vector3.left;
            }
            var rd = Vector3.Dot(正前, Vector3.right);
            if (rd > 近似夹角)
            {
                近似夹角 = rd;
                近似方向 = Vector3.right;
            }

            if (近似方向 == Vector3.forward) { 朝向 = 0; }
            else if (近似方向 == Vector3.back) { 朝向 = 180; }
            else if (近似方向 == Vector3.left) { 朝向 = 90; }
            else if (近似方向 == Vector3.right) { 朝向 = 270; }

            switch (类型)
            {
                case 设备类型.发射器: 焦点发射器朝向文本.text = $"{发射器朝向}"; break;
                case 设备类型.接收器: 焦点接收器朝向文本.text = $"{接收器朝向}"; break;
            }
        }
        public void 更新焦点物体(Thing thing)
        {
            if (thing == null || !校正么) { return; }
            if (thing == 焦点发射器引用 || thing == 焦点接收器引用) { return; }
            if (焦点发射器引用 && 焦点接收器引用) { return; }
            // 入口类.Log.LogInfo($"({thing.GetType()}) {thing.DisplayName}");

            switch (thing)
            {
                case PowerTransmitter 发射器:
                    焦点发射器引用 = 发射器;
                    焦点发射器名称.text = 焦点发射器引用.DisplayName;
                    // 入口类.Log.LogInfo($"{焦点发射器引用.DisplayName}");
                    break;
                case PowerReceiver 接收器:
                    焦点接收器引用 = 接收器;
                    焦点接收器名称.text = 焦点接收器引用.DisplayName;
                    // 入口类.Log.LogInfo($"{焦点接收器引用.DisplayName}");
                    break;
            }
        }
        private void 构造UI()
        {
            // UI必须设Canvas为父级
            根画布 = Utils.构造节点<Canvas>(PlayerStateWindow.Instance.InfoInternalVerticalLayoutGroup.transform.parent.parent, "根画布");
            根画布.renderMode = RenderMode.ScreenSpaceOverlay;
            根画布.pixelPerfect = true;
            根画布.scaleFactor = 1;
            // 将面板放置到最上面
            根画布.transform.SetAsFirstSibling();
            // 消息分发
            根画布.gameObject.AddComponent<GraphicRaycaster>();

            var 区域尺寸 = new Vector2(68, 38);
            // 画布比较特殊,会读取屏幕分辨率以捕获模式获取区域,因此增加一个尺寸组件,修改尺寸
            根画布区域 = 根画布.gameObject.GetComponent<RectTransform>();
            根画布区域.pivot = Vector2.right;
            var 根画布区域尺寸 = 根画布.gameObject.AddComponent<LayoutElement>();
            根画布区域尺寸.preferredWidth = 区域尺寸.x;
            根画布区域尺寸.preferredHeight = 区域尺寸.y;

            面板开关 = Utils.构造节点<Button>(根画布区域, "面板开关");
            var 面板开关事件区域 = 面板开关.gameObject.AddComponent<RawImage>();
            面板开关.targetGraphic = 面板开关事件区域;
            var 面板开关区域 = 面板开关事件区域.rectTransform;
            面板开关区域.pivot = Vector2.right;
            面板开关区域.sizeDelta = 区域尺寸;

            var 面板开关标签 = Utils.构造TMP(面板开关区域, "输电", false, "面板开关标签");
            面板开关标签.fontSize = 23;
            面板开关标签.alignment = TextAlignmentOptions.Center;
            面板开关标签.font = AssetsLoad.单例.内置TMP字体;
            面板开关标签.alpha = 0.8f;
            面板开关标签.rectTransform.pivot = Vector2.right;
            面板开关标签.rectTransform.sizeDelta = 区域尺寸;

            // 设置按钮的颜色变化
            Color 正常颜色 = new Color(0, 0, 0, 0.8f);
            Color 悬停颜色 = new Color(0, 0.3f, 0, 0.8f);
            Color 点击颜色 = new Color(0, 0, 0.3f, 0.8f);

            var 按钮颜色 = 面板开关.colors;

            按钮颜色.normalColor = 正常颜色;    // 默认颜色
            按钮颜色.highlightedColor = 悬停颜色; // 悬停高亮颜色
            按钮颜色.pressedColor = 点击颜色;    // 点击颜色
            按钮颜色.disabledColor = 点击颜色;  // 禁用颜色
            按钮颜色.selectedColor = 悬停颜色;  // 活动项颜色

            面板开关.colors = 按钮颜色;
            面板开关.transition = Selectable.Transition.ColorTint;

            // 关掉按钮焦点,启用此选项时,按钮状态会保持在最后单击的那个按钮上,导致颜色不好看
            var 按钮焦点 = 面板开关.navigation;
            按钮焦点.mode = Navigation.Mode.None;
            面板开关.navigation = 按钮焦点;

            var 输电面板 = Utils.构造VL_(根画布区域, "输电面板");
            var 输电面板区域 = 输电面板.GetComponent<RectTransform>();
            输电面板区域.pivot = Vector2.right;
            输电面板区域.sizeDelta = new Vector2(405, 99);
            输电面板区域.anchoredPosition = new Vector2(-80, 0);

            面板开关.onClick.AddListener(() =>
         {
             输电面板.gameObject.SetActive(!输电面板.gameObject.activeSelf); // 切换面板的显示/隐藏
             校正么 = 输电面板.gameObject.activeSelf;
             // 入口类.Log.LogInfo($"单击 面板状态: {输电面板.gameObject.activeSelf}");
             if (输电面板.gameObject.activeSelf == false)
             {
                 焦点发射器引用 = null;
                 焦点接收器引用 = null;
                 焦点发射器名称.text = "无焦点发射器";
                 焦点接收器名称.text = "无焦点接收器";
             }
         });

            GameObject 输电面板内容;
            // 复杂的UI用纯代码有点累人,因此输电面板内容使用unity引擎创建一个预制体并打包成AssetBundle
            using (Stream 读 = Assembly.GetExecutingAssembly().GetManifestResourceStream("meanran_xuexi_mods.Resources.预制体.无线输电面板"))
            {
                var 预制体 = AssetBundle.LoadFromStream(读).LoadAllAssets<GameObject>().FirstOrDefault();
                输电面板内容 = UnityEngine.Object.Instantiate(预制体);
                AssetBundle.Destroy(预制体);
            }

            var 根节点 = 输电面板内容.transform.GetChild(0);
            根节点.SetParent(输电面板区域, false);
            // 在unity引擎中创建预制体必须有一个画布层级,在运行时修改了父级后,这个画布层级就可以销毁了
            Utils.销毁节点(输电面板内容);

            var 标题背景 = 根节点.GetChild(0);
            var 标题文本 = 标题背景.GetChild(0).GetComponent<TMP_Text>();
            标题文本.font = AssetsLoad.单例.内置TMP字体;

            var 内容区 = 根节点.GetChild(1);

            var 左内容 = 内容区.GetChild(0);
            // 亦可在unity引擎中直接拖动相关对象到公共字段中,但是这需要创建一个本模组和预制体共用的引用,涉及循环依赖,容易出BUG
            获取公共字段(左内容, ref 焦点发射器名称, 设备类型.发射器);
            var 右内容 = 内容区.GetChild(1);
            获取公共字段(右内容, ref 焦点接收器名称, 设备类型.接收器);

            Utils.唤醒节点(根画布);
            LayoutRebuilder.ForceRebuildLayoutImmediate(输电面板区域);
            面板开关.onClick.Invoke();
        }
        private void 获取公共字段(Transform UI节点, ref TMP_Text 设备名称, 设备类型 类型)
        {
            设备名称 = UI节点.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            设备名称.font = AssetsLoad.单例.内置TMP字体;
            设备名称.overflowMode = TextOverflowModes.Truncate;

            var 朝向节点 = UI节点.GetChild(1);
            var 朝向文本 = 朝向节点.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            朝向文本.font = AssetsLoad.单例.内置TMP字体;

            var 按钮节点 = 朝向节点.GetChild(1);
            var 朝向增加 = 按钮节点.GetChild(0).GetComponent<Button>();
            var 朝向减少 = 按钮节点.GetChild(1).GetComponent<Button>();

            switch (类型)
            {
                case 设备类型.发射器:
                    焦点发射器朝向文本 = 朝向文本;
                    朝向增加.onClick.AddListener(() => { 发射器朝向 += 90; 发射器朝向 %= 360; 朝向文本.text = $"{发射器朝向}"; });
                    朝向减少.onClick.AddListener(() => { 发射器朝向 = (发射器朝向 - 90) < 0 ? 270 : 发射器朝向 - 90; 朝向文本.text = $"{发射器朝向}"; });
                    break;
                case 设备类型.接收器:
                    焦点接收器朝向文本 = 朝向文本;
                    朝向增加.onClick.AddListener(() => { 接收器朝向 += 90; 接收器朝向 %= 360; 朝向文本.text = $"{接收器朝向}"; });
                    朝向减少.onClick.AddListener(() => { 接收器朝向 = (接收器朝向 - 90) < 0 ? 270 : 接收器朝向 - 90; 朝向文本.text = $"{接收器朝向}"; });
                    break;
            }
        }
    }
}