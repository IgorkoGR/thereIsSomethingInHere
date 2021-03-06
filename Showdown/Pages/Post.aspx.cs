﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Showdown.Pages
{
    public partial class Post : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int uid = -1;

            HttpCookie c = HttpContext.Current.Request.Cookies.Get("User");

            string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (MySqlConnection con = new MySqlConnection(constr))
            {
                con.Open();
                try
                {

                }
                catch
                {

                }
            }

            if (c != null && c.Value != "")
            {
                uid = int.Parse(c.Value);
            }
            else
            {
                uid = -1;
            }

            if (!this.IsPostBack && uid != -1)
            {
                //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
                using (MySqlConnection con = new MySqlConnection(constr))
                {
                    using (MySqlCommand cmd = new MySqlCommand("SELECT Username FROM test WHERE UserID='" + uid + "'"))
                    {
                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                try
                                {
                                    sda.Fill(dt);
                                    //GridView GridView1 = new GridView();
                                    //GridView1.DataSource = dt.Select("text");
                                    //GridView1.DataBind();
                                    Query_Result.Text = dt.Rows[0][0].ToString();
                                }
                                catch (MySql.Data.MySqlClient.MySqlException exc)
                                {

                                }
                            }
                        }
                        cmd.CommandText = "SELECT *FROM post WHERE UserID IN ( SELECT UserID FROM test WHERE UserID = " + uid + ")";

                        using (MySqlDataAdapter sda = new MySqlDataAdapter())
                        {
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            using (DataTable dt = new DataTable())
                            {
                                try
                                {
                                    sda.Fill(dt);
                                    //Query_Result.Text = dt.Rows[0][0].ToString();
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        LiteralControl BlogElem = new LiteralControl();
                                        {
                                            BlogElem.ID = "divcreated";
                                        }
                                        BlogElem.Text = "<div style=\"wdith: 85%; background-color:dimgray; margin: 4% auto; padding: 2% 2%;\">" +
                                            "<h1 style = \"text-align: center; color: black;\" > " + dt.Rows[i][1].ToString() + " </ h1 >" +
                                             "<h4 style = \"text-align: left; color: black;\" >" + dt.Rows[i][2].ToString() + "</ h4 >" +
                                             "</div>";
                                        BlogString.Controls.Add(BlogElem);
                                    }
                                }
                                catch (MySql.Data.MySqlClient.MySqlException exc)
                                {

                                }
                            }
                        }

                    }

                }



            }
        }

        protected void LogoutButton_Click(object sender, EventArgs e)
        {
            HttpCookie c = HttpContext.Current.Response.Cookies.Get("User");
            c.Value = "";
            Query_Result.Text = "";
            //Page_Load(sender, e);
            Response.Redirect("../index.aspx");
        }

    }
}