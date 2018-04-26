using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace Fafalymo
{
    internal static class GameResources
    {
        private static IDictionary<string, string> BnpNames = new SortedDictionary<string, string>();

        static GameResources()
        {
            ReadResources(Properties.Resources.bnpcname_exh_ko, 0, 1,
                          Properties.Resources.bnpcname_exh_en, 0, 1,
                          BnpNames);
        }

        public static string TranslateBnpName(string value)
        {
            return BnpNames.ContainsKey(value) ? BnpNames[value] : value;
        }

        private static void ReadResources(string koText, int koKeyIndex, int koValueIndex, string enText, int enKeyIndex, int enValueIndex, IDictionary<string, string> dic)
        {
            var krDic = new Dictionary<int, string>();
            var enDic = new Dictionary<int, string>();

            Read(koText, koKeyIndex, koValueIndex, krDic);
            Read(enText, enKeyIndex, enValueIndex, enDic);

            foreach (var v in krDic)
                if (enDic.ContainsKey(v.Key))
                    dic[v.Value] = enDic[v.Key];
        }

        private static void Read(string text, int keyIndex, int valueIndex, IDictionary<int, string> dic)
        {
            using (var stream = new StringReader(text))
            using (var csv = new CsvReader(stream))
            {

                while (csv.Read())
                    if (csv.TryGetField(keyIndex, out int index) && csv.TryGetField(valueIndex, out string value) && !string.IsNullOrEmpty(value))
                        dic.Add(index, value);
            }
        }
    }
}
