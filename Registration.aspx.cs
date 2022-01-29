using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text.RegularExpressions;
using System.Drawing;

namespace SITConnect_201605R
{
    public partial class Registration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_checkPassword_Click(object sender, EventArgs e)
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
                    status = "Strong";
                    break;
                case 5:
                    status = "Excellent";
                    break;
                default:
                    break;
            }
            pwd_length_checker.Text = "Status : " + status;
            if (scores < 4)
            {
                pwd_length_checker.ForeColor = Color.Red;
                return;
            }
            pwd_length_checker.ForeColor = Color.Green;


            Response.Redirect("Home.aspx?Message=" + HttpUtility.UrlEncode(HttpUtility.HtmlEncode(tb_firstname.Text)));

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

    }
}