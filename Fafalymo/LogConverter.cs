using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Fafalymo
{
    static class LogConverter
    {
        private static readonly Regex PluginVersion = new Regex("FFXIV PLUGIN VERSION: ([^,]+)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public static string Translate(string line)
        {
            if (line.StartsWith("00|"))
                return line;

            var ll = line.Split('|');

            if (int.TryParse(ll[0], out int packetType))
            {
                switch (packetType)
                {
                    #region 253 ACT Version
                    case 253:
                        var m = PluginVersion.Match(ll[2]);
                        if (m.Success)
                            ll[2] = ll[2].Replace(m.Groups[1].Value, "1.7.0.11");
                        break;
                    #endregion

                    #region 03 AddCombatant
                    //0  1                                 2        3
                    //03|2018-04-11T20:37:20.3460000+09:00|1008a9e5|륜아린|13|46|ddf7|1ba8|0|40e1235a0fcb323a53be446f892c6208
                    case 03:
                        if (ll.Length > 3)
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                        break;
                    #endregion

                    #region 04 RemoveCombatant
                    //0  1                                 2        3
                    //04|2018-04-11T20:38:41.7210000+09:00|4000c10b|요정 셀레네|0|46|0|0|1008a19a|12470716a0a01a3ca4e1f973b26fc436
                    case 04:
                        if (ll.Length > 3)
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                        break;
                    #endregion

                    #region 20 StartsCasting
                    //0  1                                 2        3    4    5        6        7
                    //20|2018-04-11T20:37:43.2370000+09:00|1008B999|큐니|1D94|프로테스|1008B999|큐니||ba6a543f7d42ef197c9f681578dfc7cc
                    case 20:
                        if (ll.Length > 7)
                        {
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                            ll[7] = GameResources.TranslateBnpName(ll[7]);
                        }
                        break;
                    #endregion

                    #region 21 Ability
                    //0  1                                 2        3             4   5            6        7
                    //21|2018-04-13T21:34:18.4740000+09:00|1028F4A2|Sasarino Mari|DD2|Goring Blade|40002B3E|Kefka|...
                    case 21:
                        if (ll.Length > 7)
                        {
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                            ll[7] = GameResources.TranslateBnpName(ll[7]);
                        }

                        // Ability_Fix_Value(ll);
                        break;
                    #endregion

                    #region 22 Ability (Area of Effect)
                    //0  1                                 2        3             4   5            6        7
                    //21|2018-04-13T21:34:18.4740000+09:00|1028F4A2|Sasarino Mari|DD2|Goring Blade|40002B3E|Kefka|...
                    case 22:
                        if (ll.Length > 7)
                        {
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                            ll[7] = GameResources.TranslateBnpName(ll[7]);
                        }

                        // Ability_Fix_Value(ll);
                        break;
                    #endregion

                    #region 23 Ability (Cancel)
                    //0  1                                 2        3    4    5        6         78
                    //23|2018-04-11T20:39:11.1400000+09:00|1008B999|큐니|1D12|말레피가|Cancelled| |76c30918ddaba06d7331a8849d6f3bf0
                    case 23:
                        if (ll.Length > 3)
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                        break;
                    #endregion

                    #region 24 DoT
                    //0  1                                 2        3
                    //24|2018-04-13T22:07:33.4890000+09:00|4000CCAF|Kefka|DoT|0|250C|31511364|31559062|12000|12000|1000|1000| 0.07623291| 0.8086548|0||0337775c5b5d6ffecd3503605593935e
                    case 24:
                        if (ll.Length > 3)
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                        break;
                    #endregion

                    #region 25 Death
                    //0  1                                 2        3           4        5
                    //25|2018-04-11T20:39:31.7640000+09:00|4000C197|석화 구체 I|E0000000|            ||ae668daa538d17a382350aaa3db2f5d8
                    //25|2018-03-13T20:21:46.2520000+09:00|1008B37C|루비블러드 |40002D48|카타스트로피||1611ce1093d7d8e71f933f26991039c2

                    case 25:
                        if (ll.Length > 3)
                        {
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                            ll[5] = GameResources.TranslateBnpName(ll[5]);
                        }
                        break;
                    #endregion

                    #region 26 Buff
                    //0  1                                 2   3            4     5        6      7        8        9  10       11
                    //26|2018-04-11T21:35:47.0020000+09:00|2d5|꿰뚫는 검격 |21.00|1008A9E5|륜아린|4000F5B1|엑스데스|00|15649287|56823||6ac0f894b2d38e67104a331defed055f
                    case 26:
                        if (ll.Length > 8)
                        {
                            ll[6] = GameResources.TranslateBnpName(ll[6]);
                            ll[8] = GameResources.TranslateBnpName(ll[8]);
                        }
                        break;
                    #endregion

                    #region 27 TargetIcon
                    //0  1                                 2        3
                    //27|2018-04-11T20:39:45.1810000+09:00|10089EED|웨이드|0000|0000|0073|0000|0000|0000||be5b51dd43f4fc6bfbb6040e1b2d947d
                    case 27:
                        if (ll.Length > 3)
                            ll[3] = GameResources.TranslateBnpName(ll[3]);
                        break;
                    #endregion

                    #region 30 Buff (Remove)
                    //0  1                                 2   3                   4    5        6      7        8
                    //30|2018-04-11T20:38:53.4760000+09:00|323|용의 꼬리 시전 가능|0.00|1008AB80|쿨쿨자|1008AB80|쿨쿨자|00|41765|41765||147683b4e68862d8b1a803a928f5ed3e
                    case 30:
                        if (ll.Length > 8)
                        {
                            ll[6] = GameResources.TranslateBnpName(ll[6]);
                            ll[8] = GameResources.TranslateBnpName(ll[8]);
                        }
                        break;
                    #endregion
                }
            }

            return string.Join("|", ll);
        }

        static readonly MD5 md5 = MD5.Create();
        public static string RecalcHash(string line, int logLine)
        {
            var frontPart = line.Substring(0, line.LastIndexOf('|') + 1);

            return frontPart + ToHexString(md5.ComputeHash(Encoding.UTF8.GetBytes(frontPart + logLine.ToString())));
        }

        private static string ToHexString(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length * 2);
            for (int i = 0; i < bytes.Length; ++i)
                sb.Append(bytes[i].ToString("x2"));

            return sb.ToString();
        }
        
        private static void Ability_Fix_Value(string[] ll)
        {
            if (ll.Length <= 23)
                return;

            long k;

            for (int i = 8; i <= 23; i += 2)
            {
                if (ll[i].Length == 0)
                    continue;

                try
                {
                    k = Convert.ToInt64(ll[i], 16);
                }
                catch
                {
                    continue;
                }

                if (k != 0)
                {
                    var value = ll[i + 1].PadLeft(8, '0');

                    // Fixed Damage
                    /*
                    if ((Convert.ToInt16(text2.Substring(2, 2), 16) & 0x80) == 0)               PART B
                    entry.amount = Convert.ToInt32(data2.Substring(4, 4), 16);                  PART C
				    if ((Convert.ToInt32(data2.Substring(2, 1), 16) & 4) == 4)                  PART B
					    entry.amount *= 10L;                                                    PART C * 10
                    */
                    // [00][0][0][0000]
                    // 0 | 0 | Unkown
                    // 2 | 2 | Flag
                    // 4 | 4 | Amount

                    var flag = value.Substring(2, 2);
                    long amount = Convert.ToInt32(value.Substring(4, 4), 16);

                    if ((Convert.ToInt32(value.Substring(2, 1), 16) & 4) == 4)
                        amount *= 10L;

                    /*
                    if ((Convert.ToInt16(text2.Substring(6, 2), 16) & 0x80) == 0)               PART B

                    entry.amount = Convert.ToInt32(data2.Substring(0, 4), 16);                  PART C
                    if ((Convert.ToInt32(data2.Substring(4, 1), 16) & 4) == 4)                  PART A
                        entry.amount += Convert.ToInt32(data2.Substring(6, 2), 16) * 65535;
                    */
                    // [00][0][0][0000]
                    // 0 | 4 | Amount
                    // 4 | 1 | Is Over 65535
                    // 5 | 0 | Unknown
                    // 6 | 2 | Flag

                    var sb = new StringBuilder();
                    if (amount <= 0xFFFF)
                    {
                        sb.Append(amount.ToString("X4"));
                        sb.Append("0");
                        sb.Append("0");
                        sb.Append(flag);
                    }
                    else
                    {
                        sb.Append((amount % 0xFFFF).ToString("X4"));
                        sb.Append("4");
                        sb.Append("0");
                        sb.Append(((Convert.ToUInt16(flag, 16) & ~((ushort)0x40U)) | (ushort)(amount / 0xFFFF)).ToString("X2"));
                    }

                    ll[i + 1] = Convert.ToInt64(sb.ToString(), 16).ToString("X");
                }
            }
        }
    }
}
