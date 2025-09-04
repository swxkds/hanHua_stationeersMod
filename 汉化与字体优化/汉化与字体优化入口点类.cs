using System;
using System.Collections;
using Assets.Scripts.Inventory;
using Assets.Scripts.Objects.Entities;
using Assets.Scripts.UI;
using BepInEx;
using Cysharp.Threading.Tasks;
using HarmonyLib;
using meanran_xuexi_mods;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Rendering;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [BepInPlugin("8cf7ba96-99ac-4d72-b313-63211f3e441c", "汉化与字体优化", "1.0.0")]
    // [BepInProcess("Stationeers.exe")]
    // [BepInProcess("rocketstation.exe")]
    public class 入口类 : BaseUnityPlugin
    {
        public static BepInEx.Logging.ManualLogSource Log;
        public static Harmony 补丁;
        private void Awake()
        {
            Log = base.Logger;
            Log.LogMessage("Plugin 汉化与字体优化 is loaded!");
            var a = AssetsLoad.单例;
            补丁 = new Harmony("汉化与字体优化");
            补丁.PatchAll();
            关闭字体警报();
        }
        private void Start()
        {
            并发构造().Forget();
        }
        private async UniTaskVoid 并发构造()
        {
            while (!Human.LocalHuman)
            { await UniTask.NextFrame(); }
            无线输电面板类.构造();
            手持钻机矿石扫描.构造();
            控制面板.构造();
            InventoryWindowManager.Instance.WindowGrid.spacing = 5f;
            入口类.Log.LogInfo($"成功修改背包UI间距");
            InventoryManager.Instance.UIProgressionBar.遍历修改大小(文本字段偏移表工具<UIProgressionBar>.文本字段偏移表, 24);
        }
        private void 关闭字体警报()
        {
            try // 字体缺少某字时会刷红字,很烦,这个字段就是关闭TMP警报日志
            { Traverse.Create(TMP_Settings.instance).Field("m_warningsDisabled").SetValue(true); }
            catch (Exception) { }
            入口类.Log.LogInfo($"成功关闭字体警报");
        }
    }
}
