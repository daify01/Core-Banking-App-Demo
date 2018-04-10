using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.ManageCustomerAcct
{
    public partial class AddLoanAcct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            if (eod[0].IsClosed == false)
            {
                if (!IsPostBack)
                {
                    var glAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveOtherAssettypes();
                    DropDownListLoanAccount.DataSource = glAccount;
                    DropDownListLoanAccount.DataValueField = "Id";
                    DropDownListLoanAccount.DataTextField = "GlAccountName";
                    DropDownListLoanAccount.DataBind();

                    DropDownListPaymentSchedule.DataSource = Enum.GetNames(typeof(PaymentSchedule));
                    DropDownListPaymentSchedule.DataBind();

                    TextBoxNameCustAcctName.Disabled = true;
                    //TextBoxNameNumberOfDays.Visible = false;
                } 
            }

            else if (eod[0].IsClosed)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Business is closed. Open Business" +
               "', function(){location = '/Start/Default.aspx';});</script>", false);
            }
            
        }

        protected void verifysubmit_OnServerClick(object sender, EventArgs e)
        {
            try
            {
                //if (string.IsNullOrWhiteSpace(TextBoxNameCustAcctName.Value)) throw new Exception("Account Name field is required");
                CustomerAccounts customerAccounts =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                    .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                if (customerAccounts.AccountNumber == TextBoxNameCustAcctNo.Value)
                {
                    TextBoxNameCustAcctName.Value = customerAccounts.AccountName;
                }
                else if (customerAccounts.AccountNumber != TextBoxNameCustAcctNo.Value)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Invalid Account Number" +
                    "', function(){});</script>", false);
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

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameCustAcctNo.Value)) throw new Exception("Account Number field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameAmount.Value)) throw new Exception("Amount field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameDuration.Value)) throw new Exception("Duration field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameCustAcctName.Value)) throw new Exception("Account Name field is required");
                if (DropDownListPaymentSchedule.SelectedValue == Core.PaymentSchedule.Days.ToString())
                {
                    if (string.IsNullOrWhiteSpace(TextBoxNameNumberOfDays.Value)) throw new Exception("Number of Days field is required");
                }
                

                CustomerAccounts checkclosedoropen =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                            .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                if (checkclosedoropen.IsClosed == false)
                {
                    if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                    {
                        LoanAccount loanAccount = new LoanAccount();

                        CustomerAccounts customerAccounts =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                        //LoanConfig getLoanById = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>()
                        //        .RetrieveById(1);
                        IList<LoanConfig> getLoanByIds =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>()
                                .RetrieveAll();
                        IList<EOD> eods =
                           Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                        TextBoxNameCustAcctName.Value = customerAccounts.AccountName;
                        loanAccount.LinkedAccount = new CustomerAccounts();
                        loanAccount.LinkedAccount.Id = customerAccounts.Id;
                        loanAccount.LoanConfig = new LoanConfig();
                        //loanAccount.LoanConfig.Id = getLoanById.Id;
                        loanAccount.LoanConfig.Id = getLoanByIds[0].Id;
                        loanAccount.AccountName = TextBoxNameCustAcctName.Value;
                        loanAccount.LoanAmount = double.Parse(TextBoxNameAmount.Value);
                        loanAccount.LoanDuration = double.Parse(TextBoxNameDuration.Value);
                        loanAccount.LoanInterest = getLoanByIds[0].debitInterestRate;
                        DateTime today = DateTime.Now;
                        loanAccount.LoanStartDate = today;
                        loanAccount.LoanDueDate = today.AddDays(loanAccount.LoanDuration);
                        Random rand = new Random();
                        String randomPart = Convert.ToString(rand.Next(10000, 99999));
                        String customerId = customerAccounts.Customer.Id.ToString();
                        loanAccount.AccountNumber = '3' + customerId + randomPart;
                        loanAccount.Balance = loanAccount.Balance + double.Parse(TextBoxNameAmount.Value);
                        loanAccount.DateAdded = DateTime.Now;
                        loanAccount.DateUpdated = DateTime.Now;
                        loanAccount.PaymentSchedule = (PaymentSchedule)Enum.Parse(typeof(PaymentSchedule), DropDownListPaymentSchedule.SelectedValue);
                        loanAccount.LoanStatus = LoanStatus.BeingPaid;
                        loanAccount.TransactionDate = eods[0].FinancialDate;
                        if (DropDownListPaymentSchedule.SelectedValue == Core.PaymentSchedule.Days.ToString())
                        {
                            loanAccount.NumberOfDays = int.Parse(TextBoxNameNumberOfDays.Value);
                        }
                        
                        GlAccount glAccount =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .RetrieveById(int.Parse(DropDownListLoanAccount.SelectedValue));
                        GlPostingLogic glPostingLogic = new GlPostingLogic();
                        glAccount.Balance = glPostingLogic.DebitGlAccount(glAccount, loanAccount.LoanAmount);

                        User user = new User();
                        user = (User)Session["User"];
                        //Code to update balance in savings account GL
                        var updateSavingsGlBalance =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>()
                            .RetrieveByBranch(user.Branch.Id);
                        updateSavingsGlBalance.SavingsAccountGL.Balance =
                            glPostingLogic.CreditGlAccount(updateSavingsGlBalance.SavingsAccountGL, loanAccount.LoanAmount);



                        //Put Code to update balance in current account GL here:
                        var updateCurrentGlBalance =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>()
                                .RetrieveByBranch(user.Branch.Id);
                        updateCurrentGlBalance.currentAccountGL.Balance =
                            glPostingLogic.CreditGlAccount(updateCurrentGlBalance.currentAccountGL,
                                loanAccount.LoanAmount);


                        IList<EOD> eod =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                        //Code to Save savings Transaction in GL Posting:
                        GlPosting savingsGlPosting = new GlPosting();
                        savingsGlPosting.Amount = loanAccount.LoanAmount;
                        savingsGlPosting.CreditNarration = String.Format("{0} Credited", updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                        savingsGlPosting.DebitNarration = String.Format("{0} Debited", glAccount.GlAccountName);
                        savingsGlPosting.GlAccountToCredit = new GlAccount();
                        savingsGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                        savingsGlPosting.GlAccountToDebit = new GlAccount();
                        savingsGlPosting.GlAccountToDebit.Id = glAccount.Id;
                        savingsGlPosting.TransactionDate = eod[0].FinancialDate;
                        savingsGlPosting.DateAdded = DateTime.Now;
                        savingsGlPosting.DateUpdated = DateTime.Now;

                        //Code to Save Current A/C Transaction in GL Posting:
                        GlPosting currentGlPosting = new GlPosting();
                        currentGlPosting.Amount = loanAccount.LoanAmount;
                        currentGlPosting.CreditNarration = String.Format("{0} Credited", updateCurrentGlBalance.currentAccountGL.GlAccountName);
                        currentGlPosting.DebitNarration = String.Format("{0} Debited", glAccount.GlAccountName);
                        currentGlPosting.GlAccountToCredit = new GlAccount();
                        currentGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                        currentGlPosting.GlAccountToDebit = new GlAccount();
                        currentGlPosting.GlAccountToDebit.Id = glAccount.Id;
                        currentGlPosting.TransactionDate = eod[0].FinancialDate;
                        currentGlPosting.DateAdded = DateTime.Now;
                        currentGlPosting.DateUpdated = DateTime.Now;

                        TellerPostingLogic tellerPostingLogic = new TellerPostingLogic();
                        var updateCustAcctBal =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                        updateCustAcctBal.Balance = tellerPostingLogic.CreditCustomerAccounts(customerAccounts,
                            loanAccount.LoanAmount);



                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().InsertData(loanAccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().UpdateData(glAccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().UpdateData(updateCustAcctBal);
                        if (customerAccounts.AccountType == AccountType.Savings)
                        {
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                             .UpdateData(updateSavingsGlBalance.SavingsAccountGL);
                        }
                        if (customerAccounts.AccountType == AccountType.Current)
                        {
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(updateCurrentGlBalance.currentAccountGL);
                        }
                        if (customerAccounts.AccountType == AccountType.Savings)
                        {
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(savingsGlPosting);
                        }
                        if (customerAccounts.AccountType == AccountType.Current)
                        {
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .InsertData(currentGlPosting);
                        }


                    }

                    else
                    {


                    }


                    TextBoxNameCustAcctNo.Value = String.Empty;
                    TextBoxNameCustAcctName.Value = String.Empty;
                    TextBoxNameAmount.Value = String.Empty;
                    TextBoxNameDuration.Value = String.Empty;

                    if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Loan Disbursed Successfully." + "', function(){location = '/ManageCustomerAcct/AddLoanAcct.aspx';});</script>", false);
                    }
                }

                else if (checkclosedoropen.IsClosed)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                        "<script type='text/javascript'>alertify.alert('Message', '" +
                        "Customer Account is closed. Open Account" +
                        "', function(){location = '/ManageCustomerAcct/AddLoanAcct.aspx';});</script>",
                        false);
                }

            }
            catch (Exception ex)
            {
                if (DropDownListLoanAccount.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Loan Account Not Selected. Please Select a Loan Account " + "', function(){});</script>", false);
                }
                //if (DropDownListPaymentSchedule.SelectedValue == "10")
                //{
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Payment Schedule Not Selected. Please Select a Payment Schedule" + "', function(){});</script>", false);
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