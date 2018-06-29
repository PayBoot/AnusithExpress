<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BarCodePage.aspx.cs" Inherits="AnousithExpress.web.mvc.Reports.BarCodePage" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>

      
            <CR:CrystalReportViewer ID="BarcodeCRP" runat="server" AutoDataBind="true" ToolPanelView="None" />

      
        </div>
    </form>
</body>
</html>
