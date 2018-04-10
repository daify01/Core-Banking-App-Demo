using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.ManageSavingsAcct
{
    public partial class EditAcctConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                var glAccount =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveExpenseGLtypes();
                DropDownGlCategory.DataSource = glAccount;
                DropDownGlCategory.DataValueField = "Id";
                DropDownGlCategory.DataTextField = "GlAccountName";
                DropDownGlCategory.DataBind();

                var glAccounts =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveLiablilityGltypes();
                DropDownSavingsGL.DataSource = glAccounts;
                DropDownSavingsGL.DataValueField = "Id";
                DropDownSavingsGL.DataTextField = "GlAccountName";
                DropDownSavingsGL.DataBind();
                //if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                //{
                //    int id = Convert.ToInt32(Request.QueryString["id"]);
                //    SavingsConfig savingsConfig =
                //        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>()
                //            .RetrieveById(id);
                //    TextBoxNameCreditInterestRate.Value = savingsConfig.creditInterestRate.ToString();
                //    TextBoxNameMinimumBalance.Value = savingsConfig.minimumBalance.ToString();
                //    DropDownGlCategory.SelectedValue = savingsConfig.InterestExpenseGlAccount.GlAccountName;
                //    DropDownSavingsGL.SelectedValue = savingsConfig.SavingsAccountGL.GlAccountName;
                //    TextBoxId.Value = savingsConfig.Id.ToString();

                //}
                IList<SavingsConfig> savingsConfig =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>()
                            .RetrieveAll();
                if (savingsConfig[0] != null)
                {
                    TextBoxNameCreditInterestRate.Value = savingsConfig[0].creditInterestRate.ToString();
                    TextBoxNameMinimumBalance.Value = savingsConfig[0].minimumBalance.ToString();
                    DropDownGlCategory.SelectedValue = savingsConfig[0].InterestExpenseGlAccount.Id.ToString();
                    DropDownSavingsGL.SelectedValue = savingsConfig[0].SavingsAccountGL.Id.ToString();
                    TextBoxId.Value = savingsConfig[0].Id.ToString();
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameCreditInterestRate.Value)) throw new Exception("Credit Interest Rate field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameMinimumBalance.Value)) throw new Exception("Minimum Balance field is required");
                IList<SavingsConfig> savingsConfig =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>()
                            .RetrieveAll();
                if (savingsConfig[0] == null)
                {

                    //SavingsConfig savingsConfig = new SavingsConfig();
                    savingsConfig[0].creditInterestRate = Int32.Parse(TextBoxNameCreditInterestRate.Value);
                    savingsConfig[0].minimumBalance = Int32.Parse(TextBoxNameMinimumBalance.Value);
                    savingsConfig[0].InterestExpenseGlAccount = new GlAccount();
                    savingsConfig[0].InterestExpenseGlAccount.Id = Int32.Parse(DropDownGlCategory.SelectedValue);
                    savingsConfig[0].SavingsAccountGL = new GlAccount();
                    savingsConfig[0].SavingsAccountGL.Id = Int32.Parse(DropDownSavingsGL.SelectedValue);
                    User user = new User();
                    user = (User) Session["User"];
                    savingsConfig[0].Branch = new Branch();
                    savingsConfig[0].Branch.Id = user.Branch.Id;
                    savingsConfig[0].DateAdded = DateTime.Now;
                    savingsConfig[0].DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().InsertData(savingsConfig[0]);
                }

                else if (savingsConfig[0] != null)
                {
                    savingsConfig[0].creditInterestRate = Int32.Parse(TextBoxNameCreditInterestRate.Value);
                    savingsConfig[0].minimumBalance = Int32.Parse(TextBoxNameMinimumBalance.Value);
                    savingsConfig[0].InterestExpenseGlAccount = new GlAccount();
                    savingsConfig[0].InterestExpenseGlAccount.Id = Int32.Parse(DropDownGlCategory.SelectedValue);
                    savingsConfig[0].SavingsAccountGL = new GlAccount();
                    savingsConfig[0].SavingsAccountGL.Id = Int32.Parse(DropDownSavingsGL.SelectedValue);
                    User user = new User();
                    user = (User)Session["User"];
                    savingsConfig[0].Branch = new Branch();
                    savingsConfig[0].Branch.Id = user.Branch.Id;
                    savingsConfig[0].DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().UpdateData(savingsConfig[0]);


                }


                TextBoxNameCreditInterestRate.Value = String.Empty;
                TextBoxNameMinimumBalance.Value = String.Empty;


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Savings Account Config Saved Successfully" + "', function(){location = '/ManageSavingsAcct/EditAcctConfig.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                if (DropDownSavingsGL.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Savings GL Not Selected. Please Select a Savings GL " + "', function(){});</script>", false);
                }
                if (DropDownGlCategory.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Interest Expense GL Not Selected. Please Select an Interest Expense GL " + "', function(){});</script>", false);
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