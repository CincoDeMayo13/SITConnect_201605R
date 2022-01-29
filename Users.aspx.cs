using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace SITConnect_201605R
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void BtnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    DataSet dset = new DataSet();
                    SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString());
                    using (conn)
                    {
                        conn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter();
                        string sqlQuery = string.Format("SELECT userID, name, email FROM user_info WHERE user_info WHERE userID =@0");
                        SqlCommand cmd = new SqlCommand(sqlQuery, conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@0", txtUserID.Text);
                        adapter.SelectCommand = cmd;
                        adapter.Fill(dset);
                        gvUserInfo.DataSource = dset;
                        gvUserInfo.DataBind();
                    }
                }
                catch (SqlException ex)
                {
                    //Lbl_error.Text = "Invalid search inpuy"
                }
            }
        }
    }
}