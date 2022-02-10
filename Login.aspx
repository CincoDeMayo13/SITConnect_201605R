<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect_201605R.Login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lca6UQeAAAAAA1b3V54q_eyGjgaV7ozK9LX8U-Y"></script>
</head>
<body>
    <a href="Registration.aspx">Register</a>
    <form id="form1" runat="server">
        <fieldset>
            <legend>Login</legend>
                <br />
                <asp:Label ID="Label1" runat="server" Text="Email"></asp:Label>
                <br />
                <asp:TextBox required="true" TextMode="Email"  ID="tb_email" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label2" runat="server" Text="Password"></asp:Label>
                <br />
                <asp:TextBox required="true" ID="tb_password" runat="server" TextMode="Password"></asp:TextBox>
                </p>
                <p>
                </p>
                <asp:Button ID="Button1" runat="server" Text="Login" OnClick="btn_Submit_Click" />
                <br />
                <br />          
                <br />
                <br />
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
                <asp:Label ID="lbl_message" runat="server" EnableViewState="false"></asp:Label>
                <script>
                     grecaptcha.ready(function () {
                         grecaptcha.execute('6Lca6UQeAAAAAA1b3V54q_eyGjgaV7ozK9LX8U-Y', { action: 'Login' }).then(function (token) {
                     document.getElementById("g-recaptcha-response").value = token;
                     });
                     });
                </script>
        </fieldset>
    </form>
</body>
</html>
