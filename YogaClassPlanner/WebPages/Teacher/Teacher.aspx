<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Teacher.aspx.cs" Inherits="YogaClassPlanner.WebPages.Teacher" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../CSS/teacherstyle.css" />
    <%--<script src="../Scripts/jquery.timepicker.js"></script>--%>
    <script src="../../Scripts/jquery-3.5.1.min.js"></script>  
    <link href="../../Scripts/jquery.timepicker.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.timepicker.js"></script>
    <link href="../../Scripts/jquery.timepicker.min.css" rel="stylesheet" />
    <script src="../../Scripts/jquery.timepicker.min.js"></script>
    <script src="../../Scripts/jquery-ui.js"></script>
    <script src="../../Scripts/jquery-ui.min.js"></script>
    <link href="../../Scripts/jquery-ui.css" rel="stylesheet" />
   </head>
<body>
    <form id="teachform" runat="server">
        <div class="container">
            <div class="teacher">
                <div class="nav">
                    <asp:TextBox ID="searchBar" runat="server" placeholder="ex:boat,flexibility..." CssClass="searchbar"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="searchbtn" OnClick="btnSearch_Click"/>
                    <asp:Button ID="BtnSignOut" CssClass="nav-Btn"  runat="server" Text="Sign Out" OnClick="BtnSignOut_Click"/>
                    <asp:Button ID="BtnScheduleClass" CssClass="nav-Btn"  runat="server" Text="Schedule Class" OnClick="BtnScheduleClass_Click" />
                    <asp:Button ID="BtnAddEditPose" CssClass="nav-Btn"  runat="server" Text="Add/Edit Poses" OnClick="BtnAddEditPose_Click" />
                    <asp:Button ID="BtnDirectory" CssClass="nav-Btn" runat="server" Text="Pose Directory" OnClick="BtnDirectory_Click" />                     
                </div>
                <div class="full-body">
                    <asp:Panel ID="panelSchedule" CssClass="classSchedule" runat="server">
                        <div id="divcalendar" runat="server">                            
                            <asp:Calendar ID="Calendar" runat="server" BackColor="White" BorderColor="Black" CssClass="clscalendar" DayNameFormat="Shortest" Font-Names="Times New Roman" Font-Size="10pt" ForeColor="Black" Height="550px" NextPrevFormat="ShortMonth" OnDayRender="Calendar_DayRender" OnSelectionChanged="Calendar_SelectionChanged" SelectionMode="Day" TitleFormat="Month" Width="550px">
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
                        <div id="clsdateTime" runat="server">
                            <asp:Label ID="lblTitle" runat="server" CssClass="scheduler" Text="Title">Title :</asp:Label>
                            <asp:RequiredFieldValidator ID="titleValidator" runat="server" Text="*" ForeColor="Red"
                                ErrorMessage="Title is Required" ControlToValidate="txtTitle" Display="Dynamic" ValidationGroup="clsdateTimeValidation"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtTitle" runat="server" CssClass="scheduler"></asp:TextBox>

                            <asp:Label ID="lblDate" runat="server" CssClass="scheduler" Text="Date">Date :</asp:Label>
                            <asp:RequiredFieldValidator ID="DateValidator" runat="server" Text="*" ForeColor="Red" 
                                     ErrorMessage="Date is Required" ControlToValidate="txtDate" Display="Dynamic" ValidationGroup="clsdateTimeValidation"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtDate" runat="server" CssClass="scheduler"></asp:TextBox>
                            
                            <asp:Label ID="lblFromTime" runat="server" Text="Start Time :" CssClass="scheduler"></asp:Label>
                            <asp:RequiredFieldValidator ID="FromTimeValidator" runat="server" Text="*" ForeColor="Red" 
                                     ErrorMessage="From Time is Required" ControlToValidate="txtFromTime" Display="Dynamic" ValidationGroup="clsdateTimeValidation"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtFromTime" runat="server" ClientIDMode="Static" CssClass="scheduler"></asp:TextBox>

                            <asp:Label ID="lblToTime" runat="server" Text="End Time :" CssClass="scheduler"></asp:Label>
                            <asp:RequiredFieldValidator ID="ToTimeValidator" runat="server" Text="*" ForeColor="Red" 
                                     ErrorMessage="To Time is Required" ControlToValidate="txtToTime" Display="Dynamic" ValidationGroup="clsdateTimeValidation"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtToTime" runat="server" CssClass="scheduler"></asp:TextBox>

                            <asp:Label ID="lblUserName" runat="server" Text="Instructor Id:" CssClass="scheduler" ></asp:Label>
                            <asp:RequiredFieldValidator ID="UserNameValidator" runat="server" Text="*" ForeColor="Red" 
                                     ErrorMessage="Username is Required" ControlToValidate="txtUserName" Display="Dynamic" ValidationGroup="clsdateTimeValidation"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="scheduler" ReadOnly="true"></asp:TextBox>

                            <asp:Button type="reset" ID="btnCreateCls" runat="server" Text="Create Class" CssClass="scheduler" OnClick="btnCreateCls_Click" ValidationGroup="clsdateTimeValidation"/><br />
                            <asp:ValidationSummary ID="clsdateTimeSummary" runat="server" ForeColor="Red" ValidationGroup="clsdateTimeValidation"/>
                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red" CssClass="scheduler"></asp:Label>
                       </div>
                        <div id="plannedclass" runat="server">
                            <asp:GridView ID="GridClasses" runat="server" AllowPaging="True" PageSize="6" ShowFooter="True" ShowHeaderWhenEmpty="True"
                             EmptyDataText="There are no data records to display." DataKeyField="ClassID" AutoGenerateColumns="False" OnRowUpdating="OnRowUpdating" OnRowCancelingEdit="OnRowCancelingEdit"
                                RowStyle-BorderStyle="Ridge" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" OnRowCommand="GridClasses_RowCommand" 
                                BorderWidth="1px" CellPadding="2" OnRowEditing="OnRowEditing" OnRowDeleting="OnRowDeleting" >
                                <Columns>
                                    <asp:TemplateField HeaderText="Id">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ID" runat="server" Text='<%# Bind("Id") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Class Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditEventName" runat="server" Text='<%# Bind("EventName") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEditEventName" runat="server" Text='<%# Bind("EventName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Date">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditPlannedDate" CssClass="txtGridPlannedDate" runat="server" Text='<%# Bind("PlannedDate") %>'></asp:TextBox>
                                            <%--<asp:ImageButton ID="ImgBtnCalendar" runat="server" onClick="ImgBtnCalendar_Click" ImageUrl ="../../Images/Calendar.png"/>--%>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEditPlannedDate" runat="server" Text='<%# Bind("PlannedDate") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="From">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditStartTime" CssClass="txtEditGridStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEditStartTime" runat="server" Text='<%# Bind("StartTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="To">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtEditEndTime" CssClass="txtEditGridStartTime" runat="server" Text='<%# Bind("Endtime") %>'></asp:TextBox>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEditEndTime" runat="server" Text='<%# Bind("Endtime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Instructor" SortExpression="Teacher Name">
                                       <ItemTemplate>
                                            <asp:Label ID="lblInstructor" runat="server" Text='<%# Bind("TeacherName") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                         <ItemTemplate>
                                              <asp:LinkButton Text="Reschedule" runat="server" CommandName="Edit" />
                                          </ItemTemplate>
                                          <EditItemTemplate>
                                              <asp:LinkButton Text="Update" runat="server"  CommandName="Update" />
                                              <asp:LinkButton Text="Cancel" runat="server" CommandName="Cancel"/>
                                          </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ShowHeader="False">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="CancelClass" runat="server" CausesValidation="False" CommandName="Delete" Text="Cancel Class"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnAddSequence" runat="server" Text="Add Sequence" CommandName="selectAddSequence" CommandArgument="<%# Container.DataItemIndex %>"/>
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
                            <asp:Button ID="btnAddOtherClass" runat="server" Text="Create Another Class" CssClass="btnOtherClass" OnClick="btnAddOtherClass_Click"/>
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="PanelAddPose" runat="server" CssClass="classAddPose">
                     <asp:Button ID="btnAddPose" runat="server" Text="Add Pose" CssClass="addpose-button" OnClick="BtnAddPose_Click" />
                        <div id="addPoseForm" runat="server">

                            <asp:Label ID="lblPoseName" runat="server" Text="Pose Name" CssClass="add_pose_form_ele">Pose Name :</asp:Label> 
                            <asp:RequiredFieldValidator ID="PoseNameValidator" runat="server" Text="*" ForeColor="Red" 
                                ErrorMessage="PoseName is Required" ControlToValidate="txtPoseName" Display="Dynamic" validationGroup="addposegroup"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtPoseName" runat="server"  CssClass="add_pose_form_ele"></asp:TextBox>

                            <asp:Label ID="lblSteps" runat="server" Text="Steps"  CssClass="add_pose_form_ele">Steps :</asp:Label>
                            <asp:RequiredFieldValidator ID="StepsValidator" runat="server" Text="*" ForeColor="Red" 
                                ErrorMessage="Steps are Required" ControlToValidate="txtSteps" Display="Dynamic" validationGroup="addposegroup"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtSteps" runat="server" TextMode="MultiLine"  CssClass="add_pose_form_ele_multi"></asp:TextBox>

                            <asp:Label ID="lblContradications" runat="server" Text="Contradications"  CssClass="add_pose_form_ele">Contraindications :</asp:Label> 
                            <asp:RequiredFieldValidator ID="ContraindicationsValidator" runat="server" Text="*" ForeColor="Red" 
                                ErrorMessage="Contraindications are Required" ControlToValidate="txtContraindications" Display="Dynamic" validationGroup="addposegroup"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtContraindications" runat="server" TextMode="MultiLine"  CssClass="add_pose_form_ele_multi"></asp:TextBox>

                            <asp:Label ID="lblBenefits" runat="server" Text="Benefits"  CssClass="add_pose_form_ele">Benefits :</asp:Label> 
                            <asp:RequiredFieldValidator ID="BenefitsValidator" runat="server" Text="*" ForeColor="Red" 
                                ErrorMessage="Benefits are Required" ControlToValidate="txtBenefits" Display="Dynamic" validationGroup="addposegroup"></asp:RequiredFieldValidator>
                            <asp:TextBox ID="txtBenefits" runat="server" TextMode="MultiLine"  CssClass="add_pose_form_ele_multi"></asp:TextBox>

                            <asp:Label ID="lblPoseLevel" runat="server" Text="Level" CssClass="add_pose_form_ele"></asp:Label>
                            <asp:DropDownList ID="ddPoseLevel" CssClass="add_pose_form_ele" runat="server">
                                <asp:ListItem Text="Not defined" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Beginner"></asp:ListItem>
                                <asp:ListItem Text="Intermediate"></asp:ListItem>
                                <asp:ListItem Text="Advanced"></asp:ListItem>
                            </asp:DropDownList>

                            <asp:FileUpload ID="uploadPoseImg" runat="server" AllowMultiple="false" CssClass="add_pose_form_ele"/>

                            <asp:Button ID="BtnSavePose" runat="server" Text="Save Pose" validationGroup="addposegroup" CssClass="add_pose_form_ele" OnClick="BtnSavePose_Click" />
                            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" validationGroup="addposegroup" CssClass="add_pose_form_ele" OnClick="btnCancelEdit_Click" />

                            <asp:ValidationSummary ID="AddPoseSummary" runat="server" ValidationGroup="addposegroup" ForeColor="Red" />
                            <asp:Label ID="lblPoseMsg" runat="server" ForeColor="Red" CssClass="add_pose_form_ele" validationGroup="addposegroup"></asp:Label>
                        </div> 
                    </asp:Panel>                    
                    <asp:Panel ID="PanelDirectory" runat="server">
                        <div id="gvDirectoryDiv" runat="server">
                        <asp:Label ID="lblResultPoses" runat="server" />
                        <asp:GridView ID="gridPanelDirectory" runat="server" BackColor="#CCCCCC" BorderColor="#999999" 
                            BorderStyle="Solid" BorderWidth="3px" CellPadding="4" CellSpacing="2" ForeColor="Black" 
                            AutoGenerateColumns="False" AllowPaging="True" OnRowCommand="gridPanelDirectory_RowCommand" PageSize="2" OnPageIndexChanging="gridPanelDirectory_PageIndexChanging">
                            <Columns>
                                <asp:ImageField DataImageUrlField="path" ControlStyle-Height="300" ControlStyle-Width="400">
                                    <ControlStyle Height="300px" Width="300px" />
                                </asp:ImageField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEditPoseName" Height="400" Width="700" runat="server" Text='<%# String.Format("{0} <br/><br/> {1} <br/><br/> {2} <br/><br/> {3} <br/><br/> {4}" ,Eval("poseName"),Eval("level"),Eval("steps"),Eval("benefits"),Eval("contraindications")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnEditPose" runat="server" Text="Edit Pose" CommandName="selectEditPose" CommandArgument="<%# Container.DataItemIndex %>"/>
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
                    <asp:Panel ID="pnlAddPosesToClass" runat="server" Visible="false">
                        <div class="divAddPosesToClass" runat="server">
                            <asp:Label ID="lblClass_ID" runat="server"></asp:Label>
                            <div class="dvAllAvailablePoses" runat="server">
                                <asp:Label ID="lblDragPoses" runat="server" Text="click on poses to create sequence"></asp:Label>
                                 <asp:DataList ID="dlAvailablePoses" runat="server" RepeatColumns="4" RepeatDirection="Horizontal" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" GridLines="Both" OnItemCommand="dlistdrag_ItemCommand">
                                    <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                    <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                    <ItemStyle BackColor="White" ForeColor="#003399"  BorderStyle="Groove" BorderColor="#3333ff"/>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgAvailablepose1" runat="server"  ImageUrl='<%# Bind("path") %>' Height="100px" Width="100px" CommandName="drag" CommandArgument='<%# Bind("poseName")%>'/>
                                        <%--<asp:Image ID="imgAvailablePose" CssClass="draggable" runat="server" ImageUrl='<%# Bind("path") %> ' Height="100px" Width="100px" DescriptionUrl='<%# Bind("poseName") %>'/>--%>
                                       <%-- <br />
                                        <asp:Label ID="lblAvailablePoseName" runat="server" Text='<%# Bind("poseName") %>' />--%>
                                  </ItemTemplate>
                                    <SelectedItemStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                </asp:DataList>
                               </div>
                            <div class="dvPosesInClass" runat="server">
                                <asp:Label ID="lblSequenceName" runat="server" CssClass="dvPosesInClass_ele" Text="Sequence Name "></asp:Label>
                                <asp:RequiredFieldValidator ID="txtsequencevalidator" validationGroup="createsequencegroup" Text="*" ForeColor="Red" runat="server" ErrorMessage="Sequence name is required" ControlToValidate="txtSequenceName"></asp:RequiredFieldValidator>
                                <asp:TextBox ID="txtSequenceName" runat="server" CssClass="dvPosesInClass_ele" TextMode="SingleLine"></asp:TextBox>
                                <br />  
                                <div class="dvDroppingAreaForPoses" runat="server">
                                    <asp:DataList ID="dlAddedPoses" runat="server" RepeatColumns="3" RepeatDirection="Horizontal" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" GridLines="Both">
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgAddedPose1" runat="server" Height="100px" Width="100px" ImageUrl='<%# Bind("path") %>' DescriptionUrl='<%# Bind("poseName") %>' />
                                                <%--<asp:Image ID="imgAddedPose" CssClass="droppable" runat="server" Height="100px" Width="100px"/>--%>
                                                <%-- <br />
                                               <asp:Label ID="lblAvailablePoseName" runat="server" Text='<%# Bind("poseName") %>' />--%>
                                            </ItemTemplate>
                                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                                        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                                        <ItemStyle BackColor="White" ForeColor="#003399"  BorderStyle="Groove" BorderColor="#00ffff"/>
                                        <SelectedItemStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                                    </asp:DataList>
                                </div>
                               
                                <asp:Button ID="btnCreateSequence" runat="server" Text="Submit" validationGroup="createsequencegroup" OnClick="btnCreateSequence_click" CssClass="dvPosesInClass_ele"/>
                                <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="dvPosesInClass_ele" OnClick="btnBack_click"/>
                                <asp:Label ID="lblAddedpose" runat="server" ForeColor="Red" CssClass="dvPosesInClass_ele" validationGroup="createsequencegroup"></asp:Label>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
              </div>
        </div>
    </form> 
