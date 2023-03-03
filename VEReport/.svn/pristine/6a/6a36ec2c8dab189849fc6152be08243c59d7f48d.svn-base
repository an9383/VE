using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

public class CustomReportStorageWebExtension :
     DevExpress.XtraReports.Web.Extensions.ReportStorageWebExtension
{
    private DataTable reportsTable = new DataTable();
    private SqlDataAdapter reportsTableAdapter;
    string connectionString = "SERVER=VE-MESDB-SVR01,1435;Database=VECAMDB;User ID=sa;Password=Dentalimageno.1";
    string corpId = "";

    public CustomReportStorageWebExtension(string _corpId)
    {
        this.corpId = _corpId;

        reportsTableAdapter =
            new SqlDataAdapter(String.Format("Select * from {0}_ReportDB.dbo.ReportItem", corpId), new SqlConnection(connectionString));
        SqlCommandBuilder builder = new SqlCommandBuilder(reportsTableAdapter);
        reportsTableAdapter.InsertCommand = builder.GetInsertCommand();
        reportsTableAdapter.UpdateCommand = builder.GetUpdateCommand();
        reportsTableAdapter.DeleteCommand = builder.GetDeleteCommand();
        reportsTableAdapter.Fill(reportsTable);
        DataColumn[] keyColumns = new DataColumn[1];
        keyColumns[0] = reportsTable.Columns[2];
        reportsTable.PrimaryKey = keyColumns;
    }
    public override bool CanSetData(string url)
    {
        return GetUrls()[url].Contains("ReadOnly") ? false : true;
    }
    public override byte[] GetData(string url)
    {
        // Get the report data from the storage.
        DataRow row = reportsTable.Rows.Find(url);
        if (row == null) return null;

        byte[] reportData = System.Text.Encoding.UTF8.GetBytes(row["LayoutData"].ToString().Replace("?<?", "<?"));
        return reportData;
    }
    public override Dictionary<string, string> GetUrls()
    {
        reportsTable.Clear();
        reportsTableAdapter.Fill(reportsTable);
        // Get URLs and display names for all reports available in the storage.
        var v = reportsTable.AsEnumerable()
              .ToDictionary<DataRow, string, string>(dataRow => ((Int32)dataRow["ReportId"]).ToString(),
                                                     dataRow => (string)dataRow["ReportName"]);
        return v;
    }
    public override bool IsValidUrl(string url)
    {
        return true;
    }
    public override void SetData(XtraReport report, string url)
    {
        // Write a report to the storage under the specified URL.
        DataRow row = reportsTable.Rows.Find(int.Parse(url));
        byte[] m_Buffer;
        string m_XML = "";

        if (row != null)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                report.SaveLayoutToXml(ms);
                m_Buffer = ms.ToArray();

                m_XML = System.Text.Encoding.UTF8.GetString(m_Buffer);
                m_XML = m_XML.Replace("?<?", "<?");

                row["LayoutData"] = m_XML;
            }
            reportsTableAdapter.Update(reportsTable);
        }
    }
    public override string SetNewData(XtraReport report, string defaultUrl)
    {
        // Append "1" if a new report name already exists.
        if (GetUrls().ContainsValue(defaultUrl)) defaultUrl = string.Concat(defaultUrl, "1");

        // Save a report to the storage with a new URL. 
        // The defaultUrl parameter is the report name that the user specifies.
        DataRow row = reportsTable.NewRow();
        row["CorpId"] = corpId;
        row["ReportId"] = 0;
        row["ReportName"] = defaultUrl;

        byte[] m_Buffer;
        string m_XML = "";

        using (MemoryStream ms = new MemoryStream())
        {
            report.SaveLayoutToXml(ms);
            m_Buffer = ms.ToArray();

            m_XML = System.Text.Encoding.UTF8.GetString(m_Buffer);
            m_XML = m_XML.Replace("?<?", "<?");

            row["LayoutData"] = m_XML;
        }
        reportsTable.Rows.Add(row);
        reportsTableAdapter.Update(reportsTable);
        // Refill the dataset to obtain the actual value of the new row's autoincrement key field.
        reportsTable.Clear();
        reportsTableAdapter.Fill(reportsTable);
        return reportsTable.AsEnumerable().
            FirstOrDefault(x => x["ReportName"].ToString() == defaultUrl)["ReportId"].ToString();
    }
}