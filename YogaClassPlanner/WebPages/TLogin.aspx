<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TLogin.aspx.cs" Inherits="YogaClassPlanner.WebPages.TLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../CSS/LoginStyle.css" />
</head>
<body>
<div class="container">
  <div class="Student-Teacher">
      <div class="nav">
          <button id="TeachLgnBtn">Teacher</button>
      </div>
      <div class="forms">
      <form id="TeachLgnForm" runat="server">

        <asp:RequiredFieldValidator ID="teachIDValidator" runat="server" Text="*" ForeColor="Red" 
           ErrorMessage="UserId is Required" ControlToValidate="teachID" Display="Dynamic" ValidationGroup="tchLogin"></asp:RequiredFieldValidator>
        <label for="userID">UserID</label>        
        <asp:TextBox ID="teachID" runat="server" name="txtTeachId" CssClass="inputboxes"></asp:TextBox>
        
        <asp:RequiredFieldValidator ID="teachPwdValidator" runat="server" Text="*" ForeColor="Red" Display="Dynamic"
            ErrorMessage="Password is required" ControlToValidate="teachPwd" ValidationGroup="tchLogin"></asp:RequiredFieldValidator>
        <label for="password">Password</label>
        <asp:TextBox ID="teachPwd" runat="server" name="txtTeachPwd" CssClass="inputboxes" TextMode="Password"></asp:TextBox>
        
        <asp:ValidationSummary ID="summary3" runat="server" ForeColor="Red" ValidationGroup="tchLogin"/>
        <asp:Button ID="teachSubmit" CssClass="submit-btn" runat="server" ValidationGroup="tchLogin" Text="Sign In" OnClick="teachSubmit_Click" />
          <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
        </form>
  </div>
      <div class="gbhome">
          <a href="StartPage.aspx"> Click here to go back to home page</a>
      </div>
      </div>
    </div>
</body>
</html>
