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
                        acc200 = item["Number"].ToString(),
                        aac001 = item["AccountID"].ToString(),
                        acc201 = item["ResumeName"].ToString(),
                        acc202 = item["OpenStatus"].ToString(),
                        acc205 = item["Intention"].ToString(),
                        acc206 = item["Experiences"].ToString(),
                        acc208 = item["EducationAndTrainingBackground"].ToString(),
                        acc209 = item["SelfEvaluation"].ToString(),
                        aca111 = item["JobNumber"].ToString(),
                        aca112 = item["JobName"].ToString(),
                        acc034 = item["ExpectedSalary"].ToString(),
                        acc217 = item["Years"].ToString(),

                        //必填项 
                        //必填项 
                        //是否默认简历（0：默认，1：非默认）
                        acc203 = "0",
                        aae043 = item["UpdateTime"].ToString(),
                        yae100 = "攀枝花人才中心",
                        aae036 = item["UpdateTime"].ToString(),
                        aae017 = "攀枝花人才中心",
                        aae011 = "攀枝花市",
                        aae022 = "510401000000",
                        ycb213 = item["WorkingMode"].ToString()

                    };
                    list.Add(PersonResume);
                }
                return list;

            }
            return null;
        }
    }
}
