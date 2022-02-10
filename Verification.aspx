<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Verification.aspx.cs" Inherits="SITConnect_201605R.Verification" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <fieldset>
            <legend>Verification</legend>
          
             <p>
             Enter code
             <asp:TextBox ID="txtSecurityCode" runat="server"></asp:TextBox>
             </p> 
             <asp:Button ID="btnValidate" OnClick="verifyOTP" CssClass="btn btn-primary" runat="server" Text="Validate" />  
             <br />
             <br />      
             <asp:Label ID="lblResult" runat="server" Text=""></asp:Label>
        </fieldset>
    </form>
</body>
</html>
