using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.OnUsWithdrawalSetup
{
    public partial class AddATMTerminal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    OnUsWithdrawal onUsWithdrawal = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>().RetrieveById(id);
                    TextBoxNameTerminalName.Value = onUsWithdrawal.Name;
                    TextBoxTextBoxNameTerminalID.Value = onUsWithdrawal.TerminalID;
                    TextBoxNameLocation.Value = onUsWithdrawal.Location;
                    TextBoxId.Value = onUsWithdrawal.Id.ToString();
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameTerminalName.Value)) throw new Exception("Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxTextBoxNameTerminalID.Value)) throw new Exception("Terminal ID field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameLocation.Value)) throw new Exception("Location field is required");
                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    OnUsWithdrawal onUsWithdrawal = new OnUsWithdrawal();
                    onUsWithdrawal.Name = TextBoxNameTerminalName.Value;
                    onUsWithdrawal.Location = TextBoxNameLocation.Value;
                    onUsWithdrawal.TerminalID = TextBoxTextBoxNameTerminalID.Value;
                    onUsWithdrawal.DateAdded = DateTime.Now;
                    onUsWithdrawal.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>().InsertData(onUsWithdrawal);
                }
                else
                {
                    OnUsWithdrawal onUsWithdrawal = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>().RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    onUsWithdrawal.Name = TextBoxNameTerminalName.Value;
                    onUsWithdrawal.Location = TextBoxNameLocation.Value;
                    onUsWithdrawal.TerminalID = TextBoxTextBoxNameTerminalID.Value;
                    onUsWithdrawal.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>().UpdateData(onUsWithdrawal);
                }


                TextBoxNameTerminalName.Value = String.Empty;
                TextBoxNameLocation.Value = String.Empty;
                TextBoxTextBoxNameTerminalID.Value = String.Empty;

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "ATM Terminal Saved Successfully" + "', function(){location = '/ManageBranches/AddNewBranch.aspx';});</script>", false);
                }

                //Response.Write("<script type='text/javascript'>alertify.alert('Message', '"+ "Branch Saved Successfully" + "', function(){location = '/';});</script>");
            }
            catch (Exception ex)
            {
                IList<OnUsWithdrawal> onUsWithdrawals = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IOnUsWithdrawalDb>().RetrieveAll();

                foreach (var onuswithdrawal in onUsWithdrawals)
                {
                    if (TextBoxNameTerminalName.Value == onuswithdrawal.Name)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Terminal Name Already Exists. Change Name" + "', function(){});</script>", false);
                    }

                    if (TextBoxTextBoxNameTerminalID.Value == onuswithdrawal.TerminalID)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Terminal ID Already Exists. Change RC No" + "', function(){});</script>", false);
                    }
                }
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }
                //return error message here
                //Response.Write("<script type='text/javascript'>alertify.alert('Message', '" + ex.Message + "', function(){location = '/';});</script>");
            }
        }
    }
}