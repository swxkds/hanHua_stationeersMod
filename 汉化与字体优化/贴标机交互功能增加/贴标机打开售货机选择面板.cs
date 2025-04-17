using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Assets.Scripts.Inventory;
using Assets.Scripts.Localization2;
using Assets.Scripts.Objects;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.Objects.Items;
using Assets.Scripts.UI;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
	public class VendingMachineRefrigerated_InteractWith_Patch
	{
		[HarmonyReversePatch(0)]
		[HarmonyPatch(typeof(VendingMachineRefrigerated), nameof(VendingMachineRefrigerated.InteractWith))]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Thing.DelayedActionInstance VendingMachineRefrigerated_InteractWith(VendingMachineRefrigerated instance, Interactable interactable, Interaction interaction, bool doAction = true)
		{
			return null;
		}

	}

	[HarmonyPatch(typeof(VendingMachineRefrigerated), nameof(VendingMachineRefrigerated.InteractWith))]
	public class VendingMachineRefrigerated_InteractWith_PrefixPatch
	{
		[HarmonyPrefix]
		public static bool 交互事件处理(VendingMachineRefrigerated __instance, ref Thing.DelayedActionInstance __result, Interactable interactable, Interaction interaction, bool doAction = true)
		{
			if (interaction.SourceSlot.Get() is Labeller 贴标机)
			{
				switch (interactable.Action)
				{
					// 贴标机设置堆垛量这个功能不修改
					// 若光线命中的控件是增加和减少冷藏售货机的堆垛量这两个按钮,直接调用原版的交互逻辑
					// 怎么知道控件的类型是干嘛的:原版交互逻辑中,对这两个类型分别做了堆垛量加和减
					case InteractableType.Button3: return true;
					case InteractableType.Button4: return true;
				}

				// interactable.ContextualName: 若是控件,显示"开"关";若是物体,显示名称
				var 动作状态消息 = new Thing.DelayedActionInstance
				{ Duration = 0, ActionMessage = interactable.ContextualName };
				// 冷藏售货机继承自售货机类,但是开发组重写了贴标机的交互逻辑,我们再次重写交互逻辑,将贴标机改成打开物品选择面板
				// 打开物品选择面板所使用的内存结构是相同的,这部分功能调用的汇编代码也是一样的,因此可以将冷藏售货机的实例地址传入售货机的相关函数
				__result = VendingMachine_InteractWith_PrefixPatch.唤醒选择面板(__instance, interaction, 动作状态消息, 贴标机, doAction);
				return false;
			}

			return true;
		}
	}
	public class VendingMachine_InteractWith_Patch
	{
		[HarmonyReversePatch(0)]
		[HarmonyPatch(typeof(VendingMachine), nameof(VendingMachine.InteractWith))]
		[MethodImpl(MethodImplOptions.NoInlining)]
		public static Thing.DelayedActionInstance VendingMachine_InteractWith(VendingMachine instance, Interactable interactable, Interaction interaction, bool doAction = true)
		{
			return null;
		}

	}

	[HarmonyPatch(typeof(VendingMachine), nameof(VendingMachine.InteractWith))]
	public class VendingMachine_InteractWith_PrefixPatch
	{
		[HarmonyPrefix]
		public static bool 交互事件处理(VendingMachine __instance, ref Thing.DelayedActionInstance __result, Interactable interactable, Interaction interaction, bool doAction = true)
		{
			if (interaction.SourceSlot.Get() is Labeller 贴标机)
			{
				// interactable.ContextualName: 若是控件,显示"开"关";若是物体,显示名称
				var 动作状态消息 = new Thing.DelayedActionInstance
				{ Duration = 0, ActionMessage = interactable.ContextualName };
				__result = 唤醒选择面板(__instance, interaction, 动作状态消息, 贴标机, doAction);
				return false;
			}

			return true;
		}

		public static Thing.DelayedActionInstance 唤醒选择面板(VendingMachine 售货机, Interaction 交互双方, Thing.DelayedActionInstance 默认动作状态消息, Labeller 贴标机, bool 玩家有动作么 = true)
		{
			Thing.DelayedActionInstance 动作状态消息 = null;
			默认动作状态消息.ActionMessage = ActionStrings.Vend;

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
				默认动作状态消息.AppendStateMessage("单击打开售货机物品选择面板");
				动作状态消息 = 默认动作状态消息.Succeed();

				if (玩家有动作么)
				{
					// 播放音效
					售货机.PlaySound(SimpleFabricatorBase.Search4BeepHash, 1f, 1f);

					// 售货机内部所有物品DynamicThing 物品, 
					var 下标字典 = new Dictionary<int, int>();
					var 所有物品 = new List<DynamicThing>();
					for (int i = 2; i < 售货机.Slots.Count; i++)
					{
						var _ = 售货机.Slots[i].Get();
						if (_ != null)
						{
							所有物品.Add(_);
							if (!下标字典.ContainsKey(_.PrefabHash))
							{
								下标字典.Add(_.PrefabHash, i);
							}
						}
					}

					var 内部物品表 = new Dictionary<MachineTier, List<DynamicThing>> { { MachineTier.TierOne, 所有物品 } };

					if (交互双方.SourceThing is Human 玩家 && 玩家.State == EntityState.Alive && 玩家.OrganBrain != null && 玩家.OrganBrain.LocalControl)
					{
						// 面板显示所有物品,此函数会自动去除重复相同物品
						// 标题组件是英文字体,但是我懒得去改了
						if (InputPrefabs.ShowInputPanel("Select Item", null, 内部物品表, (SimpleFabricatorBase)null))
						{
							// 当选择某个物品后,执行什么事件
							InputPrefabs.OnSubmit += prefab => 售货机.CurrentIndex = 下标字典[prefab.PrefabHash];
						}
					}
				}
			}

			return 动作状态消息;
		}
	}
}
