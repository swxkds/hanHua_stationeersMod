using System.Collections.Generic;
using System.Reflection;
using Assets.Scripts;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.UI;
using Assets.Scripts.Voxel;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using Objects.Items;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class 手持钻机矿石扫描 : MonoBehaviour
    {
        public static 手持钻机矿石扫描 单例;
        private Canvas 根画布;
        private RectTransform 根画布区域;
        private UnityEngine.UI.Button 面板开关;
        private Tool 当前;
        private bool 并发么;
        private float 计时器;
        public static void 构造()
        {
            if (单例)
                单例.销毁();

            单例 = Utils.构造节点<手持钻机矿石扫描>("手持钻机矿石扫描");
            单例.初始化();
            入口类.Log.LogMessage($"成功构造手持钻机矿石扫描");
        }
        private void 销毁()
        {
            根画布.transform.SetParent(null, false);
            Utils.销毁节点(根画布);
            单例 = null;
            Utils.销毁节点(this);
            入口类.Log.LogMessage($"成功销毁手持钻机矿石扫描");
        }
        private void 初始化()
        {
            构造UI();
            并发么 = false;
            Utils.唤醒节点(this);
        }
        private void Update()
        {
            if (WorldManager.IsGamePaused) { return; }
            计时器 += Time.deltaTime;
            if (计时器 < 0.25f) { return; }
            计时器 = 0;

            // 如果正在执行协程,或者本地玩家不存在
            if (并发么 || Human.LocalHuman == null) { return; }

            var obj = Human.LocalHuman.LeftHandSlot.Get();
            switch (obj)
            {
                case MiningDrill 电动: 当前 = 电动; break;
                case PneumaticMiningDrill 气动: 当前 = 气动; break;
                default:
                    {
                        obj = Human.LocalHuman.RightHandSlot.Get();
                        switch (obj)
                        {
                            case MiningDrill 电动: 当前 = 电动; break;
                            case PneumaticMiningDrill 气动: 当前 = 气动; break;
                            default:
                                return;
                        }
                        break;
                    }
            }

            // 如果戴上了探矿眼镜,并且插入了探矿芯片,关掉扫描功能
            if (Human.LocalHuman.GlassesSlot.Get() is SensorLenses 探矿眼镜 && 探矿眼镜.Sensor is SPUOre)
            { return; }

            if (活跃么()) { 并发渲染().Forget(); }
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

            var 面板开关标签 = Utils.构造TMP(面板开关区域, "倍速", false, "面板开关标签");
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

            面板开关.onClick.AddListener(() => Time.timeScale = Time.timeScale != 1 ? 1 : 7);

            Utils.唤醒节点(根画布);
        }
        private bool 活跃么()
        {
            // 手持矿机不在任何物品栏中,即丢在了地上
            // 手持矿机电源为开,并且电池有电,并且在任意一个手上
            if (当前.ParentSlot == null) { return false; }
            return 当前.OnOff && 当前.Powered && 当前.ParentSlot.IsHandSlot;
        }
        private List<Matrix4x4> 待移除表 = new List<Matrix4x4>(64);
        private static FieldInfo 变换矩阵表偏移 = typeof(InstancedIndirectDrawCall).GetField("_instanceData", BindingFlags.Instance | BindingFlags.NonPublic);
        private static float 扫描距离 = Mathf.Pow(15, 2);
        private static float 扫描角度 = Mathf.Cos(10 * Mathf.Deg2Rad);
        private async UniTaskVoid 并发渲染()
        {
            // 禁止Update开启新的协程
            并发么 = true;

            // 协程的退出条件
            while (活跃么())
            {
                // 注:当玩家进入某区块内,玩家坐标被舍入成整数,该整数对应区块中心点,激活该区块内部所有碰撞体,离开区块时将该区块所有碰撞体失活
                // 区块中心点+子级碰撞体本地坐标中心点=体素坐标,体素越小越精细,碰撞体越多,碰撞检测是遍历,因此区块内部依然分小区块,减少遍历的碰撞体数量
                // 游戏使用八叉树结构管理区块,如在代码中见到重复的取中心点行为,是因为在计算区块中的区块的中心点
                foreach (var 区块 in ChunkController.World.AllChunks.Values)
                {
                    if (区块 is Asteroid 行星区块 && (行星区块.Position - CameraController.CameraPosition).sqrMagnitude < 扫描距离)
                    {
                        // 每个DrawCall内保存着区块内同种矿石的所有变换矩阵,该区块有几种矿石就有几个DrawCall
                        foreach (var DrawCall in 行星区块.MineableVisualizerDrawCalls)
                        {
                            var 变换矩阵表 = 变换矩阵表偏移.GetValue(DrawCall) as List<InstancedIndirectDrawCall.MeshPerInstanceDatum>;
                            待移除表.Clear();

                            // MineableVisualizerDrawCalls 使用双缓冲一次性绘制该节点下所有同类矿石,变换矩阵表中保存着在哪些地方绘制
                            foreach (var _ in 变换矩阵表)
                            {
                                var 矩阵 = _.ObjectToWorldMatrix;
                                var 相对 = new Vector3(矩阵.m03 - CameraController.CameraPosition.x, 矩阵.m13 - CameraController.CameraPosition.y, 矩阵.m23 - CameraController.CameraPosition.z);

                                // 使用单位向量时,点乘的结果=两向量之间的余弦
                                var 余弦 = Vector3.Dot(CameraController.Instance.MainCameraForward, 相对.normalized);
                                // 余弦最大=1,余弦越大,夹角越小,只渲染夹角在0度至扫描角度之间的矩阵
                                if (余弦 < 扫描角度)
                                    待移除表.Add(矩阵);
                            }

                            // 不要永久增删矩阵,因为探矿眼镜也依赖DrawCalls渲染,永久删除矩阵由开采矿石行为负责
                            // 移除矩阵不会更新包围盒
                            foreach (var 矩阵 in 待移除表)
                                DrawCall.RemoveInstance(矩阵);

                            // 入口类.Log.LogInfo($"测试删除后包围盒范围: {DrawCall.Bounds.extents}");

                            // 使用双缓冲绘制
                            DrawCall.Draw(DrawCall.Bounds, ShadowCastingMode.Off, receiveShadows: false);

                            // 不要永久增删矩阵,因为探矿眼镜也依赖DrawCalls渲染,永久增加矩阵由地图生成行为负责
                            // 增加的矩阵在包围盒范围内,包围盒范围不会变动
                            foreach (var 矩阵 in 待移除表)
                                DrawCall.AddInstance(矩阵);

                            // 入口类.Log.LogInfo($"测试增加后包围盒范围: {DrawCall.Bounds.extents}");
                        }
                    }
                }

                // 暂停一帧时间后,从当前执行地址继续往下执行
                await UniTask.NextFrame();
            }

            // 协程执行完成后,解锁Update,允许开启新的协程
            并发么 = false;
        }
    }

    [HarmonyPatch(typeof(GameManager), nameof(GameManager.LastTickTimeSeconds), MethodType.Getter)]
    public class GameManager_LastTickTimeSeconds_Patch
    {
        public static void Postfix(ref float __result) => __result = __result * Time.timeScale;
    }
}

