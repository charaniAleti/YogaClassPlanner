<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Student.aspx.cs" Inherits="YogaClassPlanner.WebPages.Student" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../../CSS/StudentStyle.css" rel="stylesheet"  type="text/css"/>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Scripts/jquery-ui.css" rel="stylesheet" />
    <script src="../../Scripts/jquery-3.5.1.min.js"></script>  
    <title></title>
</head>
<body>
    <form class="stdform" runat="server">
        <div class="container">
            <div class="student">
                <div class="nav">
                    <asp:TextBox ID="searchBar" runat="server" placeholder="flexibility,bridge...." CssClass="searchbar"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="searchbtn" OnClick="btnSearch_Click"/>
                    <asp:Label ID="stdlblName" runat="server"></asp:Label>
                    <asp:Button ID="BtnSignOut" CssClass="nav-Btn"  runat="server" Text="Sign Out" OnClick="BtnSignOut_Click"/>
                    <asp:Button ID="BtnClasses" CssClass="nav-Btn"  runat="server" Text="Classes" OnClick="BtnClasses_Click" />
                    <asp:Button ID="BtnDirectory" CssClass="nav-Btn" runat="server" Text="Pose Directory" OnClick="BtnDirectory_Click"/> 
                </div>
                <div class="FormFullBody" runat="server">
                    <asp:Panel ID="stdPnlDirectory" CssClass="stdPanelDirectory" runat="server">                        
                        <div id="stdgvDirectoryDiv" runat="server">
                        <asp:Label ID="stdlblResultPoses" runat="server" />
                        <asp:GridView ID="stdgridPanelDirectory" runat="server" BackColor="#CCCCCC" BorderColor="#999999" 
                            BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" 
                            AutoGenerateColumns="False" AllowPaging="True" PageSize="2" OnPageIndexChanging="stdgridPanelDirectory_PageIndexChanging">
                            <Columns>
                                <asp:ImageField DataImageUrlField="path" ControlStyle-Height="300" ControlStyle-Width="400">
                                    <ControlStyle Height="300px" Width="300px" />
                                </asp:ImageField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="stdlblEditPoseName" Height="400" Width="700" runat="server" Text='<%# String.Format("{0} <br/><br/> {1} <br/><br/> {2} <br/><br/> {3}" ,Eval("poseName"),Eval("steps"),Eval("benefits"),Eval("contraindications")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>                                    
                            </Columns>
                        <FooterStyle BackColor="#CCCCCC" />
                        <HeaderStyle BackColor="Black" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#CCCCCC" ForeColor="Black" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" />
                        <SelectedRowStyle BackColor="#000099" Font-Bold="True" ForeColor="White" />
                        <SortedAscendingCellStyle BackColor="#F1F1F1" />
                        <SortedAscendingHeaderStyle BackColor="#808080" />
                        <SortedDescendingCellStyle BackColor="#CAC9C9" />
                        <SortedDescendingHeaderStyle BackColor="#383838" />
                    </asp:GridView> 
                    </div>  
                    </asp:Panel>
                    <asp:Panel ID="stdPnlClasses"  CssClass="stdPanelClasses" runat="server">
                        <div class="stdDivCalendar" runat="server">
                           <asp:Calendar ID="stdCalendar" runat="server" BackColor="White" BorderColor="Black"  CssClass="stdclscalendar" DayNameFormat="Shortest" Font-Names="Times New Roman"  Font-Size="10pt" ForeColor="Black"  Height="550px" NextPrevFormat="ShortMonth" OnDayRender="Calendar_DayRender" OnSelectionChanged="stdCalendar_SelectionChanged" SelectionMode="Day" TitleFormat="Month" Width="550px" >
                                <DayHeaderStyle BackColor="#CCCCCC" BorderWidth="2pt" Font-Bold="True" Font-Size="10pt" ForeColor="#333333" Height="20pt" Width="5pt" />
                                <DayStyle Font-Bold="True" ForeColor="#893FCD" Width="14%" Wrap="False" />
                                <NextPrevStyle Font-Size="15pt" ForeColor="White" Wrap="False" />
                                <OtherMonthDayStyle ForeColor="#999999" />
                                <SelectedDayStyle BackColor="Aqua" Font-Bold="True" ForeColor="#FF3300" />
                                <SelectorStyle BackColor="#CCCCCC" Font-Bold="True" Font-Names="Verdana" Font-Size="8pt" ForeColor="#333333" Width="1%" />
                                <TitleStyle BackColor="Black" Font-Bold="True" Font-Size="15pt" ForeColor="White" Height="20pt" />
                                <TodayDayStyle BackColor="#9999FF" ForeColor="White" />
                            </asp:Calendar> 
                           </div>
                   
                        <div id="stdplannedclass" runat="server">
                            <asp:GridView ID="StdGridClasses" runat="server" AllowPaging="True" PageSize="6" ShowFooter="True" ShowHeaderWhenEmpty="True"
                             EmptyDataText="There are no data records to display." DataKeyField="ClassID" AutoGenerateColumns="False"
                                RowStyle-BorderStyle="Ridge" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" OnRowCommand="GridClasses_RowCommand" 
                                BorderWidth="1px" CellPadding="2"  >
                                <Columns>
                                    <asp:TemplateField HeaderText="Class Name">
                                        <ItemTemplate>
                                            <asp:Label ID="stdlblEditEventName" runat="server" Text='<%# Bind("EventName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="stdlblEditPlannedDate" runat="server" Text='<%# Bind("PlannedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <ItemTemplate>
                                            <asp:Label ID="stdlblEditStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To">
                                        <ItemTemplate>
                                            <asp:Label ID="stdlblEditEndTime" runat="server" Text='<%# Bind("Endtime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instructor" SortExpression="Teacher Name">
                                       <ItemTemplate>
                                            <asp:Label ID="stdlblInstructor" runat="server" Text='<%# Bind("username") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                 
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btngridBookClass" runat="server" Text="Book Class" CommandName="selectClass" CommandArgument="<%# Container.DataItemIndex %>"/>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <FooterStyle BackColor="White" ForeColor="#000066" />
                                <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                                <RowStyle BorderStyle="Ridge" ForeColor="#000066" />
                                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                                <sortedascendingcellstyle backcolor="#F1F1F1" />
                                <sortedascendingheaderstyle backcolor="#007DBB" />
                                <sorteddescendingcellstyle backcolor="#CAC9C9" />
                                <sorteddescendingheaderstyle backcolor="#00547E" />
                            </asp:GridView>
                            <asp:Label ID="stdlblmsg" runat="server" ForeColor="blue"></asp:Label>
                            </div>
                    </asp:Panel>
                </div>
            </div> 
            </div>
        
    </form>
</body>
</html>
