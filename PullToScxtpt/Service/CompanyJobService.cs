using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using PullToScxtpt.Model;
using PullToScxtpt_px.Helper;

using PullToScxtpt_px.Model;

namespace PullToScxtpt_px
{
    public class CompanyJobService
    {

        public List<CompanyJob> QueryCompanyJobInfo()
        {

            string cmdText = @"  SELECT    j.JobNumber ,
                                            LEFT(cbi.Id, 18) cid ,
                                            j.JobName ,
                                            LEFT(j.[Description], 1000) require ,
                                            LEFT(j.[Description], 1000) Descrip ,
                                            j.Amount ,
                                            j.[Address] ,
                                            WorkingMode = CASE WHEN j.WorkingMode = 2 THEN '1'
                                                               WHEN j.WorkingMode = 1 THEN '2'
                                                               ELSE '3'
                                                          END ,
                                            j.BeginAge ,
                                            j.EndAge ,
                                            WorkYears = CASE WHEN j.WorkYears < 1
                                                                  AND j.WorkYears > 0 THEN 1
                                                             WHEN j.WorkYears < 3 THEN 2
                                                             WHEN j.WorkYears >= 3
                                                                  AND j.WorkYears <= 5 THEN 3
                                                             WHEN j.WorkYears > 5
                                                                  AND j.WorkYears <= 10 THEN 4
                                                             WHEN j.WorkYears > 10 THEN 5
                                                             ELSE 0
                                                        END ,
                                            cbi.ContactOne ,
                                            cbi.ContactOneMobile ,
                                            cbi.ContactOnePhone ,
                                            CONVERT(VARCHAR(100), j.CreateTime, 20) CreateTime ,
                                            CONVERT(VARCHAR(100), j.UpdateTime, 20) UpdateTime ,
                                            CONVERT(VARCHAR(100), j.PublishDate, 20) PublishDate ,
                                            CONVERT(VARCHAR(100), j.EndTime, 20) EndTime ,
                                            j.[Status] ,
                                            ipc.ID [sid],
                                            ipc.ParentID spid,
                                            ipc.ItemName,
                                            ca.AgentName,
                                            j.JobID,
                                            j.JobName
                                  FROM      dbo.CompanyJobs j
                                            JOIN dbo.CompanyBaseInfo cbi ON cbi.Id = j.CompanyID
                                            JOIN dbo.CompanyAgents CA ON CA.CompanyID = cbi.Id
                                            JOIN dbo.ItemProfessionalCategory ipc ON ipc.ID = j.ProfessionalCategoryID
                                  WHERE     j.[Status] = 2 AND cbi.ContactOne IS NOT NULL";

            DataTable companyjobInfoTable = PullToScxtpt_px.Helper.SqlHelper.ExecuteDataTable(cmdText, new SqlParameter("@IsAudit", 1));
            List<JobCodeMapper> codeMappers = SqlHelper.QueryJobCodeMapper();
            List<SpecialityCodeMapper> SpecialitycodeMappers = SqlHelper.QuerySpecialityCodeMapper();
            string cmdText2 = "select * from PullInfoRecord where type='招聘信息'";
            DataTable yetcompanyjobInfoTable = SqlHelper.ExecuteDataTable(cmdText2, new SqlParameter("@param", DBNull.Value));
            List<YetInsertInfo> YetInsertInfolist = new List<YetInsertInfo>();
            List<CompanyJob> companyjobInfolist = new List<CompanyJob>();

            if (yetcompanyjobInfoTable.Rows.Count > 0)
            {

                foreach (DataRow item in yetcompanyjobInfoTable.Rows)
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

            if (companyjobInfoTable.Rows.Count > 0)
            {
              
                foreach (DataRow item in companyjobInfoTable.Rows)
                {
                    CompanyJob companyJob = new CompanyJob();
                          //招聘编号
                    companyJob.acb200 = item["JobNumber"].ToString();
                    //单位编号
                    companyJob.aab001 = item["cid"].ToString();
                    //岗位名称
                    companyJob.aca112 = item["JobName"].ToString();
                    //岗位描述
                    companyJob.acb22a = item["Descrip"].ToString();
                    //岗位要求
                    
                  
                
                    var aca111 = codeMappers.Where(c => item["JobID"].ToString().ToUpper()
                .Equals(c.ID)).FirstOrDefault();
                    try
                    {
                        companyJob.aca111 = aca111.typeCode.ToString();
                    }
                    catch (Exception)
                    {
                        System.Diagnostics.Debug.WriteLine(item["JobID"].ToString() + "----" + item["JobName"].ToString());

                    }
                    if (aca111==null)
                    {
                        continue;
                    }
                    else
                    {
                        companyJob.aca111 = aca111.typeCode.ToString();
                    }
                    //所学专业
                    var aac183 = SpecialitycodeMappers.Where(c => item["sid"].ToString().ToUpper()
                   .Equals(c.ID)).FirstOrDefault();
                    if (aac183 == null)
                    {
                        var _aac183 = SpecialitycodeMappers.Where(c => item["spid"].ToString().ToUpper()
                   .Equals(c.ID)).FirstOrDefault();
                        //if (_aac183==null)
                        //{
                        //    continue;
                        //}
                    
                        //try
                        //{
                        //    companyJob.aac183 = _aac183.typeCode.ToString();
                        //}
                        //catch (Exception)
                        //{

                        //    System.Diagnostics.Debug.WriteLine(item["sid"].ToString() + "    "+ item["spid"].ToString() + "    " + item["ItemName"].ToString());
                           
                        //}

                    }
                    else
                    {
                        companyJob.aac183 = aac183.typeCode.ToString();
                    }
                  
                    //招聘人数
                    companyJob.acb240 = item["Amount"].ToString();
                    //工作地点
                    companyJob.acb202 = item["Address"].ToString();
                    //工作地点行政区划代码
                    companyJob.aab301 = "510400000000";
                    //工作性质
                    companyJob.acb239 = item["WorkingMode"].ToString();
                    //最低年龄
                    companyJob.acb221 = item["BeginAge"].ToString();
                    //最高年龄
                    companyJob.acb222 = item["EndAge"].ToString();
                    //从业年数
                    companyJob.acc217 = item["WorkYears"].ToString();
                    //联系人
                    companyJob.aae004 = item["ContactOne"].ToString();
                    if (string.IsNullOrWhiteSpace(companyJob.aae004))
                    {
                        continue;
                    }
                    //联系电话
                    companyJob.aae005 = item["ContactOneMobile"].ToString();
                    if (!string.IsNullOrWhiteSpace(companyJob.aae005))
                    {
                        if (!string.IsNullOrWhiteSpace(item["ContactOnePhone"].ToString()))
                        {
                            companyJob.aae005 = item["ContactOnePhone"].ToString();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    //创建日期
                    companyJob.aae395 = item["CreateTime"].ToString();
                    //更新日期
                    companyJob.aae396 = item["UpdateTime"].ToString();
                    //发布日期
                    companyJob.aae397 = item["PublishDate"].ToString();
                    //到期日期
                    companyJob.aae398 = item["EndTime"].ToString();
                    //审核状态
                    companyJob.ace751 = "1";
                    //发布状态
                    companyJob.ace752 = item["Status"].ToString();
                    //数据来源
                    //登记时间
                    companyJob.aae036 = item["PublishDate"].ToString();
                    //登记机构
                    companyJob.aae017 = "攀枝花市人才服务中心";
                    //登记人
                    companyJob.aae011 = "市场部";
                    //登记地行政区划代码
                    companyJob.aae022 = "510400000000";

                    companyjobInfolist.Add(companyJob);


                }
            }
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            //需要推送的信息 过滤：未插入，插入但更新时间大于推送时间

            List<CompanyJob> companyJobs1 = companyjobInfolist.Where(r => !YetInsertInfolist.Any(y => y.number == r.acb200)).ToList();
            //List<PersonResume> personResumes2 = resumeInfolist.Where(r => YetInsertInfolist.Any(y => y.number == r.acc200 && Convert.ToDateTime(y.updateTime, dtFormat)
            //< Convert.ToDateTime(r.aae043, dtFormat))).ToList();
            //List<PersonResume> personResumes = personResumes1.Union(personResumes2).ToList<PersonResume>();
            return companyJobs1;
        }
    }
}

