using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using YogaClassPlanner.WebPages;

namespace YogaClassPlanner.WebPages
{
    public partial class Student : System.Web.UI.Page
    {
        string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
        protected void Page_Load(object sender, EventArgs e)
        {
            stdlblName.Text = Request.QueryString["username"].ToString();
            if (!IsPostBack)
            {
                getAllPoses("");
                stdPnlDirectory.Visible = true;
                stdPnlClasses.Visible = false;
                stdlblName.Visible = false;
                //stdlblmsg.Visible = false;
                stdgvDirectoryDiv.Visible = true;
                stdgridPanelDirectory.Visible = true;
            }
        }

        protected void BtnSignOut_Click(object sender, EventArgs e)
        {
             FormsAuthentication.SignOut();
             Response.Redirect("../Login.aspx",true);
        }

        protected void BtnClasses_Click(object sender, EventArgs e)
        {
            stdPnlDirectory.Visible = false;
            stdPnlClasses.Visible = true;
            stdCalendar.Visible = true;
            stdplannedclass.Visible = false;
            StdGridClasses.Visible = false;
            //stdclsdetails.Visible = false;
            stdgvDirectoryDiv.Visible = false;
            stdgridPanelDirectory.Visible = false;
            searchBar.Visible = false;
            btnSearch.Visible = false;
        }

