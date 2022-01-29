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

namespace SITConnect_201605R
{

    public class MyObject
    {
        public string success { get; set; }
        public List<string> ErrorMessage { get; set; }
    }
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

      

        //public bool ValidateCaptcha()
        //{
        //    bool result = true;

        //    //When user submits the recaptcha form, the user gets a response POST parameter. 
        //    //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
        //    string captchaResponse = Request.Form["g-recaptcha-response"];

        //    //To send a GET request to Google along with the response and Secret key.
        //    HttpWebRequest req = (HttpWebRequest)WebRequest.Create
        //   (" https://www.google.com/recaptcha/api/siteverify?secret=SecretCode &response=" + captchaResponse);


        //    try
        //    {

        //        //Codes to receive the Response in JSON format from Google Server
        //        using (WebResponse wResponse = req.GetResponse())
        //        {
        //            using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
        //            {
        //                //The response in JSON format
        //                string jsonResponse = readStream.ReadToEnd();

        //                //To show the JSON response string for learning purpose
        //                //lbl_gScore.Text = jsonResponse.ToString();

        //                JavaScriptSerializer js = new JavaScriptSerializer();

        //                //Create jsonObject to handle the response e.g success or Error
        //                //Deserialize Json
        //                MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

        //                //Convert the string "False" to bool false or "True" to bool true
        //                result = Convert.ToBoolean(jsonObject.success);//

        //            }
        //        }

        //        return result;
        //    }
        //    catch (WebException ex)
        //    {
        //        throw ex;
        //    }
        //}

        protected void LoginMe(object sender, EventArgs e)
        {
            //if (ValidateCaptcha())
            //{

                //    // Check for Username and password (hard coded for this demo)
                if (tb_email.Text.Trim().Equals("u") && tb_password.Text.Trim().Equals("p"))
                {
                    Session["LoggedIn"] = tb_email.Text.Trim();

                    // createa a new GUID and save into the session
                    string guid = Guid.NewGuid().ToString();
                    Session["AuthToken"] = guid;

                    // now create a new cookie with this guid value
                    Response.Cookies.Add(new HttpCookie("AuthToken", guid));

                    Response.Redirect("Home.aspx", false);
                }
                else
                {
                    lbl_message.Text = "Wrong username or password";
                }
            //}
            //if captcha
            //else
            //{
            //    lbl_message.Text = "validate captcha to prove that you are a human.";
            //}


        }
    }
}
