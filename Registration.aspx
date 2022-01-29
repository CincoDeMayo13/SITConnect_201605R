<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect_201605R.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>


    <script type="text/javascript">
        function validate() {
            var str = document.getElementById('<%=tb_password.ClientID %>').value;
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
    <form id="form1" runat="server">
        <asp:Label ID="Label2" runat="server" Text="Registration"></asp:Label>
        <br />
        <br />
        First Name&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <br />
        <asp:TextBox ID="tb_firstname" runat="server"></asp:TextBox>
        &nbsp;&nbsp;<br />
        <br />
        Last Name<br />
        <asp:TextBox ID="tb_lastname" runat="server" ></asp:TextBox>
        <br />
        <br />
        Date of Birth<asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        <br />
        Card number<br />
        <asp:TextBox ID="tb_creditcard" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Label ID="Label5" runat="server" Text="Expiration date"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <br />
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;&nbsp;<br />
        <br />
        <asp:Label ID="Label4" runat="server" Text="CVV"></asp:Label>
        <br />
        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        <br />
        <br />
        Email address<br />
        <asp:TextBox ID="tb_emailaddress" runat="server"></asp:TextBox>
        <br />
        <br />
        Password<br />
        <asp:TextBox ID="tb_password" runat="server" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
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
        Photo<br />
        <asp:FileUpload ID="fu_photo" runat="server" />
        <br />
        <p>
            <asp:Button ID="Button1" runat="server" OnClick="btn_checkPassword_Click" Text="Register" />
        </p>
    </form> 
</body>

</html>
