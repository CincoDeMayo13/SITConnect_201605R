<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect_201605R.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblMessage" runat="server" EnableViewState="false" Text="Home"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lbl_message" runat="server" Text="Message"></asp:Label>
            <br />
            <br />
        </div>
        <asp:Button ID="btn_logout" runat="server" Text="Logout" Visible="false" OnClick="LogoutMe"/>
    </form>
</body>
</html>
