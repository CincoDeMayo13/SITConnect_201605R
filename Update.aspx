<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Update.aspx.cs" Inherits="SITConnect_201605R.Update" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script src="https://www.google.com/recaptcha/api.js?render=6Lca6UQeAAAAAA1b3V54q_eyGjgaV7ozK9LX8U-Y"></script>

    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_newpassword.ClientID %>').value;
            if (str.length < 12) {
                document.getElementById("pwd_length_checker").innerHTML = "Password is not least 12 characters";
                document.getElementById("pwd_length_checker").style.color = "Red"

            }
            else {
                document.getElementById("pwd_length_checker").innerHTML = "Password has at least 12 characters";
                document.getElementById("pwd_length_checker").style.color = "Blue"

            }
            if (str.search(/[a-z]/) == -1) {
                document.getElementById("pwd_lower_checker").innerHTML = "Password do not have at least 1 lower-case letter";
                document.getElementById("pwd_lower_checker").style.color = "Red"

            }
            else {
                document.getElementById("pwd_lower_checker").innerHTML = "Password has at least 1 lower-case letter";
                document.getElementById("pwd_lower_checker").style.color = "Blue"

            }
            if (str.search(/[A-Z]/) == -1) {
                document.getElementById("pwd_upper_checker").innerHTML = "Password do not have at least 1 upper-case letter";
                document.getElementById("pwd_upper_checker").style.color = "Red"

            }
            else {
                document.getElementById("pwd_upper_checker").innerHTML = "Password has at least 1 upper-case letter";
                document.getElementById("pwd_upper_checker").style.color = "Blue"

            }
            if (str.search(/[0-9]/) == -1) {
                document.getElementById("pwd_num_checker").innerHTML = "Password do not have at least 1 number";
                document.getElementById("pwd_num_checker").style.color = "Red"

            }
            else {
                document.getElementById("pwd_num_checker").innerHTML = "Password has at least 1 number";
                document.getElementById("pwd_num_checker").style.color = "Blue"

            }
            if (str.search(/[$@$!%*?&]/) == -1) {
                document.getElementById("pwd_special_checker").innerHTML = "password do not have at least 1 special character"
                document.getElementById("pwd_special_checker").style.color = "red"
            }
            else {
                document.getElementById("pwd_special_checker").innerHTML = "password has at least 1 special character"
                document.getElementById("pwd_special_checker").style.color = "blue"
            }

        }
    </script>

</head>
<body>

    <a href="Home.aspx">Profile</a>

    <form id="form1" runat="server">
        <div>
             <fieldset>
                    <legend>Change password</legend>      
                        <br />
                        Current Password
                        <br />
                        <asp:TextBox required="true" ID="tb_oldpassword" runat="server" TextMode="Password"></asp:TextBox>
                        <br />
                        <br />
                        New Password<br />
                        <asp:TextBox required="true" ID="tb_newpassword" runat="server" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
                        <asp:Label runat="server" ID="pwd_status"></asp:Label>
                        <br />
                        <br />
                        <asp:Label ID="pwd_length_checker" runat="server" Text="Password must have at least 12 characters" ></asp:Label>
                        <br />
                        <asp:Label ID="pwd_lower_checker" runat="server" Text="Password must have at least 1 lower-case letter"></asp:Label>
                        <br />
                        <asp:Label ID="pwd_upper_checker" runat="server" Text="Password must have at least 1 upper-case letter"></asp:Label>
                        <br />
                        <asp:Label ID="pwd_num_checker" runat="server" Text="Password must have at 1 number"></asp:Label>
                        <br />
                        <asp:Label ID="pwd_special_checker" runat="server" Text="Password must have at least 1 special character"></asp:Label>
                        <br />
                        <br />
                        <asp:Button ID="btn_update" runat="server" OnClick="btn_Submit_Click" Text="Update" />                   
                        <br />
                        <br />
                        <asp:Label ID="lbl_msg"  runat="server" Text=""></asp:Label>
                        <br />
                        <asp:Label ID="lb_error1" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lb_error2" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblMessage" runat="server"></asp:Label>
                        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response" />
                        <script>
                             grecaptcha.ready(function () {
                                 grecaptcha.execute('6Lca6UQeAAAAAA1b3V54q_eyGjgaV7ozK9LX8U-Y', { action: 'Login' }).then(function (token) {
                                     document.getElementById("g-recaptcha-response").value = token;
                                 });
                             });
                        </script>
          </fieldset>
        </div>
    </form>
</body>
</html>
