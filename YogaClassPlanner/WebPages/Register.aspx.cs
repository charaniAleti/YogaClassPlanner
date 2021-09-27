using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Collections.Specialized;
using System.Text;
using System.Security.Cryptography;

namespace YogaClassPlanner.WebPages
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
                using(SqlConnection con=new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spGetHealthConditions", con);
                    con.Open();
                    SqlDataReader drr = cmd.ExecuteReader();
                    while (drr.Read())
                    {
                        regTxtHealth.DataTextField = "healthCondition";
                        regTxtHealth.DataValueField = "healthConID";
                        regTxtHealth.DataSource = drr;
                        regTxtHealth.DataBind();
                    }                 
                }
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spGetExpectedResults", con);
                    con.Open();
                    SqlDataReader drr = cmd.ExecuteReader();
                    while (drr.Read())
                    {
                        expResuList.DataTextField = "expectedresult";
                        expResuList.DataValueField = "resultID";
                        expResuList.DataSource = drr;
                        expResuList.DataBind();
                    }
                }
            }
        }
        private bool validateEmail(string email)
        {
            string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
            using(SqlConnection con=new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("select id from tblstudentdata where Email=@email", con);
                cmd.Parameters.AddWithValue("@email", email);
                cmd.CommandType = CommandType.Text;
                con.Open();
                using(SqlDataReader sdr = cmd.ExecuteReader())
                {
                    if (sdr == null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            
        }

        protected void subBtn_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spRegisterStudent", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter email = new SqlParameter("@email", regEmail.Text);
                    //string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(regPwd.Text, "SHA1");
                    string encryptedPassword = generateHash(regPwd.Text, new SHA256CryptoServiceProvider());
                    SqlParameter password = new SqlParameter("@passwordHash", encryptedPassword);
                    SqlParameter fullname = new SqlParameter("@fullname", regFullName.Text);
                    SqlParameter level = new SqlParameter("@level", regLevelList.SelectedItem.Value);
                    StringCollection sc = new StringCollection();
                    foreach(ListItem i in regTxtHealth.Items)
                    {
                        if(i.Selected)
                        {
                            sc.Add(i.Text);
                        }
                    }
                    StringBuilder sb = new StringBuilder(string.Empty);
                    foreach(string item in sc)
                    {
                        sb.AppendFormat("{0}  ", item);
                    }
                    SqlParameter health = new SqlParameter("@healthhistory", sb.ToString());
                    //expected results
                    StringCollection sc1 = new StringCollection();
                    foreach (ListItem i in expResuList.Items)
                    {
                        if (i.Selected)
                        {
                            sc1.Add(i.Text);
                        }
                    }
                    StringBuilder sb1 = new StringBuilder(string.Empty);
                    foreach (string item in sc1)
                    {
                        sb1.AppendFormat("{0}  ", item);
                        //('{1}');
                    }
                    SqlParameter expResults = new SqlParameter("@results", sb1.ToString());

                    cmd.Parameters.Add(email);
                    cmd.Parameters.Add(fullname);
                    cmd.Parameters.Add(password);
                    cmd.Parameters.Add(level);
                    cmd.Parameters.Add(health);
                    cmd.Parameters.Add(expResults);

                    con.Open();
                    int ReturnCode = (int)cmd.ExecuteScalar();
                    if (ReturnCode == -1)
                    {
                        lblMsg.Text = "User name already in use, please choose another user name";
                    }   
                    else
                    {
                        Response.Redirect("Login.aspx");
                    }
                }
            }
        }

        private string generateHash(string text, SHA256CryptoServiceProvider sHA256CryptoServiceProvider)
        {
            Byte[] enpwd = Encoding.UTF8.GetBytes(text);

            Byte[] hashedpwd = sHA256CryptoServiceProvider.ComputeHash(enpwd);

            return BitConverter.ToString(hashedpwd);
        }
    }   
}