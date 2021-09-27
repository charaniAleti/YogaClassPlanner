using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YogaClassPlanner.WebPages
{
    public partial class Teacher : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();
        DataTable dt = new DataTable();
        string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
        public void Page_Load(object sender, EventArgs e)
        {
            txtUserName.Text = Request.QueryString["username"].ToString();
            if (!IsPostBack)
            {
                clsdateTime.Visible = false;
                panelSchedule.Visible = false;
                PanelAddPose.Visible = false;
                getAllPoses("");
                PanelDirectory.Visible = true;
                gvDirectoryDiv.Visible = true;
                gridPanelDirectory.Visible = true;
                searchBar.Visible = true;
                btnSearch.Visible = true;
                addPoseForm.Visible = false;
                btnAddOtherClass.Visible = false;
                lblResultPoses.Visible = false;
                //lblMsg.Visible = false;
                //lblPoseMsg.Visible = false;

                GridClasses.Visible = false;
                GridClasses.DataSource = null;
                GridClasses.DataBind();
            }
            
        }
        
            protected void Calendar_DayRender(object sender, DayRenderEventArgs e)
            {
            if (e.Day.IsOtherMonth || e.Day.Date.CompareTo(DateTime.Today) < 0)
            {
                e.Day.IsSelectable = false;
            }

            Literal li = new Literal();
            li.Text = "<br/>";

            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }

            //if (ClassExistsMonth(month))
            //{   
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@month", month);

                cmd.CommandText = @"select EventName,planneddate,StartTime,Endtime,(select userId from tblteacherdata where Id=Teacher_id) as TeacherName from tblScheduleClass where planneddate like '%-'+@month+'-%' ";

                using (SqlDataReader read = cmd.ExecuteReader())
                    if (read.HasRows != false)
                    {
                        {
                            while (read.Read())
                            {
                                DateTime date = read.GetDateTime(read.GetOrdinal("planneddate"));
                                if (date == e.Day.Date
                                             //&& date != null
                                             //&& read["EventName"] != null && read["StartTime"]!=null && read["EndTime"] != null
                                             //             && read["EventName"] !=DBNull.Value && read["StartTime"] != DBNull.Value && read["EndTime"] != DBNull.Value
                                             )
                                {
                                    string events = read.GetString(read.GetOrdinal("EventName"));
                                    //string teacher = read.GetString(read.GetOrdinal("Teacher_ID"));
                                    TimeSpan frtime = read.GetTimeSpan(read.GetOrdinal("StartTime"));
                                    TimeSpan entime = read.GetTimeSpan(read.GetOrdinal("Endtime"));
                                    string username = read.GetString(read.GetOrdinal("TeacherName"));
                                    Label l = new Label();
                                    l.Visible = true;
                                    l.Text = events + "-" + frtime.ToString("hh\\:mm") + " to " + entime.ToString("hh\\:mm") + "<br/>" + username;
                                    e.Cell.Controls.Add(li);
                                    e.Cell.Controls.Add(l);
                                    e.Cell.ToolTip = events + "-" + frtime.ToString("hh\\:mm") + " to " + entime.ToString("hh\\:mm") + "<br/>" + username;
                                }
                            }
                        }
                    }
            }
            //}
            //if (e.Day.Date == Calendar.SelectedDate)
            //{
            //    Literal li1 = new Literal();
            //    li1.Text = "<br/>";
            //    e.Cell.Controls.Add(li1);
            //    Label la1 = new Label();
            //    la1.Text=txtTitle.Text + "&nbsp" +txtTeacherName.Text;
            //    e.Cell.Controls.Add(la1);
            //}
        }
        protected void Calendar_SelectionChanged(object sender, EventArgs e)
        {
            if (ClassExists(Calendar.SelectedDate))
            {
                clsdateTime.Visible = false;
                plannedclass.Visible = true;
                btnAddOtherClass.Visible = true;
                GridClasses.Visible = true;
                showClasses(Calendar.SelectedDate);
            }
            else
            {
                clsdateTime.Visible = true;
                btnAddOtherClass.Visible = false;
                plannedclass.Visible = false;
                lblMsg.Visible = false;
                txtDate.Text = Calendar.SelectedDate.ToShortDateString();
            }
        }
        protected void showClasses(DateTime date)
        {
            string query = "select id,EventName,PlannedDate,StartTime,EndTime,(select userId from tblteacherdata where Id=Teacher_id) as TeacherName from tblScheduleClass where PlannedDate=@date";
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                SqlParameter par = new SqlParameter();
                par.Value = date;
                par.ParameterName = "@date";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.Add(par);
                SqlDataAdapter d = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                d.Fill(ds);
                con.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    GridClasses.DataSource = ds;
                    GridClasses.DataBind();
                }
            }
        }
        //private bool ClassExistsMonth(string month)
        //{
        //    string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(CS))
        //    {
        //        SqlCommand cmd = new SqlCommand("spCheckClassInMonth", con);
        //        cmd.CommandType = CommandType.StoredProcedure;

        //        SqlParameter plannedMonth = new SqlParameter("@month", month);

        //        cmd.Parameters.Add(plannedMonth);

        //        con.Open();
        //        int ReturnCode = (int)cmd.ExecuteScalar();
        //        if (ReturnCode == 1)
        //        {
        //            return true;
        //        }
        //        else if (ReturnCode == -1)
        //        {
        //            return false;
        //        }
        //        return false;
        //    }
        //}
        private bool ClassExists(DateTime date)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spCheckClass", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter plannedDate = new SqlParameter("@date", date);

                cmd.Parameters.Add(plannedDate);

                con.Open();
                int ReturnCode = (int)cmd.ExecuteScalar();
                if (ReturnCode == 1)
                {
                    return true;
                }
                else if (ReturnCode == -1)
                {
                    return false;
                }
                return false;
            }
        }

        protected void BtnScheduleClass_Click(object sender, EventArgs e)
        {
            panelSchedule.Visible = true;
            PanelAddPose.Visible = false;
            PanelDirectory.Visible = false;
            lblMsg.Visible = false;
            searchBar.Visible = false;
            btnSearch.Visible = false;
            gvDirectoryDiv.Visible = false;
            gridPanelDirectory.Visible = false;
            lblResultPoses.Visible = false;
        }


        protected void BtnAddEditPose_Click(object sender, EventArgs e)
        {
            panelSchedule.Visible = false;
            PanelAddPose.Visible = true;
            addPoseForm.Visible = false;
            PanelDirectory.Visible = false;
            searchBar.Visible = false;
            btnSearch.Visible = false;
            gvDirectoryDiv.Visible = false;
            gridPanelDirectory.Visible = false;
            lblResultPoses.Visible = false;
            lblPoseMsg.Visible = false;
            btnAddPose.Visible = true;
            btnCancelEdit.Visible = false;
            pnlAddPosesToClass.Visible = false;
        }

        protected void BtnDirectory_Click(object sender, EventArgs e)
        {
            panelSchedule.Visible = false;
            PanelAddPose.Visible = false;
            PanelDirectory.Visible = true;
            gridPanelDirectory.Visible = true;
            gvDirectoryDiv.Visible = true;
            searchBar.Visible = true;
            btnSearch.Visible = true;
            lblResultPoses.Visible = false;
            string searchstring = "";
            getAllPoses(searchstring);
            pnlAddPosesToClass.Visible = false;
            //divAddPosesToClass.Visible = false;
            
        }

        private void getAllPoses(string searchstring)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spgetposes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter searchString = new SqlParameter("@searchString", searchstring);

                cmd.Parameters.Add(searchString);

                con.Open();
                cmd.ExecuteNonQuery();
                SqlDataAdapter d = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                d.Fill(ds);
                con.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblResultPoses.Visible = false;
                    gridPanelDirectory.DataSource = ds;
                    gridPanelDirectory.DataBind();
                }
                else
                {
                    gridPanelDirectory.Visible = false;
                    lblResultPoses.Visible = true;
                    lblResultPoses.Text = "Results not Found.";
                }
            }
        }

        protected void BtnAddPose_Click(object sender, EventArgs e)
        {
            addPoseForm.Visible = true;
        }

        protected void btnCreateCls_Click(object sender, EventArgs e)
        {
            if (txtDate.Text != null && txtFromTime.Text != null && txtToTime.Text != null
                                    && txtDate.Text != "" && txtFromTime.Text != "" && txtToTime.Text != "")
            {
                DateTime date = Convert.ToDateTime(txtDate.Text);
                DateTime startTime = DateTime.Parse(txtFromTime.Text);
                DateTime endTime = DateTime.Parse(txtToTime.Text);
                TimeSpan duration = endTime.Subtract(startTime);
                string username = txtUserName.Text;
                string eventname = txtTitle.Text;

                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spCreateClass", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter plannedDate = new SqlParameter("@date", date);
                    SqlParameter sTime = new SqlParameter("@starttime", startTime);
                    SqlParameter eTime = new SqlParameter("@endtime", endTime);
                    SqlParameter durat = new SqlParameter("@duration", duration);
                    SqlParameter uName = new SqlParameter("@username", username);
                    SqlParameter eName = new SqlParameter("@eventname", eventname);

                    cmd.Parameters.Add(plannedDate);
                    cmd.Parameters.Add(sTime);
                    cmd.Parameters.Add(eTime);
                    cmd.Parameters.Add(durat);
                    cmd.Parameters.Add(uName);
                    cmd.Parameters.Add(eName);

                    con.Open();
                    int ReturnCode = (int)cmd.ExecuteScalar();
                    if (ReturnCode == -1)
                    {
                        txtFromTime.Text = String.Empty;
                        txtTitle.Text = String.Empty;
                        txtToTime.Text = String.Empty;
                        //txtDate.Text = String.Empty;
                        lblMsg.Visible = true;
                        lblMsg.Text = "A class of this teacher is conflicting with this class";
                    }
                    else if (ReturnCode == 1)
                    {
                        txtFromTime.Text = String.Empty;
                        txtTitle.Text = String.Empty;
                        txtToTime.Text = String.Empty;
                        //txtDate.Text = String.Empty;
                        //lblMsg.Visible = true;
                        lblMsg.Text = "Class has been created";
                        plannedclass.Visible = true;
                        clsdateTime.Visible = false;
                        showClasses(date);
                        GridClasses.Visible = true;
                        btnAddOtherClass.Visible = true;
                    }
                }
            }
            else
            {
                //lblMsg.Visible = true;
                lblMsg.Text = "Please enter all the fields";
            }
        }
        public void ClearAllTextBox(Control clsdatetime)
        {
            foreach (Control c in clsdatetime.Controls)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    ((TextBox)c).Text = String.Empty;
                }
            }
        }

        protected void BtnSignOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("../TLogin.aspx", true);
        }

        protected void btnAddOtherClass_Click(object sender, EventArgs e)
        {
            clsdateTime.Visible = true;
            plannedclass.Visible = false;
            btnAddOtherClass.Visible = false;
            txtDate.Text = Calendar.SelectedDate.ToShortDateString();
        }
        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            GridClasses.EditIndex = e.NewEditIndex;
            showClasses(Calendar.SelectedDate);
        }
        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            Label id = GridClasses.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
            TextBox EventName = GridClasses.Rows[e.RowIndex].FindControl("txtEditEventName") as TextBox;
            TextBox PlannedDate = GridClasses.Rows[e.RowIndex].FindControl("txtEditPlannedDate") as TextBox;
            TextBox StartTime = GridClasses.Rows[e.RowIndex].FindControl("txtEditStartTime") as TextBox;
            Label UserName = GridClasses.Rows[e.RowIndex].FindControl("lblInstructor") as Label;
            TextBox EndTime = GridClasses.Rows[e.RowIndex].FindControl("txtEditEndTime") as TextBox;
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("Update tblScheduleClass set EventName='" + EventName.Text + "',PlannedDate= CONVERT(DATETIME,'" + PlannedDate.Text + "' , 105) ,StartTime= CONVERT(DATETIME,'" + StartTime.Text + "' , 105),EndTime= CONVERT(DATETIME,'" + EndTime.Text + "', 105) where ID=" + Convert.ToInt32(id.Text), con);
            cmd.ExecuteNonQuery();
            con.Close();
            GridClasses.EditIndex = -1;
            if (ClassExists(Calendar.SelectedDate))
            {
                showClasses(Calendar.SelectedDate);
            }
            else
            {
                clsdateTime.Visible = true;
                btnAddOtherClass.Visible = false;
                plannedclass.Visible = false;
                txtDate.Text = Calendar.SelectedDate.ToShortDateString();
                lblMsg.Visible = true;
                lblMsg.Text = "Selected Class has been updated.";
            }
        }
        protected void OnRowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            GridClasses.EditIndex = -1;
            showClasses(Calendar.SelectedDate);
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Label id = GridClasses.Rows[e.RowIndex].FindControl("lbl_ID") as Label;
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("delete from tblScheduleClass where ID=" + Convert.ToInt32(id.Text), con);
            cmd.ExecuteNonQuery();
            con.Close();
            if (ClassExists(Calendar.SelectedDate))
            {
                showClasses(Calendar.SelectedDate);
            }
            else
            {
                clsdateTime.Visible = true;
                btnAddOtherClass.Visible = false;
                plannedclass.Visible = false;
                txtDate.Text = Calendar.SelectedDate.ToShortDateString();
                lblMsg.Visible = true;
                lblMsg.Text = "Selected class has been deleted.";
            }
            //showClasses(Calendar.SelectedDate);
        }

        private int AddPose(string posename, string steps, string contraindication, string benefits,string level)
        {
            
            string instructor = txtUserName.Text.ToString();
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAddPoses", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter paramName = new SqlParameter("@poseName", posename);
                SqlParameter paramSteps = new SqlParameter("@steps", steps);
                SqlParameter paramContraindications = new SqlParameter("@contraindications", contraindication);
                SqlParameter paramBenefits = new SqlParameter("@benefits", benefits);
                SqlParameter paramInstructor = new SqlParameter("@instructor", instructor);
                SqlParameter paramLevel = new SqlParameter("@level", level);

                cmd.Parameters.Add(paramName);
                cmd.Parameters.Add(paramSteps);
                cmd.Parameters.Add(paramContraindications);
                cmd.Parameters.Add(paramBenefits);
                cmd.Parameters.Add(paramInstructor);
                cmd.Parameters.Add(paramLevel);
                cmd.Parameters.Add("@returnId", SqlDbType.Int, 100);
                cmd.Parameters["@returnId"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                return Convert.ToInt32(cmd.Parameters["@returnId"].Value);
            }
        }
        protected void BtnSavePose_Click(object sender, EventArgs e)
        {
                string poseName = txtPoseName.Text.ToString();
                string steps = txtSteps.Text.ToString();
                string contraindication = txtContraindications.Text.ToString();
                string benefits = txtBenefits.Text.ToString();
                string level = ddPoseLevel.SelectedValue.ToString();
                lblPoseMsg.Visible = false;

                int PoseId = AddPose(poseName, steps, contraindication, benefits,level);

                if (PoseId != 0)
                {
                    if (uploadPoseImg.HasFile)
                    {
                        foreach (HttpPostedFile image in uploadPoseImg.PostedFiles)
                        {
                            string imageName = Path.GetFileName(image.FileName);
                            string extension = Path.GetExtension(imageName);
                            if (extension.ToLower() == ".jpg" || extension.ToLower() == ".png" ||
                                    extension.ToLower() == ".gif")
                            {
                                Stream strm = image.InputStream;
                                BinaryReader binaryReader = new BinaryReader(strm);
                                Byte[] bytes = binaryReader.ReadBytes((int)strm.Length);
                                image.SaveAs(Server.MapPath("../../PoseImages/" + imageName));
                                string path = "../../PoseImages/" + imageName;
                                using (SqlConnection con = new SqlConnection(CS))
                                {
                                    SqlCommand cmd = new SqlCommand("spUploadImages", con);
                                    cmd.CommandType = CommandType.StoredProcedure;

                                    SqlParameter name = new SqlParameter("@Name", poseName);
                                    SqlParameter data = new SqlParameter("@Data", bytes);
                                    SqlParameter paramPoseId = new SqlParameter("@poseId", PoseId);
                                    SqlParameter parampath = new SqlParameter("@path", path);

                                    cmd.Parameters.Add(name);
                                    cmd.Parameters.Add(data);
                                    cmd.Parameters.Add(paramPoseId);
                                    cmd.Parameters.Add(parampath);
                                    cmd.Parameters.Add("@returnId", SqlDbType.Int, 100);
                                    cmd.Parameters["@returnId"].Direction = ParameterDirection.Output;
                                    cmd.Parameters.Add("@oldpath", SqlDbType.NVarChar, 100);
                                    cmd.Parameters["@oldpath"].Direction = ParameterDirection.Output;

                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    if (Convert.ToInt32(cmd.Parameters["@returnId"].Value) == 1 && cmd.Parameters["@oldpath"].Value != null && cmd.Parameters["@oldpath"].Value != DBNull.Value)
                                    {
                                        txtBenefits.Text = string.Empty;
                                        txtContraindications.Text = string.Empty;
                                        txtPoseName.Text = string.Empty;
                                        txtSteps.Text = string.Empty;
                                        lblPoseMsg.Visible = true;
                                        if (File.Exists(cmd.Parameters["@oldpath"].Value.ToString()))
                                        {
                                            File.Delete(cmd.Parameters["@oldpath"].Value.ToString());
                                        }
                                        lblPoseMsg.Text = "pose has been updated successfully.";
                                    }
                                    else if (Convert.ToInt32(cmd.Parameters["@returnId"].Value) != 0 && cmd.Parameters["@returnId"].Value != DBNull.Value)
                                    {
                                        txtBenefits.Text = string.Empty;
                                        txtContraindications.Text = string.Empty;
                                        txtPoseName.Text = string.Empty;
                                        txtSteps.Text = string.Empty;
                                        lblPoseMsg.Visible = true;
                                        lblPoseMsg.Text = "pose has been added successfully.";
                                    }
                                    else
                                    {
                                        lblPoseMsg.Visible = true;
                                        lblPoseMsg.Text = "error occurred while adding the pose to database.";
                                    }
                                }
                            }
                            else
                            {
                                lblPoseMsg.Visible = true;
                                lblPoseMsg.Text = "Files with extension .jpg, .png and .gif are allowed.";
                            }
                        }
                    }
                    else
                    {
                        lblPoseMsg.Visible = true;
                        lblPoseMsg.Text = "Please upload an image to proceed.";
                    }
                }
                else
                {
                    lblPoseMsg.Visible = true;
                    lblPoseMsg.Text = "something went wrong in uploading the image.";
                }
            
        }

        protected void gridPanelDirectory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridPanelDirectory.PageIndex = e.NewPageIndex;
            string searchString = "";
            getAllPoses(searchString);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchString = searchBar.Text;
            getAllPoses(searchString);
        }

        protected void gridPanelDirectory_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "selectEditPose")
            {
                PanelAddPose.Visible = true;
                PanelDirectory.Visible = false;
                addPoseForm.Visible = true;
                btnAddPose.Visible = false;
                btnCancelEdit.Visible = true;

                int rowIndex = Convert.ToInt32(e.CommandArgument.ToString()) % gridPanelDirectory.PageSize; ;
                GridViewRow row = gridPanelDirectory.Rows[rowIndex];

                string PoseData = (row.FindControl("lblEditPoseName") as Label).Text;
                
                string[] delims = { "<br/><br/>" };
                string[] strings = PoseData.Split(delims, StringSplitOptions.None);

                txtPoseName.ReadOnly = true;
                txtPoseName.Text = strings[0];
                ddPoseLevel.SelectedItem.Text = strings[1];
                txtSteps.Text = strings[2];
                txtContraindications.Text = strings[3];
                txtBenefits.Text = strings[4];                
            }
        }
        protected void GridClasses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "selectAddSequence")
            {
                lblAddedpose.Visible = false;
                int rowIndex = Convert.ToInt32(e.CommandArgument.ToString()) % GridClasses.PageSize; ;
                GridViewRow row = GridClasses.Rows[rowIndex];

                int classID = Convert.ToInt32((row.FindControl("lbl_ID") as Label).Text);
                pnlAddPosesToClass.Visible = true;
                plannedclass.Visible = false;
                divcalendar.Visible = false;
                lblClass_ID.Visible = true;
                lblClass_ID.Text = classID.ToString();

                string searchstring = "";
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spgetposes", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter searchString = new SqlParameter("@searchString", searchstring);

                    cmd.Parameters.Add(searchString);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter d = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    d.Fill(ds);
                    con.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dlAvailablePoses.Visible = true;
                        dlAvailablePoses.DataSource = ds;
                        dlAvailablePoses.DataBind();
                    }
                }
                    using (SqlConnection con = new SqlConnection(CS))
                    {
                        SqlCommand cmd = new SqlCommand("spgetsequences", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        SqlParameter searchString = new SqlParameter("@classid", classID);

                        cmd.Parameters.Add(searchString);

                        con.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataAdapter d = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();
                        d.Fill(ds);
                        con.Close();
                        if (ds.Tables[0].Rows.Count > 1 && !ds.Tables[0].Columns.Contains(""))
                        {
                            dlAddedPoses.DataSource = ds;
                            dlAddedPoses.DataBind();
                            dlAddedPoses.Visible = true;
                        }
                        else
                        {
                        dlAddedPoses.Visible = false;
                        }

                    }
            }
        }

        protected void dlistdrag_ItemCommand(object sender, DataListCommandEventArgs e)
        {
            if (e.CommandName == "drag")
            {
                dlAddedPoses.Visible = true;
                string pose = e.CommandArgument.ToString();
                //string path = e.CommandSource.ToString();
                
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spgetposes", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlParameter searchString = new SqlParameter("@searchString", pose);

                    cmd.Parameters.Add(searchString);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter d = new SqlDataAdapter(cmd);
                                    
                    d.Fill(ds);
                    DataSet ds2 = (DataSet)ViewState["CurrentData"];
                    if (ds2 != null)
                    {
                        ds.Merge(ds2,false);
                    }         
                    
                    ViewState["CurrentData"] = ds;
                    con.Close();
                    if (ds.Tables[0].Rows.Count > 0)
                    {                        
                            dlAddedPoses.DataSource = ds;
                            dlAddedPoses.DataBind();                        
                    }  
                }
            }
        }
        protected void btnBack_click(object sender, EventArgs e)
        {
            divcalendar.Visible = true;
            Calendar.Visible = true;
            plannedclass.Visible = true;
            GridClasses.Visible = true;
            pnlAddPosesToClass.Visible = false;
            dlAvailablePoses.Visible = false;
            dlAddedPoses.Visible = false;
        }
        protected void btnCreateSequence_click(object sender, EventArgs e)
        {
            string sequenceName = txtSequenceName.Text;
            SqlConnection con = new SqlConnection(CS);
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into tblsequences (name) values ('"+sequenceName+"')", con);
            cmd.ExecuteNonQuery();
            con.Close();
            int class_id = Convert.ToInt32(lblClass_ID.Text);
            int posecount=dlAddedPoses.Items.Count;
            for (int i = 0; i < posecount; i++)
            {
                ImageButton imgbtn = (ImageButton)dlAddedPoses.Items[i].FindControl("imgAddedPose1");
                string posename=imgbtn.DescriptionUrl;                
                using (SqlConnection con1 = new SqlConnection(CS))
                {
                    SqlCommand cmd1 = new SqlCommand("spcreatesequences", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;

                    SqlParameter Sname = new SqlParameter("@sequencename", sequenceName);
                    SqlParameter poseName = new SqlParameter("@poseName", posename);
                    SqlParameter classid = new SqlParameter("@classid", class_id);                    

                    cmd1.Parameters.Add(Sname);
                    cmd1.Parameters.Add(poseName);
                    cmd1.Parameters.Add(classid);
                    cmd1.Parameters.Add("@returnId", SqlDbType.Int, 100);
                    cmd1.Parameters["@returnId"].Direction = ParameterDirection.Output;

                    con1.Open();
                    cmd1.ExecuteNonQuery();
                    con1.Close();
                    if (Convert.ToInt32(cmd1.Parameters["@returnId"].Value) == 0 && cmd1.Parameters["@returnId"].Value != DBNull.Value)
                    {
                        txtSequenceName.Text = string.Empty;
                        lblAddedpose.Visible = true;
                        lblAddedpose.Text = "sequence has been created successfully.";
                    }
                    else
                    {
                        lblAddedpose.Visible = true;
                        lblAddedpose.Text = "sequence name already exists.";
                    }
                }
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            txtBenefits.Text = string.Empty;
            txtContraindications.Text = string.Empty;
            txtPoseName.Text = string.Empty;
            txtSteps.Text = string.Empty;
            panelSchedule.Visible = false;
            PanelAddPose.Visible = false;
            PanelDirectory.Visible = true;
            gridPanelDirectory.Visible = true;
            searchBar.Visible = true;
            btnSearch.Visible = true;
            lblResultPoses.Visible = false;
            string searchstring = "";
            getAllPoses(searchstring);

        }
    }

}
