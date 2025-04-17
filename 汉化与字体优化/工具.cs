using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Assets.Scripts.Inventory;
using Assets.Scripts.UI;
using ImGuiNET.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class 层级
    {
        public static string 打印层级信息(GameObject 根节点, string 抬头 = "")
        {
            StringBuilder 结果 = new StringBuilder();
            Stack<(GameObject 节点, int 制表符, int 层级)> stack = new Stack<(GameObject 节点, int 制表符, int 层级)>();
            stack.Push((根节点, 0, 0));  // 将根节点压入栈中，制表符和层级从0开始

            while (stack.Count > 0)
            {
                var (当前节点, 当前制表符, 层级) = stack.Pop();
                结果.AppendLine($"\n{抬头.PadLeft(当前制表符)}层级[{层级}] = {当前节点.name}\n");
                组件信息(ref 结果, 当前节点, 当前制表符);

                // 将子节点按从下到上的顺序压入栈中，这样栈顶的子节点就会先被处理
                // 若子节点依然有子节点,则孙节点同样按照逆序入栈,将一条线性节点全部入栈后,切换到第二条节点
                for (int i = 当前节点.transform.childCount - 1; i >= 0; i--)
                {
                    Transform child = 当前节点.transform.GetChild(i);
                    stack.Push((child.gameObject, 当前制表符 + 10, 层级 + 1));  // 增加制表符和层级
                }
            }

            return 结果.ToString();
        }
        public static void 组件信息(ref StringBuilder 结果, GameObject 节点, int 制表符)
        {
            int 计数 = 0;
            foreach (var com in 节点.GetComponents<Component>())
                结果.AppendLine($"{"".PadLeft(制表符 + 5)}---组件[{++计数}]: ({com.GetType().Name}) {com.name}");
        }
    }
    public class SPDAListItem_延时修改 : 延时修改工具
    {
        protected override void 仅执行一次()
        {
            var 百科基因 = this.GetComponent<SPDAListItem>().InsertTitle;
            百科基因.text = 百科基因.text.词条匹配();
            // using (StreamWriter 写 = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/文本.txt", append: true))
            //     写.WriteLine($"case \"{百科基因.text}\": {{百科基因.text= \"\";break;}}");
        }
    }
    public class PanelHands_延时修改 : 延时修改工具
    {
        protected override void 仅执行一次()
        {
            var 左手 = this.transform.Find("LeftHand");
            var 左手文本 = 左手.Find("Text").GetComponent<TMP_Text>();
            左手文本.修改大小与遮罩(24);
            左手.Find("HandLeftSlot").GetComponent<SlotDisplayButton>().gameObject.AddComponent<SlotDisplay_延时修改>();

            var 右手 = this.transform.Find("RightHand");
            var 右手文本 = 右手.Find("Text").GetComponent<TMP_Text>();
            右手文本.修改大小与遮罩(24);
            右手.Find("HandRightSlot").GetComponent<SlotDisplayButton>().gameObject.AddComponent<SlotDisplay_延时修改>();
        }
    }
    public class SlotDisplay_延时修改 : 延时修改工具
    {
        private static bool 要改么 = true;
        protected override void 仅执行一次()
        {
            var 物品栏 = this.GetComponent<SlotDisplayButton>();
            var 捕获表 = 物品栏.遍历修改大小(文本字段偏移表工具<SlotDisplayButton>.文本字段偏移表, 23);
            foreach (var 文本 in 捕获表)
            {
                // 显示物品栏内物品数量的这个文本组件因为是数字,所以字可以调大些,并且设置下换行行为
                if (文本.name == "Quantity")
                {
                    文本.outlineWidth = 0.1f;                           // 字符的描边
                    文本.outlineColor = Color.grey;                     // 设置外轮廓颜色
                    文本.fontStyle = FontStyles.Bold;                   // 粗体字
                    文本.rectTransform.pivot = Vector2.one;             // TMP组件的文本变化时,区域也会相应缩放,锚定右上角不动,让布局区域向左下扩展
                    文本.alignment = TextAlignmentOptions.TopRight;     // 对齐方式设置为和父级区域右上对齐
                    文本.修改大小与遮罩(26);
                }
            }

            if (物品栏.gameObject.name.Contains("Hand"))
                return;

            var A = 物品栏.transform.parent;
            if (A)
            {
                var 包裹 = A.parent;
                if (包裹 && 包裹.childCount > 0)
                {
                    var B = 包裹.GetChild(0);
                    if (B && B.childCount > 0)
                    {
                        var C = B.GetChild(0);
                        if (C)
                        {
                            // 多个物品栏由一个包裹父级管理,通过层级找到物品栏标题文本组件
                            var 背包名称 = C.GetComponent<TMP_Text>();
                            if (背包名称)
                                背包名称.修改大小与遮罩(25);
                        }
                    }

                    var 适配 = 包裹.GetComponent<ContentSizeFitter>();

                    // 物品栏调整了字体尺寸后,占据的长度变长了,让背景也拉长
                    if (适配)
                        适配.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;

                    var 包裹Rect = 包裹.GetComponent<RectTransform>();
                    if (包裹Rect)
                        LayoutRebuilder.ForceRebuildLayoutImmediate(包裹Rect);
                }
            }

            if (要改么)
            {
                // 数量文本工具在循环中会改变字体大小,因此锁定此值
                const BindingFlags 标志 = BindingFlags.Static | BindingFlags.NonPublic;
                typeof(SlotDisplayButton).GetField("QuantityFontMinSize", 标志).SetValue(null, 26);
                typeof(SlotDisplayButton).GetField("QuantityFontMaxSize", 标志).SetValue(null, 26);
                要改么 = false;
                入口类.Log.LogInfo("SlotDisplay_延时修改,物品栏数量文本锁定为28大小");
            }
        }
    }
    public class UniversalPage_延时修改 : 延时修改工具
    {
        protected override void 仅执行一次()
        {
            // F1百科中的逻辑条目中 名称 和  值
            var 实例 = this.GetComponent<UniversalPage>();
            var 捕获表 = 实例.遍历修改大小(文本字段偏移表工具<UniversalPage>.文本字段偏移表, 20);
            foreach (var 文本2 in 捕获表)
            {
                if (文本2.gameObject.name == "InfoValue")
                {
                    var InfoTitle = 文本2.transform.parent.Find("InfoTitle");
                    if (InfoTitle)
                    {
                        var 文本1 = InfoTitle.GetComponent<TMP_Text>();
                        文本1.修改大小与遮罩(20);
                    }
                }
            }
        }
    }
    public class SPDAVersion_延时修改 : 延时修改工具
    {
        protected override void 仅执行一次()
        {
            // 修改F1百科的生产设备等级栏的背景框高度,不然只修改字体大小,会导致字体超出了背景框
            var Layout = this.transform.parent.GetComponent<GridLayoutGroup>();
            Layout.cellSize = new Vector2(Layout.cellSize.x, 166);
        }
    }
    public abstract class 延时修改工具 : MonoBehaviour
    {
        // private float 计时器;
        // private void OnDestroy() => 入口类.Log.LogInfo($"成功销毁{this.GetType().Name}");
        private void Update()
        {
            if (WorldManager.IsGamePaused) { return; }
            if (!扩展方法.UI已生成么()) { return; }
            // 计时器 += Time.deltaTime;
            // if (计时器 < 0.5f) { return; }
            // 计时器 = 0;
            仅执行一次();
            Destroy(this);
        }
        protected abstract void 仅执行一次();
    }
    public static partial class 扩展方法
    {
        public static string 词条匹配(this string 源)
        {
            string 结果 = null;

            switch (源)
            {
                case "Achievements": 结果 = "成就系统"; break;
                case "Calm weather": 结果 = "平静的天气"; break;

                case "Flatten": 结果 = "找平模式"; break;
                case "Default": 结果 = "标准模式"; break;

                case "Stabilizer": 结果 = "自动悬停"; break;
                case "Stabilizer {0}": 结果 = "自动悬停 {0}"; break;    // 可能被编译器优化成格式化字符串
                case "Detonate": 结果 = "引爆炸弹"; break;

                case "Weather damage": 结果 = "天气伤害"; break;
                case "Lander": 结果 = "着陆器"; break;
                case "Creative mode": 结果 = "创造模式"; break;
                case "Metabolism": 结果 = "新陈代谢"; break;
                case "Nutrition": 结果 = "新陈代谢之营养"; break;
                case "Hydration": 结果 = "新陈代谢之水分"; break;
                case "Breathing": 结果 = "新陈代谢之呼吸"; break;
                case "Robot Battery": 结果 = "机器人电池消耗"; break;
                case "Mood reduction": 结果 = "新陈代谢之情绪"; break;
                case "Hygiene reduction": 结果 = "新陈代谢之卫生"; break;
                case "Food decay": 结果 = "食物腐烂"; break;
                case "Jetpack consumption": 结果 = "飞行消耗"; break;
                case "Mining": 结果 = "矿物开采"; break;
                case "Lung damage": 结果 = "肺部伤害"; break;
                case "Offline metabolism": 结果 = "离线新陈代谢"; break;

                case "disabled": 结果 = "禁用"; break;
                case "enabled": 结果 = "启用"; break;

                case "Growth Efficiency": 结果 = "生长效率"; break;
                case "Breathing Efficiency": 结果 = "呼吸效率"; break;
                case "Temperature Efficiency": 结果 = "温度效率"; break;
                case "Pressure Efficiency": 结果 = "压力效率"; break;
                case "Light Efficiency": 结果 = "光合效率"; break;
                case "Hydration Efficiency": 结果 = "水合效率"; break;

                case "Min Ideal Temperature": 结果 = "最小理想温度"; break;
                case "Max Ideal Temperature": 结果 = "最大理想温度"; break;
                case "Current Temperature": 结果 = "当前大气温度"; break;

                case "Min Ideal Pressure": 结果 = "最小理想压力"; break;
                case "Max Ideal Pressure": 结果 = "最大理想压力"; break;
                case "Current Pressure": 结果 = "当前大气压力"; break;

                case "Light Intensity": 结果 = "光照强度"; break;
                case "Illumination Stress": 结果 = "作息紊乱率(调节光暗时长恢复)"; break;

                case "Light Deficiency": 结果 = "光照生长需求"; break;
                case "Darkness Deficiency": 结果 = "黑暗睡眠需求"; break;

                case "Creative Spawn Menu": 结果 = "物品生成窗口"; break;
                case "Search": 结果 = "搜索"; break;

                case "Helmet": 结果 = "头盔"; break;
                case "Suit": 结果 = "太空服"; break;
                case "Back": 结果 = "背部"; break;
                case "Uniform": 结果 = "制服"; break;
                case "Glasses": 结果 = "眼镜"; break;
                case "Belt": 结果 = "腰带"; break;
                case "Access Card": 结果 = "门禁卡"; break;
                case "Credit Card": 结果 = "银行卡"; break;
                case "Tool": 结果 = "工具"; break;
                case "Sensor Processing Unit": 结果 = "传感器处理单元"; break;
                case "Battery": 结果 = "电池"; break;
                case "Cartridge": 结果 = "记忆卡"; break;
                case "Programmable Chip": 结果 = "可编程芯片"; break;
                case "Ore": 结果 = "矿石"; break;


                case "Growth Speed Multiplier": { 结果 = "生长速度"; break; }
                case "Dark Per Day": { 结果 = "黑暗需求"; break; }
                case "Light Per Day": { 结果 = "光照需求"; break; }
                case "Drought Tolerance": { 结果 = "干旱抗性"; break; }
                case "Water Usage": { 结果 = "用水需求"; break; }
                case "Low Pressure Resistance": { 结果 = "低压耐受"; break; }
                case "Low Temperature Resistance": { 结果 = "低温耐受"; break; }
                case "Undesired Gas Tolerance": { 结果 = "毒素抗性"; break; }
                case "Gas Production": { 结果 = "气体呼吸"; break; }
                case "High Pressure Resistance": { 结果 = "高压耐受"; break; }
                case "High Temperature Resistance": { 结果 = "高温耐受"; break; }
                case "Suffocation Tolerance": { 结果 = "窒息抗性"; break; }
                case "Low Pressure Tolerance": { 结果 = "低压抗性"; break; }
                case "Low Temperature Tolerance": { 结果 = "低温抗性"; break; }
                case "High Pressure Tolerance": { 结果 = "高压抗性"; break; }
                case "High Temperature Tolerance": { 结果 = "高温抗性"; break; }
                case "Undesired Gas Resistance": { 结果 = "毒素耐受"; break; }
                case "Light Tolerance": { 结果 = "光照抗性"; break; }
                case "Darkness Tolerance": { 结果 = "黑暗抗性"; break; }

                case "More Ore Less": 结果 = "MoreOreLess-<size=88%>矿石</size>"; break;
                case "Asteroid Assayers": 结果 = "AsteroidAssayers-<size=88%>矿石</size>"; break;
                case "Cosmic Crush": 结果 = "CosmicCrush-<size=88%>矿石</size>"; break;
                case "Galactic Gravels": 结果 = "GalacticGravels-<size=88%>矿石</size>"; break;
                case "Nebula Nuggets": 结果 = "NebulaNuggets-<size=88%>矿石</size>"; break;
                case "Orbit Ore Oasis": 结果 = "OrbitOreOasis-<size=88%>矿石</size>"; break;
                case "Stellar Stone Supply": 结果 = "StellarStoneSupply-<size=88%>矿石</size>"; break;
                case "Interstellar Excavators": 结果 = "InterstellarExcavators-<size=88%>矿石</size>"; break;
                case "Void Vein Vendors": 结果 = "VoidVeinVendors-<size=88%>矿石</size>"; break;
                case "Planetary Pebbles": 结果 = "PlanetaryPebbles-<size=88%>矿石</size>"; break;

                case "All Alloys": 结果 = "AllAlloys-<size=88%>合金</size>"; break;
                case "Metal Mavens": 结果 = "MetalMavens-<size=88%>合金</size>"; break;
                case "AstroAlloy Emporium": 结果 = "AstroAlloyEmporium-<size=88%>合金</size>"; break;
                case "Cosmic Forge": 结果 = "CosmicForge-<size=88%>合金</size>"; break;
                case "Galactic Metallurgy": 结果 = "GalacticMetallurgy-<size=88%>合金</size>"; break;
                case "OrbitOre Outfitters": 结果 = "OrbitOreOutfitters-<size=88%>合金</size>"; break;
                case "Stellar Smelter": 结果 = "StellarSmelter-<size=88%>合金</size>"; break;
                case "Interstellar Ingots": 结果 = "InterstellarIngots-<size=88%>合金</size>"; break;
                case "Nebula Nucleus": 结果 = "NebulaNucleus-<size=88%>合金</size>"; break;
                case "Space Alloy Specialists": 结果 = "SpaceAlloySpecialists-<size=88%>合金</size>"; break;
                case "Star Smelter": 结果 = "StarSmelter-<size=88%>合金</size>"; break;

                case "Starlight Suppers": 结果 = "StarlightSuppers-<size=88%>食品</size>"; break;
                case "Galactic Groceries": 结果 = "GalacticGroceries-<size=88%>食品</size>"; break;
                case "Orbiting Organics": 结果 = "OrbitingOrganics-<size=88%>食品</size>"; break;
                case "Cosmic Cuisine": 结果 = "CosmicCuisine-<size=88%>食品</size>"; break;
                case "Asteroid Eats": 结果 = "AsteroidEats-<size=88%>食品</size>"; break;
                case "Nebula Nibbles": 结果 = "NebulaNibbles-<size=88%>食品</size>"; break;
                case "Stellar Snacks": 结果 = "StellarSnacks-<size=88%>食品</size>"; break;
                case "Interstellar Ingredients": 结果 = "InterstellarIngredients-<size=88%>食品</size>"; break;
                case "Space Spices": 结果 = "SpaceSpices-<size=88%>食品</size>"; break;
                case "Void Vegetables": 结果 = "VoidVegetables-<size=88%>食品</size>"; break;
                case "Planetary Produce": 结果 = "PlanetaryProduce-<size=88%>食品</size>"; break;

                case "Green Futures": 结果 = "GreenFutures-<size=88%>水培</size>"; break;
                case "AstroAgronomics": 结果 = "AstroAgronomics-<size=88%>水培</size>"; break;
                case "Stellar Sprouts": 结果 = "StellarSprouts-<size=88%>水培</size>"; break;
                case "HydroHarvest Haven": 结果 = "HydroHarvestHaven-<size=88%>水培</size>"; break;
                case "Orbiting Orchards": 结果 = "OrbitingOrchards-<size=88%>水培</size>"; break;
                case "Galactic Growers": 结果 = "GalacticGrowers-<size=88%>水培</size>"; break;
                case "CosmoCrop Connect": 结果 = "CosmoCropConnect-<size=88%>水培</size>"; break;
                case "Space Sprout Suppliers": 结果 = "SpaceSproutSuppliers-<size=88%>水培</size>"; break;
                case "Nebula Nurturers": 结果 = "NebulaNurturers-<size=88%>水培</size>"; break;
                case "Star Seedlings": 结果 = "StarSeedlings-<size=88%>水培</size>"; break;
                case "EcoSphere Essentials": 结果 = "EcoSphereEssentials-<size=88%>水培</size>"; break;
                case "Interstellar Irrigation": 结果 = "InterstellarIrrigation-<size=88%>水培</size>"; break;

                case "GasForLess": 结果 = "GasForLess-<size=88%>气体</size>"; break;
                case "AstroAether": 结果 = "AstroAether-<size=88%>气体</size>"; break;
                case "Cosmic Clouds": 结果 = "CosmicClouds-<size=88%>气体</size>"; break;
                case "Nebula Nectars": 结果 = "NebulaNectars-<size=88%>气体</size>|<size=88%>液体</size>"; break;
                case "Orbiting Oxygens": 结果 = "OrbitingOxygens-<size=88%>气体</size>"; break;
                case "Galactic Gases": 结果 = "GalacticGases-<size=88%>气体</size>"; break;
                case "Stellar Steam": 结果 = "StellarSteam-<size=88%>气体</size>"; break;
                case "Interstellar Inhalants": 结果 = "InterstellarInhalants-<size=88%>气体</size>"; break;
                case "Void Vapors": 结果 = "VoidVapors-<size=88%>气体</size>"; break;
                case "Space Gas Station": 结果 = "SpaceGasStation-<size=88%>气体</size>"; break;

                case "Build INC": 结果 = "Build INC-建材"; break;

                case "Payless Liquids": 结果 = "PaylessLiquids-<size=88%>液体</size>"; break;
                case "Frosty Barrels": 结果 = "FrostyBarrels-<size=88%>液体</size>"; break;
                case "Cosmic Concoctions": 结果 = "CosmicConcoctions-<size=88%>液体</size>"; break;
                case "Galactic Gush": 结果 = "GalacticGush-<size=88%>液体</size>"; break;
                case "Orbital Oceans": 结果 = "OrbitalOceans-<size=88%>液体</size>"; break;
                case "Stellar Streams": 结果 = "StellarStreams-<size=88%>液体</size>"; break;
                case "Interstellar Icicles": 结果 = "InterstellarIcicles-<size=88%>液体</size>"; break;
                case "Void Vessels": 结果 = "VoidVessels-<size=88%>液体</size>"; break;
                case "Space Springs": 结果 = "SpaceSprings-<size=88%>液体</size>"; break;
                case "Star Sippers": 结果 = "StarSippers-<size=88%>液体</size>"; break;

                case "Cosmic Tools &amp; More": 结果 = "CosmicTools&amp;More-<size=88%>成品</size>"; break;
                case "Galactic Gearworks": 结果 = "GalacticGearworks-<size=88%>成品</size>"; break;
                case "Stellar Supplies": 结果 = "StellarSupplies-<size=88%>成品</size>"; break;
                case "OrbitOps Hardware": 结果 = "OrbitOpsHardware-<size=88%>成品</size>"; break;
                case "Interstellar Implements": 结果 = "InterstellarImplements-<size=88%>成品</size>"; break;
                case "Void Ventures": 结果 = "VoidVentures-<size=88%>成品</size>"; break;
                case "Asteroid Artisans": 结果 = "AsteroidArtisans-<size=88%>成品</size>"; break;
                case "Space Spanners": 结果 = "SpaceSpanners-<size=88%>成品</size>"; break;
                case "Meteor Mechanics": 结果 = "MeteorMechanics-<size=88%>成品</size>|<size=88%>家电</size>"; break;

                case "AstroMart": 结果 = "AstroMart-<size=88%>耗材</size>"; break;
                case "Cosmo's Convenience": 结果 = "Cosmo'sConvenience-<size=88%>耗材</size>"; break;
                case "StarStop": 结果 = "StarStop-<size=88%>耗材</size>"; break;
                case "Nebula Necessities": 结果 = "NebulaNecessities-<size=88%>耗材</size>"; break;
                case "Interstellar Essentials": 结果 = "InterstellarEssentials-<size=88%>耗材</size>"; break;
                case "Void Vending": 结果 = "VoidVending-<size=88%>耗材</size>"; break;
                case "Meteor Munchies": 结果 = "MeteorMunchies-<size=88%>耗材</size>"; break;

                case "Galactic Gadgets": 结果 = "GalacticGadgets-<size=88%>家电</size>"; break;
                case "Orbitron Appliances": 结果 = "OrbitronAppliances-<size=88%>家电</size>"; break;
                case "Stellar Systems Store": 结果 = "StellarSystemsStore-<size=88%>家电</size>"; break;
                case "Space Savvy Solutions": 结果 = "SpaceSavvySolutions-<size=88%>家电</size>"; break;
                case "Interstellar Innovations": 结果 = "InterstellarInnovations-<size=88%>家电</size>"; break;
                case "Void Visions": 结果 = "VoidVisions-<size=88%>家电</size>"; break;
                case "Asteroid Appliances": 结果 = "AsteroidAppliances-<size=88%>家电</size>"; break;
            }

            return 结果 ?? 源;
        }
        public static IEnumerable<FieldInfo> 获取字段偏移表(this Type 源, Type 目标)
        {
            const BindingFlags 标志 = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            return 源.GetFields(标志).Where(t => 目标.IsAssignableFrom(t.FieldType));
        }
        public static IEnumerable<TMP_Text> 遍历修改大小<T>(this T 实例, IEnumerable<FieldInfo> 字段偏移表, int 大小)
        {
            List<TMP_Text> 捕获表 = new List<TMP_Text>();
            foreach (var 偏移 in 字段偏移表)
            {
                // 直接按照地址偏移量访问内存,即在实例首地址+N处获取I个字节,不想查找有哪些变量是TMP组件了
                var 文本工具 = 偏移.GetValue(实例) as TMP_Text;
                if (文本工具)
                {
                    文本工具.修改大小与遮罩(大小);
                    捕获表.Add(文本工具);
                }
            }
            return 捕获表;
        }
        public static void 修改大小与遮罩(this TMP_Text 文本工具, int 大小)
        {
            if (文本工具.fontSize < 大小)
            {
                文本工具.fontSize = 大小;

                if (文本工具.fontSizeMax < 大小)
                {
                    文本工具.fontSizeMax = 大小;
                }

                文本工具.fontSizeMin = 大小;
            }
            文本工具.关闭遮罩();
        }
        public static void 关闭遮罩(this TMP_Text 文本工具)
        {
            文本工具.overflowMode = TextOverflowModes.Overflow;     // 超出RectTransform区域不截断,搭配自动换行使用
        }
        public static void 修改大小与遮罩(this Text 文本工具, int 大小)
        {
            if (文本工具.fontSize < 大小)
            {
                文本工具.fontSize = 大小;

                if (文本工具.resizeTextMaxSize < 大小)
                {
                    文本工具.resizeTextMaxSize = 大小;
                }

                文本工具.resizeTextMinSize = 大小;
            }
            文本工具.关闭遮罩();
        }
        public static void 关闭遮罩(this Text 文本工具)
        {
            文本工具.verticalOverflow = VerticalWrapMode.Overflow;
            文本工具.horizontalOverflow = HorizontalWrapMode.Overflow;
        }
        public static void 创建ImGui字体定义(out FontDefinition 字体定义, string ImGui字体路径)
        {
            字体定义 = new FontDefinition();
            字体定义.FontPath = ImGui字体路径;
            字体定义.Config.FontIndexInFile = 0;   // 如果一个字体文件包含多个字体，可以指定你想要加载的字体的索引
            字体定义.Config.SizeInPixels = 24;     // 设置字体的大小（以像素为单位）
            字体定义.Config.Oversample = Vector2Int.one;   // 字体的采样倍数
            字体定义.Config.PixelSnapH = true;             // 是否对齐到像素
            字体定义.Config.GlyphExtraSpacing = Vector2.zero;  // 字符额外水平间距
            字体定义.Config.GlyphOffset = Vector2.zero;        // 字符的偏移量
            字体定义.Config.GlyphRanges = FontConfig.ScriptGlyphRanges.ChineseFull;     // 字符集范围
            字体定义.Config.GlyphMinAdvanceX = 0;  // 设置字符的最小和最大水平间距（AdvanceX）。用于调整字符对齐和宽度。
            字体定义.Config.GlyphMaxAdvanceX = 20;
            字体定义.Config.MergeIntoPrevious = false; // 字体合并到前一个字体配置中,这对于将多个字体合并为一个字体集时很有用
            字体定义.Config.RasterizerFlags = 0;       // 用于自定义字体栅格化器的标志
            字体定义.Config.RasterizerMultiply = 1;    // 字体亮度系数
            字体定义.Config.EllipsisChar = '…';        // 用于指定省略号（...）的字符
            // 字体配置.CustomGlyphRanges = new FontConfig.Range[] { new FontConfig.Range { Start = 1, End = 65535 } };  // 用户自定义的Unicode字符范围
        }
        public static bool UI已生成么() => InventoryManager.Instance && PlayerStateWindow.Instance;
        public static string 去除富文本标记(string 源) => Regex.Replace(WebUtility.HtmlDecode(源), @"<[^>]+>", string.Empty);
        public static IEnumerable<HarmonyLib.CodeInstruction> 修改IL代码中的字符串(this IEnumerable<HarmonyLib.CodeInstruction> IL)
        {
            var IL代码 = new List<HarmonyLib.CodeInstruction>(IL);
            // 指令结构请参考逻辑分拣机的指令结构,基本原理相同
            for (var i = 0; i < IL代码.Count; i++)
            {
                if (IL代码[i].opcode == OpCodes.Ldstr)
                {
                    if (IL代码[i].operand is string 内容)
                    { IL代码[i].operand = 内容.词条匹配(); }
                }
            }

            return IL代码;
        }
    }
    public static class 文本字段偏移表工具<T> where T : class
    {
        // 单例:每一个类型共用一份偏移表
        private static IEnumerable<FieldInfo> 偏移表 = null;
        public static IEnumerable<FieldInfo> 文本字段偏移表
        {
            get
            {
                if (偏移表 == null)
                    偏移表 = typeof(T).获取字段偏移表(typeof(TMP_Text));
                return 偏移表;
            }
        }
    }
    public class Crc32工具
    {
        [StructLayout(LayoutKind.Explicit, Size = sizeof(uint))]
        public struct Union32
        {
            [FieldOffset(0)]
            public uint U;
            [FieldOffset(0)]
            public int L;
        }
        static uint[] Crc32Table = GetCrc32Table();
        public static int StringToCrc32(string 源)
        {
            // 游戏中使用此算法生成翻译文件的Key
            // 使用标准Crc32算法将文本转换成整数哈希值
            byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(源);
            uint value = 0xffffffff;
            int len = buffer.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            // C#貌似有符号默认是算术位移,因此先使用无符号整数对二进制进行位移操作避免补符号位
            Union32 Union;
            Union.L = 0;
            Union.U = value ^ 0xffffffff;
            return Union.L;
        }
        static uint[] GetCrc32Table()
        {
            // 生成Crc32表
            var 表 = new uint[256];
            uint Crc;
            uint i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ 0xEDB88320;
                    else
                        Crc >>= 1;
                }
                表[i] = Crc;
            }
            return 表;
        }
    }
    public class Utils
    {
        public static T 构造节点<T>(string 名称 = null) where T : Component => 构造节点<T>((Transform)null, 名称);
        public static T 构造节点<T>(GameObject parent, string 名称 = null) where T : Component => 构造节点<T>(parent ? parent.transform : null, 名称);
        public static T 构造节点<T>(Component parent, string 名称 = null) where T : Component => 构造节点<T>(parent ? parent.transform : null, 名称);
        public static T 构造节点<T>(Transform parent, string 名称 = null) where T : Component
        {
            // Component类默认在构造中将unity引擎设为父级,纳入unity引擎的主循环
            var 节点 = new GameObject().AddComponent<T>();
            节点.name = $"({typeof(T).Name}){(名称 != null ? $" {名称}" : null)}";
            节点.gameObject.SetActive(false);
            if (parent != null)
            {
                节点.transform.SetParent(parent, false);
                节点.transform.localPosition = Vector3.zero;
                节点.transform.localRotation = Quaternion.identity;
                节点.transform.localScale = Vector3.one;
            }
            return 节点;
        }
        public static GameObject 构造节点(GameObject parent)
        {
            var 节点 = new GameObject();
            节点.SetActive(false);
            if (parent != null)
            {
                节点.transform.SetParent(parent.transform, false);
                节点.transform.localPosition = Vector3.zero;
                节点.transform.localRotation = Quaternion.identity;
                节点.transform.localScale = Vector3.one;
            }
            return 节点;
        }
        public static void 销毁节点(Component obj)
        {
            if (obj != null) { 销毁节点(obj.gameObject); }
        }
        public static void 销毁节点(GameObject obj)
        {
            if (obj != null) { UnityEngine.Object.Destroy(obj); }
        }
#pragma warning disable CS0618
        public static void 唤醒节点(Component obj) => 唤醒节点(obj ? obj.gameObject : null);
        public static void 唤醒节点(GameObject obj)
        {
            if (obj != null) { obj.SetActiveRecursively(true); }
        }
        public static void 休眠节点(Component obj) => 休眠节点(obj ? obj.gameObject : null);
        public static void 休眠节点(GameObject obj)
        {
            if (obj != null) { obj.SetActiveRecursively(false); }
        }
        public static void 销毁子级节点(Transform obj)
        {
            if (obj == null) { return; }
            foreach (Transform child in obj) { 销毁节点(child.gameObject); }
        }
        public static void 休眠子级节点(Transform obj)
        {
            if (obj == null) { return; }
            foreach (Transform child in obj) { child.gameObject.SetActive(false); }
        }
        public static VerticalLayoutGroup 构造VL_(Component parent, string 名称 = null) => 构造VL_(parent?.gameObject, 名称);   // 布局的父级可以传空
        private static VerticalLayoutGroup 构造VL_(GameObject parent, string 名称 = null) => (VerticalLayoutGroup)VlHl_Init(构造节点<VerticalLayoutGroup>(parent, 名称));
        private static HorizontalLayoutGroup 构造HL_(Component parent, string 名称 = null) => 构造HL_(parent?.gameObject, 名称); // 布局的父级可以传空
        private static HorizontalLayoutGroup 构造HL_(GameObject parent, string 名称 = null) => (HorizontalLayoutGroup)VlHl_Init(构造节点<HorizontalLayoutGroup>(parent, 名称));
        private static TextMeshProUGUI 构造TMP(Component parent, string 名称 = null) => 构造TMP(parent.gameObject, 名称);
        private static TextMeshProUGUI 构造TMP(GameObject parent, string 名称 = null) => TMPInit(构造节点<TextMeshProUGUI>(parent, 名称));
        private static HorizontalOrVerticalLayoutGroup VlHl_Init(HorizontalOrVerticalLayoutGroup layout)
        {
            // 每次布局相当于一次深度搜索,上级需要根据自身的区域和起始坐标信息配置子级的区域和子级的起始坐标信息
            layout.childAlignment = TextAnchor.UpperLeft;   // 用过排版软件的对齐功能就明白了
            layout.spacing = 0;
            layout.padding = new RectOffset(0, 0, 0, 0);
            layout.childControlWidth = false;               // 例:有两个TMP子级,A字数=2,B字数=3,而上级宽为15,以下是简易比例计算,实际还包括字间距,字体不同字宽不同等等
            layout.childControlHeight = false;              //    则15*[A字数*字宽/(A字数*字宽+B字数*字宽)]+15*[B字数*字宽/(A字数*字宽+B字数*字宽)]=15
            layout.childForceExpandWidth = false;           // 例:有两个子级,区域宽分别为2和3,而上级宽为15
            layout.childForceExpandHeight = false;          //    则15-2-3=10, 2+10*[2/(2+3)]+3+10*[3/(2+3)]=15
            layout.childScaleWidth = false;
            layout.childScaleHeight = false;
            return layout;
        }
        private static TextMeshProUGUI TMPInit(TextMeshProUGUI tmp)
        {
            tmp.alignment = TextAlignmentOptions.TopLeft;   // tmp.rectTransform.anchorMin和anchorMax设置,区域与父级区域左上对齐
            tmp.rectTransform.pivot = Vector2.up;           // TMP组件的文本变化时,区域也会相应缩放,锚定左上角不动,让布局区域向右向下扩展
            tmp.lineSpacing = 0;                            // 字符之间的行间距  
            tmp.characterSpacing = 0;                       // 字符之间的列间距  
            tmp.margin = Vector4.zero;                      // 区域内间距预留多少后是实际文本绘制区域,分别是距左,距上,距右,距下
            tmp.text = "0000";
            tmp.font = AssetsLoad.单例.内置TMP字体;
            tmp.fontSize = 24;                               // 字体尺寸请尽量设置成TMP字体原始大小的整数倍,确保缩放采样时最外围能对齐,而不是因为四舍五入导致采样像素错位导致锯齿,可通过打印字体信息获取原始大小
            tmp.color = Color.white;                        // 单字的透明度指的是对RGB分量的修饰
            tmp.alpha = 1;                                  // 文本的透明度指的是文本所在图层与其它图层混叠时的透明系数
            tmp.richText = true;                            // 是否启用富文本语言解析器
            tmp.maskable = true;                            // 是否可被遮罩
            tmp.overflowMode = TextOverflowModes.Truncate;  // 如果文本绘制和区域冲突了,以区域为准,超出部分截断 注:区域仅仅是提供绘制的起始坐标 例:A的超始坐标是(2,3),A区域宽高是(3,3),垂直分布,则B起始坐标是(2,6)
            tmp.enableWordWrapping = true;                  // 是否可自动换行
            tmp.fontStyle = FontStyles.Normal;              // 字符的描边,unity引擎对字符描边的几种模板化的配置
            tmp.outlineWidth = 0;                           // 字符的描边,自定义描边配置
            return tmp;
        }
        public static void Add_ContentSizeFitter(Component obj)
        {
            var fitter = obj.GetOrAddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }
        public static readonly Vector4 TMP内缩_屏幕 = new Vector4(3, 3, 3, 3);  // TextMeshProUGUI实际文本像素区域相对于背景区域的内缩距离
        public static readonly Vector4 TMP内缩_世界 = new Vector4(0.006f, 0.006f, 0.006f, 0.006f);  // 同上,但是世界坐标是仿真的,比如人物也只有1.5左右高,由unity引擎转换成屏幕坐标
        public static TextMeshProUGUI 构造TMP(RectTransform parentRect, string 绘制文本, bool 世界坐标系么, string 名称 = null)
        {
            var tmp = Utils.构造TMP(parentRect, 名称);
            Add_ContentSizeFitter(tmp);
            tmp.rectTransform.sizeDelta = new Vector2(parentRect.rect.width, 0);
            tmp.margin = 世界坐标系么 ? TMP内缩_世界 : TMP内缩_屏幕;
            tmp.text = 绘制文本;
            return tmp;
        }
        public Button 构造可点击渲染组件(RectTransform parentRect)
        {
            var 区域尺寸 = new Vector2(640, 50);

            var hl = 构造HL_(parentRect);
            hl.childAlignment = TextAnchor.MiddleLeft;
            hl.childControlHeight = true;

            var hlRect = hl.GetComponent<RectTransform>();
            hlRect.pivot = Vector2.up;
            hlRect.anchorMax = Vector2.up;
            hlRect.anchorMin = Vector2.up;
            hlRect.sizeDelta = 区域尺寸;

            Button 点击按钮 = hlRect.gameObject.AddComponent<Button>();
            var 点击区域 = 点击按钮.gameObject.AddComponent<RawImage>();
            点击按钮.targetGraphic = 点击区域;
            var __ = 点击区域.rectTransform;
            __.anchorMax = Vector2.one;
            __.anchorMin = Vector2.zero;

            var 类型描述 = 构造TMP(点击按钮, "类型描述");
            类型描述.修改大小与遮罩(28);
            类型描述.alignment = TextAlignmentOptions.Left;
            类型描述.font = AssetsLoad.单例.内置TMP字体;
            类型描述.rectTransform.anchorMax = Vector2.one;
            类型描述.rectTransform.anchorMin = Vector2.zero;

            // 设置按钮的颜色变化
            Color 正常颜色 = new Color(0, 0, 0, 0.8f);
            Color 悬停颜色 = new Color(0, 0.3f, 0, 0.8f);
            Color 点击颜色 = new Color(0, 0, 0.3f, 0.8f);

            var 按钮颜色 = 点击按钮.colors;

            按钮颜色.normalColor = 正常颜色;    // 默认颜色
            按钮颜色.highlightedColor = 悬停颜色; // 悬停高亮颜色
            按钮颜色.pressedColor = 点击颜色;    // 点击颜色
            按钮颜色.disabledColor = 点击颜色;  // 禁用颜色
            按钮颜色.selectedColor = 悬停颜色;  // 活动项颜色

            点击按钮.colors = 按钮颜色;
            点击按钮.transition = Selectable.Transition.ColorTint;

            // 关掉按钮焦点,启用此选项时,按钮状态会保持在最后单击的那个按钮上,导致颜色不好看
            var 按钮焦点 = 点击按钮.navigation;
            按钮焦点.mode = Navigation.Mode.None;
            点击按钮.navigation = 按钮焦点;

            return 点击按钮;
        }
    }
}
