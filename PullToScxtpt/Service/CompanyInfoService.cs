using PullToScxtpt.Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Data;
using PullToScxtpt.Model;

namespace PullToScxtpt
{
    public class CompanyInfoService
    {
       
        public List<CompanyInfo> QueryCompanyInfo()
        {
            string comText = @"SELECT top 100 cb.Number,
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
                                cl.Nsrsbm
                        FROM CompanyBaseInfo cb join CompanyLicence cl on cb.Id = cl.CompanyID
                        JOIN ItemCompanyNature icn
                            ON icn.ID = cb.NatureID join ItemCompanyScale ics
                            ON ics.ID = cb.ScaleID
                        JOIN ItemCompanyTrade ict
                            ON ict.ID = cb.TradeID where cb.IsAudit = @IsAudit";
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
                        aae036 = item["SetUpDate"].ToString(),
                        aae396 = item["InspectionDate"].ToString(),
                        
                        aae022 = "510400000000",
                        //aab998 = item["Nsrsbm"].ToString(),
                        
                        aab301 = "510400000000",
                        aae011 = "攀枝花市",
                      //  yae100 = "攀枝花市人才中心"
                    };
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

                    var aab022 = codeMappers.Where(c => item["ictItemCode"].ToString().ToUpper().
                    Equals(c.localCodeValue)).FirstOrDefault();
                    if (aab022 == null)
                    {
                        companyInfo.aab022 = "0100";

                    }
                    else
                    {
                        companyInfo.aab022= aab022.codeValue.ToString();
                    }

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
                        companyInfo.aae017 = "该字段为空";
                    }
                    else
                    {
                        companyInfo.aae017 = item["IssuingOrgan"].ToString();
                    }
                    //联系人

                    companyInfo.aae004 = item["ContactOne"].ToString();
                    if (string.IsNullOrEmpty(companyInfo.aae004))
                    {
                        companyInfo.aae004 = "该字段为空";
                    }
                    else
                    {
                        companyInfo.aae004 = item["ContactOne"].ToString();
                    }
                    //
                    companyInfo.aae006 = item["cbAddress"].ToString();
                    if (string.IsNullOrEmpty(companyInfo.aae006))
                    {
                        companyInfo.aae006 = "该字段为空";
                    }
                    else
                    {
                        companyInfo.aae006 = item["cbAddress"].ToString();
                    }
                    companyInfolist.Add(companyInfo); 
                }
            }
            List<string> ylist = YetInsertInfolist.Select(y => y.number).ToList();
            List<CompanyInfo> companyInfos = companyInfolist.Where(r => !ylist.Any(y => y == r.aab001)).ToList();
            return companyInfos;
        }
    }



}
