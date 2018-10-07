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
            string comText = @"SELECT top 1  cb.Number,
                                cb.FullName,
                                icn.ItemName icnItemName,
                                ics.ItemCode icsItemCode,
                                ict.ItemName ictItemName,
                                cb.SiteUrl,
                                cb.Postalcode,
                                cb.[Address] cbAddress,
                                cb.ContactOne,
                                cb.ContactOneMobile,
                                cb.ContactOnePhone,
                                cb.ContactOneEmail,
                                cb.Introduction,
                                cl.PersonName,
                                cl.IssuingOrgan,
                                cl.RegisteredCapital,
                                cl.SetUpDate,
                                cl.InspectionDate,
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
            
            if (companyInfoTable.Rows.Count > 0)
            {
                List<CompanyInfo> list = new List<CompanyInfo>();
                foreach (DataRow item in companyInfoTable.Rows)
                {

                    CompanyInfo companyInfo = new CompanyInfo()

                    {
                        aab001 = item["Number"].ToString(),
                        aab004 = item["FullName"].ToString(),
                        aab019 = codeMappers.Where(c => item["icnItemName"].ToString().Equals(c.localCodeExplain)).FirstOrDefault().codeValue.ToString(),
                        aab022 = codeMappers.Where(c => item["ictItemName"].ToString().Equals(c.localCodeExplain)).FirstOrDefault().codeValue.ToString(),
                        aae392 = item["SiteUrl"].ToString(),
                        aae007 = item["Postalcode"].ToString(),
                        aae006 = item["cbAddress"].ToString(),
                        aae004 = item["ContactOne"].ToString(),
                        acb501 = item["ContactOneMobile"].ToString(),
                        aae005 = item["ContactOnePhone"].ToString(),
                        aae159 = item["ContactOneEmail"].ToString(),
                        aab092 = item["Introduction"].ToString(),
                        aab013 = item["PersonName"].ToString(),
                        aae017 = item["IssuingOrgan"].ToString(),
                        aab049 = item["RegisteredCapital"].ToString(),
                        aae036 = item["SetUpDate"].ToString(),
                        aae396 = item["InspectionDate"].ToString(),
                        
                        aae022 = "510400000000",
                        //aab998 = item["Nsrsbm"].ToString(),
                        aab020 = codeMappers.Where(c => item["icnItemName"].ToString().Equals(c.localCodeExplain)).Where(c=>c.codeType.Equals("AAB020")).FirstOrDefault().codeValue.ToString(),
                        aab301 = "510400000000",
                        aae011 = "攀枝花市",
                      //  yae100 = "攀枝花市人才中心"
                    };
                    //人员规模
                    if (item["icsItemCode"].ToString() == "C" || item["icsItemCode"].ToString() == "D" || item["icsItemCode"].ToString() == "E")
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

                    list.Add(companyInfo); 
                }

                return list;
                    
            }
            return null;
        }
    }



}
//StringBuilder sb = new StringBuilder();
//sb.Append("<input>");
//                foreach (DataRow item in companyInfoTable.Rows)
//                {

//                    sb.Append("<CompanyInfo>");
//                    sb.Append("<aab001>" + item["Number"].ToString() + "</aab001>");
//                    sb.Append("<aab004>"+ item["FullName"].ToString() + "</aab004>");
//                    sb.Append("<aab019>"+ item["icnItemName"].ToString() + "</aab019>");
//                    sb.Append("<aab056>"+ item["icsItemName"].ToString() + "</aab056>");
//                    sb.Append("<aab022>"+ item["ictItemName"].ToString() + "</aab022>");
//                    sb.Append("<aae392>"+ item["SiteUrl"].ToString() + "</aae392>");
//                    sb.Append("<aae007>"+ item["Postalcode"].ToString() + "</aae007>");
//                    sb.Append("<aae006>"+ item["cbAddress"].ToString() + "</aae006>");
//                    sb.Append("<aae004>"+ item["ContactOne"].ToString() + "</aae004>");
//                    sb.Append("<acb501>"+ item["ContactOneMobile"].ToString() + "</acb501>");
//                    sb.Append("<aae005>"+ item["ContactOnePhone"].ToString() + "</aae005>");
//                    sb.Append("<aae159>"+ item["ContactOneEmail"].ToString() + "</aae159>");
//                    sb.Append("<aab092>"+ item["Introduction"].ToString() + "</aab092>");
//                    sb.Append("<aab013>"+ item["PersonName"].ToString() + "</aab013>");
//                    sb.Append("<aae017>"+ item["IssuingOrgan"].ToString() + "</aae017>");
//                    sb.Append("<aab049>"+ item["RegisteredCapital"].ToString() + "</aab049>");
//                    sb.Append("<aae036>"+ item["SetUpDate"].ToString() + "</aae036>");
//                    sb.Append("<aae396>"+ item["InspectionDate"].ToString() + "</aae396>");
//                    sb.Append("<aae022>"+ item["clAddress"].ToString() + "</aae022>");
//                    sb.Append("<aab998>"+ item["Nsrsbm"].ToString() + "</aab998>");
//                    sb.Append("<aab020>" + item["icnItemName"].ToString() + "</aab020>");
//                    sb.Append("<aab301>" + item["IssuingOrgan"].ToString() + "</aab301>");
//                    sb.Append("<aae011>" + "攀枝花市" + "</aae011>");
//                    sb.Append("<yae100>" + "攀枝花市人才中心" + "</yae100>");
//                    sb.Append("</CompanyInfo>");
//                }
//                sb.Append("</input>");