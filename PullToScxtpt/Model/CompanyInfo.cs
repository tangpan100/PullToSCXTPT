using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace PullToScxtpt
{
   // [XmlRoot(ElementName = "input")]
    public class CompanyInfo
    {
         /// <summary>
         /// 单位编号
         /// </summary>
        public string aab001 { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string aab004 { get; set; }
        /// <summary>
        /// 统一社会信用代码
        /// </summary>
        public string aab998 { get; set; }
        /// <summary>
        /// 所属行业
        /// </summary>
        public string aab022 { get; set; }
        /// <summary>
        /// 单位类型
        /// </summary>
        public string aab019 { get; set; }
        /// <summary>
        /// 经济类型 未了解
        /// </summary>
        public string aab020 { get; set; }
        /// <summary>
        /// 注册资金
        /// </summary>
        public string aab049 { get; set; }
        /// <summary>
        /// 单位地址
        /// </summary>
        public string aae006 { get; set; }
        /// <summary>
        /// 邮政编码
        /// </summary>
        public string aae007 { get; set; }
        /// <summary>
        /// 单位网站
        /// </summary>
        public string aae392 { get; set; }
        /// <summary>
        /// 单位简介
        /// </summary>
        public string aab092 { get; set; }
        /// <summary>
        /// 法定代表人
        /// </summary>
        public string aab013 { get; set; }
        /// <summary>
        /// 法定代表人电话
        /// </summary>
        public string aab015 { get; set; }
        /// <summary>
        /// 联系人
        /// </summary>
        public string aae004 { get; set; }
        /// <summary>
        /// 联系电话
        /// </summary>
        public string aae005 { get; set; }
        /// <summary>
        /// 联系手机
        /// </summary>
        public string acb501 { get; set; }
        /// <summary>
        /// 电子邮箱
        /// </summary>
        public string aae159 { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public string aae036 { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public string aae396 { get; set; }
        /// <summary>
        /// 登记机构
        /// </summary>
        public string aae017 { get; set; }
        /// <summary>
        /// 登记地区行政区划代码
        /// </summary>
        public string aae022 { get; set; }
        /// <summary>
        /// 人员规模
        /// </summary>
        public string aab056 { get; set; }
        /// <summary>
        /// 所属地区行政区划代码 
        /// </summary>
        public string aab301 { get; set; }
        /// <summary>
        /// 登记人（填市州名称）未了解
        /// </summary>
        public string aae011 { get; set; }
        /// <summary>
        /// 数据来源 不用填写
        /// </summary>
        public string yae100 { get; set; }
    }
}
