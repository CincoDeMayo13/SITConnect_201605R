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

using System.Text.RegularExpressions;
using System.Drawing;
using System.Diagnostics;

namespace SITConnect_201605R
{
    public partial class Update : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }

            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        string str = null;
        SqlCommand com;
        byte up;


        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=6Lca6UQeAAAAAEb6jH7c-hXFqfBOGTY4PsiDZ72I &response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose
                        //lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        private int checkPassword(string password)
        {
            int score = 0;

            if (password.Length < 12)
            {
                return 1;
            }
            else
            {
                score = 1;
            }
            if (Regex.IsMatch(password, "[a-z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[A-Z]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[0-9]"))
            {
                score++;
            }
            if (Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            {
                score++;
            }

            return score;
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("validating");
            if (ValidateCaptcha())
            {
                Debug.WriteLine("scoring");
                // implement codes for the button event
                // Extract data from textbox
                int scores = checkPassword(tb_newpassword.Text);
                string status = "";
                switch (scores)
                {
                    case 1:
                        status = "Very Weak";
                        break;
                    case 2:
                        status = "Weak";
                        break;
                    case 3:
                        status = "Medium";
                        break;
                    case 4:
                        status = "Slightly Good";
                        break;
                    case 5:
                        status = "Excellent";
                        break;
                    default:
                        break;
                }
                pwd_status.Text = "Status : " + status;

                if (scores <= 4)
                {
                    Debug.WriteLine("bad password");
                    pwd_status.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    pwd_status.ForeColor = Color.Green;
                    //get the file name of the posted image  


                    string pwd = HttpUtility.HtmlEncode(tb_oldpassword.Text.ToString().Trim());
                    string email = (string)Session["LoggedIn"];

                    SHA512Managed hashing = new SHA512Managed();
                    string dbHash = getDBHash(email);
                    string dbSalt = getDBSalt(email);

                    Debug.WriteLine("starting");

                    try
                    {
                        if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                        {
                            Debug.WriteLine("hashing");

                            string pwdWithSalt = pwd + dbSalt;
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                            string userHash = Convert.ToBase64String(hashWithSalt);

                            Debug.WriteLine(userHash);

                            Debug.WriteLine(dbHash);

                            if (userHash.Equals(dbHash))
                            {
                                Debug.WriteLine("found account");
                                string pwd2 = HttpUtility.HtmlEncode(tb_newpassword.Text.ToString().Trim()); 

                                //Generate random "salt" 
                                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                                byte[] saltByte = new byte[8];

                                //Fills array of bytes with a cryptographically strong sequence of random values.
                                rng.GetBytes(saltByte);
                                salt = Convert.ToBase64String(saltByte);

                                SHA512Managed hashing2 = new SHA512Managed();

                                string pwdWithSalt2 = pwd2 + salt;
                                byte[] plainHash = hashing2.ComputeHash(Encoding.UTF8.GetBytes(pwd2));
                                byte[] hashWithSalt2 = hashing2.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt2));

                                finalHash = Convert.ToBase64String(hashWithSalt2);
                                RijndaelManaged cipher = new RijndaelManaged();
                                cipher.GenerateKey();
                                Key = cipher.Key;
                                IV = cipher.IV;
                                if (changepwd())
                                {
                                    Debug.WriteLine("success");
                                    updateLog();
                                    lblMessage.Text = "Password changed Successfully";
                                  

                                }
                                else
                                {
                                    Debug.WriteLine("failed");
                                    lblMessage.Text = "Password changed Unsuccessful";

                                }
                            }
                            else
                            {
                                Console.WriteLine("starting");
                                lblMessage.Text = "Wrong password";
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                    finally { }
                }
            }
        }




        protected bool changepwd()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Account SET [PasswordHash]=@hash,[PasswordSalt]=@Salt WHERE email='" + Session["LoggedIn"].ToString() + "'"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            try
                            {
                                cmd.CommandType = CommandType.Text;
                                cmd.Parameters.AddWithValue("@hash", finalHash);
                                cmd.Parameters.AddWithValue("@Salt", salt);
                                /*cmd.Parameters.AddWithValue("@iv", Convert.ToBase64String(IV));
                                cmd.Parameters.AddWithValue("@keyy", Convert.ToBase64String(Key));*/
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                                return true;
                            }
                            catch (SqlException ex)
                            {
                                lblMessage.Text = ex.ToString();
                                return false;
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

        protected string getDBHash(string userid)
        {
            string h = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return h;
        }
        protected string getDBSalt(string userid)
        {
            string s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { connection.Close(); }
            return s;
        }
        protected void updateLog()
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
                            cmd.Parameters.AddWithValue("@userlog", Session["LoggedIn"].ToString());
                            cmd.Parameters.AddWithValue("@action", "Successfully updated password");
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
                                lb_error1.Text = ex.ToString();
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