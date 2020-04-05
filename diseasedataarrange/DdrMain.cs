using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using DdrRowList = System.Collections.Generic.List<diseasedataarrange.DdrData>;
using DdrGroup = System.Tuple<diseasedataarrange.DdrRowGroup, System.Collections.Generic.List<diseasedataarrange.DdrData>>;
using DdrGroupList = System.Collections.Generic.List<System.Tuple<diseasedataarrange.DdrRowGroup, System.Collections.Generic.List<diseasedataarrange.DdrData>>>;

using DdrFmtFunc = System.Func<string[], string>;


namespace diseasedataarrange
{
    public class DdrMain
    {
        public DdrMain()
        {
        }

        public bool DoWork(string[] args)
        {
            //DebugDdrRowHeaderInfo();
            if (!ParamsParse(args))
            {
                return false;
            }

            GetCSVRecords();
            GC.Collect();
            DataProcess();
            GC.Collect();
            FillData();
            GC.Collect();
            WriteDataToFile();
            GC.Collect();
            Console.WriteLine("data count:{0}", AllRecords.Count);
            return true;
        }

        private string CSVFile { set; get; }
        private string OutputDir { set; get; }
        private DateTime StartTime { set; get; }
        private NameValueCollection WordReplace { set; get; }
        private char[] CSVSeparators { set; get; }
        private string CSVDateTimeFmt { set; get; }
        private bool GenerateCSV { set; get; }
        private bool GenerateDictionaryJSON { set; get; }
        private bool GenerateArrayJSON { set; get; }
        private int[] CSVIntIndexs { set; get; }
        private string[] CSVStrIndexs { set; get; }
        private string DateTimeFormat { set; get; }
        private bool GenerateAllFields { set; get; }
        private CultureInfo CurrentCulture { get; set; }

        private bool ParamsParse(string[] args)
        {
            var wordReplase = ConfigurationManager.GetSection("wordReplace") as NameValueCollection;
            if (wordReplase == null)
            {
                Console.WriteLine("app.config can not find or error!");
                return false;
            }

            var settings = ConfigurationManager.AppSettings;
            var csvIndex = settings.Get("CSVIndex");
            if (string.IsNullOrWhiteSpace(csvIndex))
            {
                Console.WriteLine("app.config CSVIndex config error!");
                return false;
            }
            var csvStrIndexs = csvIndex.Split(',');

            if (csvStrIndexs.Length != (int)DdrCSVIndex.CSVIndexLen)
            {
                Console.WriteLine("app.config CSVIndex length error!");
                return false;
            }
            var cstIntIndexs = csvStrIndexs.Select(x =>
            {
                int v = -1;
                if (!int.TryParse(x, out v))
                {
                    v = -1;
                }
                return v;
            }).ToArray();

            var dateTimeFmt = settings.Get("DateTimeFormat");
            if (string.IsNullOrWhiteSpace(dateTimeFmt))
            {
                Console.WriteLine("app.config DateTimeFormat config error!");
                return false;
            }

            var csvSeparators = settings.Get("CSVSeparators");
            if (string.IsNullOrWhiteSpace(csvSeparators))
            {
                Console.WriteLine("app.config CSVSeparators config error!");
                return false;
            }

            var csvDateTimeFmt = settings.Get("CSVDateTimeFmt");
            if (string.IsNullOrWhiteSpace(csvDateTimeFmt))
            {
                Console.WriteLine("app.config CSVDateTimeFmt config error!");
                return false;
            }

            var culture = settings.Get("Culture");
            if (string.IsNullOrWhiteSpace(csvSeparators))
            {
                Console.WriteLine("app.config Culture config error!");
                return false;
            }

            var curCulture = System.Globalization.CultureInfo.GetCultureInfo(culture);
            System.Threading.Thread.CurrentThread.CurrentCulture = curCulture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = curCulture;

   

            var file = settings.Get("CSVFile");
            var dirName = settings.Get("OutputDir");
            var tms = settings.Get("StartTime");            
            var csvfmt = settings.Get("GenerateCSV");
            var jsonafmt = settings.Get("GenerateArrayJSON");
            var jsondfmt = settings.Get("GenerateDictionaryJSON");
            var fcsv = settings.Get("FileSuffixCSV");
            var fdj = settings.Get("FileSuffixDictionaryJSON");
            var faj = settings.Get("FileSuffixArray");
            var gaf = settings.Get("GenerateAllFields");

            var tm = new DateTime();
            if (!DateTime.TryParse(tms, out tm))
            {
                Console.WriteLine("app.config StartTime config error!");
                return false;
            }

            if (args.Length >= 1)
            {
                file = args[0];
            }

            if (args.Length >= 2)
            {
                dirName = args[1];
            }

            if (!File.Exists(file))
            {
                Console.WriteLine("error csvfile arg:{0}", file);
                PrintCommandMsg();
                return false;
            }

            if (!Directory.Exists(dirName))
            {
                Console.WriteLine("error directory arg:{0}", dirName);
                PrintCommandMsg();
                return false;
            }


            CSVFile = file;
            OutputDir = dirName;
            StartTime = tm;
            GenerateCSV = csvfmt != "0";
            GenerateArrayJSON = jsonafmt != "0";
            GenerateDictionaryJSON = jsondfmt != "0";
            GenerateAllFields = gaf != "0";
            WordReplace = wordReplase;
            CSVIntIndexs = cstIntIndexs;
            CSVStrIndexs = csvStrIndexs;
            DateTimeFormat = "\"{0:" + dateTimeFmt + "}\"";
            CSVSeparators = csvSeparators.ToArray();
            CSVDateTimeFmt = csvDateTimeFmt;
            CurrentCulture = curCulture;
            return true;
        }

