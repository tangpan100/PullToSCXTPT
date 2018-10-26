using PullToScxtpt_px.Helper;
using PullToScxtpt_px.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace PullToScxtpt_px
{
    public class PersonInfoService
    {
        public List<PersonInfo> QueryPersonInfo()
        {

            string cmdText = @"SELECT    LEFT(p.AccountID, 18) AccountID ,
                                        p.PersonName ,
                                        p.IDCardNo ,
                                        Sex = CASE WHEN p.Sex = '男' THEN 1
                                                   ELSE 2
                                              END ,
                                        nat.ID natItemCode ,
                                        CONVERT(VARCHAR(100), p.Birthday, 20) Birthday ,
                                        MaritalStatus = CASE WHEN p.MaritalStatus = 0
                                                             THEN 1
                                                             WHEN p.MaritalStatus = 1
                                                             THEN 2
                                                             ELSE 4
                                                        END ,
                                        ps.ID psItemCode ,
                                        Years = CASE WHEN we.Years < 1
                                                          AND we.Years > 0
                                                     THEN 1
                                                     WHEN we.Years < 3 THEN 2
                                                     WHEN we.Years >= 3
                                                          AND we.Years <= 5
                                                     THEN 3
                                                     WHEN we.Years > 5
                                                          AND we.Years <= 10
                                                     THEN 4
                                                     WHEN we.Years > 10 THEN 5
                                                     ELSE 0
                                                END ,
                                        deg.ID degItemCode ,
                                        eat.GraduateSchool ,
                                        eat.Major ,
                                        CONVERT(VARCHAR(100), eat.GraduationTime, 20) GraduationTime ,
                                        p.Mobile ,
                                        p.Height ,
                                        ic.Addr
                              FROM      ( SELECT    pbi.Id pid ,
                                                    pbi.AccountID ,
                                                    pbi.PersonName ,
                                                    pbi.Sex ,
                                                    pbi.IDCardNo ,
                                                    pbi.Birthday ,
                                                    pbi.NationID ,
                                                    pbi.PermanentResidenceID ,
                                                    pbi.PoliticalStatusID ,
                                                    pbi.MaritalStatus ,
                                                    pbi.Height ,
                                                    pbi.Mobile ,
                                                    res.Id rid ,
                                                    res.InfoID ,
                                                    res.IsAudited
                                          FROM      PersonBaseInfo pbi
                                                    JOIN dbo.PersonResumes res ON res.InfoID = pbi.Id
                                          WHERE     res.IsAudited = 1 AND IDCardNo !='' AND IDCardNo IS NOT NULL
                                        ) p
                                        LEFT  JOIN ItemNation nat ON nat.ID = p.NationID
                                        LEFT  JOIN ItemPoliticalStatus ps ON ps.ID = p.PoliticalStatusID
                                        LEFT  JOIN dbo.PersonWorkExperience we ON we.ResumeID = rid
                                        LEFT  JOIN dbo.PersonEducationAndTraining eat ON eat.ResumeID = rid
                                        LEFT  JOIN dbo.ItemDegree deg ON eat.HighestDegreeID = deg.ID
                                        LEFT  JOIN ( SELECT c1.ID ,
                                                            c2.ItemName
                                                            + c1.ItemName Addr
                                                     FROM   dbo.ItemCities c1 ,
                                                            dbo.ItemCities c2
                                                     WHERE  c1.ParentID = CONVERT(VARCHAR(50), c2.ID)
                                                   ) ic ON ic.ID = p.PermanentResidenceID";
            DataTable personTable = SqlHelper.ExecuteDataTable(cmdText, new SqlParameter("@param", DBNull.Value));
            List<CodeMapper> codeMappers = SqlHelper.QueryCodeMapper();

            string cmdText2 = "select * from PullInfoRecord where type='个人信息'";
            DataTable yetpersonInfoTable = SqlHelper.ExecuteDataTable(cmdText2, new SqlParameter("@param", DBNull.Value));
            List<YetInsertInfo> YetInsertInfolist = new List<YetInsertInfo>();
            List<PersonInfo> personInfolist = new List<PersonInfo>();

            if (yetpersonInfoTable.Rows.Count > 0)
            {

                foreach (DataRow item in yetpersonInfoTable.Rows)
                {
                    YetInsertInfo yetInsertInfo = new YetInsertInfo()
                    {
                        number = item["number"].ToString(),
                        type = item["type"].ToString(),
                        updateTime = item["updateTime"].ToString()

                    };
                    YetInsertInfolist.Add(yetInsertInfo);
                }
            }

            if (personTable.Rows.Count > 0)
            {

                foreach (DataRow item in personTable.Rows)
                {
                    PersonInfo personInfo = new PersonInfo()
                    {
                        aac001 = item["AccountID"].ToString(),
                        aac003 = item["PersonName"].ToString(),
                     
                        aac004 = item["Sex"].ToString(),

                        aac006 = item["Birthday"].ToString(),
                        aac017 = item["MaritalStatus"].ToString(),

                        acc217 = item["Years"].ToString(),


                       
                        //aac183 = "070900",
                       
                       
                        aac010 = item["Height"].ToString(),
                        aab301 = "510400000000",


                        aae011 = item["PersonName"].ToString(),
                        aae017 = "攀西人才网",
                        aae036 = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        aae396 = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss"),
                        aae022 = "510400000000",
                    };
                    //毕业时间
                    string yac01g = item["GraduationTime"].ToString();
                    if (string.IsNullOrWhiteSpace(yac01g))
                    {
                        personInfo.yac01g = "无";
                    }
                    else
                    {
                        personInfo.yac01g = yac01g;
                    }
                    //毕业院校
                    string yau002 = item["GraduateSchool"].ToString();
                    if (yau002!=null&& yau002.Length>200)
                    {
                        personInfo.yau002 = yau002.Substring(0, 200);
                    }
                    else
                    {
                        personInfo.yau002 = "无";
                    }

                    //身份证号
                    string aac002 = item["IDCardNo"].ToString().ToLower();
                    if (string.IsNullOrWhiteSpace(aac002))
                    {
                        continue;
                        //身份证号为空则不传这条数据
                    }
                    else
                    {
                        personInfo.aac002 = aac002;
                    }
                   string acb501 = item["Mobile"].ToString();
                    //手机号为空就不传
                    if (string.IsNullOrWhiteSpace(acb501))
                    {
                        continue;
                    }
                    else
                    {
                        personInfo.acb501 = acb501;
                    }
                    //政治面貌
                    var aac024 = codeMappers.Where(c => item["psItemCode"].ToString()
                    .Equals(c.localCodeValue)).Where(c=>c.codeType.Equals("AAC024")).FirstOrDefault();
                    if (aac024 == null)
                    {
                        //群众
                        personInfo.aac024 = "13";
                    }
                    else
                    {
                        personInfo.aac024 = aac024.codeValue.ToString();
                    }
                    //民族
                    var aac005 = codeMappers.Where(c => item["natItemCode"].ToString().
                    Equals(c.localCodeValue)).FirstOrDefault();

                    if (aac005 == null)
                    {
                        personInfo.aac005 = "01";
                    }
                    else
                    {
                        personInfo.aac005 = aac005.codeValue.ToString();
                    }

                    //文化程度
                    var aac011 = codeMappers.Where(c => item["degItemCode"].ToString().
                    Equals(c.localCodeValue)).Where(c => c.codeType.Equals("AAC011")).FirstOrDefault();
                    if (aac011 == null)
                    {
                        personInfo.aac011 = "0";
                    }
                    else
                    {
                        personInfo.aac011 = aac011.codeValue.ToString();
                    }
                    //联系地址
                    personInfo.aae006 = item["Addr"].ToString();
                    if (string.IsNullOrEmpty(personInfo.aae006))
                    {
                        personInfo.aae006 = "攀枝花市";
                    }
                    else
                    {
                        personInfo.aae006 = item["Addr"].ToString();
                    }
                
                    personInfo.aae011 = item["PersonName"].ToString();
                    //经办人
               
                    if (string.IsNullOrWhiteSpace(personInfo.aae011))
                    {
                        personInfo.aae011 = "攀枝花市人才服务中心";
                    }
                    else
                    {
                        personInfo.aae011 = item["IDCardNo"].ToString();
                    }
                    personInfolist.Add(personInfo);
                }

            }
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            //需要推送的信息 过滤：未插入，插入但更新时间大于推送时间
            List<PersonInfo> personInfos1 = personInfolist.Where(r => !YetInsertInfolist.Any(y => y.number == r.aac001)).ToList();
            //List<PersonInfo> personInfos2 = personInfolist.Where(r => YetInsertInfolist.Any(y => y.number == r.aac001 && Convert.ToDateTime(y.updateTime, dtFormat)
            //< Convert.ToDateTime(r.aae396, dtFormat))).ToList();
            //List<PersonInfo> personInfos = personInfos1.Union(personInfos2).ToList();
            return personInfos1;
        }


    }
}
