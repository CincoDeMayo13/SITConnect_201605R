using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Google.Authenticator;

namespace SITConnect_201605R
{
    public partial class Verification : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Restricted access. Please login!!')", true);
                    Response.Redirect("Login.aspx", false);
                }
            }
            else
            {
                Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Restricted access. Please login!!')", true);
                Response.Redirect("Login.aspx", false);
            }
        }
        
        protected string getOTPCode(string email)
        {
            string otp = null;

            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "select authcode from Account where Email = @email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        otp = reader["authcode"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                con.Close();
            }
            return otp;
        }

        protected void verifyOTP(object sender, EventArgs e)
        {
            if (HttpUtility.HtmlEncode(txtSecurityCode.Text.ToString()) == getOTPCode(Session["LoggedIn"].ToString()))
            {
                loginLog();
                Response.Redirect("Home.aspx", false);
                
            }
            else
            {
                lblResult.Text = "Verification code is wrong, please re-enter.";
            }
        }

        protected void loginLog()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Logs VALUES(@datetimelog,@userlog,@action)"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@datetimelog", DateTime.Now);
                            cmd.Parameters.AddWithValue("@userlog", HttpUtility.HtmlEncode(Session["LoggedIn"].ToString()));
                            cmd.Parameters.AddWithValue("@action", "Successfully logged into account".ToString());
                            cmd.Connection = con;
                            try
                            {
                                con.Open();
                                cmd.ExecuteNonQuery();
                                //con.Close();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.ToString());
                                return;
                            }
                            finally
                            {
                                con.Close();
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}