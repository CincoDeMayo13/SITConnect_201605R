using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;

using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace SITConnect_201605R
{
    public partial class Home : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        string user = null;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
         

            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    lbl_message.Text = "Congratutlation!, you are logged in.";
                    lbl_message.ForeColor = System.Drawing.Color.Green;
                    btn_logout.Visible = true;

                    user = (string)Session["LoggedIn"];
                    displayUserProfile(user);

                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }





        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM Account WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["fname"] != DBNull.Value)
                        {
                            lbl_fname.Text = reader["fname"].ToString();
                        }
                        if (reader["lname"] != DBNull.Value)
                        {
                            lbl_lname.Text = reader["lname"].ToString();
                        }
                       
                        if (reader["email"] != DBNull.Value)
                        {
                            lbl_email.Text = reader["email"].ToString();
                        }
                        if (reader["dob"] != DBNull.Value)
                        {
                            lbl_dob.Text = reader["dob"].ToString();
                        }
                        if (reader["photo"] != DBNull.Value)
                        {
                            lbl_photo.Attributes["src"] = reader["photo"].ToString();
                        }
                   
                    }
      
                }
            }//try
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecryot = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string
                            plainText = srDecryot.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception ex) { throw new Exception(ex.ToString()); }
            finally { }
            return plainText;
        }



        protected void LogoutMe(object sender, EventArgs e)
        {
          
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            Response.Redirect("Login.aspx", false);

            if (Request.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-20);
            }

            if (Request.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-20);
            }

        }

      
    }
}