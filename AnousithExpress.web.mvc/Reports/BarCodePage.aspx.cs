using System;
namespace AnousithExpress.web.mvc.Reports
{
    public partial class BarCodePage : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            //ItemBarcodeDataSet barcodeDataSet = (ItemBarcodeDataSet)Session["barcodeModel"];
            ////InvoiceDtSet rptSource = (InvoiceDtSet)System.Web.HttpContext.Current.Session["rptSource"];
            //ReportDocument rptDoc = new ReportDocument();
            //// Your .rpt file path will be below
            //rptDoc.Load(Server.MapPath("~/Reports/ItemBarCode.rpt"));
            ////set dataset to the report viewer.
            //rptDoc.SetDataSource(barcodeDataSet);
            //CRViewer.ReportSource = rptDoc;

        }
    }
}