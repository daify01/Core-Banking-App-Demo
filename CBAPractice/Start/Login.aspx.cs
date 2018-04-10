using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.Start
{
    public partial class Login : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            IList<User> User =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>()
                    .RetrieveAll();
            for (int i = 0; i < User.Count; i++)
            {
                bool validUsername = string.Equals(TextBoxNameUserName.Value, User[i].UserName);
                bool validPassword = string.Equals(TextBoxNamePassword.Value, User[i].Password);
                if (validUsername && validPassword)
                {
                    //FormsAuthentication.RedirectFromLoginPage(TextBoxNameUserName.Value, chkRemember.Checked);
                    
                    Session["User"] = User[i];
                    Response.Redirect("~/Start/Default.aspx");
                    Session.RemoveAll();
                }

                else Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + "Invalid Login Credentials" + @""", function(){});</script>", false);
                
            }

            //Response.Write("<script type='text/javascript'>alertify.alert('Message', '" + "Invalid Login Details" + "', function(){location = '/';});</script>");
            //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + "Invalid Login Credentials" + @""", function(){});</script>", false);
            //InvalidCredentialsMessage.Visible = true;
        }

    }
    
}

