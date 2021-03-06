﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace diseasedataarrange
{
    public static class DdrHelp
    {

        public static float? _Div(this int? a, int? b)
        {
            float? r = null;
            if (b != null && b != 0 && a != null)
            {
                r = (float)a / b;
            }
            return r;
        }

        public static float? _Div(this float? a, float? b)
        {
            float? r = null;
            if (b != null && b != 0 && a != null)
            {
                r = (float)a / b;
            }
            return r;
        }

        public static int? _ParseInt(this string s)
        {
            int r = 0;
            if (Int32.TryParse(s, out r))
            {
                return r;
            }
            else
            {
                return null;
            }
        }

        public static float? _ParseFlt(this string s)
        {
            float r = 0;
            if (float.TryParse(s, out r))
            {
                return r;
            }
            else
            {
                return null;
            }
        }

        public static DateTime? _ParseDateTime(this string s, string fmt, System.Globalization.CultureInfo info)
        {
            DateTime r = new DateTime();
            if (DateTime.TryParseExact(s, fmt, info, System.Globalization.DateTimeStyles.None, out r))
            {
                return r;
            }
            else
            {
                return null;
            }
        }

        public static bool _Parse(ref DateTime? tm, string s, string fmt, System.Globalization.CultureInfo info)
        {
            DateTime r = new DateTime();
            if (DateTime.TryParseExact(s, fmt, info, System.Globalization.DateTimeStyles.None, out r))
            {
                tm = r;
                return true;
            }
            else
            {
                tm = null;
                return false;
            }
        }

        public static bool _Parse(ref int? v, string s)
        {
            int r = 0;
            if (Int32.TryParse(s, out r))
            {
                v = r;
                return true;
            }
            else
            {
                v = null;
                return false;
            }
        }

        public static bool _Parse(ref float? v, string s)
        {
            float r = 0;
            if (float.TryParse(s, out r))
            {
                v = r;
                return true;
            }
            else
            {
                v = null;
                return false;
            }
        }

        public static bool _Parse(ref string v, string s, string fmt, string[] cols)
        {
            v = string.IsNullOrWhiteSpace(fmt)? 
                s : 
                string.Format(fmt, cols);
            return string.IsNullOrWhiteSpace(v);
        }

        public static float? _Norm(this float? v, float? min, float? max)
        {
            var len = max - min;
            v = v - min;
            if (len != null && len > 0 && v != null)
            {
                return v / len;
            }
            else
            {
                return null;
            }
        }

        public static float? _FltDays(this DateTime? a, DateTime? b)
        {
            float? r = null;
            if (a != null && b != null)
            {
                TimeSpan ts = a.Value.Subtract(b.Value);
                r = (float)(ts.TotalSeconds / 86400.0);
            }
            return r;
        }

        public static string[] GetEnumNames<T>()
        {
            var fields = typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public);

            return fields.Select(x => x.Name).ToArray();
        }

        public static string[] GetEnumNames(Enum e)
        {
            var fields = e.GetType().GetFields(BindingFlags.Static | BindingFlags.Public);

            return fields.Select(x => x.Name).ToArray();
        }

        public static PropertyInfo[] GetPropertiesByNames<T>(string[] names)
        {
            return typeof(T).GetProperties()
                .Where(x => x.CanRead && names.Contains(x.Name))
                .OrderBy(x => Array.IndexOf(names, x.Name))
                .ToArray();
        }

        public static object[] GetValuseByProperties<T>(T t, PropertyInfo[] props)
        {
            return props.Select(x => x.GetValue(t, null)).ToArray();
        }

        public static string FormatString(string[] names, string separator, string start, string end, bool allQuo, params int[] quoIdxs)
        {
            var fmt = CreateStringFormat(names.Length, separator, start, end, allQuo, quoIdxs);            
            return string.Format(fmt, names);
        }

        public static string CreateStringFormat(int len, string separator, string start, string end, bool allQuo, params int[] quoIdxs)
        {
            StringBuilder text = new StringBuilder(len * 10);

            text.Append(start);

            for (var i = 0; i < len; i++)
            {
                var ss = allQuo || quoIdxs.Contains(i) ? "\"" : "";
                var sd = i == 0 ? "" : separator;
                text.AppendFormat("{3}{4}{0}{1}{2}{4}", "{", i, "}", sd, ss);
            }

            text.Append(end);

            return text.ToString();
        }

        public static float? _CalcRate(this float? v, float? v1)
        {
            if (v != null)
            {
                return v._Div(v+ v1);
            }
            else
            {
                return null;
            }
        }

        public  static string CreateStringFormat(string[] names, string separator, string start, string end, bool allQuo, params int[] quoIdxs)
        {
            int len = names.Length;

            StringBuilder text = new StringBuilder(len * 28);

            text.Append(start);

            for (var i = 0; i < len; i++)
            {
                var ss = allQuo || quoIdxs.Contains(i) ? "\"" : "";
                var sd = i == 0 ? "" : separator;
                text.AppendFormat("{3}\"{5}\":{4}{0}{1}{2}{4}", "{", i, "}", sd, ss, names[i]);
            }
            text.Append(end);

            return text.ToString();
        }

        public static int MaxFmtStrIndex(string str)
        {
            Regex regex = new Regex(@"(?<=\{)[^}]*(?=\})", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(str);
            if (matches.Count != 0)
            {
                var val = matches.Cast<Match>()
                    .Select(m => m.Value.Split(':').FirstOrDefault().Trim())
                    .Distinct()
                    .Select(x =>
                    {
                        int v = 0;
                        if (!int.TryParse(x, out v))
                        {
                            v = 0;
                        }
                        return v;
                    }).Max();

                return val;
            }
            else
            {
                return 0;
            }
        }

    }
    
}
