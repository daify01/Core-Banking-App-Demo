using System;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.ManageUsers
{
    public partial class ChangeUserPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNamePassword.Value)) throw new Exception("Password field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameNewPassword.Value)) throw new Exception("New Password field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameConfNewPassword.Value)) throw new Exception("Confirm Password field is required");
                User user =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrievebyPassword(TextBoxNamePassword.Value);

                
                if (TextBoxNamePassword.Value == user.Password)
                {
                    if (TextBoxNameNewPassword.Value == TextBoxNameConfNewPassword.Value)
                    {
                        user.Password = TextBoxNameNewPassword.Value;
                        
                    }
                    else
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Mismatch. Confirm Password Details Were Entered Correctly" + "', function(){location = '/ManageUsers/ChangeUserPassword.aspx';});</script>", false);
                    }
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>()
                            .UpdateData(user);

                }

                

                
                TextBoxNamePassword.Value = String.Empty;
                TextBoxNameNewPassword.Value = String.Empty;
                TextBoxNameConfNewPassword.Value = String.Empty;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Password Changed Successfully" + "', function(){location = '/Start/Login.aspx';});</script>", false);
                }
            }
            catch (Exception ex)
            {
                //string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                //if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                //{
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                //}

                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Wrong Old Password. Enter correctly" + "', function(){location = '/ManageUsers/ChangeUserPassword.aspx';});</script>", false);
                }
            }
        }
    }
}