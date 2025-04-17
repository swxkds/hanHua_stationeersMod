using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using Assets.Scripts.Util;
using HarmonyLib;
using TraderUI;
using Trading;

namespace meanran_xuexi_mods_xiaoyouhua
{
    [HarmonyPatch(typeof(TraderContact), nameof(TraderContact.LoadStationContacts), new Type[] { typeof(IEnumerable<StationContactData>), typeof(int) })]
    public class TraderContacts_LoadStationContacts_Patch
    {
        public static void Prefix(ref IEnumerable<StationContactData> contacts)
        {
            var 新 = contacts.ToArray();
            for (var i = 0; i < 新.Length; i++)
                新[i].ContactName = 新[i].ContactName.词条匹配();
            contacts = 新;
        }
        public static void Postfix()
        {
            foreach (var obj in TraderData.AllTraderData)
                for (var i = 0; i < obj.Names.Count; i++)
                    obj.Names[i].Value = obj.Names[i].Value.词条匹配();
        }
    }
}