        private static void PrintCommandMsg()
        {
            Console.WriteLine("command like this:diseasedataarrange /xxxx/pathto/DXYArea.csv /xxxx/outputdir");
        }

        private static void DebugDdrRowHeaderInfo()
        {
            var names = typeof(DdrData).GetProperties().Where(x => x.CanRead).Select(x => x.Name);
            Console.WriteLine(string.Join(",", names));
        }

        private string[] GetCSVFileLines()
        {
            StringBuilder text = null;
            using (var reader = new StreamReader(this.CSVFile))
            {
                text = new StringBuilder(reader.ReadToEnd());
            }

            foreach (string key in WordReplace.Keys)
            {
                var val = WordReplace.Get(key);
                text.Replace(key, val);
            }

            var lines = text.ToString().Split('\n');

            text = null;
            GC.Collect();
            return lines;
        }

        private void GetCSVRecords()
        {
            var lines = GetCSVFileLines();
            var dateTimeFmt = this.CSVDateTimeFmt;
            var info = this.CurrentCulture;

            DdrRowList list = new DdrRowList();
            var i = 0;
            var error = 0;

            var maxLen = Math.Max(CSVIntIndexs.Max(), DdrHelp.MaxFmtStrIndex(string.Join(",", CSVStrIndexs))) + 1;
            var item = new DdrData();
            var ret = 0;
            foreach (var s in lines)
            {
                i++;
                if (string.IsNullOrWhiteSpace(s))
                {
                    continue;
                }
                ret = item.DXYDataParse(s, CSVSeparators, maxLen, CSVIntIndexs, CSVStrIndexs, dateTimeFmt, info);
                if (ret == 0)
                {
                    list.Add(item);
                    item = new DdrData();
                }
                else if (i != 1)
                {
                    error++;
                    Console.WriteLine("line {0} parse error {1}! ", i, ret);
                }
            }

            AllRecords = list;

            Console.WriteLine("data error count:{0}", error);
        }

