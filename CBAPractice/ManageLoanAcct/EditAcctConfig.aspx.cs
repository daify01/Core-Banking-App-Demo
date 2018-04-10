using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.ManageLoanAcct
{
    public partial class EditAcctConfig : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                var glAccount =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveAll();
                DropDownGlAccount.DataSource = glAccount;
                DropDownGlAccount.DataValueField = "Id";
                DropDownGlAccount.DataTextField = "GlAccountName";
                DropDownGlAccount.DataBind();

                var glAccounts =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveOtherAssettypes();
                DropDownLoanPrincipalGL.DataSource = glAccount;
                DropDownLoanPrincipalGL.DataValueField = "Id";
                DropDownLoanPrincipalGL.DataTextField = "GlAccountName";
                DropDownLoanPrincipalGL.DataBind();
                //if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                //{
                //    int id = Convert.ToInt32(Request.QueryString["id"]);
                //    LoanConfig loanConfig =
                //        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>()
                //            .RetrieveById(id);
                //    TextBoxNameDebitInterestRate.Value = loanConfig.debitInterestRate.ToString();
                //    DropDownGlAccount.SelectedValue = loanConfig.InterestIncomeGlAccount.GlAccountName;
                //    TextBoxId.Value = loanConfig.Id.ToString();

                //}
                IList<LoanConfig> loanConfig =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>()
                            .RetrieveAll();
                if (loanConfig[0] != null)
                {
                    TextBoxNameDebitInterestRate.Value = loanConfig[0].debitInterestRate.ToString();
                    DropDownGlAccount.SelectedValue = loanConfig[0].InterestIncomeGlAccount.Id.ToString();
                    DropDownLoanPrincipalGL.SelectedValue = loanConfig[0].LoanPrincipalGlAccount.Id.ToString();
                    TextBoxId.Value = loanConfig[0].Id.ToString();
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameDebitInterestRate.Value)) throw new Exception("Debit Interest Rate field is required");
                IList<LoanConfig> loanConfig =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>()
                            .RetrieveAll();
                if (loanConfig[0] == null)
                {

                    //LoanConfig loanConfig = new LoanConfig();
                    loanConfig[0].debitInterestRate = Int32.Parse(TextBoxNameDebitInterestRate.Value);
                    loanConfig[0].InterestIncomeGlAccount = new GlAccount();
                    loanConfig[0].InterestIncomeGlAccount.Id = Int32.Parse(DropDownGlAccount.SelectedValue);
                    loanConfig[0].LoanPrincipalGlAccount = new GlAccount();
                    loanConfig[0].LoanPrincipalGlAccount.Id = Int32.Parse(DropDownLoanPrincipalGL.SelectedValue);
                    loanConfig[0].DateAdded = DateTime.Now;
                    loanConfig[0].DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>().InsertData(loanConfig[0]);
                }

                else if (loanConfig[0] != null)
                {
                    loanConfig[0].debitInterestRate = Int32.Parse(TextBoxNameDebitInterestRate.Value);
                    loanConfig[0].InterestIncomeGlAccount = new GlAccount();
                    loanConfig[0].InterestIncomeGlAccount.Id = Int32.Parse(DropDownGlAccount.SelectedValue);
                    loanConfig[0].LoanPrincipalGlAccount = new GlAccount();
                    loanConfig[0].LoanPrincipalGlAccount.Id = Int32.Parse(DropDownLoanPrincipalGL.SelectedValue);
                    loanConfig[0].DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>().UpdateData(loanConfig[0]);


                }


                TextBoxNameDebitInterestRate.Value = String.Empty;
                


                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Loan Account Configuration Saved Successfully" + "', function(){location = '/ManageLoanAcct/EditAcctConfig.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                if (DropDownLoanPrincipalGL.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Loan Principal GL Not Selected. Please Select One " + "', function(){});</script>", false);
                }
                if (DropDownGlAccount.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Interest Income GL Not Selected. Please Select One " + "', function(){});</script>", false);
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