﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;

namespace XCharts
{
    /// <summary>
    /// the type of serie.
    /// 系列类型。
    /// </summary>
    public enum SerieType
    {
        /// <summary>
        /// 折线图。折线图是用折线将各个数据点标志连接起来的图表，用于展现数据的变化趋势。可用于直角坐标系和极坐标系上。
        /// </summary>
        Line,
        /// <summary>
        /// 柱状图。柱状/条形图 通过 柱形的高度/条形的宽度 来表现数据的大小，用于有至少一个类目轴或时间轴的直角坐标系上。
        /// </summary>
        Bar,
        /// <summary>
        /// 饼图。饼图主要用于表现不同类目的数据在总和中的占比。每个的弧度表示数据数量的比例。
        /// 饼图更适合表现数据相对于总数的百分比等关系。
        /// </summary>
        Pie,
        /// <summary>
        /// 雷达图。雷达图主要用于表现多变量的数据，例如球员的各个属性分析。依赖 radar 组件。
        /// </summary>
        Radar,
        /// <summary>
        /// 散点图。直角坐标系上的散点图可以用来展现数据的 x，y 之间的关系，如果数据项有多个维度，
        /// 其它维度的值可以通过不同大小的 symbol 展现成气泡图，也可以用颜色来表现。
        /// </summary>
        Scatter,
        /// <summary>
        /// 带有涟漪特效动画的散点图。利用动画特效可以将某些想要突出的数据进行视觉突出。
        /// </summary>
        EffectScatter
    }

    /// <summary>
    /// Whether to show as Nightingale chart, which distinguishs data through radius. 
    /// 是否展示成南丁格尔图，通过半径区分数据大小。
    /// </summary>
    public enum RoseType
    {
        /// <summary>
        /// Don't show as Nightingale chart.不展示成南丁格尔玫瑰图
        /// </summary>
        None,
        /// <summary>
        /// Use central angle to show the percentage of data, radius to show data size.
        /// 扇区圆心角展现数据的百分比，半径展现数据的大小。
        /// </summary>
        Radius,
        /// <summary>
        /// All the sectors will share the same central angle, the data size is shown only through radiuses.
        /// 所有扇区圆心角相同，仅通过半径展现数据大小。
        /// </summary>
        Area
    }

    /// <summary>
    /// 系列。每个系列通过 type 决定自己的图表类型。
    /// </summary>
    [System.Serializable]
    public class Serie : JsonDataSupport
    {
        [SerializeField] [DefaultValue("true")] private bool m_Show = true;
        [SerializeField] private SerieType m_Type;
        [SerializeField] private string m_Name;
        [SerializeField] private string m_Stack;
        [SerializeField] [Range(0, 1)] private int m_AxisIndex = 0;
        [SerializeField] private int m_RadarIndex = 0;
        [SerializeField] private LineStyle m_LineStyle = new LineStyle();
        [SerializeField] private AreaStyle m_AreaStyle = AreaStyle.defaultAreaStyle;
        [SerializeField] private SerieSymbol m_Symbol = new SerieSymbol();
        #region PieChart field
        [SerializeField] private bool m_ClickOffset = true;
        [SerializeField] private RoseType m_RoseType = RoseType.None;
        [SerializeField] private float m_Space;
        [SerializeField] private float[] m_Center = new float[2] { 0.5f, 0.5f };
        [SerializeField] private float[] m_Radius = new float[2] { 0, 80 };
        #endregion
        [SerializeField] private SerieLabel m_Label = new SerieLabel();
        [SerializeField] private SerieLabel m_HighlightLabel = new SerieLabel();
        [SerializeField] [Range(1, 10)] private int m_ShowDataDimension;
        [SerializeField] private bool m_ShowDataName;
        [FormerlySerializedAs("m_Data")]
        [SerializeField] private List<float> m_YData = new List<float>();
        [SerializeField] private List<float> m_XData = new List<float>();
        [SerializeField] private List<SerieData> m_Data = new List<SerieData>();

