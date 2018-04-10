using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.Start
{
    public partial class RecoverPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameUserName.Value)) throw new Exception("User Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameEmail.Value)) throw new Exception("Email field is required");
                User user =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrievebyEmail(TextBoxNameEmail.Value);

                if (TextBoxNameEmail.Value == user.Email)
                {
                    UserLogic userLogic = new UserLogic();
                    user.Password = userLogic.EncryptPassword(userLogic.CreatePassword());
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().UpdateData(user);
                    userLogic.SendMail(user.Email, user.Password);
                }

                else if (TextBoxNameEmail.Value != user.Email)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Incorrect Email. Please Enter email you registered with" + "', function(){location = '/Start/RecoverPassword.aspx';});</script>", false);
                }


                TextBoxNameEmail.Value = String.Empty;
                TextBoxNameUserName.Value = String.Empty;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Password Changed Successfully" + "', function(){location = '/ManageBranches/AddNewBranch.aspx';});</script>", false);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }
       
            }
            
        }
    }
}