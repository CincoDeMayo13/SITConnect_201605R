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

using System.Diagnostics;
using System.Net.Mail;

namespace SITConnect_201605R
{

    public class MyObject
    {
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
    public partial class Login : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";
        public string code;

        protected void Page_Load(object sender, EventArgs e)
        {

        }

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

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            if (ValidateCaptcha())
            {

                //Response.Write("<script>window.alert('before getDBHash.')</script>");         
                string pwd = HttpUtility.HtmlEncode(tb_password.Text.ToString().Trim());
                string email = HttpUtility.HtmlEncode(tb_email.Text.ToString().Trim());

                SHA512Managed hashing = new SHA512Managed();
                string dbHash = getDBHash(email);
                string dbSalt = getDBSalt(email);

                try
                {
                    if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                    {
                        if (islock(email))
                        {
                            Debug.WriteLine("going in");
                            if (!expirelock(email))
                            {
                                return;
                            }

                        }
                        string pwdWithSalt = pwd + dbSalt;
                        byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                        string userHash = Convert.ToBase64String(hashWithSalt);

                        Debug.WriteLine(userHash);

                        Debug.WriteLine(dbHash);

                        if (userHash.Equals(dbHash))
                        {
                            Session["LoggedIn"] = email;

                            // createa a new GUID and save into the session
                            string guid = Guid.NewGuid().ToString();
                            Session["AuthToken"] = guid;
                            //var result = checkauth(email);

                            //Debug.WriteLine(result);

                            Random random = new Random();
                            code = random.Next(000000, 999999).ToString();
                            createcode(email, code);

                            emailCode(code);

                            // now create a new cookie with this guid value
                            Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                          
                            Response.Redirect("Verification.aspx", false);     
                        }
                        else
                        {
                            Console.WriteLine("starting");
                            lbl_message.Text = "Wrong username or password";
                            errorLogin(email);
                            
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }

                finally { lbl_message.Text = "Wrong username or password"; }
            }

            //if captcha
            else
            {
                lbl_message.Text = "validate captcha to prove that you are a human.";
            }
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

        protected string decryptData(byte[] cipherText)
        {

            string decryptedString = null;
            //byte[] cipherText = Convert.FromBase64String(cipherString);

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                //Decrypt
                //byte[] decryptedText = decryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                //decryptedString = Encoding.UTF8.GetString(decryptedText);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return decryptedString;
        }
        public void errorLogin(string email)
        {
            var count = 0;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select loginfailed from Account where email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["loginfailed"] != null)
                        {
                            if (reader["loginfailed"] != DBNull.Value)
                            {
                                Console.WriteLine("found count");
                                count = Convert.ToInt32(reader["loginfailed"].ToString());
                                Console.WriteLine(count);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }


            if (count >= 2)
            {
                Console.WriteLine("locking");
                SqlCommand cmd = new SqlCommand("Update Account SET lockoutenable=1 WHERE email=@email", connection);

                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();



                SqlCommand cmd2 = new SqlCommand("Update Account SET accountlocktime=@time WHERE email=@email", connection);
                var time = DateTime.Now;
                cmd2.CommandType = CommandType.Text;
                cmd2.Parameters.AddWithValue("@email", email);
                cmd2.Parameters.AddWithValue("@time", time);
                cmd2.ExecuteNonQuery();
            }
            else
            {
                Console.WriteLine("adding");
                SqlCommand cmd = new SqlCommand("Update Account Set loginfailed=loginfailed+1 WHERE email=@email", connection);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@email", email);
                cmd.ExecuteNonQuery();
            }
            connection.Close();

        }

        private bool islock(string email)
        {
            var F = false;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select lockoutenable from Account where email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["lockoutenable"] != null)
                        {
                            if (reader["lockoutenable"] != DBNull.Value)
                            {
                                F = Convert.ToBoolean(reader["lockoutenable"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            connection.Close();
            return F;
        }

        private bool expirelock(string email)
        {
            bool changed = false;
            Debug.WriteLine("starting");
            DateTime f;
            var timenow = DateTime.Now;
            int difference = 0;
            var locked = false;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * from Account where email=@email";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@email", email);
            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["accountlocktime"] != null)
                        {
                            if (reader["accountlocktime"] != DBNull.Value)
                            {

                                f = Convert.ToDateTime(reader["accountlocktime"].ToString());
                                difference = (f - timenow).Minutes;
                                difference = -(difference);
                                Debug.WriteLine(f);
                                Debug.WriteLine(timenow);
                                Debug.WriteLine(difference);

                            }
                        }
                        if (reader["lockoutenable"] != null)
                        {
                            if (reader["lockoutenable"] != DBNull.Value)
                            {
                                locked = Convert.ToBoolean(reader["lockoutenable"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            if (difference >= 1 && locked == true)
            {
                string sql1 = "Update Account Set lockoutenable=0 WHERE email=@email";
                SqlCommand command1 = new SqlCommand(sql1, connection);
                command1.Parameters.AddWithValue("@email", email);
                command1.ExecuteNonQuery();

                string sql2 = "Update Account Set accountlocktime=@time WHERE email=@email";
                SqlCommand command2 = new SqlCommand(sql2, connection);
                command2.Parameters.AddWithValue("@email", email);
                command2.Parameters.AddWithValue("@time", DBNull.Value);
                command2.ExecuteNonQuery();

                string sql3 = "Update Account Set loginfailed=0 WHERE email=@email";
                SqlCommand command3 = new SqlCommand(sql3, connection);
                command3.Parameters.AddWithValue("@email", email);
                command3.Parameters.AddWithValue("@time", DBNull.Value);
                command3.ExecuteNonQuery();

                changed = true;

            }
            connection.Close();
            return changed;

        }
        protected string createcode(string email, string code)
        {
            string otp = null;
            SqlConnection con = new SqlConnection(MYDBConnectionString);
            string sql = "update Account set authcode = @authcode where email = @email";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@authcode", code);
            cmd.Parameters.AddWithValue("@email", email);
            try
            {
                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["authcode"] != null)
                        {
                            otp = reader["authcode"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { con.Close(); }
            return otp;
        }

        protected string emailCode(string otp)
        {
            string from = "SITConnect <sitconnect66@gmail.com>";
            string str = null;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sitconnect66@gmail.com", "Sitconnect123"),
                EnableSsl = true
            };
            var mailMessage = new MailMessage
            {
                Subject = "SIT Connect OTP",
                Body = "Dear " + HttpUtility.HtmlEncode(tb_email.Text.ToString()) + ", your verification code is: " + otp
            };
            mailMessage.To.Add(HttpUtility.HtmlEncode(tb_email.Text.ToString()));
            mailMessage.From = new MailAddress(from);
            try
            {
                smtpClient.Send(mailMessage);
                return str;
            }
            catch
            {
                throw;
            }



        }
    }
}
