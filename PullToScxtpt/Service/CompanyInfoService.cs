using PullToScxtpt_px.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using PullToScxtpt_px.Model;
using System.Globalization;

namespace PullToScxtpt_px
{
    public class CompanyInfoService
    {
       
        public List<CompanyInfo> QueryCompanyInfo()
        {
            string comText = @"SELECT * FROM (SELECT  ROW_NUMBER() OVER(PARTITION BY cb.Number ORDER BY cb.Number)AS num,
                                cb.Number,
                                cb.FullName,
                                icn.ID icnItemCode,
                                ics.ID icsItemCode,  
                                ict.ID ictItemCode,
                                cb.SiteUrl,
                                cb.Postalcode,
                                substring(cb.[Address],0,10)cbAddress,
                                cb.ContactOne,
                                cb.ContactOneMobile,
                                cb.ContactOnePhone,
                                cb.ContactOneEmail,
                                cb.Introduction,
                                cl.PersonName,
                                cl.IssuingOrgan,
                                cl.RegisteredCapital,
                                CONVERT(varchar(100),  cl.SetUpDate, 20)SetUpDate ,
                                CONVERT(varchar(100),  cl.InspectionDate, 20)InspectionDate ,
                                cl.[Address] clAddress,
                                ca.AgentName
                        FROM CompanyBaseInfo cb join CompanyLicence cl on cb.Id = cl.CompanyID
                        JOIN ItemCompanyNature icn
                            ON icn.ID = cb.NatureID join ItemCompanyScale ics
                            ON ics.ID = cb.ScaleID
                        JOIN ItemCompanyTrade ict
                            ON ict.ID = cb.TradeID
                        JOIN (SELECT ca.CompanyID, ca.AgentName FROM CompanyAgents ca GROUP BY ca.CompanyID,ca.AgentName) ca ON ca.CompanyID=cb.Id                                    
                         where cb.IsAudit = @IsAudit )aaa WHERE aaa.num=1";
            DataTable companyInfoTable = SqlHelper.ExecuteDataTable(comText, new SqlParameter("@IsAudit",1));
            List<CodeMapper> codeMappers = SqlHelper.QueryCodeMapper();
            string cmdText2 = "select * from PullInfoRecord where type='企业信息'";
            DataTable yetcompanyInfoTable = SqlHelper.ExecuteDataTable(cmdText2, new SqlParameter("@param", DBNull.Value));
            List<YetInsertInfo> YetInsertInfolist = new List<YetInsertInfo>();
            List<CompanyInfo> companyInfolist = new List<CompanyInfo>();

            if (yetcompanyInfoTable.Rows.Count > 0)
            {

                foreach (DataRow item in yetcompanyInfoTable.Rows)
                {
                    YetInsertInfo yetInsertInfo = new YetInsertInfo()
                    {
                        number = item["number"].ToString(),
                        type = item["number"].ToString(),
                        updateTime = item["updateTime"].ToString()

                    };
                    YetInsertInfolist.Add(yetInsertInfo);
                }
            }


