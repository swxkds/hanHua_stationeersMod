using HarmonyLib;
using Assets.Scripts.Objects.Motherboards;
using Assets.Scripts.Util;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Assets.Scripts;
using Assets.Scripts.UI;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(ScannedContactUIData), nameof(ScannedContactUIData.GenerateContactStatusText), typeof(ScannedContactUIData))]
    public class ScannedContactUIData_GenerateContactStatusText_Patch
    {
        public static bool Prefix(ref string __result, ScannedContactUIData uiData)
        {
            if (uiData.CurrentlyTrading)
            {
                __result = "正在交易";
            }
            switch (uiData.ContactState)
            {
                case ContactState.None:
                    __result = string.Empty; break;
                case ContactState.Unknown:
                    __result = "预估时间"; break;
                case ContactState.Resolving:
                    __result = "完成时间: " + StringManager.Get(uiData.CurrentTimeTillResolve); break;
                case ContactState.Resolved:
                    __result = "目标夹角: " + StringManager.Get(uiData.DishAlignment) + "°   已锁定位置" + 计算转轴角度(uiData); break;
                case ContactState.Interrogating:
                    __result = "通讯进度: " + StringManager.Get((int)Mathf.Clamp(uiData.NormalizedSecondsConnected / uiData.SecondsRequiredToContact * 100f, 0f, 100f)) + "%"; break;
                case ContactState.Contacted:
                    __result = "等待着陆许可"; break;
                default:
                    __result = string.Empty; break;
            }
            return false;
        }
        private static string 计算转轴角度(ScannedContactUIData uiData)
        {
            var 目标商船 = uiData.TraderContact;
            var 地面天线 = uiData.ScanningDish;

            // 入口点类.Log.LogMessage($"目标商船: {目标商船.Angle} - {目标商船.Angle.magnitude}");
            // 入口点类.Log.LogMessage($"地面天线: {地面天线.DishForward} - {地面天线.DishForward.magnitude}");

            // 注:此处的向量是单位向量
            float 方位夹角 = Mathf.Atan2(目标商船.Angle.x, 目标商船.Angle.z) - Mathf.Atan2(地面天线.DishForward.x, 地面天线.DishForward.z);
            方位夹角 = Mathf.Rad2Deg * 方位夹角;

            float 俯仰夹角 = Mathf.Asin(地面天线.DishForward.y / 地面天线.DishForward.magnitude) - Mathf.Asin(目标商船.Angle.y / 目标商船.Angle.magnitude);
            俯仰夹角 = Mathf.Rad2Deg * 俯仰夹角;

            float 方位 = (float)((地面天线.GetLogicValue(LogicType.Horizontal) + 方位夹角 + 360) % 360);
            float 俯仰 = (float)((地面天线.GetLogicValue(LogicType.Vertical) + 俯仰夹角) % 90);

            return $"[{Mathf.Round(方位)}°,{Mathf.Round(俯仰)}°]";
        }
    }
    [HarmonyPatch(typeof(CommsTerminal), nameof(CommsTerminal.ShowInputPanel), new Type[] { typeof(TraderContact), typeof(CommsMotherboard) })]
    public class CommsTerminal_ShowInputPanel_Patch
    {
        private static MethodInfo __WaitForInput = typeof(CommsTerminal).GetMethod("WaitForInput", BindingFlags.Instance | BindingFlags.NonPublic);
        private static FieldInfo LastDish = typeof(CommsTerminal).GetField("LastDish", BindingFlags.Static | BindingFlags.NonPublic);
        private static FieldInfo LastMessageWattageRatio = typeof(CommsTerminal).GetField("LastMessageWattageRatio", BindingFlags.Static | BindingFlags.NonPublic);
        public static bool Prefix(ref bool __result, ref TraderContact contact, ref CommsMotherboard commsMotherboard)
        {
            if (CommsTerminal.InputState != InputPanelState.None)
            {
                __result = false;
                return false;
            }
            CursorManager.SetCursor(false);
            CommsTerminal.CurrentContact = contact;
            CommsTerminal.CommsMotherboard = commsMotherboard;
            var selectedDish = commsMotherboard.SelectedDish;
            CommsTerminal.InputState = InputPanelState.Waiting;
            CommsTerminal.Instance.SetVisible(true);
            CommsTerminal.Instance.StartCoroutine((IEnumerator)__WaitForInput.Invoke(CommsTerminal.Instance, null));
            CommsTerminal.Instance.TitleText.text = (contact.ContactName ?? "");
            List<string> messageText = contact.GetMessageText(CommsTerminal.CommsMotherboard);
            bool flag = messageText.Count > 1;
            float wattageOnContact = selectedDish.GetWattageOnContact(contact);
            float num = contact.InterrogationWattageRatio(selectedDish);
            float f = contact.TotalInterrogationTimeAtWattage(wattageOnContact);
            float value = contact.InterrogationTimeRemainingAtWattage(wattageOnContact);
            if (contact.InterrogatingDish != null && contact.InterrogatingDish != selectedDish)
            {
                messageText.Add("请等待当前通讯任务完成");
                CommsTerminal.Instance.ButtonConfirmText.SetText("N/A", true);
                CommsTerminal.Instance.ButtonConfirm.interactable = false;
                CommsTerminal.Instance.Icon.gameObject.SetActive(false);
            }
            else if (contact.InterrogatingDish && !contact.Contacted)
            {
                messageText.Add("通讯中");
                messageText.Add("天线有效功率: " + StringManager.Get(Mathf.Floor(wattageOnContact)) + "W   天线实际功率: " + StringManager.Get(selectedDish.Setting) + "W");
                messageText.Add("预估完成时间: " + value.ToStringRounded() + "秒");
                messageText.Add("通讯天线名称: " + selectedDish.DisplayName);
                CommsTerminal.Instance.ButtonConfirmText.SetText("N/A", true);
                CommsTerminal.Instance.ButtonConfirm.interactable = false;
                CommsTerminal.Instance.Icon.gameObject.SetActive(false);
            }
            else if (contact.Contacted)
            {
                if (flag)
                {
                    CommsTerminal.Instance.ButtonConfirmText.SetText("着陆", true);
                    CommsTerminal.Instance.ButtonConfirm.interactable = false;
                    CommsTerminal.Instance.Icon.gameObject.SetActive(false);
                }
                else
                {
                    messageText.Add("通讯完成，可发放着陆许可");
                    CommsTerminal.Instance.ButtonConfirmText.SetText("着陆", true);
                    CommsTerminal.Instance.ButtonConfirm.interactable = true;
                    CommsTerminal.Instance.Icon.gameObject.SetActive(true);
                }
            }
            else if (!contact.Contacted && !contact.InterrogatingDish)
            {
                messageText.Add(string.Concat(new string[]
                {
                    "预估通讯功率: ",
                    StringManager.Get(Mathf.Ceil(CommsTerminal.CurrentContact.MinimumWattsToContact)),
                    "W   预估通讯时长: ",
                    StringManager.Get(Mathf.Ceil(CommsTerminal.CurrentContact.SecondsRequiredToContact)),
                    "秒"
                }));
                messageText.Add("天线有效功率: " + StringManager.Get(Mathf.Floor(wattageOnContact)) + "W   天线实际功率: " + StringManager.Get(selectedDish.Setting) + "W\n将天线对准可提高天线有效功率");
                messageText.Add("通讯天线名称: " + selectedDish.DisplayName);
                if (num >= 1f)
                {
                    if (selectedDish.InterrogatingContact != null)
                    {
                        messageText.Add("通讯天线正在运行中, 请稍候, 或使用单独的通信终端");
                        CommsTerminal.Instance.ButtonConfirmText.SetText("通讯", true);
                        CommsTerminal.Instance.ButtonConfirm.interactable = false;
                        CommsTerminal.Instance.Icon.gameObject.SetActive(false);
                    }
                    else
                    {
                        messageText.Add("预估 " + StringManager.Get(Mathf.Ceil(f)) + " 秒后完成通讯");
                        CommsTerminal.Instance.ButtonConfirmText.SetText("通讯", true);
                        CommsTerminal.Instance.ButtonConfirm.interactable = true;
                        CommsTerminal.Instance.Icon.gameObject.SetActive(true);
                    }
                }
                else
                {
                    messageText.Add("当前天线有效功率不足,无法与目标进行通讯");
                    CommsTerminal.Instance.ButtonConfirmText.SetText("N/A", true);
                    CommsTerminal.Instance.ButtonConfirm.interactable = false;
                    CommsTerminal.Instance.Icon.gameObject.SetActive(false);
                }
            }
            LastDish.SetValue(null, selectedDish);
            LastMessageWattageRatio.SetValue(null, num);
            CommsTerminal.Instance.Message.text = CommsTerminal.ParseText(messageText);
            __result = true;

            return false;
        }
    }
}