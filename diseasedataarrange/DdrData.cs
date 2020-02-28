using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diseasedataarrange
{

    public class DdrData
    {
        public float? TimeOffset { get; set; }
        public string ParentName { get; set; }
        public int? ParentConfirmedCount { get; set; }
        public int? ParentSuspectedCount { get; set; }
        public int? ParentCuredCount { get; set; }
        public int? ParentDeadCount { get; set; }
        public string ChildName { get; set; }
        public int? ChildConfirmedCount { get; set; }
        public int? ChildSuspectedCount { get; set; }
        public int? ChildCuredCount { get; set; }
        public int? ChildDeadCount { get; set; }
        public DateTime? UpdateTime { get; set; }

        public string ParentName_ChildName
        {
            get
            {
                return string.Format("{0}_{1}", ParentName, ChildName) ;
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

        public int? ParentTreatingCount
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

        public int? ChildTreatingCount
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

        public float? ParentConfirmedNorm { get; set; }
        public float? ParentSuspectedNorm { get; set; }
        public float? ParentCuredNorm { get; set; }
        public float? ParentDeadNorm { get; set; }
        public float? ParentTreatingNorm { get; set; }

        public float? ParentDeadDivideCuredNorm { get; set; }
        public float? ParentCuredDivideDeadNorm { get; set; }

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

        public float? ChildDeadDivideCuredNorm { get; set; }
        public float? ChildCuredDivideDeadNorm { get; set; }

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
            ParentDeadDivideCuredNorm = ParentDeadDivideCured._Norm(group.ParentDeadDivideCuredMin, group.ParentDeadDivideCuredMax);
            ParentCuredDivideDeadNorm = ParentCuredDivideDead._Norm(group.ParentCuredDivideDeadMin, group.ParentCuredDivideDeadMax);
        }

        public void FillDataByLastChild(DdrData last, DdrRowGroup group)
        {
            ChildConfirmedNorm = ChildConfirmedCount._Div(last.ChildConfirmedCount);
            ChildSuspectedNorm = ChildSuspectedCount._Div(last.ChildSuspectedCount);
            ChildCuredNorm = ChildCuredCount._Div(last.ChildCuredCount);
            ChildDeadNorm = ChildDeadCount._Div(last.ChildDeadCount);
            ChildTreatingNorm = ChildTreatingCount._Div(group.ChildTreatingMax);
            ChildDeadDivideCuredNorm = ChildDeadDivideCured._Norm(group.ChildDeadDivideCuredMin, group.ChildDeadDivideCuredMax);
            ChildCuredDivideDeadNorm = ChildCuredDivideDead._Norm(group.ChildCuredDivideDeadMin, group.ChildCuredDivideDeadMax);
        }

        public bool DXYDataParse(string line, char[] csvSeparators, int maxLen, int[] indexs)
        {
            var cols = line.Split(csvSeparators);
            if (cols.Length < maxLen)
            {
                return false;
            }

            UpdateTime = GetCSVColsVal(cols, indexs, DdrCSVIndex.UpdateTime)._ParseDateTime();
            if (UpdateTime == null)
            {
                return false;
            }

            ParentName = GetCSVColsVal(cols, indexs, DdrCSVIndex.ParentName);
            ParentConfirmedCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ParentConfirmedCount)._ParseInt();
            ParentSuspectedCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ParentSuspectedCount)._ParseInt();
            ParentCuredCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ParentCuredCount)._ParseInt();
            ParentDeadCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ParentDeadCount)._ParseInt();
            ChildName = GetCSVColsVal(cols, indexs, DdrCSVIndex.ChildName);
            ChildConfirmedCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ChildConfirmedCount)._ParseInt();
            ChildSuspectedCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ChildSuspectedCount)._ParseInt();
            ChildCuredCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ChildCuredCount)._ParseInt();
            ChildDeadCount = GetCSVColsVal(cols, indexs, DdrCSVIndex.ChildDeadCount)._ParseInt();

            return true;
        }

        private static string GetCSVColsVal(string[] cols, int[] indexs, DdrCSVIndex e)
        {
            return cols[indexs[(int)e]];
        }
    }

    public class DdrRowGroup : DdrData
    {
        public float? ParentConfirmedMax { get; set; }
        public float? ParentSuspectedMax { get; set; }
        public float? ParentCuredMax { get; set; }
        public float? ParentDeadMax { get; set; }
        public int? ParentTreatingMax { get; set; }
        public float? ParentDeadDivideCuredMax { get; set; }
        public float? ParentCuredDivideDeadMax { get; set; }
        public float? ChildConfirmedMax { get; set; }
        public float? ChildSuspectedMax { get; set; }
        public float? ChildCuredMax { get; set; }
        public float? ChildDeadMax { get; set; }
        public int? ChildTreatingMax { get; set; }
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