            if (companyInfoTable.Rows.Count > 0)
            {
            
                foreach (DataRow item in companyInfoTable.Rows)
                {

                    CompanyInfo companyInfo = new CompanyInfo()

                    {
                        aab001 = item["Number"].ToString(),
                        aab004 = item["FullName"].ToString(),
                       // aae392 = item["SiteUrl"].ToString(),
                       // aae007 = item["Postalcode"].ToString(),
                      
                      
                        acb501 = item["ContactOneMobile"].ToString(),
                        aae005 = item["ContactOnePhone"].ToString(),
                       // aae159 = item["ContactOneEmail"].ToString(),
                      
                        aab013 = item["PersonName"].ToString(),
                      
                        aab049 = item["RegisteredCapital"].ToString(),
                     
                        aae396 = item["InspectionDate"].ToString(),
                        
                        aae022 = "510400000000",
                        //aab998 = item["Nsrsbm"].ToString(),
                        
                        aab301 = "510400000000",
                        aae011 = "攀枝花市",
                      //  yae100 = "攀枝花市人才中心"
                    };
                    //单位类型
                    var aab019 = codeMappers.Where(c => item["icnItemCode"].ToString().ToUpper().
                    Equals(c.localCodeValue)).Where(c => c.codeType.Equals("AAB019")).FirstOrDefault();
                    if (aab019==null)
                    {
                        companyInfo.aab019 = "10";

                    }
                    else
                    {
                        companyInfo.aab019 = aab019.codeValue.ToString();
                    }
                    // 所属行业
                    var aab022 = codeMappers.Where(c => item["ictItemCode"].ToString().ToUpper().
                    Equals(c.localCodeValue)).FirstOrDefault();
                    if (aab022 == null)
                    {
                        companyInfo.aab022 = "0800";

                    }
                    else
                    {
                        companyInfo.aab022= aab022.codeValue.ToString();
                    }
                    // 经济类型
                    var aab020 = codeMappers.Where(c => item["icnItemCode"].ToString().ToUpper().
                    Equals(c.localCodeValue)).Where(c => c.codeType.Equals("AAB020")).FirstOrDefault();
                    if (aab020 == null)
                    {
                        companyInfo.aab020 = "150";
                    }
                    else
                    {
                        companyInfo.aab020 = aab020.codeValue.ToString();
                    }
                    //登记时间
                    var aae036 = item["SetUpDate"].ToString();
                    if (string.IsNullOrEmpty(aae036))
                    {
                        companyInfo.aae036 = DateTime.Now.ToLocalTime().ToString();
                    }
                    else
                    {
                        companyInfo.aae036 = aae036;
                    }
                    //人员规模
                    if (item["icsItemCode"].ToString() == "C" || item["icsItemCode"].ToString() == "D" 
                        || item["icsItemCode"].ToString() == "E")
                    {
                        companyInfo.aab056 = "5";
                    }
                    else if (item["icsItemCode"].ToString() =="F")
                    {
                        companyInfo.aab056 = "4";
                    }
                    else if (true)
                    {
                        companyInfo.aab056 = "6";
                    }

                    //登记机构
                    companyInfo.aae017 = item["IssuingOrgan"].ToString();
                    if (string.IsNullOrEmpty(companyInfo.aae017))
                    {
                        companyInfo.aae017 = "攀枝花市人才服务中心";
                    }
                    else
                    {
                        companyInfo.aae017 = item["IssuingOrgan"].ToString();
                    }
                    //联系人

                    companyInfo.aae004 = item["ContactOne"].ToString();
                    if (string.IsNullOrEmpty(companyInfo.aae004))
                    {
                        companyInfo.aae004 = item["AgentName"].ToString();
                    }
                    else
                    {
                        companyInfo.aae004 = item["ContactOne"].ToString();
                    }
                    //联系人地址
                    companyInfo.aae006 = item["cbAddress"].ToString();
                    if (string.IsNullOrEmpty(companyInfo.aae006))
                    {
                        companyInfo.aae006 = "攀枝花市";
                    }
                    else
                    {
                        companyInfo.aae006 = item["cbAddress"].ToString();
                    }
                    companyInfolist.Add(companyInfo); 
                }
            }
            DateTimeFormatInfo dtFormat = new DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            //需要推送的信息 过滤：未插入，插入但更新时间大于推送时间

            List<CompanyInfo> companyInfos1 = companyInfolist.Where(r => !YetInsertInfolist.Any(y => y.number == r.aab001)).ToList();
            List<CompanyInfo> companyInfos2 = companyInfolist.Where(r => YetInsertInfolist.Any(y => y.number == r.aab001 && Convert.ToDateTime(y.updateTime, dtFormat)
            < Convert.ToDateTime(r.aae396, dtFormat))).ToList();
            List<CompanyInfo> companyInfos = companyInfos1.Union(companyInfos2).ToList<CompanyInfo>();
            return companyInfos;
        }
    }



}
