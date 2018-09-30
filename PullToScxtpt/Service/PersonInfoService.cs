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
          
            string cmdText = @"SELECT  pbi.AccountID ,
                                pbi.PersonName ,
                                pbi.IDCardNo ,
                                pbi.Sex ,
                                nat.ItemName natItem ,
                                pbi.Birthday ,
                                pbi.MaritalStatus ,
                                ps.ItemName psItem ,
                                we.Years ,
                                deg.ItemName degItem ,
                                eat.GraduateSchool ,
                                eat.Major ,
                                eat.GraduationTime ,
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
            DataTable personTable = SqlHelper.ExecuteDataTable(cmdText,new SqlParameter());
            List<PersonInfo> list = new List<PersonInfo>();
            if (personTable.Rows.Count>0)
            {
                foreach (DataRow item in personTable.Rows)
                {
                    PersonInfo personInfo = new PersonInfo()
                    {
                        AAC001 = item["AccountID"].ToString(),
                        AAC003 = item["PersonName"].ToString(),
                        AAC002 = item["IDCardNo"].ToString(),
                        AAC004 = item["Sex"].ToString(),
                        AAC005 = item["natItem"].ToString(),
                        AAC006 = item["Birthday"].ToString(),
                        AAC017 = item["MaritalStatus"].ToString(),
                        AAC024 = item["psItem"].ToString(),
                        ACC217 = item["Years"].ToString(),
                        AAC011 = item["degItem"].ToString(),
                        YAU002 = item["GraduateSchool"].ToString(),
                        AAC183 = item["Major"].ToString(),
                        YAC01G = item["GraduationTime"].ToString(),
                        ACB501 = item["Mobile"].ToString(),
                        AAC010 = item["Height"].ToString(),
                        AAB301 = item["name"].ToString(),

                    };

                    list.Add(personInfo);
                }
                return list;
            }
            return null;
        }

        
         
                        

      
     
    }
}
