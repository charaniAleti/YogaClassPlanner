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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string AuthenticateUser(string username, string password)
        {
            string CS = ConfigurationManager.ConnectionStrings["yogaDB"].ConnectionString;
            using (SqlConnection con = new SqlConnection(CS))
            {
                SqlCommand cmd = new SqlCommand("spAuthenticateStudent", con);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter userid = new SqlParameter("@email", username);                
                SqlParameter pwrd = new SqlParameter("@passwordhash", password);

                cmd.Parameters.Add(userid);
                cmd.Parameters.Add(pwrd); 
                cmd.Parameters.Add("@name", SqlDbType.VarChar, 100);
                cmd.Parameters["@name"].Direction = ParameterDirection.Output;

                con.Open();
                cmd.ExecuteNonQuery();
                string message = (string)cmd.Parameters["@name"].Value;
                con.Close();
                return message;
                //int ReturnCode = (int)cmd.ExecuteScalar();
                //return Convert.ToBoolean(ReturnCode);             
            }
        }

        protected void stdSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                string email =stdEmail.Text;
                string pwd = stdPwd.Text;
                //string encryptedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(stdPwd.Text, "SHA1");
                string encryptedPassword = generateHash(pwd, new SHA256CryptoServiceProvider());
                string username = AuthenticateUser(email, encryptedPassword);
                if (username != "")
                {
                    Response.Redirect("/WebPages/Student/Student.aspx?username="+ username);
                }
                else
                {
                    lblMsg.Text = "Invalid User Name and/or Password";
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