        private DdrRowGroup Total { get; set; }
        private DdrRowList AllRecords { get; set; }
        private DdrRowList AllParents { get; set; }
        private DdrRowList LastParents { get; set; }
        private DdrRowList AllChildren { get; set; }
        private DdrRowList LastChildren { get; set; }
        private DdrGroupList GroupParents { get; set; }
        private DdrGroupList GroupChildren { get; set; }
        private DdrGroupList GroupLastChildren { get; set; }

        private void DataProcess()
        {
            {
                var query = from x in AllRecords
                            orderby x.ParentName_ChildName ascending, x.UpdateTime ascending
                            select x;

                AllChildren = query.ToList();
            }

            {
                var query1 = from x in AllChildren
                             where !string.IsNullOrWhiteSpace(x.ParentName)
                             group x by x.UpdateTime_ParentName into t
                             let maxCC = t.Min(y => y.ParentConfirmedCount)
                             select t.First(y => y.ParentConfirmedCount == maxCC);

                var query2 = from x in query1
                             orderby x.ParentName ascending, x.UpdateTime ascending
                             select x;

                AllParents = query2.ToList();
            }

            {
                var query1 = from x in AllChildren
                             where !string.IsNullOrWhiteSpace(x.ChildName)
                             group x by new
                             {
                                 x.ParentName,
                                 x.ChildName
                             } into t
                             let maxTm = t.Max(y => y.UpdateTime)
                             select t.First(y => y.UpdateTime == maxTm);

                var query2 = from x in query1
                             orderby x.UpdateTime descending, x.ParentName_ChildName ascending
                             select x;

                LastChildren = query2.ToList();
            }


            {
                var query1 = from x in AllParents
                             group x by x.ParentName into t
                             let maxTm = t.Max(y => y.UpdateTime)
                             select t.First(y => y.UpdateTime == maxTm);

                var query2 = from x in query1
                             orderby x.UpdateTime descending, x.ParentName ascending
                             select x;

                LastParents = query2.ToList();
            }


            GroupChildren = (from x in AllChildren
                             where !string.IsNullOrWhiteSpace(x.ChildName)
                             group x by new
                          {
                              ParentName = x.ParentName,
                              ChildName = x.ChildName,
                          } into t
                          select new DdrGroup(
                              new DdrRowGroup()
                              {
                                  ParentName = t.Key.ParentName,
                                  ChildName = t.Key.ChildName,
                                  ChildTreatingMax = t.Max(z => z.ChildTreatingCount),
                                  ChildDeadDivideCuredMax = t.Max(z => z.ChildDeadDivideCured),
                                  ChildDeadDivideCuredMin = t.Min(z => z.ChildDeadDivideCured),
                                  ChildCuredDivideDeadMax = t.Max(z => z.ChildCuredDivideDead),
                                  ChildCuredDivideDeadMin = t.Min(z => z.ChildCuredDivideDead),
                              },
                              (from y in t
                               orderby y.UpdateTime ascending
                               select y).ToList())
                          ).ToList();


            GroupParents = (from x in AllParents
                              group x by new
                              {
                                  ParentName = x.ParentName,
                              } into t
                              select new DdrGroup(
                                  new DdrRowGroup()
                                  {
                                      ParentName = t.Key.ParentName,
                                      ParentTreatingMax = t.Max(z => z.ParentTreatingCount),
                                      ParentDeadDivideCuredMax = t.Max(z => z.ParentDeadDivideCured),
                                      ParentDeadDivideCuredMin = t.Min(z => z.ParentDeadDivideCured),
                                      ParentCuredDivideDeadMax = t.Max(z => z.ParentCuredDivideDead),
                                      ParentCuredDivideDeadMin = t.Min(z => z.ParentCuredDivideDead),
                                  },
                                  (from y in t
                                   orderby y.UpdateTime ascending
                                   select y).ToList())
                              ).ToList();

            GroupLastChildren = (from x in LastChildren
                              group x by new
                              {
                                  ParentName = x.ParentName,
                              } into t
                              select new DdrGroup(
                                  new DdrRowGroup()
                                  {
                                      ParentName = t.Key.ParentName,
                                  },
                                  (from y in t
                                   orderby y.UpdateTime descending, y.ParentName_ChildName ascending
                                   select y).ToList())
                  ).ToList();

            Total = new DdrRowGroup()
            {
                UpdateTime = LastParents.Max(x => x.UpdateTime),
                ParentName = "Total",
                ParentConfirmedCount = LastParents.Sum(x => x.ParentConfirmedCount),
                ParentSuspectedCount = LastParents.Sum(x => x.ParentSuspectedCount),
                ParentCuredCount = LastParents.Sum(x => x.ParentCuredCount),
                ParentDeadCount = LastParents.Sum(x => x.ParentDeadCount),
                ChildConfirmedCount = LastChildren.Max(x => x.ChildConfirmedCount),
                ChildSuspectedCount = LastChildren.Max(x => x.ChildSuspectedCount),
                ChildCuredCount = LastChildren.Max(x => x.ChildCuredCount),
                ChildDeadCount = LastChildren.Max(x => x.ChildDeadCount),

                ParentTreatingMax = AllParents.Max(x => x.ParentTreatingCount),
                ChildTreatingMax = AllChildren.Max(x => x.ChildTreatingCount)
            };
        }

