using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PullToScxtpt.Model
{
    /// <summary>
    /// 个人简历
    /// </summary>
    public class PersonResume
    {
        //简历编号
        public string ACC200 { get;set;}
        //个人编号
        public string AAC001 { get;set;}
        //简历名称
        public string ACC201 { get;set;}
        //是否公开简历（0：公开，1：不公开）
        public string ACC202 { get;set;}
        //是否存在求职意向（0：是，1：否）
        public string ACC205 { get;set;}
        //是否存在工作经历（0：是，1：否）
        public string ACC206 { get;set;}
        //是否存在培训经历（0：是，1：否）
        public string ACC208 { get;set;}
        //自我评价
        public string ACC209 { get;set;}
        //岗位编号 
        public string ACA111 { get;set;}
        //岗位名称 
        public string ACA112 { get; set; }
        //期望薪资
        public string ACC034 { get;set;}
        //工作年限
        public string ACC217 { get; set; }


        //必填项 
        //是否默认简历（0：默认，1：非默认）
        public string ACC203 { get; set; }
        //登记日期
        public string AAE043 { get; set; }
        //数据来源
        public string YAE100 { get; set; }
        //登记时间
        public string AAE036 { get; set; }
        //登记机构
        public string AAE017 { get; set; }
        //登记人
        public string AAE011 { get; set; }
        //登记地区行政区划代码
        public string AAE022 { get; set; }
        //工作方式：全职，兼职，实习
        public string YCB213 { get; set; }
   
    }

}
