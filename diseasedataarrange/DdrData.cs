using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using _DdrInt = System.Single;

namespace diseasedataarrange
{

    public class DdrData

    {
        public float? TimeOffset { get; set; }

        private string _ParentName;
        public string ParentName { set { _ParentName = value; } get { return _ParentName; } }
        private _DdrInt? _ParentConfirmedCount;
        public _DdrInt? ParentConfirmedCount { set { _ParentConfirmedCount = value; } get { return _ParentConfirmedCount; } }
        private _DdrInt? _ParentSuspectedCount;
        public _DdrInt? ParentSuspectedCount { set { _ParentSuspectedCount = value; } get { return _ParentSuspectedCount; } }
        private _DdrInt? _ParentCuredCount;
        public _DdrInt? ParentCuredCount { set { _ParentCuredCount = value; } get { return _ParentCuredCount; } }
        private _DdrInt? _ParentDeadCount;
        public _DdrInt? ParentDeadCount { set { _ParentDeadCount = value; } get { return _ParentDeadCount; } }
        private string _ChildName;
        public string ChildName { set { _ChildName = value; } get { return _ChildName; } }
        private _DdrInt? _ChildConfirmedCount;
        public _DdrInt? ChildConfirmedCount { set { _ChildConfirmedCount = value; } get { return _ChildConfirmedCount; } }
        private _DdrInt? _ChildSuspectedCount;
        public _DdrInt? ChildSuspectedCount { set { _ChildSuspectedCount = value; } get { return _ChildSuspectedCount; } }
        private _DdrInt? _ChildCuredCount;
        public _DdrInt? ChildCuredCount { set { _ChildCuredCount = value; } get { return _ChildCuredCount; } }
        private _DdrInt? _ChildDeadCount;
        public _DdrInt? ChildDeadCount { set { _ChildDeadCount = value; } get { return _ChildDeadCount; } }
        private DateTime? _UpdateTime;
        public DateTime? UpdateTime { set { _UpdateTime = value; } get { return _UpdateTime; } }


        public string ParentName_ChildName
        {
            get
            {
                return string.Format("{0}_{1}", ParentName, ChildName);
            }
        }

        public string UpdateTime_ParentName
        {
            get
            {
                long ticks = 0;
                var tm = UpdateTime;
                if (tm != null)
                {
                    ticks = tm.Value.Ticks;
                }
                return string.Format("{0:D20}_{1}", ticks, ParentName);
            }
        }
        public _DdrInt? ParentTreatingCount
        {
            get
            {
                return ParentConfirmedCount - ParentCuredCount - ParentDeadCount;
            }
        }

        public float? ParentCuredRate
        {
            get
            {
                return ParentCuredCount._Div(ParentConfirmedCount);
            }
        }

        public float? ParentDeadRate
        {
            get
            {
                return ParentDeadCount._Div(ParentConfirmedCount);
            }
        }

        public float? ParentTreatingRate
        {
            get
            {
                return ParentTreatingCount._Div(ParentConfirmedCount);
            }
        }

        public float? ParentDeadDivideCured
        {
            get
            {
                return ParentDeadCount._Div(ParentCuredCount);
            }
        }

        public float? ParentCuredDivideDead
        {
            get
            {
                return ParentCuredCount._Div(ParentDeadCount);
            }
        }

        public float? ParentDeadRateEx
        {
            get
            {
                return ParentDeadDivideCured._CalcRate();
            }
        }

        public float? ParentCuredRateEx
        {
            get
            {
                return ParentCuredDivideDead._CalcRate();
            }
        }

        public _DdrInt? ChildTreatingCount
        {
            get
            {
                if (ChildConfirmedCount == null || ChildConfirmedCount <= 0)
                {
                    return null;
                }
                return ChildConfirmedCount - ChildCuredCount - ChildDeadCount;
            }
        }

        public float? ChildCuredRate
        {
            get
            {
                return ChildCuredCount._Div(ChildConfirmedCount);
            }
        }

        public float? ChildDeadRate
        {
            get
            {
                return ChildDeadCount._Div(ChildConfirmedCount);
            }
        }

        public float? ChildTreatingRate
        {
            get
            {
                return ChildTreatingCount._Div(ChildConfirmedCount);
            }
        }

        public float? ChildDeadDivideCured
        {
            get
            {
                return ChildDeadCount._Div(ChildCuredCount);
            }
        }

        public float? ChildCuredDivideDead
        {
            get
            {
                return ChildCuredCount._Div(ChildDeadCount);
            }
        }

