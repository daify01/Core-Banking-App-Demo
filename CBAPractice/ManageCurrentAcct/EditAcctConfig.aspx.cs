using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.ManageCurrentAcct
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
                DropDownCurrentGL.DataSource = glAccounts;
                DropDownCurrentGL.DataValueField = "Id";
                DropDownCurrentGL.DataTextField = "GlAccountName";
                DropDownCurrentGL.DataBind();

                var glAccountsIncome =
               Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveIncomeGltypes();
                DropDownCOTIncomeGL.DataSource = glAccountsIncome;
                DropDownCOTIncomeGL.DataValueField = "Id";
                DropDownCOTIncomeGL.DataTextField = "GlAccountName";
                DropDownCOTIncomeGL.DataBind();
                //if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                //{
                //    int id = Convert.ToInt32(Request.QueryString["id"]);
                //    CurrentConfig currentConfig =
                //        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>()
                //            .RetrieveById(id);
                //    TextBoxNameCreditInterestRate.Value = currentConfig.creditInterestRate.ToString();
                //    TextBoxNameMinimumBalance.Value = currentConfig.minimumBalance.ToString();
                //    DropDownGlCategory.SelectedValue = currentConfig.InterestExpenseGlAccount.GlAccountName;
                //    DropDownCurrentGL.SelectedValue = currentConfig.currentAccountGL.GlAccountName;
                //    DropDownCOTIncomeGL.SelectedValue = currentConfig.coTIncomeGl.GlAccountName;
                //    TextBoxId.Value = currentConfig.Id.ToString();

                //}

                IList<CurrentConfig> currentConfig =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>()
                            .RetrieveAll();
                if (currentConfig[0] != null)
                {
                    TextBoxNameCreditInterestRate.Value = currentConfig[0].creditInterestRate.ToString();
                    TextBoxNameMinimumBalance.Value = currentConfig[0].minimumBalance.ToString();
                    TextBoxNameCOT.Value = currentConfig[0].coT.ToString();
                    DropDownGlCategory.SelectedValue = currentConfig[0].InterestExpenseGlAccount.Id.ToString();
                    DropDownCurrentGL.SelectedValue = currentConfig[0].currentAccountGL.Id.ToString();
                    DropDownCOTIncomeGL.SelectedValue = currentConfig[0].coTIncomeGl.Id.ToString();
                    TextBoxId.Value = currentConfig[0].Id.ToString();
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameCreditInterestRate.Value)) throw new Exception("Credit Interest Rate field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameMinimumBalance.Value)) throw new Exception("Minimum Balance field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameCOT.Value)) throw new Exception("COT field is required");
                IList<CurrentConfig> currentConfig =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>()
                            .RetrieveAll();
                if (currentConfig[0] == null)
                {

                    //CurrentConfig currentConfig = new CurrentConfig();
                    currentConfig[0].creditInterestRate = Int32.Parse(TextBoxNameCreditInterestRate.Value);
                    currentConfig[0].minimumBalance = Int32.Parse(TextBoxNameMinimumBalance.Value);
                    currentConfig[0].InterestExpenseGlAccount = new GlAccount();
                    currentConfig[0].InterestExpenseGlAccount.Id = Int32.Parse(DropDownGlCategory.SelectedValue);
                    currentConfig[0].currentAccountGL = new GlAccount();
                    currentConfig[0].currentAccountGL.Id = Int32.Parse(DropDownCurrentGL.SelectedValue);
                    currentConfig[0].coTIncomeGl = new GlAccount();
                    currentConfig[0].coTIncomeGl.Id = int.Parse(DropDownCOTIncomeGL.SelectedValue);
                    currentConfig[0].coT = double.Parse(TextBoxNameCOT.Value);
                    
                    User user = new User();
                    user = (User)Session["User"];
                    currentConfig[0].Branch = new Branch();
                    currentConfig[0].Branch.Id = user.Branch.Id;
                    currentConfig[0].DateAdded = DateTime.Now;
                    currentConfig[0].DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().InsertData(currentConfig[0]);
                }

                else if (currentConfig[0] != null)
                {
                    currentConfig[0].creditInterestRate = Int32.Parse(TextBoxNameCreditInterestRate.Value);
                    currentConfig[0].minimumBalance = Int32.Parse(TextBoxNameMinimumBalance.Value);
                    currentConfig[0].InterestExpenseGlAccount = new GlAccount();
                    currentConfig[0].InterestExpenseGlAccount.Id = Int32.Parse(DropDownGlCategory.SelectedValue);
                    currentConfig[0].currentAccountGL = new GlAccount();
                    currentConfig[0].currentAccountGL.Id = Int32.Parse(DropDownCurrentGL.SelectedValue);
                    currentConfig[0].coTIncomeGl = new GlAccount();
                    currentConfig[0].coTIncomeGl.Id = int.Parse(DropDownCOTIncomeGL.SelectedValue);
                    currentConfig[0].coT = double.Parse(TextBoxNameCOT.Value);

                    User user = new User();
                    user = (User)Session["User"];
                    currentConfig[0].Branch = new Branch();
                    currentConfig[0].Branch.Id = user.Branch.Id;
                    currentConfig[0].DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().UpdateData(currentConfig[0]);


                }


                TextBoxNameCreditInterestRate.Value = String.Empty;
                TextBoxNameMinimumBalance.Value = String.Empty;
                TextBoxNameCOT.Value = String.Empty;


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Current Account Config Saved Successfully" + "', function(){location = '/ManageCurrentAcct/EditAcctConfig.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                if (DropDownCurrentGL.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Current GL Not Selected. Please Select a Current GL " + "', function(){});</script>", false);
                }
                if (DropDownCOTIncomeGL.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "COT Income GL Not Selected. Please Select One " + "', function(){});</script>", false);
                }
                if (DropDownGlCategory.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Interest Expense GL Not Selected. Please Select One " + "', function(){});</script>", false);
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