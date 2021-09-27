<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="YogaClassPlanner.WebPages.Login" %>

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
                <button id="stdLgnBtn" class="active">Student</button>
            </div>
            <div class="forms">
                <form id="StdLgnForm" runat="server">
                   <asp:RequiredFieldValidator ID="stdEmailReqValidator" runat="server" Text="*" ForeColor="Red" 
                       ErrorMessage="Email is Required" ControlToValidate="stdEmail" Display="Dynamic"></asp:RequiredFieldValidator>
                   <asp:RegularExpressionValidator ID="stdEmailExpreValidator" runat="server" ControlToValidate="stdEmail" Text="*"
                        Display="Dynamic" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Invalid Email format" ValidationGroup="stdLogin"></asp:RegularExpressionValidator>
                   <label for="Email">Email</label>
                   <asp:TextBox ID="stdEmail" runat="server" name="TxtStdEmail" CssClass="inputboxes"></asp:TextBox>
                 
                   <asp:RequiredFieldValidator ID="stdPwdReqValidator" runat="server" Text="*" ForeColor="Red" Display="Dynamic"
                        ErrorMessage="Password is Required" ControlToValidate="stdPwd" ValidationGroup="stdLogin"></asp:RequiredFieldValidator>
                   <label for="password">Password</label>
                   <asp:TextBox ID="stdPwd" runat="server" name="TxtStdPwd" CssClass="inputboxes" TextMode="Password"></asp:TextBox>
                   
                   <asp:Button ID="stdSubmit" runat="server" Text="Login" CssClass="submit-btn" ValidationGroup="stdLogin" OnClick="stdSubmit_Click" />
                   <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                   <asp:ValidationSummary ID="summary2" ForeColor="Red" runat="server" ValidationGroup="stdLogin"/>                   
                   <div class="regLink">
                        <a href="Register.aspx">New here? Click here to register</a>
                   </div>
                    <div class="gbhome">
                        <a href="StartPage.aspx"> Click here to go back to home page</a>
                    </div>
                </form>
            </div>               
        </div>     
    </div>    
</body>
</html>
<script src="../Scripts/jquery-3.5.1.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        
    //    $("#stdLgnBtn").click(function () {
    //        $("#StdLgnForm").css({ "left": "0px", "opacity": "1" });
    //        $("#TeachLgnForm").css({ "left": "430px", "opacity": "0" })
    //        $("#forgot").css({ "left": "0px", "margin-top":"15px", "opacity": "1" });
    //        $("#stdLgnBtn").addClass("active");
    //        $("#TeachLgnBtn").removeClass("active");
    //    });
    //    $("#TeachLgnBtn").click(function () {
    //        $("#TeachLgnForm").css({ "left": "0px", "margin-top": "-300px", "opacity": "1" });
    //        $("#StdLgnForm").css({ "left": "-430px", "opacity": "0" })
    //        $("#forgot").css({ "left": "0px", "margin-top":"-15px", "opacity": "1" });
    //        $("#TeachLgnBtn").addClass("active");
    //        $("#stdLgnBtn").removeClass("active");
    //    });
        //$("#RegLink").click(function () {
        //    window.location.href = "../WebPages/Register.aspx";
        //});

    });

</script>
