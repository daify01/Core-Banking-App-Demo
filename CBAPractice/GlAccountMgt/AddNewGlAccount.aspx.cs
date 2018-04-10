using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.GlAccountMgt
{
    public partial class AddNewGlAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var branches =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveAll();
                DropDownBranch.DataSource = branches;
                DropDownBranch.DataValueField = "Id";
                DropDownBranch.DataTextField = "BranchName";
                DropDownBranch.DataBind();

                var glCategory =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>().RetrieveAll();
                DropDownGlCategory.DataSource = glCategory;
                DropDownGlCategory.DataValueField = "Id";
                DropDownGlCategory.DataTextField = "GlCategoryName";
                DropDownGlCategory.DataBind();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    GlAccount glAccount =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(id);
                    TextBoxNameGlAccountName.Value = glAccount.GlAccountName;
                    DropDownBranch.SelectedValue = glAccount.Branch.Id.ToString();
                    DropDownGlCategory.SelectedValue = glAccount.GlCategory.Id.ToString();
                    TextBoxId.Value = glAccount.Id.ToString();
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
           
         
            var tr = Enum.Parse(typeof (MainAccountCategory), DropDownGlCategory.SelectedValue);
            int categoryID = (int)tr;

            GlCategory glCategory =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>()
                    .RetrieveById(categoryID);
           
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameGlAccountName.Value)) throw new Exception("Gl Account Name field is required");

                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    
                    GlAccount glAccount = new GlAccount();
                    Branch branch = new Branch();
                    glAccount.Branch = branch;
                    GlAccountLogic glAccountLogic = new GlAccountLogic();
                    glAccount.GlAccountName = TextBoxNameGlAccountName.Value;
                    string glBranchId = DropDownBranch.SelectedValue;
                    glAccount.Branch.Id = int.Parse(glBranchId);
                    glAccount.Branch.BranchName = Convert.ToString(glAccount.Branch.Id);
                    glAccount.GlCategory = glCategory;
                    glAccount.GlAccountCodes = glAccountLogic.GetGlAccountCode(glAccount);
                    glAccount.DateAdded = DateTime.Now;
                    glAccount.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().InsertData(glAccount);
                }

                else
                {
                    GlAccount glAccount =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    glAccount.GlAccountName = TextBoxNameGlAccountName.Value;
                    glAccount.GlAccountName = TextBoxNameGlAccountName.Value;
                    //glAccount.GlCategory.Id = int.Parse(DropDownGlCategory.SelectedValue);
                    //glAccount.GlCategory.GlCategoryName = DropDownGlCategory.SelectedValue;
                    GlAccountLogic glAccountLogic = new GlAccountLogic();
                    glAccount.GlAccountCodes = glAccountLogic.GetGlAccountCode(glAccount);
                    glAccount.GlCategory = glCategory;
                    glAccount.Branch.Id = int.Parse(DropDownBranch.SelectedValue);
                    glAccount.Branch.BranchName = DropDownBranch.SelectedValue;
                    glAccount.DateUpdated = DateTime.Now;
                    
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                        .UpdateData(glAccount);
                }


                TextBoxNameGlAccountName.Value = String.Empty;


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gl Account Saved Successfully" + "', function(){location = '/GlAccountMgt/AddNewGlAccount.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                IList<GlAccount> glaccount = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveAll();

                foreach (var glaccounttest in glaccount)
                {
                    if (TextBoxNameGlAccountName.Value == glaccounttest.GlAccountName)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gl Account Name Already Exists. Change it" + "', function(){});</script>", false);
                    }
                }
                if (DropDownGlCategory.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gl Category Not Selected. Please Select One " + "', function(){});</script>", false);
                }
                if (DropDownBranch.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Branch Not Selected. Please Select a Branch " + "', function(){});</script>", false);
                }
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }

            }
        }
    }
}