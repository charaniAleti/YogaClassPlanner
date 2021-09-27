<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="YogaClassPlanner.WebPages.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="../CSS/RegStyle.css" />
    <title></title>
</head>
<body>
    <div class="container">
        <div class="Register">
            <div class="RegForm">
                <form id="RegisterForm" method="post" runat="server">
                    <h2 id="title">Student Registration</h2>
                    <asp:RequiredFieldValidator ID="regEmailValidation" ControlToValidate="regEmail" Text="*" ForeColor="Red"
                        runat="server" ErrorMessage="Email Required" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regEmailExpreValidator" runat="server" ControlToValidate="regEmail" Text="*"
                        Display="Dynamic" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        ErrorMessage="Invalid Email" ValidationGroup="stdRegister"></asp:RegularExpressionValidator>
                    <label for="email">Email</label>
                    <asp:TextBox ID="regEmail" name="TxtEmail" runat="server"></asp:TextBox>          
                    <label for="ErrorEmail"></label>

                    <asp:RequiredFieldValidator ID="regFullNameValidator" runat="server" Text="*" ForeColor="Red" ControlToValidate="regFullName"
                        ErrorMessage="FullName is required" ValidationGroup="stdRegister"></asp:RequiredFieldValidator>
                    <label for="name">FullName</label>
                    <asp:TextBox ID="regFullName" name="TxtFullName" runat="server"></asp:TextBox>  
                    
                    <label for="healthHis">Health History</label>                    
                    <asp:ListBox ID="regTxtHealth" runat="server" Height="100px" SelectionMode="Multiple" AutoPostBack="True" BackColor="Transparent" />
             
                    <label for="level">Experience in Yoga</label>
                    <asp:DropDownList ID="regLevelList" runat="server">
                        <asp:ListItem Value="">Please select</asp:ListItem>
                        <asp:ListItem Text="Beginner" Value="Beginner">Beginner</asp:ListItem>
                        <asp:ListItem Text="Intermediate" Value="Intermediate">Intermediate</asp:ListItem>
                        <asp:ListItem Text="Advanced" Value="Advanced">Advanced</asp:ListItem>
                    </asp:DropDownList>

                    <label for="Results">Expected Results</label>
                    <asp:ListBox ID="expResuList" runat="server" Height="100px" SelectionMode="Multiple" AutoPostBack="True" BackColor="Transparent"/>

                    <asp:RequiredFieldValidator ID="regPwdReqValidator" runat="server" ErrorMessage="Password is required" Text="*"
                        ForeColor="Red" ControlToValidate="regPwd" ValidationGroup="stdRegister"></asp:RequiredFieldValidator>
                    <label for="password">Password</label>
                    <asp:TextBox ID="regPwd" name="TxtPwd" runat="server" TextMode="Password"></asp:TextBox>
                    
                    <asp:RequiredFieldValidator ID="regCnfmPwdReqValidator" runat="server" ErrorMessage="Confirm Password is required" Text="*"
                        ForeColor="Red" ControlToValidate="regCnfmPwd" ValidationGroup="stdRegister"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="pwdCompareValidator" ValidationGroup="stdRegister" runat="server" ControlToCompare="regPwd" Display="Dynamic" Type="String" Operator="Equal"
                        ControlToValidate="regCnfmPwd" ErrorMessage="Password and confirm password must match." Text="*" ForeColor="Red"></asp:CompareValidator>
                    <label id="confirmpwd">Confirm Password</label>
                    <asp:TextBox ID="regCnfmPwd" name="TxtCnfmPwd" runat="server" TextMode="Password"></asp:TextBox>   

                    <asp:Button ID="subBtn" CssClass="btn-sub" runat="server" ValidationGroup="stdRegister" Text="Register" OnClick="subBtn_Click" />
                    <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                    <asp:ValidationSummary ID="summery1" ForeColor="Red" ValidationGroup="stdRegister" runat="server" />
                    <%--<button class="btn-sub" value="Register" onclick="RegisterBtn_click">Register</button>--%>
                    <div class="LgnLnk">
                        <a href="Login.aspx" >Already registered? Click here to login.</a>
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
        $("#regEmail").keyup(function () {
            $.ajax({
                type: "POST",
                url: "Register.aspx/validateEmail",
                data: $(this).val(),
                dataType: "json",
                contentType: "application/json;charset=utf-8",
                success: function (result) {
                    if (result == true) {
                        $("#ErrorEmail").hide();
                        $("#ErrorEmail").innerHTML("");
                    }
                    else {
                        $("#ErrorEmail").show();
                        $("#ErrorEmail").innerHTML("User already exists with the given email. Try with other email");
                    }
                }
            });
        });
        //$(".btn-sub").click(function () {
        //    $.ajax({
        //        type: "POST",
        //        url: "Register.aspx/RegisterBtn_click",
        //        data:"",
        //        dataType: "json",
        //        contentType: "application/json;charset=utf-8",
        //    })
        //})
    });
</script>
