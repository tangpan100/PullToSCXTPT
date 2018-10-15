using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PullToScxtpt.Model
{
    public class CodeMapper
    {
        //代码类别

        public string codeType { get; set; }
        //代码名称

        public string codeName { get; set; }
        //代码值

        public string codeValue { get; set; }
        //代码说明

        public string codeExplain { get; set; }
        //本地代码值

        public string localCodeValue { get; set; }
        //本地代码说明
        public string localCodeExplain { get; set; }

    }
    public class JobCodeMapper
    {
        //岗位编号
        public string ID { get; set; }
        //岗位名称
        public string ItemName{ get; set; }
        //大类代码值
        public string typeCode { get; set; }

    }
   


}
