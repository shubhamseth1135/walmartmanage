<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WalmartManagementApplication.Views.stores.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\WalmartDatabase.mdf;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework" ProviderName="System.Data.SqlClient" SelectCommand="SELECT CONVERT (date, dateofbill) AS Expr1, SUM(totalamount) AS totalamount FROM bill WHERE (CONVERT (date, dateofbill) BETWEEN CONVERT (date, @startdate) AND CONVERT (date, @enddate)) AND (storeid = @storeid) GROUP BY CONVERT (date, dateofbill)">
            <SelectParameters>
                <asp:Parameter Name="startdate" />
                <asp:Parameter Name="enddate" />
                <asp:Parameter Name="storeid" />
            </SelectParameters>
        </asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