</body>
</html> 
    <script type="text/javascript">
        $(document).ready(function () {
            $("#txtFromTime").focus(function () {
                var datetime = new Date(Date.now());
                var dd = datetime.getDate();
                var mm = datetime.getMonth()+1;
                var yyyy = datetime.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                } 
                if (mm < 10) {
                    mm = '0' + mm;
                } 
                var givendate = dd + '/' + mm + '/' + yyyy;
                console.log(givendate);
                //alert(datetime.toLocaleDateString());
                if ($("#txtDate").val() == givendate) {
                    //alert("if");
                    //console.log("inside if");
                    var time = datetime.getHours() + ":" + datetime.getMinutes();
                    $("#txtFromTime").timepicker({
                        'disableTimeRanges': [['12am', time]],
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '6:00am',
                        'maxTime': '10:00pm',
                        'step': 30
                    });
                    var txtFromTime = $("#txtFromTime").val();
                    $("#txtToTime").timepicker({
                        'disableTimeRanges': [['12am', time], ['12am', txtFromTime]],
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '7:00am',
                        'maxTime': '11:00pm',
                        'step': 30
                    });
                }
                else {
                   // alert("else");
                    $("#txtFromTime").timepicker({
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '6:00am',
                        'maxTime': '10:00pm',
                        'step': 30
                    });
                    var txtFromTime = $("#txtFromTime").val();
                    $("#txtToTime").timepicker({
                        'disableTimeRanges': [['12am', txtFromTime]],
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '7:00am',
                        'maxTime': '11:00pm',
                        'step': 30
                    });
                }
            })
            $('.txtEditGridStartTime').keyup(function () {
                var datetime = new Date(Date.now());
                var dd = datetime.getDate();
                var mm = datetime.getMonth() + 1;
                var yyyy = datetime.getFullYear();
                if (dd < 10) {
                    dd = '0' + dd;
                }
                if (mm < 10) {
                    mm = '0' + mm;
                }
                var givendate = dd + '/' + mm + '/' + yyyy;
                var datetime = new Date(Date.now());
                if ($('.txtGridPlannedDate').val() == givendate) {
                    var time = datetime.getHours() + ":" + datetime.getMinutes();
                    $('.txtEditGridStartTime').timepicker({
                        'disableTimeRanges': [['12am', time]],
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '6:00am',
                        'maxTime': '10:00pm',
                        'step': 30
                    });
                    var txtEditGridStartTime = $('.txtEditGridStartTime').val();
                    $('.txtEditGridEndTime').timepicker({
                        'disableTimeRanges': [['12am', time], ['12am', txtEditGridStartTime]],
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '7:00am',
                        'maxTime': '11:00pm',
                        'step': 30
                    });
                }
                else {
                    $('.txtEditGridStartTime').timepicker({
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '6:00am',
                        'maxTime': '10:00pm',
                        'step': 30
                    });
                    var txtEditGridStartTime = $('.txtEditGridStartTime').val();
                    $('.txtEditGridEndTime').timepicker({
                        'disableTimeRanges': [['12am', txtEditGridStartTime]],
                        'timeFormat': 'h:i A',
                        'scrollDefault': 'now',
                        'dropdown': true,
                        'minTime': '7:00am',
                        'maxTime': '11:00pm',
                        'step': 30
                    });
                }
            }) 
            $('.txtGridPlannedDate').datepicker(
                {
                    dateFormat: 'dd/mm/yy',
                    changeMonth: true,
                    changeYear: true,
                    yearRange: '2020:2100',
                    minDate: new Date()
                });
            function imgAvailablepose1_Click(value) {
                console.log(value);
            }
            //$(function () {
            //    $(".draggable").draggable({
            //        helper: "clone",
            //        revert:"invalid",
            //        drag: function (event, ui) {
            //            ui.helper.addClass("draggable");
            //            //event.dataTransfer.setData("text", event.target.descriptionUrl);
            //        }
            //    });
            //    $(".dvDroppingAreaForPoses").droppable({
            //        accept: ".draggable",
            //        activeClass: 'droppable-active',
            //        hoverClass: 'droppable-hover',
            //        drop: function (event, ui) {
            //            ui.draggable.addClass("dropped");
            //            $(".dvDroppingAreaForPoses").append(ui.draggable);
            //            sendData(ui.draggable., this);
            //            //var data = event.dataTransfer.getData("text");
            //            //console.log(data);
            //            //var droppedItem = $(ui.draggable).clone();

            //            //alert(droppedItem);
            //            //$(this)
            //            //    .addClass("ui-state-highlight")
            //            //    .find("ItemTemplate")
            //            //    .html("Dropped!");
            //        }
            //    });
               //function sendData(item, spot)
               // {
               //     console.log(item);
               //     console.log(spot);
               // }
            });
            //$('#btnAddPose').on('click', function () {
            //    $('#addPoseForm').css({ "opacity": "1" });
            //});
        //});
    </script>

