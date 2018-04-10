using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.ManageCustomerAcct
{
    public partial class EditCustomerAccountReal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var branch =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveAll();
                DropDownBranchNames.DataSource = branch;
                DropDownBranchNames.DataBind();

                DropDownAccountTypes.DataSource = Enum.GetNames(typeof(AccountType));
                DropDownAccountTypes.DataBind();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    var customerAccounts = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveById(id);
                //    var customer =
                //Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>().RetrieveById(id);

                    TextBoxNameAcctNames.Value = customerAccounts.AccountName;
                    DropDownBranchNames.SelectedValue = customerAccounts.Branch.Id.ToString();
                    DropDownAccountTypes.SelectedValue = customerAccounts.AccountType.ToString();
                    TextBoxId.Value = customerAccounts.Id.ToString();

                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            //if(string.IsNullOrWhiteSpace(DropDownAccountType.SelectedValue)) throw new Exception("Select An Account type");

            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameAcctNames.Value)) throw new Exception("Account Name field is required");
                if (!string.IsNullOrWhiteSpace(TextBoxId.Value))
                {

                    CustomerAccounts customerAccounts = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveById(int.Parse(TextBoxId.Value));
                    customerAccounts.AccountName = TextBoxNameAcctNames.Value;

                    customerAccounts.Branch = new Branch();
                    string customerBranchId = DropDownBranchNames.SelectedValue;
                    customerAccounts.Branch.Id = int.Parse(customerBranchId);
                    customerAccounts.Branch.BranchName = Convert.ToString(customerAccounts.Branch.Id);
                    
                   customerAccounts.AccountType =
                        (AccountType)Enum.Parse(typeof(AccountType), DropDownAccountTypes.SelectedValue);





                    
                    customerAccounts.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().UpdateData(customerAccounts);
                }

                TextBoxNameAcctNames.Value = String.Empty;


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Customer Account Saved Successfully" + "', function(){location = '/ManageCustomerAcct/ViewAllCustomerAcct.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                if (DropDownBranchNames.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Branch Not Selected. Please Select a Branch " + "', function(){});</script>", false);
                }
                //if (DropDownAccountTypes.SelectedValue == "0")
                //{
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Account Type Not Selected. Please Select a Account Type " + "', function(){});</script>", false);
                //}
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }

            }
        }
    }
}