        /// <summary>
        /// Whether to show serie in chart.
        /// 系列是否显示在图表上。
        /// </summary>
        public bool show { get { return m_Show; } set { m_Show = value; } }
        /// <summary>
        /// the chart type of serie.
        /// 系列的图表类型。
        /// </summary>
        public SerieType type { get { return m_Type; } set { m_Type = value; } }
        /// <summary>
        /// Series name used for displaying in tooltip and filtering with legend.
        /// 系列名称，用于 tooltip 的显示，legend 的图例筛选。
        /// </summary>
        public string name { get { return m_Name; } set { m_Name = value; } }
        /// <summary>
        /// If stack the value. On the same category axis, the series with the same stack name would be put on top of each other.
        /// 数据堆叠，同个类目轴上系列配置相同的stack值后，后一个系列的值会在前一个系列的值上相加。
        /// </summary>
        public string stack { get { return m_Stack; } set { m_Stack = value; } }
        /// <summary>
        /// Index of axis to combine with, which is useful for multiple x axes in one chart.
        /// 使用的坐标轴轴的 index，在单个图表实例中存在多个坐标轴轴的时候有用。
        /// </summary>
        public int axisIndex { get { return m_AxisIndex; } set { m_AxisIndex = value; } }
        /// <summary>
        /// Index of radar component that radar chart uses.
        /// 雷达图所使用的 radar 组件的 index。
        /// </summary>
        public int radarIndex { get { return m_RadarIndex; } set { m_RadarIndex = value; } }
        /// <summary>
        /// The style of line.
        /// 线条样式。
        /// </summary>
        /// <value></value>
        public LineStyle lineStyle { get { return m_LineStyle; } set { m_LineStyle = value; } }
        /// <summary>
        /// The style of area.
        /// 区域填充样式。
        /// </summary>
        /// <value></value>
        public AreaStyle areaStyle { get { return m_AreaStyle; } set { m_AreaStyle = value; } }
        /// <summary>
        /// the symbol of serie data item.
        /// 标记的图形。
        /// </summary>
        public SerieSymbol symbol { get { return m_Symbol; } set { m_Symbol = value; } }
        /// <summary>
        /// Whether offset when mouse click pie chart item.
        /// 鼠标点击时是否开启偏移，一般用在PieChart图表中。
        /// </summary>
        public bool clickOffset { get { return m_ClickOffset; } set { m_ClickOffset = value; } }
        /// <summary>
        /// Whether to show as Nightingale chart.
        /// 是否展示成南丁格尔图，通过半径区分数据大小。
        /// </summary>
        public RoseType roseType { get { return m_RoseType; } set { m_RoseType = value; } }
        /// <summary>
        /// the space of pie chart item.
        /// 饼图项间的空隙留白。
        /// </summary>
        public float space { get { return m_Space; } set { m_Space = value; } }
        /// <summary>
        /// the center of pie chart.
        /// 饼图的中心点。
        /// </summary>
        public float[] center { get { return m_Center; } set { m_Center = value; } }
        /// <summary>
        /// the radius of pie chart.
        /// 饼图的半径。radius[0]表示内径，radius[1]表示外径。
        /// </summary>
        public float[] radius { get { return m_Radius; } set { m_Radius = value; } }
        /// <summary>
        /// Text label of graphic element,to explain some data information about graphic item like value, name and so on. 
        /// 图形上的文本标签，可用于说明图形的一些数据信息，比如值，名称等。
        /// </summary>
        public SerieLabel label { get { return m_Label; } set { m_Label = value; } }
        /// <summary>
        /// Text label of highlight graphic element.
        /// 高亮时的文本标签配置。
        /// </summary>
        public SerieLabel highlightLabel { get { return m_HighlightLabel; } set { m_HighlightLabel = value; } }
        /// <summary>
        /// 维度Y的数据列表。默认对应yAxis。
        /// </summary>
        public List<float> yData { get { return m_YData; } }
        /// <summary>
        /// 维度X的数据列表。默认对应xAxis。
        /// </summary>
        public List<float> xData { get { return m_XData; } }
        /// <summary>
        /// 系列中的数据内容数组。SerieData可以设置1到n维数据。
        /// </summary>
        public List<SerieData> data { get { return m_Data; } }

        /// <summary>
        /// The index of serie,start at 0.
        /// 系列的索引，从0开始。
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// Whether the serie is highlighted.
        /// 该系列是否高亮，一般由图例悬停触发。
        /// </summary>
        public bool highlighted { get; set; }
        /// <summary>
        /// the count of data list.
        /// 数据项个数。
        /// </summary>
        public int dataCount { get { return m_Data.Count; } }

        private int filterStart { get; set; }
        private int filterEnd { get; set; }
        private List<SerieData> filterData { get; set; }

