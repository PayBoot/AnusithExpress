using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AnousithExpress.web.mvc.Reports
{
    public partial class ConsolidationAllReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            ConsolidationDataSet ds = (ConsolidationDataSet)Session["ConsolidateRerpotDataSet"];

            ReportDocument rptDoc = new ReportDocument();

            rptDoc.Load(Server.MapPath("~/Reports/ConsolidationListReport.rpt"));

            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("dt1", Session["dt1"].ToString());
            rptDoc.SetParameterValue("dt2", Session["dt2"].ToString());
            ConsolidateAllCRP.ReportSource = rptDoc;
        }
    }
}