        public float? ChildConfirmedCountRate
        {
            get
            {
                return ChildConfirmedCount._Div(ParentConfirmedCount);
            }
        }

        public float? ChildSuspectedCountRate
        {
            get
            {
                return ChildSuspectedCount._Div(ParentSuspectedCount);
            }
        }

        public float? ChildCuredCountRate
        {
            get
            {
                return ChildCuredCount._Div(ParentCuredCount);
            }
        }

        public float? ChildDeadCountRate
        {
            get
            {
                return ChildDeadCount._Div(ParentDeadCount);
            }
        }

        public float? ChildTreatingCountRate
        {
            get
            {
                return ChildTreatingCount._Div(ParentTreatingCount);
            }
        }

        public float? ChildDeadRateEx
        {
            get
            {
                return ChildDeadDivideCured._CalcRate();
            }
        }

        public float? ChildCuredRateEx
        {
            get
            {
                return ChildCuredDivideDead._CalcRate();
            }
        }

        public float? ParentConfirmedNorm { get; set; }
        public float? ParentSuspectedNorm { get; set; }
        public float? ParentCuredNorm { get; set; }
        public float? ParentDeadNorm { get; set; }
        public float? ParentTreatingNorm { get; set; }

        public float? ParentTatalConfirmedNorm { get; set; }
        public float? ParentTatalSuspectedNorm { get; set; }
        public float? ParentTatalCuredNorm { get; set; }
        public float? ParentTatalDeadNorm { get; set; }
        public float? ParentTatalTreatingNorm { get; set; }

        public float? ChildConfirmedNorm { get; set; }
        public float? ChildSuspectedNorm { get; set; }
        public float? ChildCuredNorm { get; set; }
        public float? ChildDeadNorm { get; set; }
        public float? ChildTreatingNorm { get; set; }

        public float? ChildTatalConfirmedNorm { get; set; }
        public float? ChildTatalSuspectedNorm { get; set; }
        public float? ChildTatalCuredNorm { get; set; }
        public float? ChildTatalDeadNorm { get; set; }
        public float? ChildTatalTreatingNorm { get; set; }


        public void FillDataByTatal(DdrRowGroup total)
        {
            ParentTatalConfirmedNorm = ParentConfirmedCount._Div(total.ParentConfirmedCount);
            ParentTatalSuspectedNorm = ParentSuspectedCount._Div(total.ParentSuspectedCount);
            ParentTatalCuredNorm = ParentCuredCount._Div(total.ParentCuredCount);
            ParentTatalDeadNorm = ParentDeadCount._Div(total.ParentDeadCount);
            ParentTatalTreatingNorm = ParentTreatingCount._Div(total.ParentTreatingMax);

            ChildTatalConfirmedNorm = ChildConfirmedCount._Div(total.ChildConfirmedCount);
            ChildTatalSuspectedNorm = ChildSuspectedCount._Div(total.ChildSuspectedCount);
            ChildTatalCuredNorm = ChildCuredCount._Div(total.ChildCuredCount);
            ChildTatalDeadNorm = ChildDeadCount._Div(total.ChildDeadCount);
            ChildTatalTreatingNorm = ChildTreatingCount._Div(total.ChildTreatingMax);
        }

        public void FillDataByLastParent(DdrData last, DdrRowGroup group)
        {
            ParentConfirmedNorm = ParentConfirmedCount._Div(last.ParentConfirmedCount);
            ParentSuspectedNorm = ParentSuspectedCount._Div(last.ParentSuspectedCount);
            ParentCuredNorm = ParentCuredCount._Div(last.ParentCuredCount);
            ParentDeadNorm = ParentDeadCount._Div(last.ParentDeadCount);
            ParentTreatingNorm = ParentTreatingCount._Div(group.ParentTreatingMax);
        }

        public void FillDataByLastChild(DdrData last, DdrRowGroup group)
        {
            ChildConfirmedNorm = ChildConfirmedCount._Div(last.ChildConfirmedCount);
            ChildSuspectedNorm = ChildSuspectedCount._Div(last.ChildSuspectedCount);
            ChildCuredNorm = ChildCuredCount._Div(last.ChildCuredCount);
            ChildDeadNorm = ChildDeadCount._Div(last.ChildDeadCount);
            ChildTreatingNorm = ChildTreatingCount._Div(group.ChildTreatingMax);
        }