// CursorManager.Instance.FoundAsteroid
// SPUOre
// VoxelTerrain.MineMineable(); // 矿石开采
// OreScanner  // 查找矿石体素

// 植物生长时间
// public void OnLifeTick(float deltaTime)
// 		{
// 			if (this.GrowthStates.Count == 0)
// 			{
// 				return;
// 			}
// 			this.SetGrowthEfficiencyTooltip();
// 			float num = this.lifeRequirements.GrowthEfficiency();
// 			if (this.DamageState.Total > 0f && this.PlantStatus.CanHeal(this))
// 			{
// 				this.DamageState.Heal(0.25f * num * this.FertilizerBoost);
// 			}
// 			this.PlantRecord.UpdateRecord(this, deltaTime);
// 			if (this.CurrentStage.Length < 0f)
// 			{
// 				return;
// 			}
// 			this._stageTime += deltaTime * num * this.FertilizerBoost;
// 			if (float.IsNaN(this._stageTime))
// 			{
// 				this._stageTime = 0f;
// 			}
// 			if (this._stageTime > this.CurrentStage.Length)
// 			{
// 				this.SetNextStage();
// 			}
// 			if (Plant.CustomPlantGrowthSpeed)
// 			{
// 				this.SetCustomPlantGrowth();
// 			}
// 		}