        private void FillData()
        {
            Total.TimeOffset = Total.UpdateTime._FltDays(StartTime);

            AllChildren.ForEach(x =>
            {
                x.TimeOffset = x.UpdateTime._FltDays(StartTime);
                x.FillDataByTatal(Total);
            });

            GroupParents.ForEach(x =>
            {
                var grp = x.Item1;
                var list = x.Item2;
                var last = list.Last();

                list.ForEach(y =>
                {
                    y.FillDataByLastParent(last, grp);
                });
            });

            GroupChildren.ForEach(x =>
            {
                var grp = x.Item1;
                var list = x.Item2;
                var last = list.Last();

                list.ForEach(y =>
                {
                    y.FillDataByLastChild(last, grp);
                });
            });

        }

        private string GetOutputFile(params string[] names)
        {
            var name = string.Join("", names);
            var paths = new List<string>() { OutputDir };
            paths.AddRange(name.Split('\\'));
            var file = Path.Combine(paths.ToArray());
            var dirName = Path.GetDirectoryName(file);
            Directory.CreateDirectory(dirName);
            return file;
        }

        private void WriteDataToFile()
        {
            var b = GenerateAllFields;
            Enum a = DdrAllField.TimeOffset;

            WirteDataToFile(b ? a : DdrAllField.TimeOffset, AllRecords, @"AllRecords");
            WirteDataToFile(b ? a : DdrAllParentField.TimeOffset, AllParents, @"AllParents");
            WirteDataToFile(b ? a : DdrAllChildField.TimeOffset, AllChildren, @"AllChildren");
            WirteDataToFile(b ? a : DdrAllParentField.TimeOffset, new DdrRowList() { Total }, @"Total");
            WirteDataToFile(b ? a : DdrAllChildField.TimeOffset, LastChildren, @"LastChildren");
            WirteDataToFile(b ? a : DdrAllParentField.TimeOffset, LastParents, @"LastParents");
            WirteDataToFile(b ? a : DdrAllParentField.TimeOffset, GroupParents, @"ParentDetail\{0}");
            WirteDataToFile(b ? a : DdrAllChildField.TimeOffset, GroupChildren, @"ChildDetail\{0}\{1}");
            WirteDataToFile(b ? a : DdrAllChildField.TimeOffset, GroupLastChildren, @"ParentLastChildren\{0}");
        }

        private void WirteDataToFile(Enum e, DdrGroupList list, string nameFmt)
        {
            foreach (var item in list)
            {
                var key = item.Item1;
                var fileName = string.Format(nameFmt, key.ParentName, key.ChildName);
                WirteDataToFile(e, item.Item2, fileName);
            }
        }

