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

namespace SITConnect_201605R
{
    public partial class Registration : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;

        static string line = "\r";

        //static string isDebug = ConfigurationManager.AppSettings["isDebug"].ToString();

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

        private int checkPassword(string password)
        {
            int score = 0;

            if(password.Length < 12)
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
            if (ValidateCaptcha())
            {
                    // implement codes for the button event
                    // Extract data from textbox
                    int scores = checkPassword(tb_password.Text);
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
                    pwd_status.ForeColor = Color.Red;
                    return;
                }
                else
                {
                    pwd_status.ForeColor = Color.Green;
                    //get the file name of the posted image  
                    string imgName = FileUpload1.FileName;
                    //sets the image path  
                    string imgPath = "Images/" + imgName;
                    //get the size in bytes that  

                    int imgSize = FileUpload1.PostedFile.ContentLength;

                    //validates the posted file before saving  
                    if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.FileName != "")
                    {
                        // 10240 KB means 10MB, You can change the value based on your requirement  
                        if (FileUpload1.PostedFile.ContentLength > 500000)
                        {
                            uploadchecker.Text = "File size too big";
                            uploadchecker.ForeColor = Color.Red;
                            /*                        Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('File is too big.')", true);
                            */
                        }
                        else
                        {
                            //then save it to the Folder  
                            FileUpload1.SaveAs(Server.MapPath(imgPath));
                            Image1.ImageUrl = "~/" + imgPath;
                            //Page.ClientScript.RegisterClientScriptBlock(typeof(Page), "Alert", "alert('Image saved!')", true);


                            string pwd = tb_password.Text.ToString().Trim(); ;

                            //Generate random "salt" 
                            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                            byte[] saltByte = new byte[8];

                            //Fills array of bytes with a cryptographically strong sequence of random values.
                            rng.GetBytes(saltByte);
                            salt = Convert.ToBase64String(saltByte);

                            SHA512Managed hashing = new SHA512Managed();

                            string pwdWithSalt = pwd + salt;
                            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

                            finalHash = Convert.ToBase64String(hashWithSalt);

                            lb_error1.Text = "Salt:" + salt;
                            lb_error2.Text = "Hash with salt:" + finalHash;

                            RijndaelManaged cipher = new RijndaelManaged();
                            cipher.GenerateKey();
                            Key = cipher.Key;
                            IV = cipher.IV;


                            createAccount(imgPath);
                            registrationLog();

                            Response.Redirect("Login.aspx");
                        }
                    }
                }

            }

            //if captcha
            else
            {
                lbl_message.Text = "validate captcha to prove that you are a human.";
            }
        }


        protected void createAccount(string imgPath)
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@fname,@lname,@dob,@cardnum,@expirydate,@cvv,@email,@passwordsalt,@passwordhash,@photo,@iv,@key,@loginfailed,@lockoutenable,@accountlocktime,@authcode)"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@fname", HttpUtility.HtmlEncode(tb_firstname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@lname", HttpUtility.HtmlEncode(tb_lastname.Text.Trim()));
                            cmd.Parameters.AddWithValue("@dob", HttpUtility.HtmlEncode(DOB.Text.Trim()));
                            cmd.Parameters.AddWithValue("@cardnum", Convert.ToBase64String(encryptData(HttpUtility.HtmlEncode(tb_cardnumber.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@expirydate", Convert.ToBase64String(encryptData(HttpUtility.HtmlEncode(tb_expiry.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@cvv", Convert.ToBase64String(encryptData(HttpUtility.HtmlEncode(tb_cvv.Text.Trim()))));
                            cmd.Parameters.AddWithValue("@email", HttpUtility.HtmlEncode(tb_emailaddress.Text.Trim()));
                            cmd.Parameters.AddWithValue("@passwordsalt", salt);
                            cmd.Parameters.AddWithValue("@passwordhash", finalHash);
                            cmd.Parameters.AddWithValue("@photo", imgPath);
                            cmd.Parameters.AddWithValue("@iv", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@key",Convert.ToBase64String(Key));
                            cmd.Parameters.AddWithValue("@loginfailed", 0);
                            cmd.Parameters.AddWithValue("@lockoutenable", 0);
                            cmd.Parameters.AddWithValue("@accountlocktime", DBNull.Value);
                            cmd.Parameters.AddWithValue("@authcode", DBNull.Value);
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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        protected void registrationLog()
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
                            cmd.Parameters.AddWithValue("@userlog", HttpUtility.HtmlEncode(tb_emailaddress.Text.Trim()));
                            cmd.Parameters.AddWithValue("@action","Successfully registered an account");
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