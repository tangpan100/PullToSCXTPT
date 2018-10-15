using PullToScxtpt.Helper;
using PullToScxtpt.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PullToScxtpt
{
    public class PersonInfoService
    {
        public List<PersonInfo> QueryPersonInfo()
        {
          
            string cmdText = @"SELECT top 100  LEFT(pbi.AccountID,20)AccountID ,
                                pbi.PersonName ,
                                pbi.IDCardNo ,
                                Sex= case when pbi.Sex='男' then 1 else 2 end ,
                                nat.ItemName natItem ,
                                CONVERT(varchar(100),  pbi.Birthday, 20)Birthday ,
                                MaritalStatus =case when pbi.MaritalStatus=0 then 1  when pbi.MaritalStatus=1 then 2 else 4 end ,
                                ps.ItemName psItem ,
                                Years = case when we.Years<1 and we.Years>0 then 1  when we.Years<3 then 2
                                when we.Years>=3 and we.Years<=5 then 3  when we.Years>5 and we.Years<=10 then 4 when we.Years>10 then 5 else 0 end,
                                deg.ItemName degItem ,
                                eat.GraduateSchool ,
                                eat.Major ,
                                CONVERT(varchar(100),  eat.GraduationTime, 20)GraduationTime ,
                                pbi.Mobile ,
                                pbi.Height,
                                ic.name
                        FROM    PersonBaseInfo pbi
                                JOIN ItemNation nat ON nat.ID = pbi.NationID
                                JOIN ItemPoliticalStatus ps ON ps.ID = pbi.PoliticalStatusID
                                JOIN dbo.PersonResumes res ON res.InfoID = pbi.Id
                                JOIN dbo.PersonWorkExperience we ON we.ResumeID = res.Id
                                JOIN dbo.PersonEducationAndTraining eat ON eat.ResumeID = res.Id
                                JOIN dbo.ItemDegree deg ON eat.HighestDegreeID = deg.ID
                                JOIN (SELECT   c1.ID ,
                                                c2.ItemName + c1.ItemName name
                                       FROM     dbo.ItemCities c1 ,
                                                dbo.ItemCities c2
                                       WHERE    c1.ParentID = CONVERT(VARCHAR(50),c2.ID))ic
                                       ON ic.id=pbi.PermanentResidenceID";
            DataTable personTable = SqlHelper.ExecuteDataTable(cmdText, new SqlParameter("@param", DBNull.Value));
            List<CodeMapper> codeMappers = SqlHelper.QueryCodeMapper();

            if (personTable.Rows.Count>0)
            {
                List<PersonInfo> list = new List<PersonInfo>();
                foreach (DataRow item in personTable.Rows)
                {
                    PersonInfo personInfo = new PersonInfo()
                    {
                        aac001 = item["AccountID"].ToString(),
                        aac003 = item["PersonName"].ToString(),
                        aac002 = item["IDCardNo"].ToString(),
                        aac004 = item["Sex"].ToString(),
                        
                        aac005 = codeMappers.Where(c => item["natItem"].ToString().Equals(c.localCodeExplain)).FirstOrDefault().codeValue.ToString(),
                        aac006 = item["Birthday"].ToString(),
                        aac017 = item["MaritalStatus"].ToString(),
                       
                        aac024 = codeMappers.Where(c => item["psItem"].ToString().Equals(c.localCodeExplain)).FirstOrDefault().codeValue.ToString(),
                        acc217 = item["Years"].ToString(),
                      
                        aac011 = codeMappers.Where(c => item["degItem"].ToString().Equals(c.localCodeExplain)).FirstOrDefault().codeValue.ToString(),

                        yau002 = item["GraduateSchool"].ToString(),
                        //aac183 = "070900",
                        yac01g = item["GraduationTime"].ToString(),
                        acb501 = item["Mobile"].ToString(),
                        aac010 = item["Height"].ToString(),
                        aab301 = "510400000000",

                        aae006= item["name"].ToString(),
                        aae011= item["PersonName"].ToString(),
                        aae017="攀西人才网",
                        aae036 = DateTime.Now.ToLocalTime().ToString(),
                        aae396 = DateTime.Now.ToLocalTime().ToString(),
                        aae022 = "510400000000",
                    };

                    list.Add(personInfo);
                }
                return list;
            }
            return null;
        }

        
         
                        

      
     
    }
}
