using CrystalDecisions.CrystalReports.Engine;
using System;

namespace AnousithExpress.web.mvc.Reports
{
    public partial class BarCodePage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {


            ItemBarcodeDataSet ds = (ItemBarcodeDataSet)Session["barcodeModel"];


            ReportDocument rptDoc = new ReportDocument();

            rptDoc.Load(Server.MapPath("~/Reports/ItemBarCode.rpt"));

            rptDoc.SetDataSource(ds);

            BarcodeCRP.ReportSource = rptDoc;
        }
    }
}