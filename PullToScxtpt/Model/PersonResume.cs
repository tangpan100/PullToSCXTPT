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
        public string acc200 { get;set;}
        //个人编号
        public string aac001 { get;set;}
        //简历名称
        public string acc201 { get;set;}
        //是否公开简历（0：公开，1：不公开）
        public string acc202 { get;set;}
        //是否存在求职意向（0：是，1：否）
        public string acc205 { get;set;}
        //是否存在工作经历（0：是，1：否）
        public string acc206 { get;set;}
        //是否存在培训经历（0：是，1：否）
        public string acc208 { get;set;}
        //自我评价
        public string acc209 { get;set;}
        //岗位编号 
        public string aca111 { get;set;}
        //岗位名称 
        public string aca112 { get; set; }
        //期望薪资
        public string acc034 { get;set;}
        //工作年限
        public string acc217 { get; set; }


        //必填项 
        //是否默认简历（0：默认，1：非默认）
        public string acc203 { get; set; }
        //登记日期
        public string aae043 { get; set; }
        //数据来源
        public string yae100 { get; set; }
        //登记时间
        public string aae036 { get; set; }
        //登记机构
        public string aae017 { get; set; }
        //登记人
        public string aae011 { get; set; }
        //登记地区行政区划代码
        public string aae022 { get; set; }
        //工作方式：全职，兼职，实习
        public string ycb213 { get; set; }
        //工作地点代码
        public string acb215 { get; set; }

    }

}
