using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Pipes;
using HarmonyLib;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.Localization2;

namespace meanran_xuexi_mods_xiaoyouhua
{
    // Interactable: 控件,注意是控件,而不是物体
    // Interaction: 交互双方. 玩家/玩家活动手插槽/视线交互焦点所在的控件或物体或另一个玩家
    // doAction: 玩家当前是在执行动作还是单纯的就看看(拿着工具不操作也算无动作)

    // AttackWith是操作事件,是对整个物体的相关状态变量进行修改
    // InteractWith是交互事件,是对物体内部的某个控件相关状态变量进行修改
    // 管道没有控件(例开关/按钮/刻度盘),因此是AttackWith,管道也没有
    // AttackWith和InteractWith这两个函数由游戏的事件总线每帧调用,会传递交互双方信息,我们只需要根据这些信息的组合,返回对应消息即可
    [HarmonyPatch(typeof(Pipe), nameof(Pipe.AttackWith))]
    public class Pipe_AttackWith_PrefixPatch
    {
        [HarmonyPrefix]
        public static bool 操作事件处理(Structure __instance, ref Thing.DelayedActionInstance __result, Attack attack, bool doAction = true)
        {
            // 发起交互方的活动手上的工具是角磨机,并且管道不是炸开状态
            if (attack.SourceItem is AngleGrinder 角磨机 && __instance is Pipe 管道 && 管道.IsBurst == Assets.Scripts.Networks.PipeBurst.False)
            {
                var 动作消息 = new Thing.DelayedActionInstance
                { Duration = 0, ActionMessage = "" };
                __result = 角磨机爆破管道(管道, 动作消息, 角磨机, doAction);
                return false;
            }

            // 其他情况执行游戏自带的交互逻辑
            return true;
        }

        public static Thing.DelayedActionInstance 角磨机爆破管道(Pipe 管道, Thing.DelayedActionInstance 默认动作消息, AngleGrinder 角磨机, bool 玩家有动作么 = true)
        {
            // 重要!重要!玩家有动作么只有在协程内部才会写入true,在正常时永远是false
            // 当调用默认动作状态消息.Succeed()给IsDisabled写入false时,表示允许开启协程,当单击左键时,创建动作协程
            // 当调用默认动作状态消息.Fail()给IsDisabled写入true,表示禁止开启协程,当单击左键时,直接忽略
            // 在协程函数内部,判断若鼠标左键弹起或者视线乱唤导致光线命中的焦点物体改变了,直接结束协程,不产生任何AttackWith消息
            // 在协程函数内部,若长按时长>=默认动作状态消息.Duration,产生一个玩家有动作么=true的AttackWith消息

            Thing.DelayedActionInstance 动作状态消息 = null;
            // 仅用于协程函数的时长定义,正常不使用Duration
            默认动作消息.Duration = 2;
            // 仅用于正常函数的视线焦点物体工具提示,左上角
            默认动作消息.ActionMessage = "切开";

            // 角磨机电池未安装或者没有电量,仅显示工具提示面板:未通电,无法交互
            if (!角磨机.IsOperable)
            {
                // ActionMessage:禁用时切开是红色字,不允许创建动作协程
                动作状态消息 = 默认动作消息.Fail(GameStrings.DeviceNoPower);
            }

            // 角磨机有电量
            if (动作状态消息 == null)
            {
                // 仅用于正常函数的视线焦点物体工具提示,主面板
                默认动作消息.AppendStateMessage("使用角磨机切开管道,使流体排出");
                // ActionMessage:启用时切开是绿色字,允许创建动作协程
                动作状态消息 = 默认动作消息.Succeed();


                // 协程函数完美执行结束后,发送一条玩家有动作么=true的AttackWith消息,播放角磨机音效,并调用管道高压炸开协程函数
                if (玩家有动作么)
                {
                    // 播放音效
                    角磨机.PlaySound(AngleGrinder.UnEquipGrinderHash, 1f, 1f);
                    管道.BurstPipe(Assets.Scripts.Networks.PipeBurst.Pressure).Forget();
                }
            }

            return 动作状态消息;
        }

    }


    // DelayedActionInstance：创建限时协程函数的各种状态值，如持续时间、消息、状态、音效等
    // Duration: 动作时长>=Duration时,触发回调并结束协程函数
    // OverrideTitle: 提示面板标题
    // ActionMessage: 提示面板显示的操作类型消息,例:开启/关闭/拆除/安装/放置
    // IsDisabled: 是否允许鼠标长按时创建限时协程函数
    // SwitchTitleForTooltip: 是否将标题用作工具提示
    // ExtendedMessage: 扩展消息，通常用于显示更多详细信息;
    // Slider: 滑块的值，限时协程函数内部进度条的值,不需要设置,自动的
    // Selection: 光线命中的控件
    // ActionSoundHash: 动作执行时的音效哈希值;
    // ActionCompleteSoundHash: 动作完成时的音效哈希值;











