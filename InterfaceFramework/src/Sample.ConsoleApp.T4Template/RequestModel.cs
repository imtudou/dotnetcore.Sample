using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sample.ConsoleApp.T4Template
{
    /// <summary>
    /// 接口信息
    /// </summary>
    public  class RequestModel
    {
        /// <summary>
        /// 接口名称
        /// </summary>
        public string ApiName { get; set; }
        /// <summary>
        /// 接口描述
        /// </summary>
        public string ApiDescribe { get; set; }
        /// <summary>
        /// 接口数据类型
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 参数类型{List:集合,T或者其它:一条}
        /// </summary>
        public string ParamType { get; set; }
        /// <summary>
        /// 接口参数
        /// </summary>
        public List<Params> Params { get; set; }

        /// <summary>
        /// 数据转换
        /// </summary>
        public List<DataConvertItem> DataConvertItems { get; set; }
        /// <summary>
        /// 流程节点配置
        /// </summary>

        public List<ProcessNode> ProcessNodes { get; set; }
        /// <summary>
        /// api后置事件
        /// </summary>

        public List<EventItem> PostpositionEvent { get; set; }
        /// <summary>
        /// 转发()
        /// </summary>
        public List<DataForwardItem> DataForwardItems { get; set; }

        ///// <summary>
        ///// DB数据转换
        ///// </summary>
        //public List<DataConvertItem> DBDataConvertItems { get; set; }
        ///// <summary>
        ///// WS数据转换
        ///// </summary>
        //public List<DataConvertItem> WSDataConvertItems { get; set; }
    }

    #region 参数部分
    /// <summary>
    /// 接口参数
    /// </summary>
    public class Params
    {
        /// <summary>
        /// 主键(虚拟)
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 参数名称
        /// </summary>
        public string ParamName { get; set; }
        /// <summary>
        /// 参数类型{List:集合,T:一条}
        /// </summary>
        public string ParamType { get; set; }
        /// <summary>
        /// 参数描述
        /// </summary>
        public string ParamDescribe { get; set; }
        /// <summary>
        /// 子参数
        /// </summary>
        public List<Params> SubParam { get; set; }
        /// <summary>
        /// 验证
        /// </summary>
        public List<Validation> Validations { get; set; }
    }
    /// <summary>
    /// 校验
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// 校验类型
        /// </summary>
        public ValidationTypeEnum ValidationType { get; set; }
        /// <summary>
        /// 参数
        /// </summary>

        public string Parameter { get; set; }
        /// <summary>
        /// 校验类型
        /// </summary>
        public string ErrorMessage { get; set; }
    }
    /// <summary>
    /// 校验类型
    /// </summary>
    public enum ValidationTypeEnum
    {
        /// <summary>
        /// 必须满足不为空
        /// </summary>
        Required,
        /// <summary>
        /// 指定字符允许的范围
        /// </summary>
        StringLength,
        /// <summary>
        /// 正则表达式验证
        /// </summary>
        RegularExpression
    }

    #endregion

    #region 数据转换部分
    //数据转换
    /// <summary>
    /// 数据转换项
    /// </summary>
    public class DataConvertItem
    {
        /// <summary>
        /// 数据转换唯一标识
        /// </summary>
        public string DataConvertCode { get; set; }
        /// <summary>
        /// 数据转换名称
        /// </summary>
        public string DataConvertName { get; set; }
        /// <summary>
        /// 数据转换类型{db:数据库,webapi:webapi接口,ws:webservice,f:函数}
        /// </summary>
        public string DataConvertType { get; set; }
        /// <summary>
        /// 前置转换
        /// </summary>
        public string PreDataConvert { get; set; }
        /// <summary>
        /// 数据库名称(用户获取数据库连接字符串)
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// 数据表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// web 服务地址
        /// </summary>
        public string WSURL { get; set; }
        /// <summary>
        /// we服务函数
        /// </summary>
        public string WSMethodName { get; set; }
        /// <summary>
        /// 结果匹配配置
        /// </summary>
        public List<ResultMatching> ResultMatchings { get; set; }
        /// <summary>
        /// 转换明细
        /// </summary>
        public List<Items> Items { get; set; }
    }
    /// <summary>
    /// 转换项
    /// </summary>
    public class Items
    {
        /// <summary>
        /// 唯一值
        /// </summary>
        public string Unique { get; set; }
        /// <summary>
        /// 字段名|表名称
        /// <para>1.转换类型!=(List||T) 时 该项为数据库字段</para> 
        /// <para>2.转换类型==(List||T) 时 该项为数据库表名称</para>
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// json类型{List:数组,T:对象}
        /// </summary>
        public string JsonTpye { get; set; }
        /// <summary>
        /// 转换类型{序列:系统自增字段,源编号:第一次落地的编号,保存:直接保存或者格式化,高级保存:全局参数,Guid:自动生成Guid编号,List:数据表(插入多条数据),T:数据表(插入一条数据)}
        /// </summary>
        public string ConvertType { get; set; }
        /// <summary>
        /// 参数格式化(可直接输入参数名或者格式化)
        /// </summary>
        public string FormatParam { get; set; }
        /// <summary>
        /// 字段配置
        /// </summary>
        public DictionaryConfig DictionaryConfig { get; set; }
        /// <summary>
        /// 子表数据(转换类型==(List||T) 时 该项有效)
        /// </summary>
        public List<Items> SubItems { get; set; }
    }
    /// <summary>
    /// 字典转换配置
    /// </summary>
    public class DictionaryConfig
    {
        string valueField = "Value";
        /// <summary>
        /// 字典Code值
        /// </summary>
        public string CodeValue { set; get; }
        /// <summary>
        /// 值字段
        /// </summary>
        public string ValueField
        {
            get { return valueField; }
            set { valueField = value; }
        }
    }
    /// <summary>
    /// 结果匹配
    /// </summary>
    public class ResultMatching
    {
        /// <summary>
        /// 结果匹配的运算符
        /// </summary>
        public string ResultMatchingOperator { set; get; }
        /// <summary>
        /// 结果匹配类型({string:字符串匹配,CodeSnippet:代码片段匹配})
        /// </summary>
        public string ResultMatchingType { set; get; }
        /// <summary>
        /// 结果匹配表达式
        /// </summary>
        public string ResultMatchingExpression { set; get; }
    }
    #endregion

    #region 流程节点
    /// <summary>
    /// 流程节点
    /// </summary>
    public class ProcessNode
    {
        /// <summary>
        /// 节点描述
        /// </summary>
        public string NodeDescribe { get; set; }
        /// <summary>
        /// 节点(转换唯一标识 DataConvertItem.DataConvertCode)
        /// </summary>
        public string Node { get; set; }
        /// <summary>
        /// 节点类型
        /// </summary>
        public string NodeType { get; set; }
        /// <summary>
        /// 节点条件项
        /// </summary>
        public List<NodeConditionItem> NodeConditionItems { get; set; }
    }
    /// <summary>
    /// 节点条件项
    /// </summary>
    public class NodeConditionItem
    {
        /// <summary>
        /// 结果值类型
        /// </summary>
        public string ResultsType { get; set; }
        /// <summary>
        /// 条件表达式
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// 下一个节点
        /// </summary>
        public List<string> NextNodes { get; set; }
    }
    #endregion

    #region 转发部分
    public class DataForwardItem
    {
        public string billno { get; set; }
        public string Url { get; set; }
    }
    #endregion

    #region 事件部分
    /// <summary>
    /// 事件明细
    /// </summary>
    public class EventItem
    {
        /// <summary>
        /// 事件名称
        /// </summary>
        public string EventName { get; set; }
        /// <summary>
        /// 事件描述
        /// </summary>
        public string EventDes { get; set; }
        /// <summary>
        /// 事件类型{proc:存储过程}
        /// </summary>
        public string EventType { get; set; }
        /// <summary>
        /// 事件发生所在数据库
        /// </summary>
        public string DataBaseName { get; set; }
        /// <summary>
        /// 事件内容(例如存储过程名称)
        /// </summary>
        public string EventContent { get; set; }
    }
    #endregion
}