        private void WirteDataToFile(Enum e, DdrRowList list, string fileName)
        {
            if (GenerateCSV)
            {
                var file = GetOutputFile(@"csv\", fileName, ".csv");
                WirteListToFile(e, list, file, false, WirteCSVHeader, WirteCSVLine);
            }

            if (GenerateDictionaryJSON)
            {
                var file = GetOutputFile(@"djson\", fileName, ".json");
                WirteListToFile(e, list, file, true, WirteDJSONHeader, WirteDJSONLine);
            }

            if (GenerateArrayJSON)
            {
                var file = GetOutputFile(@"ajson\", fileName, ".json");
                WirteListToFile(e, list, file, true, WirteAJSONHeader, WirteAJSONLine);
            }
        }

        private string WirteCSVHeader(string[] names)
        {
            return DdrHelp.FormatString(names, ",", "", "", false);
        }

        private string WirteCSVLine(string[] names)
        {
            return DdrHelp.CreateStringFormat(names.Length, ",", "", "", false);
        }

        private string WirteAJSONHeader(string[] names)
        {
            return DdrHelp.FormatString(names, ",", "[\n[", "],", true);
        }

        private string WirteAJSONLine(string[] names)
        {
            return DdrHelp.CreateStringFormat(names.Length, ",", "[", "],", false);
        }


        private string WirteDJSONHeader(string[] names)
        {
            return "[";
        }

        private string WirteDJSONLine(string[] names)
        {
            return DdrHelp.CreateStringFormat(names, ",", "{{", "}},", false);
        }
        
        private void WirteListToFile(Enum e, DdrRowList list, string file, bool isJson,
            DdrFmtFunc funcHeader, DdrFmtFunc funcLine)
        {
            var names = DdrHelp.GetEnumNames(e);
            var props = DdrHelp.GetPropertiesByNames<DdrData>(names);
            Debug.Assert(names.Length == props.Length);
            
            var nameIdx = new int[]
            {
                //Array.IndexOf(names,"UpdateTime"),
                Array.IndexOf(names,"ParentName"),
                Array.IndexOf(names,"ChildName"),
            };

            var timeIdx = Array.IndexOf(names, "UpdateTime");

            using (var w = new StreamWriter(file, false, Encoding.UTF8))
            {
                w.NewLine = "\n";

                var csvHeaderText = funcHeader(names);
                w.WriteLine(csvHeaderText);

                var lineFmt = funcLine(names);
                StringBuilder builder = new StringBuilder(18000);

                var idx = 0;
                var len = list.Count;

                list.ForEach(x =>
                {
                    var vals = DdrHelp.GetValuseByProperties(x, props);
                    PrepareVals(isJson, nameIdx, timeIdx, vals);

                    builder.Clear();
                    builder.AppendFormat(lineFmt, vals);
                    var line = builder.ToString();

                    if (isJson && ++idx == len)
                    {
                        line = line.TrimEnd(',');
                    }

                    w.WriteLine(line);
                });

                if (isJson)
                {
                    w.WriteLine("]");
                }

                w.Flush();
            }
        }

        private void PrepareVals(bool isJson, int[] nameIdx, int timeIdx, object[] vals)
        {
            if (timeIdx >= 0)
            {
                var time = vals[timeIdx] as DateTime?;
                if (time != null)
                {
                    vals[timeIdx] = string.Format(DateTimeFormat, time.Value);
                }
            }
            if (isJson)
            {
                Array.ForEach(nameIdx, y =>
                {
                    if (y >= 0)
                    {
                        vals[y] = string.Format("\"{0}\"", vals[y]);
                    }
                });

                for (int i = 0; i < vals.Length; i++)
                {
                    if (vals[i] == null)
                    {
                        vals[i] = "null";
                    }
                }
            }
        }
    }
}