        public int DXYDataParse(string line, char[] csvSeparators, int maxLen, int[] intIndexs, string[] strIndexs, string dateTimeFmt, System.Globalization.CultureInfo info)
        {
            var cols = line.Split(csvSeparators);
            if (cols.Length < maxLen)
            {
                return maxLen;
            }

            var ret = 0;

            ret = ColPareseVal(ref _UpdateTime, cols, intIndexs, strIndexs, DdrCSVIndex.UpdateTime, dateTimeFmt, info);
            if (ret != 0 )
            {
                return ret;
            }

            System.Diagnostics.Debug.Assert(UpdateTime != null);

            ret = ColPareseVal(ref _ParentName, cols, intIndexs,  strIndexs,DdrCSVIndex.ParentName, dateTimeFmt, info);
            ret = ColPareseVal(ref _ParentConfirmedCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ParentConfirmedCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ParentSuspectedCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ParentSuspectedCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ParentCuredCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ParentCuredCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ParentDeadCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ParentDeadCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ChildName, cols, intIndexs,  strIndexs,DdrCSVIndex.ChildName, dateTimeFmt, info);
            ret = ColPareseVal(ref _ChildConfirmedCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ChildConfirmedCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ChildSuspectedCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ChildSuspectedCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ChildCuredCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ChildCuredCount, dateTimeFmt, info);
            ret = ColPareseVal(ref _ChildDeadCount, cols, intIndexs,  strIndexs,DdrCSVIndex.ChildDeadCount, dateTimeFmt, info);

            return 0;
        }

        private static string GetCSVColsVal(string[] cols, int[] indexs, DdrCSVIndex e)
        {
            return cols[indexs[(int)e]];
        }

        private static int ColPareseVal(ref _DdrInt? t, string[] cols, int[] intIndexs, string[] strIndexs, DdrCSVIndex e, string fmt, System.Globalization.CultureInfo info)
        {
            return DdrHelp._Parse(ref t, GetCSVColsVal(cols, intIndexs, e)) ? 0: 1 + (int)e;
        }

        private static int ColPareseVal(ref DateTime? t, string[] cols, int[] intIndexs, string[] strIndexs, DdrCSVIndex e, string fmt, System.Globalization.CultureInfo info)
        {
            return DdrHelp._Parse(ref t, GetCSVColsVal(cols, intIndexs, e), fmt, info) ? 0 : 1 + (int)e;
        }

        private static int ColPareseVal(ref string t, string[] cols, int[] intIndexs, string[] strIndexs, DdrCSVIndex e, string fmt, System.Globalization.CultureInfo info)
        {
            var index = intIndexs[(int)e];
            fmt = index == -1 ? strIndexs[(int)e] : null;
            var v = index != -1 ? GetCSVColsVal(cols, intIndexs, e) : null;
            return DdrHelp._Parse(ref t, v, fmt, cols) ? 0 : 1 + (int)e;
        }
    }

    public class DdrRowGroup : DdrData
    {
        public float? ParentConfirmedMax { get; set; }
        public float? ParentSuspectedMax { get; set; }
        public float? ParentCuredMax { get; set; }
        public float? ParentDeadMax { get; set; }
        public _DdrInt? ParentTreatingMax { get; set; }
        public float? ParentDeadDivideCuredMax { get; set; }
        public float? ParentCuredDivideDeadMax { get; set; }
        public float? ChildConfirmedMax { get; set; }
        public float? ChildSuspectedMax { get; set; }
        public float? ChildCuredMax { get; set; }
        public float? ChildDeadMax { get; set; }
        public _DdrInt? ChildTreatingMax { get; set; }
        public float? ChildDeadDivideCuredMax { get; set; }
        public float? ChildCuredDivideDeadMax { get; set; }


        public float? ParentConfirmedMin { get; set; }
        public float? ParentSuspectedMin { get; set; }
        public float? ParentCuredMin { get; set; }
        public float? ParentDeadMin { get; set; }
        public float? ParentTreatingMin { get; set; }
        public float? ParentDeadDivideCuredMin { get; set; }
        public float? ParentCuredDivideDeadMin { get; set; }
        public float? ChildConfirmedMin { get; set; }
        public float? ChildSuspectedMin { get; set; }
        public float? ChildCuredMin { get; set; }
        public float? ChildDeadMin { get; set; }
        public float? ChildTreatingMin { get; set; }
        public float? ChildDeadDivideCuredMin { get; set; }
        public float? ChildCuredDivideDeadMin { get; set; }

    }

}
