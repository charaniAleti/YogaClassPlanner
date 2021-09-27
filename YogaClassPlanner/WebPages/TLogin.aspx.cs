using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YogaClassPlanner.WebPages
{
    public partial class TLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void teachSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string username = teachID.Text;
                string pw = teachPwd.Text;
                string encryptedPassword = generateHash(pw, new SHA256CryptoServiceProvider());
                if (AuthenticateUser(username, encryptedPassword))
                {
                    Response.Redirect("/WebPages/Teacher/Teacher.aspx?username=" + username);
                    //RedirectFromLoginPage(username, true);
                }
                else
                {
                    lblMsg.Text = "Invalid User ID and/or Password";
                }
            }
        }

        private bool AuthenticateUser(string username,string password)
        {           
                string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
                using (SqlConnection con = new SqlConnection(CS))
                {
                    SqlCommand cmd = new SqlCommand("spAuthenticateTeacer", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlParameter userid = new SqlParameter("@userId", username);
                    //string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(teachPwd.Text, "SHA256");
                    //string encryptedPassword = generateHash(teachPwd.Text, new SHA256CryptoServiceProvider());
                    SqlParameter pwrd = new SqlParameter("@passwordhash", password);

                    cmd.Parameters.Add(userid);
                    cmd.Parameters.Add(pwrd);

                    con.Open();
                    int ReturnCode = (int)cmd.ExecuteScalar();
                    return Convert.ToBoolean(ReturnCode);                    
                    //{
                    //    Response.Redirect("/WebPages/Teacher/Teacher.aspx");
                    //}
                    //else
                    //{
                    //    lblMsg.Text = "Invalid User ID and/or Password";
                    //}
                }            
        }
        private static string generateHash(string encryptedPassword, SHA256CryptoServiceProvider alg)
        {
            Byte[] enpwd = Encoding.UTF8.GetBytes(encryptedPassword);

            Byte[] hashedpwd = alg.ComputeHash(enpwd);

            return BitConverter.ToString(hashedpwd);
        }

    }
}