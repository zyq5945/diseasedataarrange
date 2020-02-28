
## 1.介绍说明

diseasedataarrange是一个对疾病数据进行整理的C#控制台程序，在windows系统运行需要安装Net4.0，其他系统请安装mono运行环境。

## 2.运行方法

目前是针对[DXY-COVID-19-Data](https://github.com/BlankerL/DXY-COVID-19-Data)的数据写的默认配置项，将该项目的最新的DXYArea.csv文件拷贝到程序所在目录，双击程序运行完成后，在程序目录下csv目录下产生整理后的csv文件。

如若需要针对其他数据进行整理，请参考配置选项的configuration/appSettings和configuration/wordReplace的字段说明进行修改

### 2.1带参数的运行方式

#### 2.1.1windows系统

##### 命令行带参数运行命令：

```
x:/xxxx/pathto/diseasedataarrange.exe x:/xxxx/pathto/DXYArea.csv x:/xxxx/pathto/outputdir
```

##### 路径有空格的：

```
"x:/xxxx/path to/diseasedataarrange.exe" "x:/xxxx/path to/DXYArea.csv" "x:/xxxx/path to/outputdir"
```

#### 2.1.2其他系统mono环境

##### shell带参数运行命令：

```
mono /xxxx/pathto/diseasedataarrange.exe /xxxx/pathto/DXYArea.csv /xxxx/pathto/outputdir
```

##### 路径有空格的：

```
mono "/xxxx/path to/diseasedataarrange.exe" "/xxxx/path to/DXYArea.csv" "/xxxx/path to/outputdir"
```

注：各参数之间是空格分开，若路径有空格，请用英文双引号将路径和名称作为一个整体包起来。

## 3.配置选项

程序目录下的XML格式的配置文件diseasedataarrange.exe.config说明：

#### 3.1configuration/appSettings配置字段说明

| 字段 | 默认值 |  说明 |
|---|---|---|
| CSVFile | ./DXYArea.csv | 默认的CSV文件地址，会被命令行第一个参数覆盖 |
| OutputDir | ./ | 默认的输出文件目录，会被命令行第二个参数覆盖 |
| StartTime | 2020/1/24 | 数据的开始计算时间，对应TimeOffset字段的浮点数值 |
| CSVIndex | 14,0,6,7,8,9,3,10,11,12,13 | CSV数据字段对应索引，从0计数，依次为“更新时间,父名称,父确诊数,父疑似数,父康复数,父死亡数,子名称,子确诊数,子疑似数,子康复数,子死亡数” |
| CSVSeparators | ,;\t | CSV数据字段的分隔符，\t可用“&#x9;”来表示 |
| GenerateCSV | 1| 是否生成CSV文件，0为不生成，1为生成 |
| GenerateDictionaryJSON | 0 | 是否生成字典形式JSON文件，0为不生成，1为生成 |
| GenerateArrayJSON | 0 | 是否生成数组形式JSON文件，0为不生成，1为生成 |
| GenerateAllFields | 0 | 是否生成所有字段数据，0为不生成，1为生成 |
| DateTimeFormat | yyyy-MM-dd HH:mm:ss.fff| 生成的UpdateTime字段时间数据格式 |

#### 3.2configuration/wordReplace配置字段说明

由于数据是多人协作统计的，难免会出现同义不同名的情况，此次配置提供文字替换方便名称合并统一。

注：名称合并，先替换长名称，再替换短名称

配置多个条目格式如下：

```
<add key="需要替换的名称" value="替换后的名称" />
```


## 4.生成文件的各字段说明

注：标准化是指将值缩放到浮点数是0-1之间的值

| 字段 |  说明 |
|---|---|
|  TimeOffset | 浮点型的天数时间偏移 |
|  UpdateTime | 更新时间 |
|  ParentName | 父名称 |
|  ParentConfirmedCount | 父确诊数 |
|  ParentSuspectedCount | 父疑似数 |
|  ParentCuredCount | 父康复数 |
|  ParentDeadCount | 父死亡数 |
|  ParentTreatingCount | 父正在治疗数 |
|  ParentCuredRate | 父康复率 |
|  ParentDeadRate | 父死亡率 |
|  ParentTreatingRate | 父正在治疗率 |
|  ParentDeadDivideCured | 父死亡除以康复数 |
|  ParentCuredDivideDead | 父康复除以死亡数 |
|  ParentConfirmedNorm | 父确诊数标准化 |
|  ParentSuspectedNorm | 父疑似数标准化 |
|  ParentCuredNorm | 父康复数标准化 |
|  ParentDeadNorm | 父死亡数标准化 |
|  ParentTreatingNorm | 父正在治疗数标准化 |
|  ParentDeadDivideCuredNorm | 父确诊数标准化 |
|  ParentCuredDivideDeadNorm | 父康复除以死亡数标准化 |
|  ParentTatalConfirmedNorm | 总计的父确诊数标准化 |
|  ParentTatalSuspectedNorm | 总计的父疑似数标准化 |
|  ParentTatalCuredNorm | 总计的父康复数标准化 |
|  ParentTatalDeadNorm | 总计的父死亡数标准化 |
|  ParentTatalTreatingNorm | 总计的父正在治疗数标准化 |
|  ChildName | 子名称 |
|  ChildConfirmedCount | 子确诊数 |
|  ChildSuspectedCount | 子疑似数 |
|  ChildCuredCount | 子康复数 |
|  ChildDeadCount | 子死亡数 |
|  ChildTreatingCount | 子正在治疗数 |
|  ChildCuredRate | 子康复率 |
|  ChildDeadRate | 子死亡率 |
|  ChildTreatingRate | 子正在治疗率 |
|  ChildDeadDivideCured | 子死亡除以康复数 |
|  ChildCuredDivideDead | 子康复除以死亡数 |
|  ChildConfirmedCountRate | 子确诊数相对父占比率 |
|  ChildSuspectedCountRate | 子疑似数相对父占比率 |
|  ChildCuredCountRate | 子确康复数相对父占比率 |
|  ChildDeadCountRate | 子确死亡数相对父占比率 |
|  ChildTreatingCountRate | 子正在治疗数相对父占比率 |
|  ChildConfirmedNorm | 子确诊数标准化 |
|  ChildSuspectedNorm | 子疑似数标准化 |
|  ChildCuredNorm | 子康复数标准化 |
|  ChildDeadNorm | 子死亡数标准化 |
|  ChildTreatingNorm | 子正在治疗数标准化 |
|  ChildDeadDivideCuredNorm | 子确诊数标准化 |
|  ChildCuredDivideDeadNorm | 子康复除以死亡数标准化 |
|  ChildTatalConfirmedNorm | 总计的子确诊数标准化 |
|  ChildTatalSuspectedNorm | 总计的子疑似数标准化 |
|  ChildTatalCuredNorm | 总计的子康复数标准化 |
|  ChildTatalDeadNorm | 总计的子死亡数标准化 |
|  ChildTatalTreatingNorm | 总计的子正在治疗数标准化 |

## 5.程序使用的一些注意事项

mono环境下使用Linq对第二字段进行排序会有问题，而且程序在mono的运行效率会比在windows的Net环境要慢一些。


中文有多音字的情况，比如重庆的“重”可能会按读音zhòng来进行排序，所以排序会有些问题。

## 6.一些个人的DXY-COVID-19数据分析心得

康复率，死亡率和政治治疗率需要合在一起考虑，若事件结束后最终康复率教高，早期阶段数据少会造成数据有很大波动，中间阶段死亡率会逐步上升，死亡率会缓慢上升，正在治疗率会逐步下降。以死亡率在中期阶段大数据情况下，这会使得较少数据“噪点”数据就出现较大数值变动成为历史，就会出现缓慢上升这个前提条件来看，整体而言，患者多的地区较容易出现高死亡率，北方比南方较容易出现较高死亡率，越冷就越如此，这和[最终统计的2003年北京的SARS的死亡率](https://zyq5945.github.io/zyq5945/blog_10.html)比广东的差是一致的。

瞎琢磨尝试用安徽省的康复数标准化数据进行Boltzmann数据拟合，还挺符合的。

## 7.LICENSE

Based on [BSD](./LICENSE) protocol.