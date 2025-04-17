using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class AssetsLoad
    {
        private static AssetsLoad _单例 = null;
        public static AssetsLoad 单例
        {
            get
            {
                if (_单例 == null)
                    _单例 = new AssetsLoad();
                return _单例;
            }
        }
        // public static TMP_FontAsset TMP字体 = null;
        public static Font Font字体 = null;
        private TMP_FontAsset _内置TMP字体 = null;
        public TMP_FontAsset 内置TMP字体
        {
            get
            {
                if (_内置TMP字体 == null)
                    _所有资源 = 查找所有资源();
                return _内置TMP字体;
            }
        }
        private List<UnityEngine.Object> _所有资源 = null;
        public List<UnityEngine.Object> 所有资源
        {
            get
            {
                if (_所有资源 == null)
                    _所有资源 = 查找所有资源();
                return _所有资源;
            }
        }
        public AssetsLoad()
        {
            // .exe所在的目录
            string 游戏程序目录 = System.AppDomain.CurrentDomain.BaseDirectory;
            string 导出 = 游戏程序目录;

            var 程序集 = Assembly.GetExecutingAssembly();
            string 标识 = "meanran_xuexi_mods.Resources.";       // .csproj里配置的默认命名空间

            if (!Directory.Exists(导出))
                Directory.CreateDirectory(导出);

            // 遍历所有Resources
            foreach (var _ in 程序集.GetManifestResourceNames())
            {
                // 打包后的资源斜杆目录符会自动被转换成小数点的点,因此需要替换回去
                if (_.StartsWith(标识))         // 仅处理特定命名空间+目录下的资源
                {
                    const string 汉化目录 = "rocketstation_Data";
                    var 下标 = _.IndexOf(汉化目录);  // 找到所有"rocketstation_Data"目录下的资源
                    if (下标 != -1)
                    {
                        string 相对 = null;
                        string 路径 = null;

                        相对 = _.Substring(下标);          // 移除掉"rocketstation_Data"的上级目录路径
                        下标 = 相对.LastIndexOf('.');      // 将后缀名以外的点替换回系统目录分割符
                        路径 = 相对.Substring(0, 下标).Replace('.', Path.DirectorySeparatorChar);
                        string 文件后缀名 = 相对.Substring(下标);   // 获取文件名
                        相对 = 路径 + 文件后缀名;
                        入口类.Log.LogMessage($"已还原文件路径: {相对}");

                        string 绝对 = Path.Combine(导出, 相对);
                        路径 = Path.GetDirectoryName(绝对);     // 判断去掉文件名信息后的目录路径是否存在
                        if (!Directory.Exists(路径))
                            Directory.CreateDirectory(路径);

                        // 流对象就是双缓冲,在一个大小受限的二进制字节数组上读写,写满了拷贝一次,降低API调用
                        using (Stream 读 = 程序集.GetManifestResourceStream(_))
                        {
                            using (FileStream 写 = new FileStream(绝对, FileMode.Create, FileAccess.Write))
                            { 读.CopyTo(写); }
                        }
                    }
                    else
                    {
                        // const string TMP路径 = "TmpFont.assets";
                        const string Font路径 = "TextFont.assets";

                        if (_.IndexOf(Font路径) != -1)
                        {
                            using (Stream 读 = 程序集.GetManifestResourceStream(_))
                            {
                                Font字体 = AssetBundle.LoadFromStream(读).LoadAllAssets<Font>().FirstOrDefault();
                                入口类.Log.LogMessage($"成功加载Font字体: {Font字体.name}");
                            }
                        }
                        // else if (_.IndexOf(TMP路径) != -1)
                        // {
                        //     using (Stream 读 = 程序集.GetManifestResourceStream(_))
                        //     {
                        //         TMP字体 = AssetBundle.LoadFromStream(读).LoadAllAssets<TMP_FontAsset>().FirstOrDefault();
                        //         入口点类.Log.LogMessage($"成功加载TMP字体: {TMP字体.name}");
                        //     }
                        // }
                    }
                }
            }
        }

        private List<UnityEngine.Object> 查找所有资源()
        {
            List<UnityEngine.Object> 有效 = new List<UnityEngine.Object>();
            object[] All = Resources.FindObjectsOfTypeAll(typeof(UnityEngine.Object));

            foreach (var obj in All)
            {
                var type = obj.GetType();
                // 过滤掉纯资源文件,比如字体/贴图/材质/模型顶点
                if (type == typeof(GameObject) || typeof(Component).IsAssignableFrom(type))
                    有效.Add((UnityEngine.Object)obj);
                else if (type == typeof(TMP_FontAsset))
                {
                    var 字体 = obj as TMP_FontAsset;
                    if (字体 && 字体.name == "font_cjk")
                    {
                        _内置TMP字体 = 字体;
                        入口类.Log.LogMessage($"成功加载TMP字体: {_内置TMP字体.name}");
                    }
                }
            }
            return 有效;
        }
    }
}