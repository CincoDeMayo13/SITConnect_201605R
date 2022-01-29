<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="SITConnect_201605R.Users" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
       <div style="width: 50%; margin: 0 auto; text-align: center;">
         <table>
         <tr>
         <td colspan="2">
         <h2>
         SQL Injection Demo</h2>
         </td>
         </tr>
         <tr>
         <td>
         Search by userid
        <asp:textbox id="txtUserID" runat="server">
         </asp:textbox>
         </td>
         <td>
         <asp:button id="btnSubmit" onclick="BtnSubmit_Click"
         runat="server" text="Search" />
         </td>
         </tr>
         <tr>
         <asp:gridview id="gvUserInfo" width="100%"
         runat="server" datakeynames="userID" autogeneratecolumns="false">
         <Columns>
         <asp:BoundField DataField="userID" HeaderText="ID" />
         <asp:BoundField DataField="fname" HeaderText="First name" />
         <asp:BoundField DataField="lname" HeaderText="Last name" />
         <asp:BoundField DataField="dob" HeaderText="Date of birth" />
         <asp:BoundField DataField="name" HeaderText="Name" />
         <asp:BoundField DataField="cardnumber" HeaderText="Card number" />
         <asp:BoundField DataField="cardexpiry" HeaderText="Card expiry" />
         <asp:BoundField DataField="cvv" HeaderText="CVV" />
         <asp:BoundField DataField="email" HeaderText="Email" />
         <asp:BoundField DataField="password" HeaderText="Password" />
         <asp:BoundField DataField="photo" HeaderText="Photo" />
         <asp:HyperLinkField DataNavigateUrlFields="userID"
         DataNavigateUrlFormatString="viewuser.aspx?userid={0}"
         Text="View User" HeaderText="action" />
         </Columns>
         </asp:gridview>
         </tr>
         </table>
         </div>

    </form>
</body>
</html>
