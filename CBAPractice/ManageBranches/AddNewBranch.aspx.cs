using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.ManageBranches;

namespace CBAPractice.ManageBranches
{
    public partial class AddNewBranch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    Branch branch = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveById(id);
                    TextBoxNameBranch.Value = branch.BranchName;
                    TextBoxNameRCNo.Value = branch.RcNumber;
                    TextBoxNameAddress.Value = branch.Address;
                    TextBoxId.Value = branch.Id.ToString();
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameBranch.Value)) throw new Exception("Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameAddress.Value)) throw new Exception("Address field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameRCNo.Value)) throw new Exception("RC No. field is required");
                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    Branch branch = new Branch();
                    branch.BranchName = TextBoxNameBranch.Value;
                    branch.Address = TextBoxNameAddress.Value;
                    branch.RcNumber = TextBoxNameRCNo.Value;
                    branch.DateAdded = DateTime.Now;
                    branch.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().InsertData(branch);
                }
                else
                {
                    Branch branch = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    branch.BranchName = TextBoxNameBranch.Value;
                    branch.Address = TextBoxNameAddress.Value;
                    branch.RcNumber = TextBoxNameRCNo.Value;
                    branch.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().UpdateData(branch);
                }

                
                TextBoxNameBranch.Value = String.Empty;
                TextBoxNameRCNo.Value = String.Empty;
                TextBoxNameAddress.Value = String.Empty;

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Branch Saved Successfully" + "', function(){location = '/ManageBranches/AddNewBranch.aspx';});</script>", false);
                }
            
                //Response.Write("<script type='text/javascript'>alertify.alert('Message', '"+ "Branch Saved Successfully" + "', function(){location = '/';});</script>");
            }
            catch (Exception ex)
            {
                IList<Branch> branches = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveAll();

                foreach (var branchtest in branches)
                {
                    if (TextBoxNameBranch.Value == branchtest.BranchName)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Branch Name Already Exists. Change Name" + "', function(){});</script>", false);
                    }

                    if (TextBoxNameRCNo.Value == branchtest.RcNumber)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "RC Number Already Exists. Change RC No" + "', function(){});</script>", false);
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