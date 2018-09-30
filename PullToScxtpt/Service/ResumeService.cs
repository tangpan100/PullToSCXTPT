using PullToScxtpt.Helper;
using PullToScxtpt.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PullToScxtpt.Service
{
    public class ResumeService
    {
        public List<PersonResume> QueryPersonResume()
        {
            string comText = @"SELECT pr.Number,pbi.AccountID ,pr.UpdateTime,
                                    pr.ResumeName ,
                                    pr.OpenStatus ,
                                    Intention = CASE WHEN jwi.Intention = NULL THEN 1
                                                     ELSE 0
                                                END ,
                                    Experiences = CASE WHEN pwe.Experiences = NULL THEN 1
                                                       ELSE 0
                                                  END ,
                                    EducationAndTrainingBackground = CASE WHEN et.EducationAndTrainingBackground = NULL
                                                                          THEN 1
                                                                          ELSE 0
                                                                     END ,
                                    pr.SelfEvaluation ,
                                    cj.JobNumber ,
                                    cj.JobName,
                                    WorkingMode=CASE WHEN jwi.WorkingMode=1 THEN '全职' WHEN jwi.WorkingMode=2 THEN '兼职' ELSE '实习' END,
                                    pwe.Years
                             FROM   dbo.PersonResumes pr
                                    JOIN dbo.PersonBaseInfo pbi ON pr.InfoID=pbi.Id
                                    JOIN dbo.PersonJobWantedIntention jwi ON jwi.ResumeID = pr.Id
                                    JOIN PersonWorkExperience pwe ON pwe.ResumeID = pr.Id
                                    JOIN dbo.PersonEducationAndTraining et ON et.ResumeID = pr.Id
                                    JOIN dbo.CompanyJobs cj ON CONVERT(VARCHAR(50), cj.JobID) = jwi.ExpectedJobID
                                
                             WHERE  pr.IsAudited = 1";
            DataTable resumeInfoTable = SqlHelper.ExecuteDataTable(comText, new SqlParameter());
            if (resumeInfoTable.Rows.Count > 0)
            {
                List<PersonResume> list = new List<PersonResume>();
                foreach (DataRow item in resumeInfoTable.Rows)
                {
                    PersonResume PersonResume = new PersonResume()
                    {
                        ACC200 = item["Number"].ToString(),
                        AAC001 = item["AccountID"].ToString(),
                        ACC201 = item["ResumeName"].ToString(),
                        ACC202 = item["OpenStatus"].ToString(),
                        ACC205 = item["Intention"].ToString(),
                        ACC206 = item["Experiences"].ToString(),
                        ACC208 = item["EducationAndTrainingBackground"].ToString(),
                        ACC209 = item["SelfEvaluation"].ToString(),
                        ACA111 = item["JobNumber"].ToString(),
                        ACA112 = item["JobName"].ToString(),
                        ACC034 = item["ExpectedSalary"].ToString(),
                        ACC217 = item["Years"].ToString(),

                        //必填项 
                        //必填项 
                        //是否默认简历（0：默认，1：非默认）
                        ACC203 = "0",
                        AAE043 = item["UpdateTime"].ToString(),
                        YAE100 = "攀枝花人才中心",
                        AAE036 = item["UpdateTime"].ToString(),
                        AAE017 = "攀枝花人才中心",
                        AAE011 = "攀枝花市",
                        AAE022 = "510401000000",
                        YCB213 = item["WorkingMode"].ToString()

                    };
                    list.Add(PersonResume);
                }
                return list;

            }
            return null;
        }
    }
}
