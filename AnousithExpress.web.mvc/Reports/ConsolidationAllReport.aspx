<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ConsolidationAllReport.aspx.cs" Inherits="AnousithExpress.web.mvc.Reports.ConsolidationAllReport" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.3500.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <CR:CrystalReportViewer ID="ConsolidateAllCRP" runat="server" AutoDataBind="true" DisplayToolbar="False" EnableDatabaseLogonPrompt="False" EnableParameterPrompt="False" ToolPanelView="None" />
        </div>
    </form>
</body>
</html>
