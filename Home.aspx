<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect_201605R.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>Profile</legend>
        <div>
            <br />
            <asp:Label ID="lbl_message" runat="server" Text=""></asp:Label>
            <br />
            <br />
            <asp:Image Height="100" Width="100" ID="lbl_photo" runat="server" />
            <br />
            <br />
            <asp:Label ID="lbl_fname" runat="server" Text=""></asp:Label>
             <br />
             <br />
             <asp:Label ID="lbl_lname" runat="server" Text=""></asp:Label>
             <br />
             <br />
             <asp:Label ID="lbl_dob" runat="server" Text=""></asp:Label>
             <br />
             <br />
             <asp:Label ID="lbl_email" runat="server" Text=""></asp:Label>
            <br />
            <br />

            <a href="Update.aspx">Change password</a>

            <br />
            <br />
        </div>
        <asp:Button ID="btn_logout" runat="server" Text="Logout" Visible="false" OnClick="LogoutMe"/>
        </fieldset>
    </form>


</body>
</html>
