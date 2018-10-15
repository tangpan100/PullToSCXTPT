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
            string comText = @"SELECT top 100 pr.Number,left(pbi.AccountID,20)AccountID,
                                    CONVERT(varchar(100),  pr.UpdateTime, 20)UpdateTime ,
                                    pr.ResumeName ,
                                    OpenStatus =case when pr.OpenStatus=0 then 0 else 1 end,
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
                                    SUBSTRING(pr.SelfEvaluation, 0, 500) SelfEvaluation ,
                                    cj.JobID ,
                                    cj.JobName,
                                    WorkingMode=CASE WHEN jwi.WorkingMode=2 THEN '1' WHEN jwi.WorkingMode=1 THEN '2' ELSE '3' END,
                                    Years = case when pwe.Years<1 and pwe.Years>0 then 1  when pwe.Years<3 then 2
                                    when pwe.Years>=3 and pwe.Years<=5 then 3  when pwe.Years>5 and pwe.Years<=10 then 4 when pwe.Years>10 then 5 else 0 end
                             FROM   dbo.PersonResumes pr
                                    JOIN dbo.PersonBaseInfo pbi ON pr.InfoID=pbi.Id
                                    JOIN dbo.PersonJobWantedIntention jwi ON jwi.ResumeID = pr.Id
                                    JOIN PersonWorkExperience pwe ON pwe.ResumeID = pr.Id
                                    JOIN dbo.PersonEducationAndTraining et ON et.ResumeID = pr.Id
                                    JOIN (select *from( select *,ROW_NUMBER() over(partition by p2.JobId 
                                    order by p2.JobId)sequence from RecruitmentJobs p2 )t where t.sequence=1)
                                     cj ON CONVERT(VARCHAR(50), cj.JobID) = (SELECT top 1 * FROM dbo.StringSplit(jwi.ExpectedJobID ,','))
                                
                             WHERE  pr.IsAudited = 1";
            DataTable resumeInfoTable = SqlHelper.ExecuteDataTable(comText, new SqlParameter("@param", DBNull.Value));
            List<JobCodeMapper> codeMappers = SqlHelper.QueryJobCodeMapper();

            string cmdText2 = "select * from PullInfoRecord where type='个人简历'";
            DataTable yetresumeInfoTable = SqlHelper.ExecuteDataTable(cmdText2, new SqlParameter("@param", DBNull.Value));
            List<YetInsertInfo> YetInsertInfolist = new List<YetInsertInfo>();
            List<PersonResume> resumeInfolist = new List<PersonResume>();
            if (yetresumeInfoTable.Rows.Count>0)
            {
               
                foreach (DataRow item in yetresumeInfoTable.Rows)
                {
                    YetInsertInfo yetInsertInfo = new YetInsertInfo()
                    {
                        number = item["number"].ToString(),
                        type = item["number"].ToString(),
                        updateTime =item["updateTime"].ToString()

                    };
                    YetInsertInfolist.Add(yetInsertInfo);
                }
            }

            if (resumeInfoTable.Rows.Count > 0)
            {
              
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
                        //acc209 = item["SelfEvaluation"].ToString(),

                        //aca111 = 

                     
                        aca112 = item["JobName"].ToString(),

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
                        ycb213 = item["WorkingMode"].ToString(),
                        acb215 = "510401000000"
                    };
                    //PersonResume.aca111
                        JobCodeMapper cm= codeMappers.Where(c => !string.IsNullOrWhiteSpace(c.ID)&& c.ID.Equals(item["JobID"])).FirstOrDefault();
                    if (cm == null)
                    {
                        PersonResume.aca111 = "4010000";
                    }
                    else
                    {
                        PersonResume.aca111 = cm.typeCode;
                    }
                    resumeInfolist.Add(PersonResume);
                }
           

            }

            List<string> ylist = YetInsertInfolist.Select(y => y.number).ToList();
            List<PersonResume> personResumes = resumeInfolist.Where(r => !ylist.Any(y => y == r.acc200)).ToList();
            return personResumes;
        }
    }
}