        protected void stdgridPanelDirectory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            stdgridPanelDirectory.PageIndex = e.NewPageIndex;
            string searchString = "";
            getAllPoses(searchString);
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
                    stdlblResultPoses.Visible = false;
                    stdgridPanelDirectory.Visible = true;
                    stdPnlDirectory.Visible = true;
                    stdgvDirectoryDiv.Visible = true;
                    stdgridPanelDirectory.DataSource = ds;
                    stdgridPanelDirectory.DataBind();
                }
                else
                {
                    stdPnlDirectory.Visible = false;
                    stdgvDirectoryDiv.Visible = false;
                    stdgridPanelDirectory.Visible = false;
                    stdlblResultPoses.Visible = true;
                    stdlblResultPoses.Text = "Results not Found.";
                }
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            string searchString = searchBar.Text;
            getAllPoses(searchString);
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
            string stdname = stdlblName.Text;
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            using (SqlConnection con = new SqlConnection(CS))
            {
                con.Open();
                SqlCommand cmd = con.CreateCommand();
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@month", month);
                cmd.Parameters.AddWithValue("@stdname", stdname);

                cmd.CommandText = @"select distinct tsc.PlannedDate,tsc.EventName,tsc.starttime,tsc.endtime,(select userid from tblteacherdata where id=tsc.teacher_id) as username  from tblscheduleclass tsc with (nolock)   
                                    inner join tblsequences ts with (nolock) on ts.SequenceID=tsc.sequence_id  
                                    inner join tblPose_Sequence tps with (nolock) on tps.SequenceID=ts.SequenceID  
                                    inner join tblposes tp with (nolock) on tp.Id=tps.PoseID  
                                    and tsc.PlannedDate like '%-'+@month+'-%' and   
                                    ((tp.benefits in (select Expected_results from tblstudentdata where id=(select id from tblstudentdata where FullName=@stdname)))  
                                    or (tp.level= (select level from tblstudentdata where id=(select id from tblstudentdata where FullName=@stdname))))";
               

                using (SqlDataReader read = cmd.ExecuteReader())
                    if (read.HasRows != false)
                    {
                        {
                            while (read.Read())
                            {
                                DateTime date = read.GetDateTime(read.GetOrdinal("PlannedDate"));
                                if (date == e.Day.Date)
                                {
                                    string events = read.GetString(read.GetOrdinal("EventName"));
                                    //string teacher = read.GetString(read.GetOrdinal("Teacher_ID"));
                                    TimeSpan frtime = read.GetTimeSpan(read.GetOrdinal("starttime"));
                                    TimeSpan entime = read.GetTimeSpan(read.GetOrdinal("endtime"));
                                    string username = read.GetString(read.GetOrdinal("username"));
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
        }
       protected void stdCalendar_SelectionChanged(object sender, EventArgs e)
       {
            if (ClassExists(stdCalendar.SelectedDate))
            {
                StdGridClasses.Visible = true;
                stdplannedclass.Visible = true;
                stdlblmsg.Visible = false;
                //stdclsdetails.Visible = false;
                //plannedclass.Visible = true;
                //btnAddOtherClass.Visible = true;
                //GridClasses.Visible = true;
                showClassdetails(stdCalendar.SelectedDate);
            }
            else
            {
                StdGridClasses.Visible = false;
                stdplannedclass.Visible = false;
                //clsdateTime.Visible = true;
                //btnAddOtherClass.Visible = false;
                //plannedclass.Visible = false;
                stdlblmsg.Visible = false;
                //txtDate.Text = stdCalendar.SelectedDate.ToShortDateString();
            }
       }

        protected void GridClasses_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "selectClass")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument.ToString()) % StdGridClasses.PageSize;
                GridViewRow row = StdGridClasses.Rows[rowIndex];

                //int classID = Convert.ToInt32((row.FindControl("stdlbl_ID") as Label).Text);
                DateTime date = Convert.ToDateTime((row.FindControl("stdlblEditPlannedDate") as Label).Text);
                DateTime startTime = DateTime.Parse((row.FindControl("stdlblEditStartTime") as Label).Text);
                DateTime endTime = DateTime.Parse((row.FindControl("stdlblEditEndTime") as Label).Text);
                string username = (row.FindControl("stdlblInstructor") as Label).Text;
                string eventname = (row.FindControl("stdlblEditEventName") as Label).Text;
                string student = stdlblName.Text;

                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spBookClass", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter plannedDate = new SqlParameter("@date", date);
                    SqlParameter sTime = new SqlParameter("@starttime",startTime);
                    SqlParameter eTime = new SqlParameter("@endtime", endTime);
                    SqlParameter uName = new SqlParameter("@username", username);
                    SqlParameter eName = new SqlParameter("@eventname", eventname);
                    SqlParameter studentname = new SqlParameter("@student", student);

                    cmd.Parameters.Add(plannedDate);
                    cmd.Parameters.Add(sTime);
                    cmd.Parameters.Add(eTime);
                    cmd.Parameters.Add(uName);
                    cmd.Parameters.Add(eName);
                    cmd.Parameters.Add(studentname);

                    con.Open();
                    int ReturnCode = (int)cmd.ExecuteScalar();
                    if (ReturnCode == 1)
                    {
                        stdlblmsg.Text = "Class has been booked successfully";
                        stdlblmsg.Visible = true;
                    }
                    else if (ReturnCode == -1)
                    {
                        stdlblmsg.Text = "You have a class conflicting with this class or you have already booked this class. ";
                        stdlblmsg.Visible = true;
                    }
                }
            }
        }

        private bool ClassExists(DateTime date)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spCheckClassForStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter plannedDate = new SqlParameter("@date", date);
                SqlParameter stdname = new SqlParameter("@stdname", stdlblName.Text);

                cmd.Parameters.Add(plannedDate);
                cmd.Parameters.Add(stdname);

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

        protected void showClassdetails(DateTime date)
        {
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spShowclassForStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter plannedDate = new SqlParameter("@date", date);
                SqlParameter stdname = new SqlParameter("@stdname", stdlblName.Text);

                cmd.Parameters.Add(plannedDate);
                cmd.Parameters.Add(stdname);

                con.Open();
                SqlDataAdapter d = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                d.Fill(ds);
                con.Close();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StdGridClasses.DataSource = ds;
                    StdGridClasses.DataBind();
                }                       
            }         
        }

        protected void BtnDirectory_Click(object sender, EventArgs e)
        {
            stdPnlClasses.Visible = false;
            stdPnlDirectory.Visible = true;
            stdgridPanelDirectory.Visible = true;
            searchBar.Visible = true;
            btnSearch.Visible = true;
            string search = "";
            getAllPoses(search);
        }
    }
}