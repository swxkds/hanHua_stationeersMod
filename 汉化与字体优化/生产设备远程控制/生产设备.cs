using Assets.Scripts.Objects;
using HarmonyLib;
using Assets.Scripts.Objects.Electrical;
using UnityEngine;
using System.Reflection;
using Assets.Scripts.Inventory;

namespace meanran_xuexi_mods_xiaoyouhua
{
    // Interactable: 控件,注意是控件,而不是物体
    // Interaction: 交互双方. 玩家/玩家活动手插槽/视线交互焦点所在的控件或物体或另一个玩家
    // doAction: 玩家当前是在执行动作还是单纯的就看看(拿着工具不操作也算无动作)

    // AttackWith是操作事件,是对整个物体的相关状态变量进行修改
    // InteractWith是交互事件,是对物体内部的某个控件相关状态变量进行修改
    // 管道没有控件(例开关/按钮/刻度盘),因此是AttackWith,管道也没有
    // AttackWith和InteractWith这两个函数由游戏的事件总线每帧调用,会传递交互双方信息,我们只需要根据这些信息的组合,返回对应消息即可

    // [HarmonyPatch(typeof(InventoryManager), "UsePrimaryComplete")]
    // public class Autolathe_Patch_1
    // {
    //     [HarmonyPostfix]
    //     public static void 添加引用(InventoryManager __instance)
    //     {
    //         // 注: ConstructionCursor是拷贝构造函数的母版, 实际放置的是拷贝后的新物体的引用
    //         var __ = InventoryManager.ConstructionCursor as SimpleFabricatorBase;
    //         if (__)
    //         {
    //             入口类.Log.LogInfo($"添加测试: {__.ReferenceId}");

    //             if (!控制面板.车床表.ContainsKey(__.ReferenceId))
    //             {
    //                 控制面板.车床表.Add(__.ReferenceId, __);
    //                 // 若该设备需要多次安装才能达到完成状态, 还需要额外判断建造状态
    //                 入口类.Log.LogInfo("将处于结构状态的生产设备添加到控制面板");
    //             }
    //         }
    //     }
    // }

    public class 对生产设备进行功能注入 : MonoBehaviour
    {
        private void Start()
        {
            // 由于Unity引擎是组件模式, 且复制物体是通过克隆(类似拷贝构造函数)实现的, 因此可以将本类的实例添加到生产设备母体组件表中
            // 这样可以监控到任何生产设备
            var __ = this.GetComponent<SimpleFabricatorBase>();
            if (__)
            {
                if (__.ReferenceId == 0) { return; }

                入口类.Log.LogInfo($"测试=>建造了生产设备 ID: {__.ReferenceId}  名称: {__.DisplayName}");

                if (!控制面板.生产设备表.ContainsKey(__.ReferenceId))
                {
                    控制面板.生产设备表.Add(__.ReferenceId, __);
                    // 若该设备需要多次安装才能达到完成状态, 还需要额外判断建造状态
                    入口类.Log.LogInfo("生产设备已添加到远程控制面板");
                }
            }
        }

        private void OnDestroy()
        {
            // 组成生产设备的各控件和模型不可能只销毁一部分, 因此调用了销毁, 则表示该生产设备彻底被释放了内存
            入口类.Log.LogInfo("测试=>生产设备被解构时释放了内存而不是放入复用池");
        }

    }

    [HarmonyPatch(typeof(ToolUse), nameof(ToolUse.Deconstruct))]
    public class Autolathe_Patch_2
    {
        [HarmonyPrefix]
        public static void 拆除生产设备(ToolUse __instance, ConstructionEventInstance eventInstance)
        {
            // 生产设备结构变成套件时, 调用的是该函数
            var __ = eventInstance.Parent as SimpleFabricatorBase;
            if (__)
            {
                入口类.Log.LogInfo($"测试=>拆除了生产设备 ID: {__.ReferenceId}  名称: {__.DisplayName}");

                if (控制面板.生产设备表.ContainsKey(__.ReferenceId))
                {
                    控制面板.生产设备表.Remove(__.ReferenceId);
                    入口类.Log.LogInfo("生产设备已从远程控制面板中移除");
                }
            }
        }
    }
}


// [HarmonyPatch(typeof(InventoryManager), "UsePrimaryComplete")]
//     public class Autolathe_Patch_1
//     {
//         [HarmonyPostfix]
//         public static void 添加引用(InventoryManager __instance)
//         {
//             // 注: ConstructionCursor是拷贝构造函数的母版, 实际放置的是拷贝后的新物体的引用
//             var __ = InventoryManager.ConstructionCursor as SimpleFabricatorBase;
//             if (__)
//             {
//                 入口类.Log.LogInfo($"添加测试: {__.ReferenceId}");

//                 if (!控制面板.车床表.ContainsKey(__.ReferenceId))
//                 {
//                     控制面板.车床表.Add(__.ReferenceId, __);
//                     // 若该设备需要多次安装才能达到完成状态, 还需要额外判断建造状态
//                     入口类.Log.LogInfo("将处于结构状态的生产设备添加到控制面板");
//                 }
//             }
//         }
//     }


// [HarmonyPatch(typeof(Constructor), nameof(Constructor.SpawnConstruct))]

// HydraulicPipeBender
// ElectronicsPrinter
// ToolManufactory
// SecurityPrinter
// RocketManufactory
// AutomatedOven

//  [HarmonyPatch]
//     public static class Autolathe_Patch_1
//     {
//         public static MethodBase TargetMethod()
//         {
//             return AccessTools.Method(typeof(Thing), nameof(Thing.Create), [typeof(Thing), typeof(Vector3), typeof(Quaternion), typeof(long)]);
//             // return AccessTools.Method(typeof(Thing), nameof(Thing.Create), [typeof(Thing), typeof(Vector3), typeof(Quaternion), typeof(long)]).MakeGenericMethod(typeof(Autolathe)); // 指定 T = Autolathe
//         }
//         public static void Postfix<T>(ref T __result, Thing prefab, Vector3 worldPosition, Quaternion worldRotation, long referenceId = 0L)
//         {
//             if (__result is SimpleFabricatorBase __)
//             {
//                 入口类.Log.LogInfo($"添加测试: {__.ReferenceId}");

//                 if (!控制面板.车床表.ContainsKey(__.ReferenceId))
//                 {
//                     控制面板.车床表.Add(__.ReferenceId, __);
//                     // 若该设备需要多次安装才能达到完成状态, 还需要额外判断建造状态
//                     入口类.Log.LogInfo("将处于结构状态的生产设备添加到控制面板");
//                 }
//             }
//         }
//     }