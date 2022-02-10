<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect_201605R.Registration" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <script src="https://www.google.com/recaptcha/api.js?render=6Lca6UQeAAAAAA1b3V54q_eyGjgaV7ozK9LX8U-Y"></script>
    <script src="//code.jquery.com/jquery-1.11.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=Image1.ClientID%>').prop('src', e.target.result)
                         .width(240)
                         .height(150);
                 };
                 reader.readAsDataURL(input.files[0]); 
             }
        }
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
      <div>
        <form id="form1" runat="server">
            <fieldset>
                <legend>Register</legend>
                <br />
                First Name
                <br />
                <asp:TextBox required="true" ID="tb_firstname" runat="server"></asp:TextBox>
                <br />
                <br />
                Last Name<br />
                <asp:TextBox required="true" ID="tb_lastname" runat="server" ></asp:TextBox>
                <br />
                <br />
                Date of Birth
                <div class="form-group mb-4">
                <asp:TextBox required="true" ID="DOB" TextMode="Date" CssClass="form-control border-0 shadow form-control-lg text-base" placeholder="Date of Birth" runat="server"></asp:TextBox>
                <br />
                </div>
                <br />
                Card number
                <br />
                <asp:TextBox required="true" ID="tb_cardnumber" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label5" runat="server" Text="Expiration date"></asp:Label>
                <br />
                <asp:TextBox required="true" ID="tb_expiry" runat="server"></asp:TextBox>
                <br />
                <br />
                <asp:Label ID="Label4" runat="server" Text="CVV"></asp:Label>
                <br />
                <asp:TextBox  required="true" ID="tb_cvv" runat="server"></asp:TextBox>
                <br />
                <br />
                Email address<br />
                <asp:TextBox required="true" TextMode="Email" ID="tb_emailaddress" runat="server"></asp:TextBox>
                <br />
                <br />
                Password<br />
                <asp:TextBox required="true" ID="tb_password" runat="server" TextMode="Password" onkeyup="javascript:validate()"></asp:TextBox>
                <asp:Label ID="pwd_status" runat="server" Text=""></asp:Label>
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
                <h6>Upload profile picture</h6>
                <asp:Image ID="Image1" Height="150px" Width="240px" runat="server" /><br />
                <asp:FileUpload ID="FileUpload1" runat="server" onchange="ShowImagePreview(this);" />
                <asp:Label ID="uploadchecker" runat="server"></asp:Label>      
                <br />
                <p>
                    <asp:Button ID="Button1" runat="server" OnClick="btn_Submit_Click" Text="Register" />
                </p>  
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>     
                <script>
                    grecaptcha.ready(function () {
                        grecaptcha.execute('6Lca6UQeAAAAAA1b3V54q_eyGjgaV7ozK9LX8U-Y', { action: 'Login' }).then(function (token) {
                            document.getElementById("g-recaptcha-response").value = token;
                        });
                    });
                </script>
                <asp:Label ID="lbl_message" runat="server" EnableViewState="false"></asp:Label>
                <br />
                <asp:Label ID="lb_error1" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lb_error2" runat="server"></asp:Label>
            </fieldset>
        </form> 
    </div>
</body>
</html>
