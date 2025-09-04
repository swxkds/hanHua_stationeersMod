using System.Reflection;
using System.Text.RegularExpressions;
using Assets.Scripts.UI;
using HarmonyLib;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(Stationpedia), "ForceSearch")]
    public class Stationpedia_ForceSearch_Patch
    {
        // 字段的初始化赋值会被编译器插入到构造函数中, 因此可以在构造函数的IL代码中修改正则表达式字符串来修改_searchRegex
        // 由于Stationpedia的实例是由Unity引擎的预制体系统反序列化得到的, 不调用构造函数, 因此无法补丁构造函数来直接修改IL代码？？？
        // "[^a-zA-Z0-9-\u4e00-\u9fa5]+",  正则：字母、数字、连字符、中文([\u4e00-\u9fa5])以外的字符
        [HarmonyPrefix]
        public static void 修改搜索正则表达式(Stationpedia __instance, string searchText)
        {
            Traverse.Create(__instance).Field("_searchRegex").SetValue(new Regex("[^a-zA-Z0-9-\u4e00-\u9fa5]+", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace));
            入口类.Log.LogMessage($"Stationpedia.ForceSearch拦截成功, 成功修改F1百科的搜索正则表达式");

            // HarmonyPatch不修改原始代码, 只拷贝原始代码并拼接编译新的代码, 因此内存中原始方法依然存在
            // dll调用方法先跳转到编译地址, 然后这个编译地址里保存的才是真实方法地址, 这个方式叫重定向表
            // 打补丁就是将重定向表中的该方法真实地址修改到指向补丁方法, 卸载补丁就是还原成原始方法地址
            入口类.补丁.Unpatch(typeof(Stationpedia).GetMethod("ForceSearch", BindingFlags.Instance | BindingFlags.NonPublic), HarmonyPatchType.Prefix, 入口类.补丁.Id);
            入口类.Log.LogMessage($"Stationpedia.ForceSearch前置补丁已卸载, 调用地址重新指向了原始方法");
        }
    }
}