        /// <summary>
        /// 维度Y对应数据中最大值。
        /// </summary>
        public float yMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[1] > max)
                    {
                        max = sdata.data[1];
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// 维度X对应数据中的最大值。
        /// </summary>
        public float xMax
        {
            get
            {
                float max = int.MinValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[0] > max)
                    {
                        max = sdata.data[0];
                    }
                }
                return max;
            }
        }

        /// <summary>
        /// 维度Y对应数据的最小值。
        /// </summary>
        public float yMin
        {
            get
            {
                float min = int.MaxValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[1] < min)
                    {
                        min = sdata.data[1];
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// 维度X对应数据的最小值。
        /// </summary>
        public float xMin
        {
            get
            {
                float min = int.MaxValue;
                foreach (var sdata in data)
                {
                    if (sdata.show && sdata.data[0] < min)
                    {
                        min = sdata.data[0];
                    }
                }
                return min;
            }
        }

        /// <summary>
        /// 维度Y数据的总和。
        /// </summary>
        public float yTotal
        {
            get
            {
                float total = 0;
                foreach (var sdata in data)
                {
                    if (sdata.show)
                        total += sdata.data[1];
                }
                return total;
            }
        }

        /// <summary>
        /// 维度X数据的总和。
        /// </summary>
        public float xTotal
        {
            get
            {
                float total = 0;
                foreach (var sdata in data)
                {
                    if (sdata.show)
                        total += sdata.data[0];
                }
                return total;
            }
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void ClearData()
        {
            m_XData.Clear();
            m_YData.Clear();
            m_Data.Clear();
        }

        /// <summary>
        /// 移除指定索引的数据
        /// </summary>
        /// <param name="index"></param>
        public void RemoveData(int index)
        {
            m_XData.RemoveAt(index);
            m_YData.RemoveAt(index);
            m_Data.RemoveAt(index);
        }

        /// <summary>
        /// 添加一个数据到维度Y（此时维度X对应的数据是索引）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dataName"></param>
        /// <param name="maxDataNumber"></param>
        public void AddYData(float value, string dataName = null, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            int xValue = m_XData.Count;
            m_XData.Add(xValue);
            m_YData.Add(value);
            m_Data.Add(new SerieData() { data = new List<float>() { xValue, value }, name = dataName });
        }

        /// <summary>
        /// 添加（x，y）数据到维度X和维度Y
        /// </summary>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        /// <param name="dataName"></param>
        /// <param name="maxDataNumber"></param>
        public void AddXYData(float xValue, float yValue, string dataName = null, int maxDataNumber = 0)
        {
            if (maxDataNumber > 0)
            {
                while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
                while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
            }
            m_XData.Add(xValue);
            m_YData.Add(yValue);
            m_Data.Add(new SerieData() { data = new List<float>() { xValue, yValue }, name = dataName });
        }

        /// <summary>
        /// 将一组数据添加到系列中。
        /// 如果数据只有一个，默认添加到维度Y中。
        /// </summary>
        /// <param name="valueList"></param>
        /// <param name="dataName"></param>
        /// <param name="maxDataNumber"></param>
        public void AddData(List<float> valueList, string dataName = null, int maxDataNumber = 0)
        {
            if (valueList == null || valueList.Count == 0) return;
            if (valueList.Count == 1)
            {
                AddYData(valueList[0], dataName, maxDataNumber);
            }
            else if (valueList.Count == 2)
            {
                AddXYData(valueList[0], valueList[1], dataName, maxDataNumber);
            }
            else
            {
                if (maxDataNumber > 0)
                {
                    while (m_XData.Count > maxDataNumber) m_XData.RemoveAt(0);
                    while (m_YData.Count > maxDataNumber) m_YData.RemoveAt(0);
                    while (m_Data.Count > maxDataNumber) m_Data.RemoveAt(0);
                }
                var serieData = new SerieData();
                serieData.name = dataName;
                for (int i = 0; i < valueList.Count; i++)
                {
                    if (i == 0) m_XData.Add(valueList[i]);
                    else if (i == 1) m_YData.Add(valueList[i]);
                    serieData.data.Add(valueList[i]);
                }
                m_Data.Add(serieData);
            }
        }

        /// <summary>
        /// 获得维度Y索引对应的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public float GetYData(int index, DataZoom dataZoom = null)
        {
            if (index < 0) return 0;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                return serieData[index].data[1];
            }
            return 0;
        }

        /// <summary>
        /// 获得维度Y索引对应的数据和数据名
        /// </summary>
        /// <param name="index">索引</param>
        /// <param name="yData">对应的数据值</param>
        /// <param name="dataName">对应的数据名</param>
        /// <param name="dataZoom">区域缩放</param>
        public void GetYData(int index, out float yData, out string dataName, DataZoom dataZoom = null)
        {
            yData = 0;
            dataName = null;
            if (index < 0) return;
            var serieData = GetDataList(dataZoom);
            if (index < serieData.Count)
            {
                yData = serieData[index].data[1];
                dataName = serieData[index].name;
            }
        }

        /// <summary>
        /// 获得指定索引的数据项
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public SerieData GetSerieData(int index, DataZoom dataZoom = null)
        {
            var data = GetDataList(dataZoom);
            if (index >= 0 && index <= data.Count - 1)
            {
                return data[index];
            }
            return null;
        }

        /// <summary>
        /// 获得指定索引的维度X和维度Y的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="dataZoom"></param>
        /// <param name="xValue"></param>
        /// <param name="yVlaue"></param>
        public void GetXYData(int index, DataZoom dataZoom, out float xValue, out float yVlaue)
        {
            xValue = 0;
            yVlaue = 0;
            if (index < 0) return;
            var showData = GetDataList(dataZoom);
            if (index < showData.Count)
            {
                var serieData = showData[index];
                xValue = serieData.data[0];
                yVlaue = serieData.data[1];
            }
        }

        /// <summary>
        /// 获得系列的数据列表
        /// </summary>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public List<SerieData> GetDataList(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((m_Data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((m_Data.Count - 1) * dataZoom.end / 100);
                var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                if (filterData == null || filterData.Count != count)
                {
                    UpdateFilterData(dataZoom);
                }
                return filterData;
            }
            else
            {
                return m_Data;
            }
        }

        /// <summary>
        /// 获得指定维数的最大最小值
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="dataZoom"></param>
        /// <returns></returns>
        public void GetMinMaxData(int dimension, out float minValue, out float maxValue, DataZoom dataZoom = null)
        {
            var dataList = GetDataList(dataZoom);
            float max = float.MinValue;
            float min = float.MaxValue;
            for (int i = 0; i < dataList.Count; i++)
            {
                var serieData = dataList[i];
                if (serieData.data.Count > dimension)
                {
                    var value = serieData.data[dimension];
                    if (value > max) max = value;
                    if (value < min) min = value;
                }
            }
            maxValue = max;
            minValue = min;
        }

        /// <summary>
        /// 根据dataZoom更新数据列表缓存
        /// </summary>
        /// <param name="dataZoom"></param>
        public void UpdateFilterData(DataZoom dataZoom)
        {
            if (dataZoom != null && dataZoom.show)
            {
                var startIndex = (int)((data.Count - 1) * dataZoom.start / 100);
                var endIndex = (int)((data.Count - 1) * dataZoom.end / 100);
                if (startIndex != filterStart || endIndex != filterEnd)
                {
                    filterStart = startIndex;
                    filterEnd = endIndex;
                    var count = endIndex == startIndex ? 1 : endIndex - startIndex + 1;
                    if (m_Data.Count > 0)
                    {
                        filterData = m_Data.GetRange(startIndex, count);
                    }
                    else
                    {
                        filterData = m_Data;
                    }
                }
                else if (endIndex == 0)
                {
                    filterData = new List<SerieData>();
                }
            }
        }

        /// <summary>
        /// 更新指定索引的维度Y数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void UpdateYData(int index, float value)
        {
            UpdateData(index, 2, value);
        }

        /// <summary>
        /// 更新指定索引的维度X和维度Y的数据
        /// </summary>
        /// <param name="index"></param>
        /// <param name="xValue"></param>
        /// <param name="yValue"></param>
        public void UpdateXYData(int index, float xValue, float yValue)
        {
            UpdateData(index, 1, xValue);
            UpdateData(index, 2, yValue);
        }

        /// <summary>
        /// 更新指定索引指定维数的数据
        /// </summary>
        /// <param name="index">要更新数据的索引</param>
        /// <param name="dimension">要更新数据的维数</param>
        /// <param name="value">新的数据值</param>
        public void UpdateData(int index, int dimension, float value)
        {
            if (index < 0) return;
            if (dimension == 1)
            {
                if (index < m_XData.Count) m_XData[index] = value;
            }
            else if (dimension == 2)
            {
                if (index < m_YData.Count) m_YData[index] = value;
            }
            if (index < m_Data.Count && dimension < m_Data[index].data.Count)
            {
                m_Data[index].data[dimension] = value;
            }
        }

        /// <summary>
        /// 清除所有数据的高亮标志
        /// </summary>
        public void ClearHighlight()
        {
            highlighted = false;
            foreach (var sd in m_Data)
            {
                sd.highlighted = false;
            }
        }

        /// <summary>
        /// 设置指定索引的数据为高亮状态
        /// </summary>
        /// <param name="index"></param>
        public void SetHighlight(int index)
        {
            if (index <= 0) return;
            for (int i = 0; i < m_Data.Count; i++)
            {
                m_Data[i].highlighted = index == i;
            }
        }

        public Color GetAreaColor(ThemeInfo theme, int index, bool highlight)
        {
            if (areaStyle.color != Color.clear)
            {
                var color = areaStyle.color;
                if (highlight) color *= color;
                color.a *= areaStyle.opactiy;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= areaStyle.opactiy;
                return color;
            }
        }

        public Color GetLineColor(ThemeInfo theme, int index, bool highlight)
        {
            if (lineStyle.color != Color.clear)
            {
                var color = lineStyle.color;
                if (highlight) color *= color;
                color.a *= lineStyle.opactiy;
                return color;
            }
            else
            {
                var color = (Color)theme.GetColor(index);
                if (highlight) color *= color;
                color.a *= lineStyle.opactiy;
                return color;
            }
        }

        /// <summary>
        /// 从json中导入数据
        /// </summary>
        /// <param name="jsonData"></param>
        public override void ParseJsonData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData) || !m_DataFromJson) return;
            jsonData = jsonData.Replace("\r\n", "");
            jsonData = jsonData.Replace(" ", "");
            jsonData = jsonData.Replace("\n", "");
            int startIndex = jsonData.IndexOf("[");
            int endIndex = jsonData.LastIndexOf("]");
            if (startIndex == -1 || endIndex == -1)
            {
                Debug.LogError("json data need include in [ ]");
                return;
            }
            ClearData();
            string temp = jsonData.Substring(startIndex + 1, endIndex - startIndex - 1);
            if (temp.IndexOf("],") > -1 || temp.IndexOf("] ,") > -1)
            {
                string[] datas = temp.Split(new string[] { "],", "] ," }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var data = datas[i].Split(new char[] { '[', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var serieData = new SerieData();
                    for (int j = 0; j < data.Length; j++)
                    {
                        var txt = data[j].Trim().Replace("]", "");
                        float value;
                        var flag = float.TryParse(txt, out value);
                        if (flag)
                        {
                            serieData.data.Add(value);
                            if (j == 0) m_XData.Add(value);
                            else if (j == 1) m_YData.Add(value);
                        }
                        else serieData.name = txt.Replace("\"", "").Trim();
                    }
                    m_Data.Add(serieData);
                }
            }
            else if (temp.IndexOf("value") > -1 && temp.IndexOf("name") > -1)
            {
                string[] datas = temp.Split(new string[] { "},", "} ,", "}" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < datas.Length; i++)
                {
                    var arr = datas[i].Replace("{", "").Split(',');
                    var serieData = new SerieData();
                    foreach (var a in arr)
                    {
                        if (a.StartsWith("value:"))
                        {
                            float value = float.Parse(a.Substring(6, a.Length - 6));
                            serieData.data = new List<float>() { i, value };
                        }
                        else if (a.StartsWith("name:"))
                        {
                            string name = a.Substring(6, a.Length - 6 - 1);
                            serieData.name = name;
                        }
                        else if (a.StartsWith("selected:"))
                        {
                            string selected = a.Substring(9, a.Length - 9);
                            serieData.selected = bool.Parse(selected);
                        }
                    }
                    m_Data.Add(serieData);
                }
            }
            else
            {
                string[] datas = temp.Split(',');
                for (int i = 0; i < datas.Length; i++)
                {
                    float value;
                    var flag = float.TryParse(datas[i].Trim(), out value);
                    if (flag)
                    {
                        var serieData = new SerieData();
                        serieData.data = new List<float>() { i, value };
                        m_Data.Add(serieData);
                    }
                }
            }
        }
    }
}
