using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;

namespace VXI_Trainer_Survey.Repository
{
    public class ExcelHelper
    {
        public static async Task<DataTable> ToDataTableAsync(SqlConnection con, string tableName, string sql)
        {
            var data = new DataTable(tableName);
            using (var cmd = new SqlCommand(sql, con))
            {
                using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess))
                {
                    var schemaTable = reader.GetSchemaTable();
                    if (schemaTable == null)
                    {
                        return data;
                    }

                    foreach (DataRow row in schemaTable.Rows)
                    {
                        var colName = row.Field<string>("ColumnName");
                        var t = row.Field<Type>("DataType");
                        data.Columns.Add(colName, t);
                    }
                    if (!reader.HasRows)
                    {
                        return data;
                    }

                    while (await reader.ReadAsync())
                    {
                        var newRow = data.Rows.Add();
                        for (var index = 0; index < data.Columns.Count; index++)
                        {
                            var col = data.Columns[index].ColumnName;
                            newRow[col] = reader[col];
                        }
                    }
                }
            }
            return data;
        }

        public static DataSet ToDataSet<S, T>(string _connectionString, string query, params string[] sheetNames) where S : IDbConnection, new()
            where T : class, IDbDataAdapter, IDisposable, new()
        {
            using (var conn = new S())
            {
                using (var da = new T())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        string timeoutLimit = ConfigurationManager.AppSettings["commandTimeout"].ToString();
                        da.SelectCommand.CommandTimeout = Convert.ToInt32(timeoutLimit);
                        da.SelectCommand.CommandText = query;
                        da.SelectCommand.Connection.ConnectionString = _connectionString;
                        var ds = new DataSet();
                        da.Fill(ds);
                        for (var i = 0; i < sheetNames.Length; i++)
                        {
                            ds.Tables[i].TableName = sheetNames[i];
                        }
                        return ds;
                    }
                }
            }
        }

        public static DataSet ToDataSet(string connectionString, string sql, params string[] sheetNames)
        {
            return ToDataSet<SqlConnection, SqlDataAdapter>(connectionString, sql, sheetNames);
        }

        public static async Task<string> ToExcelAsync(string constr, params SqlParameter[] parameters)
        {
            var rawFile = HostingEnvironment.MapPath("~/public/template/File.xlsx");
            var today = DateTime.Now;
            var time = today.TimeOfDay;
            var tmpFilePath = HostingEnvironment.MapPath($"~/public/template/tmp");
            var tmpFile = HostingEnvironment.MapPath($"~/public/template/tmp/File{today:MM.dd.yyyy}.{time.Hours}.{time.Minutes}.{time.Seconds}.{time.Milliseconds}.xlsx");
            using (var con = new SqlConnection(constr))
            {
                await con.OpenAsync();

                if (!System.IO.File.Exists(rawFile))
                {
                    throw new Exception("Template file not found");
                }

                if (!System.IO.Directory.Exists(tmpFilePath))
                {
                    System.IO.Directory.CreateDirectory(tmpFilePath ?? throw new InvalidOperationException("cannot create tmp folder"));
                }

                if (rawFile != null && tmpFile != null)
                {
                    System.IO.File.Copy(rawFile, tmpFile, true);
                }

                if (!System.IO.File.Exists(tmpFile))
                {
                    throw new Exception("Temp Template file not found");
                }

                using (var wb = new XLWorkbook(tmpFile))
                {
                    try
                    {
                        //remove the default sheet
                        wb.Worksheet("Sheet1").Delete();
                    }
                    catch
                    {
                        // ignored
                    }

                    var sheetNames = parameters.Select(parameter => parameter.ParameterName).ToArray();
                    var dsQueries = parameters.Aggregate("", (current, parameter) => current + $"{(parameter.Value as string)}");
                    var dataSet = ToDataSet(constr, dsQueries, sheetNames);
                    foreach (DataTable dsTable in dataSet.Tables)
                    {
                        var ws = wb.Worksheets.Add(dsTable.TableName);
                        var colCtr = 1;
                        foreach (DataColumn column in dsTable.Columns)
                        {
                            var cell = ws.Cell(1, colCtr);
                            cell.Value = column.ColumnName;
                            cell.Style.Fill.SetBackgroundColor(XLColor.Black);
                            cell.Style.Font.FontColor = XLColor.White;
                            cell.Style.Font.Bold = true;
                            colCtr++;
                        }
                        ws.Cell(2, 1).InsertData(dsTable.Rows);
                        dsTable.Clear();
                        ws.Columns().AdjustToContents();
                    }
                    wb.SaveAs(tmpFile);
                }
            }
            return tmpFile;
        }
    }
}