using HarmonyLib;
using System;
using System.Collections.Generic;
using Assets.Scripts;
using System.IO;
using System.Reflection;
using Reagents;
using Assets.Scripts.Atmospherics;
using static Assets.Scripts.Localization;
using System.Linq;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(Localization.Language), nameof(Localization.Language.UpdateRecord), new Type[] { typeof(Localization.Record), typeof(Dictionary<int, string>) })]
    public class Localization_Language_UpdateRecord_Patch
    {
        // private static bool 打印么 = true;
        public static void Postfix(ref Localization.Language __instance, ref Localization.Record stringLocalized, ref Dictionary<int, string> dictionary)
        {
            // Localization.Language类从本地翻译中解析词条组成Dictionary<TKey, TValue>,每一个语言文件都会单独构建一个对象
            // Localization类的内存结构和Language类高度相似,当切换语言时,就是从对应的Language对象将Dictionary<TKey, TValue>一条条覆盖过去
            // 硬编码的部分无法通过此操作修改
            int hashCode = stringLocalized.GetHashCode();
            dictionary[hashCode] = dictionary[hashCode].词条匹配();
            // 入口点类.Log.LogInfo($"找到词条: {stringLocalized.Key} => {翻译}");

            // if (打印么)
            //     打印词条(__instance);
        }
        private static void 打印词条(Localization.Language __instance)
        {
            using (StreamWriter 写 = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Desktop)}/11.txt", append: false))
            {
                var LocalizedInterfaces = Localization.LocalizedInterfaces;
                写.WriteLine($"LocalizedInterfaces=>\n{string.Join("\n", LocalizedInterfaces.Select(t => $"{t.StringKey} == {t.TextMesh.text}"))}");
                写.WriteLine("\n\n\n");

                var ReagentName = typeof(Localization).GetField("ReagentName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Reagent, string>;
                写.WriteLine($"ReagentName=>\n{string.Join("\n", ReagentName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var GasName = typeof(Localization).GetField("GasName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Chemistry.GasType, string>;
                写.WriteLine($"GasName=>\n{string.Join("\n", GasName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var KeyName = typeof(Localization).GetField("KeyName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"KeyName=>\n{string.Join("\n", KeyName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var ActionName = typeof(Localization).GetField("ActionName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"ActionName=>\n{string.Join("\n", ActionName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var ThingLocalized = typeof(Localization).GetField("ThingLocalized", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, LocalizationThingDat>;
                写.WriteLine($"ThingLocalized=>\n{string.Join("\n", ThingLocalized.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var SlotsName = typeof(Localization).GetField("SlotsName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"SlotsName=>\n{string.Join("\n", SlotsName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var InterfaceText = typeof(Localization).GetField("InterfaceText", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"InterfaceText=>\n{string.Join("\n", InterfaceText.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var InteractableName = typeof(Localization).GetField("InteractableName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"InteractableName=>\n{string.Join("\n", InteractableName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var ColorNames = typeof(Localization).GetField("ColorNames", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"ColorNames=>\n{string.Join("\n", ColorNames.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var MineableName = typeof(Localization).GetField("MineableName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"MineableName=>\n{string.Join("\n", MineableName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var ToolTips = typeof(Localization).GetField("ToolTips", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"ToolTips=>\n{string.Join("\n", ToolTips.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var GameTips = typeof(Localization).GetField("GameTips", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as List<string>;
                写.WriteLine($"GameTips=>\n{string.Join("\n", GameTips)}");
                写.WriteLine("\n\n\n");

                var FallbackReagentName = typeof(Localization).GetField("FallbackReagentName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Reagent, string>;
                写.WriteLine($"FallbackReagentName=>\n{string.Join("\n", FallbackReagentName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackGasName = typeof(Localization).GetField("FallbackGasName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<Chemistry.GasType, string>;
                写.WriteLine($"FallbackGasName=>\n{string.Join("\n", FallbackGasName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackKeyName = typeof(Localization).GetField("FallbackKeyName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackKeyName=>\n{string.Join("\n", FallbackKeyName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackActionName = typeof(Localization).GetField("FallbackActionName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackActionName=>\n{string.Join("\n", FallbackActionName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackThingsLocalized = typeof(Localization).GetField("FallbackThingsLocalized", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, LocalizationThingDat>;
                写.WriteLine($"FallbackThingsLocalized=>\n{string.Join("\n", FallbackThingsLocalized.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackSlotsName = typeof(Localization).GetField("FallbackSlotsName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackSlotsName=>\n{string.Join("\n", FallbackSlotsName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackInterfaceText = typeof(Localization).GetField("FallbackInterfaceText", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackInterfaceText=>\n{string.Join("\n", FallbackInterfaceText.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackInteractableName = typeof(Localization).GetField("FallbackInteractableName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackInteractableName=>\n{string.Join("\n", FallbackInteractableName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackColorNames = typeof(Localization).GetField("FallbackColorNames", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackColorNames=>\n{string.Join("\n", FallbackColorNames.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackMineableName = typeof(Localization).GetField("FallbackMineableName", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackMineableName=>\n{string.Join("\n", FallbackMineableName.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");

                var FallbackToolTips = typeof(Localization).GetField("FallbackToolTips", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as Dictionary<int, string>;
                写.WriteLine($"FallbackToolTips=>\n{string.Join("\n", FallbackToolTips.Select(t => $"{t.Key} == {t.Value}"))}");
                写.WriteLine("\n\n\n");
                //     写.WriteLine("=======================Reagents===========================");
                //     foreach (var v in __instance.Reagents)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Gases===========================");
                //     foreach (var v in __instance.Gases)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Actions===========================");
                //     foreach (var v in __instance.Actions)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Things===========================");
                //     foreach (var v in __instance.Things)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Slots===========================");
                //     foreach (var v in __instance.Slots)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Interactables===========================");
                //     foreach (var v in __instance.Interactables)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Interface===========================");
                //     foreach (var v in __instance.Interface)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Colors===========================");
                //     foreach (var v in __instance.Colors)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Keys===========================");
                //     foreach (var v in __instance.Keys)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================Mineables===========================");
                //     foreach (var v in __instance.Mineables)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================ScreenSpaceToolTips===========================");
                //     foreach (var v in __instance.ScreenSpaceToolTips)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");

                //     写.WriteLine("=======================GameStrings===========================");
                //     foreach (var v in __instance.GameStrings)
                //         写.WriteLine(v.Key + "\t" + v.Value);
                //     写.WriteLine("============================================================");
            }
        }
    }
}