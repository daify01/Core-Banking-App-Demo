using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.ManageCustomerAcct
{
    public partial class EditCustomerAcct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var branch =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveAll();
                DropDownBranchName.DataSource = branch;
                DropDownBranchName.DataBind();

                DropDownAccountType.DataSource = Enum.GetNames(typeof(AccountType));
                DropDownAccountType.DataBind();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    CustomerAccounts customerAccounts = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveById(id);
                    var customer =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>().RetrieveById(id);

                    TextBoxNameAcctName.Value = customer.LastName + " " + customer.FirstName + " " + customer.OtherNames;
                    TextBoxId.Value = customer.Id.ToString();

                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {


            //if(string.IsNullOrWhiteSpace(DropDownAccountType.SelectedValue)) throw new Exception("Select An Account type");
            IList<CustomerAccounts> customerAccountss = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveAll();

            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameAcctName.Value)) throw new Exception("Account Name field is required");
                if (!string.IsNullOrWhiteSpace(TextBoxId.Value))
                {


                    CustomerAccounts customerAccounts = new CustomerAccounts();
                    customerAccounts.AccountName = TextBoxNameAcctName.Value;

                    customerAccounts.Branch = new Branch();
                    string customerBranchId = DropDownBranchName.SelectedValue;
                    customerAccounts.Branch.Id = int.Parse(customerBranchId);
                    customerAccounts.Branch.BranchName = Convert.ToString(customerAccounts.Branch.Id);


                    customerAccounts.Customer = new Customer();
                    customerAccounts.Customer.Id = int.Parse(TextBoxId.Value);





                    customerAccounts.AccountType =
                        (AccountType)Enum.Parse(typeof(AccountType), DropDownAccountType.SelectedValue);


                    Random rand = new Random();
                    String randomPart = Convert.ToString(rand.Next(10000, 99999));
                    String customerId = TextBoxId.Value;
                    if (customerAccounts.AccountType == Core.AccountType.Savings)
                    {
                        customerAccounts.AccountNumber = '1' + customerId + randomPart;
                    }
                    else if (customerAccounts.AccountType == Core.AccountType.Current)
                    {
                        customerAccounts.AccountNumber = '2' + customerId + randomPart;
                    }
                    else
                    {
                        throw new Exception("Select An Account type");
                    }



                    customerAccounts.DateAdded = DateTime.Now;
                    customerAccounts.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                        .InsertData(customerAccounts);
                }




                TextBoxNameAcctName.Value = String.Empty;


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                        "<script type='text/javascript'>alertify.alert('Message', '" +
                        "Customer Account Saved Successfully" +
                        "', function(){location = '/ManageCustomerAcct/AddNewCustomerAcct.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                if (DropDownBranchName.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Branch Not Selected. Please Select a Branch " + "', function(){});</script>", false);
                }
                //if (DropDownAccountType.SelectedValue == "0")
                //{
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "AccountType Not Selected. Please Select a AccountType " + "', function(){});</script>", false);
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