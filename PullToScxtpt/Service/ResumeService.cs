﻿using PullToScxtpt_px.Helper;
using PullToScxtpt_px.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PullToScxtpt_px.Service
{
    public class ResumeService
    {
        public List<PersonResume> QueryPersonResume()
        {
            string comText = @"SELECT   *
                                               FROM     ( SELECT    pr.Number ,
                                                                    ROW_NUMBER() OVER ( PARTITION BY Number ORDER BY Number ) sequence ,
                                                                    LEFT(pbi.AccountID, 18) AccountID ,
                                                                    CONVERT(VARCHAR(100), pr.UpdateTime, 20) UpdateTime ,
                                                                    pr.ResumeName ,
                                                                    OpenStatus = CASE WHEN pr.OpenStatus = 0 THEN 0
                                                                                      ELSE 1
                                                                                 END ,
                                                                    Intention = CASE WHEN jwi.Intention = NULL THEN 1
                                                                                     ELSE 0
                                                                                END ,
                                                                    Experiences = CASE WHEN pwe.Experiences = NULL
                                                                                       THEN 1
                                                                                       ELSE 0
                                                                                  END ,
                                                                    EducationAndTrainingBackground = CASE
                                                                                                      WHEN et.EducationAndTrainingBackground = NULL
                                                                                                      THEN 1
                                                                                                      ELSE 0
                                                                                                     END ,
                                                                    SUBSTRING(pr.SelfEvaluation, 0, 500) SelfEvaluation ,
                                                                    cj.JobID ,
                                                                    cj.JobName ,
                                                                    WorkingMode = CASE WHEN jwi.WorkingMode = 2
                                                                                       THEN '1'
                                                                                       WHEN jwi.WorkingMode = 1
                                                                                       THEN '2'
                                                                                       ELSE '3'
                                                                                  END ,
                                                                    Years = CASE WHEN pwe.Years < 1
                                                                                      AND pwe.Years > 0 THEN 1
                                                                                 WHEN pwe.Years < 3 THEN 2
                                                                                 WHEN pwe.Years >= 3
                                                                                      AND pwe.Years <= 5 THEN 3
                                                                                 WHEN pwe.Years > 5
                                                                                      AND pwe.Years <= 10 THEN 4
                                                                                 WHEN pwe.Years > 10 THEN 5
                                                                                 ELSE 0
                                                                            END
                                                          FROM      dbo.PersonResumes pr
                                                                    JOIN dbo.PersonBaseInfo pbi ON pr.InfoID = pbi.Id
                                                                    JOIN dbo.PersonJobWantedIntention jwi ON jwi.ResumeID = pr.Id
                                                                    JOIN PersonWorkExperience pwe ON pwe.ResumeID = pr.Id
                                                                    JOIN dbo.PersonEducationAndTraining et ON et.ResumeID = pr.Id
                                                                    JOIN ( SELECT   *
                                                                           FROM     ( SELECT    * ,
                                                                                                ROW_NUMBER() OVER ( PARTITION BY p2.JobID ORDER BY p2.JobId ) sequence
                                                                                      FROM      RecruitmentJobs p2
                                                                                    ) t
                                                                           WHERE    t.sequence = 1
                                                                         ) cj ON CONVERT(VARCHAR(50), cj.JobID) = ( SELECT TOP 1
                                                                                                      *
                                                                                                      FROM
                                                                                                      dbo.StringSplit(jwi.ExpectedJobID,
                                                                                                      ',')
                                                                                                      )
                                                          WHERE     pr.IsAudited = 1
                                                        ) t
                                               WHERE    t.sequence = 1";
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
                        type = item["type"].ToString(),
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
                       // 登记日期
                        aae043 = item["UpdateTime"].ToString(),
                        yae100 = "攀枝花市人才服务中心",
                        //登记时间
                        aae036 = item["UpdateTime"].ToString(),
                        aae017 = "攀枝花市人才服务中心",
                        //经办机构
                        aae011 = "攀枝花市",
                        //登记地区行政区划代码
                        aae022 = "510401000000",
                        ycb213 = item["WorkingMode"].ToString(),
                        //工作地点代码
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
 
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            //需要推送的信息 过滤：未插入，插入但更新时间大于推送时间

            List<PersonResume> personResumes1 = resumeInfolist.Where(r => !YetInsertInfolist.Any(y => y.number == r.acc200)).ToList();
            //List<PersonResume> personResumes2 = resumeInfolist.Where(r => YetInsertInfolist.Any(y => y.number == r.acc200 && Convert.ToDateTime(y.updateTime, dtFormat)
            //< Convert.ToDateTime(r.aae043, dtFormat))).ToList();
            //List<PersonResume> personResumes = personResumes1.Union(personResumes2).ToList<PersonResume>();
            return personResumes1;
        }
    }
}