    // 以下已过时
    // [HarmonyPatch(typeof(Pipe), nameof(Pipe.GetPassiveTooltip))]
    // public class Pipe_GetPassiveTooltip_Patch
    // {
    //     public static void Postfix(ref PassiveTooltip __result)
    //     {
    //         if (!string.IsNullOrEmpty(__result.DeconstructString))
    //             __result.DeconstructString += $"\n需要<color=green>角磨机</color> <color=red>进行爆破</color>";
    //     }
    // }


    // public class 角磨机爆破管道类 : MonoBehaviour
    // {
    //     public static 角磨机爆破管道类 单例;
    //     private Pipe 当前焦点管道;
    //     private Pipe 之前焦点管道;
    //     private GameBase 高亮预制体;
    //     private bool 并发么;
    //     private float 计时器;
    //     public static void 构造()
    //     {
    //         if (单例)
    //             单例.销毁();

    //         单例 = Utils.构造节点<角磨机爆破管道类>("角磨机爆破管道");
    //         单例.初始化();
    //         入口类.Log.LogMessage($"成功构造角磨机爆破管道");
    //     }
    //     private void 销毁()
    //     {
    //         Utils.销毁节点(高亮预制体);
    //         单例 = null;
    //         Utils.销毁节点(this);
    //         入口类.Log.LogMessage($"成功销毁角磨机爆破管道");
    //     }
    //     private void 初始化()
    //     {
    //         并发么 = false;
    //         高亮预制体 = UnityEngine.Object.Instantiate(CursorManager.Instance.CursorSelectionHighlighter, null);
    //         Utils.唤醒节点(this);
    //     }
    //     private void Update()
    //     {
    //         if (WorldManager.IsGamePaused) { return; }
    //         计时器 += Time.deltaTime;
    //         if (计时器 < 0.25f) { return; }
    //         计时器 = 0;

    //         if (当前焦点管道 == null || 当前焦点管道.IsBurst == Assets.Scripts.Networks.PipeBurst.True)
    //         { 高亮预制体.SetVisible(isVisble: false); return; }
    //         else
    //         {
    //             var 主手物品 = InventoryWindowManager.ActiveHand.Get();
    //             if (主手物品 && 主手物品 is AngleGrinder)
    //             {
    //                 if (当前焦点管道 != 之前焦点管道)
    //                 {
    //                     之前焦点管道 = 当前焦点管道;
    //                     var selection = 当前焦点管道.GetSelection();
    //                     高亮预制体.transform.localScale = selection.GetScale();
    //                     高亮预制体.transform.position = selection.GetPosition();
    //                     高亮预制体.transform.rotation = selection.GetRotation();
    //                 }

    //                 高亮预制体.SetVisible(isVisble: true);
    //             }
    //             else
    //             { 高亮预制体.SetVisible(isVisble: false); }
    //         }
    //     }
    //     public void 更新焦点物体(Thing thing)
    //     {
    //         var 管道 = thing as Pipe;
    //         当前焦点管道 = 管道;

    //         if (并发么 || 当前焦点管道 == null || 当前焦点管道.IsBurst == Assets.Scripts.Networks.PipeBurst.True)
    //         { return; }

    //         if (Input.GetMouseButtonDown(0))
    //         {
    //             var 主手物品 = InventoryWindowManager.ActiveHand.Get();
    //             if (主手物品 && 主手物品 is AngleGrinder 角磨机)
    //                 管道爆破读条(当前焦点管道, 角磨机).Forget();
    //         }
    //     }
    //     private async UniTaskVoid 管道爆破读条(Pipe 当前焦点管道, AngleGrinder 角磨机)
    //     {
    //         并发么 = true;

    //         var 工具使用进度条 = InventoryManager.Instance.UIProgressionBar;
    //         工具使用进度条.SetItemName(当前焦点管道.DisplayName); // 进度条上方的物体名称
    //         float 进度 = 0f;
    //         工具使用进度条.SetProgress(进度); // 初始进度条长度为0
    //         工具使用进度条.SetActive(true); // 显示进度条

    //         const float 最大进度 = 1f;
    //         while (进度 < 最大进度)
    //         {
    //             if (Input.GetMouseButtonUp(0) || 当前焦点管道.IsBurst == Assets.Scripts.Networks.PipeBurst.True)
    //                 break;
    //             var 增量 = Time.deltaTime * 2.5f;
    //             角磨机.PoweredValue -= (int)增量;
    //             进度 = Mathf.Min(进度 + 增量, 1);
    //             工具使用进度条.SetProgress(进度);
    //             if (进度 >= 最大进度)
    //             {
    //                 当前焦点管道.BurstPipe(Assets.Scripts.Networks.PipeBurst.Pressure).Forget();
    //                 break;
    //             }
    //             await UniTask.NextFrame();
    //         }

    //         工具使用进度条.SetActive(false);
    //         并发么 = false;
    //     }
    // }
}




// PassiveTooltip result = new PassiveTooltip(toDefault: true);
// result.Title = 管道.DisplayName;
// result.DeconstructString = $"需要<color=green>角磨机</color> <color=red>进行爆破</color>";
// InventoryManager.Instance.TooltipRef.HandleToolTipDisplay(result);

// CursorBoundingLines.OnRenderObject();
// ImGuiExtensions.Rendering.DrawCubeBounds(管道.Bounds.min, 管道.Bounds.max);