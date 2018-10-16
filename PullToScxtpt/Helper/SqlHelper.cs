using PullToScxtpt_px.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace PullToScxtpt_px.Helper
{
    public static class SqlHelper
    {
        public static string connstr = ConfigurationManager.ConnectionStrings["Conn"].ToString();
        

        public static int ExecuteNonQuery(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteNonQuery(conn, cmdText, parameters);
            }
        }

        public static object ExecuteScalar(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteScalar(conn, cmdText, parameters);
            }
        }

        public static DataTable ExecuteDataTable(string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connstr))
            {
                conn.Open();
                return ExecuteDataTable(conn, cmdText, parameters);
            }
        }

        public static int ExecuteNonQuery(SqlConnection conn, string cmdText,
           params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }

        public static object ExecuteScalar(SqlConnection conn, string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }

        public static DataTable ExecuteDataTable(SqlConnection conn, string cmdText,
            params SqlParameter[] parameters)
        {
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.CommandText = cmdText;
                cmd.Parameters.AddRange(parameters);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static object ToDBValue(this object value)
        {
            return value == null ? DBNull.Value : value;
        }

        public static object FromDBValue(this object dbValue)
        {
            return dbValue == DBNull.Value ? null : dbValue;
        }

        public static List<CodeMapper> QueryCodeMapper()
        {
          
            string filePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString()+"代码对应表（攀枝花）.xlsx";
            if (filePath != "")
            {
                if (filePath.Contains("xls"))//判断文件是否存在
                {
             
                    string conn = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + filePath + ";Extended Properties=Excel 8.0;";
                    OleDbConnection oleCon = new OleDbConnection(conn);
                    oleCon.Open();
                    string Sql = "select * from [代码对应表$]";
                    OleDbDataAdapter mycommand = new OleDbDataAdapter(Sql, oleCon);
                    DataSet ds = new DataSet();
                    mycommand.Fill(ds, "[代码对应表$]");
                  
                    oleCon.Close();
                    int count = ds.Tables["[代码对应表$]"].Rows.Count;
                    List<CodeMapper> codeMappers = new List<CodeMapper>();
                    for (int i = 0; i < count; i++)
                    {
                        CodeMapper model = new CodeMapper();
                        string yy = ds.Tables["[代码对应表$]"].Rows[i][0].ToString().Trim();
                        string xx = ds.Tables["[代码对应表$]"].Rows[i][1].ToString().Trim();
                        model.codeType = ds.Tables["[代码对应表$]"].Rows[i][0].ToString().Trim();
                        model.codeName = ds.Tables["[代码对应表$]"].Rows[i][1].ToString().Trim();
                        model.codeValue = ds.Tables["[代码对应表$]"].Rows[i][2].ToString().Trim();
                        model.codeExplain = ds.Tables["[代码对应表$]"].Rows[i][3].ToString().Trim();
                        model.localCodeValue = ds.Tables["[代码对应表$]"].Rows[i][4].ToString().Trim();
                        model.localCodeExplain= ds.Tables["[代码对应表$]"].Rows[i][5].ToString().Trim();
                        codeMappers.Add(model);
                    }
                    return codeMappers;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }



        public static List<JobCodeMapper> QueryJobCodeMapper()
        {

            string filePath = System.AppDomain.CurrentDomain.BaseDirectory.ToString() + "代码对应表（攀枝花）.xlsx";
            if (filePath != "")
            {
                if (filePath.Contains("xls"))//判断文件是否存在
                {
                  
                    string conn = "Provider = Microsoft.ACE.OLEDB.12.0; Data Source =" + filePath + ";Extended Properties=Excel 8.0;";
                    OleDbConnection oleCon = new OleDbConnection(conn);
                    oleCon.Open();
                    string Sql = "select * from [职业分类对应表$]";
                    OleDbDataAdapter mycommand = new OleDbDataAdapter(Sql, oleCon);
                    DataSet ds = new DataSet();
                    mycommand.Fill(ds, "[职业分类对应表$]");

                    oleCon.Close();
                    int count = ds.Tables["[职业分类对应表$]"].Rows.Count;
                    List<JobCodeMapper> codeMappers = new List<JobCodeMapper>();
                    for (int i = 0; i < count; i++)
                    {
                        JobCodeMapper model = new JobCodeMapper();
                        string yy = ds.Tables["[职业分类对应表$]"].Rows[i][0].ToString().Trim();
                        string xx = ds.Tables["[职业分类对应表$]"].Rows[i][1].ToString().Trim();
                        model.typeCode = ds.Tables["[职业分类对应表$]"].Rows[i][0].ToString().Trim();
                        model.ID = ds.Tables["[职业分类对应表$]"].Rows[i][2].ToString().Trim();
                        model.ItemName = ds.Tables["[职业分类对应表$]"].Rows[i][3].ToString().Trim();
                        codeMappers.Add(model);
                    }
                    return codeMappers;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }

        }

    }
}
