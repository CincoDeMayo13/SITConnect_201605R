<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect_201605R.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>

   <%-- <script src="https://www.google.com/recaptcha/api.js?render=6LcwtegdAAAAAEaMFlJ-tYdbTJ3dthGgp60JSJsG"></script>--%>

</head>


<body>
    <form id="form1" runat="server">
        <asp:Label ID="Label3" runat="server" Text="Login"></asp:Label>
        <br />
        <div>
        </div>
        <asp:Label ID="Label1" runat="server" Text="Email"></asp:Label>
        <asp:TextBox ID="tb_email" runat="server"></asp:TextBox>
        <p>
            <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
            <asp:TextBox ID="tb_password" runat="server" TextMode="Password"></asp:TextBox>
        </p>
        <asp:Button ID="Button1" runat="server" Text="Login" OnClick="LoginMe" />

        <br />

        <%--<input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>--%>

        <asp:Label ID="lbl_message" runat="server" EnableViewState="false">Error message here (lbl_message)</asp:Label>
       

    </form>

<%--    <script>
         grecaptcha.ready(function () {
             grecaptcha.execute('6LcwtegdAAAAAEaMFlJ-tYdbTJ3dthGgp60JSJsG', { action: 'Login' }).then(function (token) {
         document.getElementById("g-recaptcha-response").value = token;
         });
         });
    </script>--%>

</body>
</html>
