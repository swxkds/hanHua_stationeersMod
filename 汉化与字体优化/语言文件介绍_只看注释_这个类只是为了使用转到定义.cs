using System;
using HarmonyLib;
using Assets.Scripts;

namespace meanran_xuexi_mods_xiaoyouhua
{
    public class 语言文件介绍_只看注释_这个类只是为了使用转到定义
    {
        public void 语言文件_本地()
        {
            // 简体中文的语言文件是simplified_chinese.xml;

            // 以下是语言文件的词条解析举例1
            //  <RecordThing>                                                 // 此条目在内存中用RecordThing类的实例来保存
            //    <Key>ItemMiningCharge</Key>                                 // RecordThing的第一个成员变量保存这个字符串
            //    <Value>矿用炸药</Value>                                      // RecordThing的第二个成员变量保存这个字符串
            //    <Description>低成本、高当量炸药, 配备10秒计时器</Description>  // RecordThing的第三个成员变量保存这个字符串
            //  </RecordThing> 

            // 以下是语言文件的词条解析举例2
            // <Record>                     // 此条目在内存中用Record类的实例来保存
            //   <Key>Powered</Key>         // Record的第一个成员变量保存这个字符串
            //   <Value>已供电</Value>       // Record的第二个成员变量保存这个字符串
            // </Record>

            // 以下是语言文件的词条解析举例3
            // <Record>                                                 // 此条目在内存中用Record类的实例来保存
            //   <Key>TradeItemStackChild</Key>                         // Record的第一个成员变量保存这个字符串
            //   <Value>包含 {LOCAL:Quantity} × {LOCAL:Item}</Value>    // Record的第二个成员变量保存这个字符串
            // </Record>
            // 注:可以看到这个Value里有两个特殊字符串{LOCAL:Quantity}和{LOCAL:Item},{}的意思是占位符,{}里的字符串
            //    只是对这个占位符的说明,在加载进内存后会将{}里的字符串变成格式化占位符的{0}、{1}
            // 示例:
            // string name = "世界"; string value = "这个";
            // String.Format("你好,{0}美好的{1}!", value, name);  // 输出:你好,这个美好的世界!

            // 以下是语言文件的词条解析举例4
            //  <RecordReagent>             // 此条目在内存中用RecordReagent类的实例来保存
            //   <Key>Flour</Key>           // RecordReagent的第一个成员变量保存这个字符串
            //   <Value>面粉</Value>        // RecordReagent的第二个成员变量保存这个字符串
            //   <Unit>g</Unit>             // RecordReagent的第三个成员变量保存这个字符串
            // </RecordReagent>

            // 以上词条解析中,可以看到第一个变量全部都是Key,这是固定的一串字符串,所有语言文件的Key都共用同一套
            // 词条被加载进内存后,以Key作为键、Value和Description作为值保存在Dictionary字典中,切换语言时,只需变更到对应语言的字典
        }
        public void 语言文件不全_从GameStrings中找缺失的Key()
        {
            // 语言文件和游戏代码是不同的人负责, 工作进度不一定能同步, 游戏代码若访问Key对应的条目实例找不到, 肯定会报错
            // 因此游戏代码首先为Key生成一个默认的词条实例, 然后解析语言文件, 若语言文件中存在同样的Key, 则以语言文件为准

            // 其中一部分默认词条实例的生成在 Assets.Scripts.Localization2.GameStrings 这个类中的字段全是用static关键字修饰过的
            // static关键字修饰=> 这个字段的初始化赋值会被编译器插入到静态构造函数中, 而静态构造函数的调用会被编译器插入到程序的加载代码中
            // 因此在启动游戏程序时, 就生成了默认的词条实例

            // 以下是这部分默认词条实例的解析举例1
            // public static readonly GameString TradeItemStackChild = GameString.Create("TradeItemStackChild", "Includes {LOCAL:Quantity} x {LOCAL:Item}", "Item", "Quantity");
            // "TradeItemStackChild"=> GameString.Create方法的第一个参数是词条的Key
            // "Includes {LOCAL:Quantity} x {LOCAL:Item}"=> GameString.Create方法的第二个参数是词条的Value
            // "Item", "Quantity"=> 若是在第二个参数中有{}占位符, 有多少个{}则此处传递多少个变长参数
            // 按照传参顺序,"Item"是第0个变长参数,"Quantity"是第1个变长参数, 则修饰后的Value变成"Includes {1} x {0}"
        }

        public void 语言文件不全_从Localization中找缺失的Key()
        {
            // 以下为本地语言文件加载到内存中的流程
            // WorldManager.ManagerAwake => Assets.Scripts.Localization.GetLanguages () => Assets.Scripts.Localization.LoadLanguageFilesFromXml () => Assets.Scripts.Localization.AddLanguagePage()
            // 注:在LoadLanguageFilesFromXml方法中反序列化本地语言文件并将各词条保存到Language类实例的各个List<>中
            // 经过以上步骤,所有本地语言文件都被加载到内存中并按照LanguageCode添加到Assets.Scripts.Localization.LanguageData中
            // 然后游戏代码从Assets.Scripts.Localization.LanguageData中取出Language类实例并调用了Assets.Scripts.Localization.LanguageFolder.LoadAll()切换到本语言字典

            // 若本地语言文件本身就是不全的, 游戏代码若访问Key对应的条目实例找不到, 肯定会报错
            // 因此游戏代码首先为Key生成一个默认的词条实例, 然后解析语言文件, 若语言文件中存在同样的Key, 则以语言文件为准
            // 不是通过static关键字修饰的方式将初始化赋值插入到静态构造函数中, 而是通过Unity引擎预制体系统反序列化直接赋值的
            // 以下这些名字中带有Fallback的就是默认的词条实例的保存地方, 但是这里的Key并不是词条Key, 而是通过Crc32算法转换后的整数Key
            // 示例:
            // var 整数Key = Animator.StringToHash(词条Key);
            // Assets.Scripts.Localization.FallbackInteractableName.TryGetValue(整数Key, out 词条Value);
            // 因此需要使用dnSpy.exe打开dll并查找以下这些集合被谁使用,因为在游戏设计中,词条Key作为固定的字符串,传递性能开销非常小
            // 一般不会直接在上层使用整数Key,而是使用可读性更好的词条Key,当找到调用了Animator.StringToHash(XX)这句代码时,这个XX就是词条Key

            // Assets.Scripts.Localization.FallbackReagentName
            // Assets.Scripts.Localization.FallbackGasName
            // Assets.Scripts.Localization.FallbackKeyName
            // Assets.Scripts.Localization.FallbackActionName
            // Assets.Scripts.Localization.FallbackThingsLocalized
            // Assets.Scripts.Localization.FallbackSlotsName
            // Assets.Scripts.Localization.FallbackInterfaceText
            // Assets.Scripts.Localization.FallbackToolTips
            // Assets.Scripts.Localization.FallbackInteractableName
            // Assets.Scripts.Localization.FallbackColorNames
            // Assets.Scripts.Localization.FallbackMineableName
        }
    }
}