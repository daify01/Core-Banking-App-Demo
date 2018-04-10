using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.EODProcess;
using CBAPractice.Logic;
using NHibernate.Util;

namespace CBAPractice.TellerPostingMgt
{
    public partial class PostTransactionsIntoCustomerAcct : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            if (eod[0].IsClosed == false)
            {
                if (!IsPostBack)
                {
                    DropDownListPostingType.DataSource = Enum.GetNames(typeof(PostingType));
                    DropDownListPostingType.DataBind();
                    TextBoxNameCustAcctName.Disabled = true;
                    User user = new User();
                    user = (User)Session["User"];
                    //IList<Teller> teller =
                    //    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>().RetrieveAll();

                    Teller teller =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                            .RetrievebyUserName(user);
                    TellerPostingLogic tellerPostingLogic = new TellerPostingLogic();
                    try
                    {
                        if (tellerPostingLogic.unsassignedTill(teller, user))
                        {
                            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Till Not assigned to User" +
                        "', function(){location = '/ManageTellers/AssignTillToUser.aspx';});</script>", false);
                        }
                        else if (teller.User.UserName == user.UserName)
                        {
                            TextBoxNameTillAcctName.Value = teller.GlAccount.GlAccountName;
                        }
                        TextBoxNameTillAcctName.Disabled = true;
                    }
                    catch (Exception)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "No Till Account Exists. Create Till GL account and Assign to a User" +
                      "', function(){location = '/GlAccountMgt/AddNewGlAccount.aspx';});</script>", false);
                    }
                    
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
                //if (string.IsNullOrWhiteSpace(TextBoxNameCustAcctNo.Value)) throw new Exception("Account Number field is required");

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
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Invalid Account Number" +
                   "', function(){});</script>", false);
                }

            }
            
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameCustAcctNo.Value)) throw new Exception("Account Number field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameAmount.Value)) throw new Exception("Amount field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameNarration.Value)) throw new Exception("Narration field is required");

                CustomerAccounts checkclosedoropen =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                            .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                if (checkclosedoropen.IsClosed == false)
                {
                    if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                    {
                        TellerPosting tellerPosting = new TellerPosting();

                        GlAccount glAccount =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .RetrieveByAccountName(TextBoxNameTillAcctName.Value);
                        GlAccount glAccountForGlPostings =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .RetrieveByAccountName(TextBoxNameTillAcctName.Value);
                        CustomerAccounts customerAccounts =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                        CustomerAccounts updatebalforglposting =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .SearchbyAccountNumber(TextBoxNameCustAcctNo.Value);
                        IList<CurrentConfig> currentConfig =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>()
                                .RetrieveAll();
                        IList<SavingsConfig> savingsConfig =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>()
                                .RetrieveAll();
                        tellerPosting.CustomerAccounts = new CustomerAccounts();
                        tellerPosting.CustomerAccounts.Id = customerAccounts.Id;
                        var glAccounts =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .RetrieveById(tellerPosting.CustomerAccounts.Id);
                        tellerPosting.GlAccount = new GlAccount();
                        tellerPosting.GlAccount.Id = glAccount.Id;
                        var Account =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .RetrieveById(tellerPosting.GlAccount.Id); //if it doesnt work, change back to tellerPosting.GlAccount.Id
                        tellerPosting.Narration = TextBoxNameNarration.Value;
                        tellerPosting.Amount = double.Parse(TextBoxNameAmount.Value);
                        tellerPosting.PostingType = (PostingType)Enum.Parse(typeof(PostingType), DropDownListPostingType.SelectedValue);

                        IList<EOD> eods =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                        tellerPosting.TransactionDate = eods[0].FinancialDate;
                        
                        //CoTCharge calculation
                        int chargeOnCoT = Convert.ToInt32(currentConfig[0].coT * tellerPosting.Amount) / 1000;
                        customerAccounts.CoTCharge = chargeOnCoT + customerAccounts.CoTCharge;

                        User user = new User();
                        user = (User)Session["User"];
                        //Code to update balance in savings account GL
                        var updateSavingsGlBalance =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>()
                                .RetrieveByBranch(user.Branch.Id);

                        //Put Code to update balance in current account GL here:
                        var updateCurrentGlBalance =
                           Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>()
                               .RetrieveByBranch(user.Branch.Id);
                        IList<EOD> eod =
                           Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();

                        TellerPostingLogic tellerPostingLogic = new TellerPostingLogic();
                        GlPosting savingsdepositGlPosting = new GlPosting();
                        GlPosting savingswithdrawalGlPosting = new GlPosting();
                        GlPosting currentdepositGlPosting = new GlPosting();
                        GlPosting currentwithdrawalGlPosting = new GlPosting();
                        GlPosting gLPostingForLoanAmountRemoval = new GlPosting();

                        //Properties useful for taking deposited money immediately from overdue acct
                        
                        IList<LoanConfig> loanConfig =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>().RetrieveAll();
                        IList<LoanAccount> HasTheCustomerTakenALoan =
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                            .SearchbyLinkedAccountId(customerAccounts.Id.ToString());

                        
                        

                        if (customerAccounts.AccountType == AccountType.Savings)
                        {
                            if (DropDownListPostingType.SelectedValue == "Withdrawal")
                            {
                                try
                                {
                                    if (
                                    tellerPostingLogic.CustomerSavingsAccountBalanceConfirmed(customerAccounts, savingsConfig[0],
                                        DropDownListPostingType.SelectedValue,
                                        tellerPosting.Amount) == false)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                                            "<script type='text/javascript'>alertify.alert('Message', '" +
                                            "Insufficient Funds. Deposit into Customer Account" +
                                            "', function(){});</script>",
                                            false);
                                    }
                                    else if (
                                tellerPostingLogic.GLAccountBalanceConfirmed(glAccount,
                                    DropDownListPostingType.SelectedValue,
                                    tellerPosting.Amount) == false)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                                            "<script type='text/javascript'>alertify.alert('Message', '" +
                                            "Insufficient Funds in Till Account" +
                                            "', function(){});</script>",
                                            false);
                                    }
                                    else if (tellerPostingLogic.CustomerSavingsAccountBalanceConfirmed(customerAccounts, savingsConfig[0],
                                            DropDownListPostingType.SelectedValue,
                                            tellerPosting.Amount) &&
                                        tellerPostingLogic.GLAccountBalanceConfirmed(glAccount,
                                            DropDownListPostingType.SelectedValue,
                                            tellerPosting.Amount))
                                    {
                                        glAccounts.Balance = tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        Account.Balance = tellerPostingLogic.GlAccountTransaction(glAccount,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        updateSavingsGlBalance.SavingsAccountGL.Balance =
                                    tellerPostingLogic.GlAccountTransaction(updateSavingsGlBalance.SavingsAccountGL,
                                        DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                    }
                                }
                                catch (Exception)
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                                            "<script type='text/javascript'>alertify.alert('Message', '" +
                                            "Insufficient Funds. Confirm Available Funds in Till And Customer Accounts" +
                                            "', function(){});</script>",
                                            false);
                                }
                            }

                            
                            
                            else if (DropDownListPostingType.SelectedValue == "Deposit")
                                {
                                    if (HasTheCustomerTakenALoan.Count == 0)
                                    {
                                        glAccounts.Balance = tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        Account.Balance = tellerPostingLogic.GlAccountTransaction(glAccount,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        updateSavingsGlBalance.SavingsAccountGL.Balance =
                                    tellerPostingLogic.GlAccountTransaction(updateSavingsGlBalance.SavingsAccountGL,
                                        DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                    }
                                        // If The customer owes an unpaid loan, deduct amount immediately he deposits
                                else if (HasTheCustomerTakenALoan.Count != 0)
                                {
                                    foreach (var customerLoanAccount in HasTheCustomerTakenALoan)
                                    {
                                        if (customerLoanAccount.LoanStatus == LoanStatus.Overdue)
                                        {
                                            glAccounts.Balance = tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                            Account.Balance = tellerPostingLogic.GlAccountTransaction(glAccount,
                                                DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                            updateSavingsGlBalance.SavingsAccountGL.Balance =
                                        tellerPostingLogic.GlAccountTransaction(updateSavingsGlBalance.SavingsAccountGL,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);

                                            //Put GLposting for deposit here
                                            //Code to Save savings Transaction in GL Posting:
                                            
                                            savingsdepositGlPosting.Amount = tellerPosting.Amount;
                                            savingsdepositGlPosting.CreditNarration = String.Format("Deposit to {0}", glAccountForGlPostings.GlAccountName);
                                            savingsdepositGlPosting.DebitNarration = String.Format("Deposit to {0}", updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                                            savingsdepositGlPosting.GlAccountToCredit = new GlAccount();
                                            savingsdepositGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                                            savingsdepositGlPosting.GlAccountToDebit = new GlAccount();
                                            savingsdepositGlPosting.GlAccountToDebit.Id = glAccount.Id;
                                            savingsdepositGlPosting.TransactionDate = eod[0].FinancialDate;
                                            savingsdepositGlPosting.DateAdded = DateTime.Now;
                                            savingsdepositGlPosting.DateUpdated = DateTime.Now;
                                            tellerPosting.DateAdded = DateTime.Now;
                                            tellerPosting.DateUpdated = DateTime.Now;

                                            if (tellerPosting.Amount <= HasTheCustomerTakenALoan[0].Balance)
                                            {
                                                glAccounts.Balance =
                                                tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                    "Withdrawal", tellerPosting.Amount);
                                                updateSavingsGlBalance.SavingsAccountGL.Balance =
                                            tellerPostingLogic.GlAccountTransaction(updateSavingsGlBalance.SavingsAccountGL,
                                                "Withdrawal", tellerPosting.Amount);
                                                loanConfig[0].LoanPrincipalGlAccount.Balance = tellerPostingLogic.GlAccountTransaction(loanConfig[0].LoanPrincipalGlAccount,
                                                DropDownListPostingType.SelectedValue, tellerPosting.Amount);

                                                //reduce the money left to be paid in loan acct
                                                HasTheCustomerTakenALoan[0].Balance = HasTheCustomerTakenALoan[0].Balance -
                                                                                      tellerPosting.Amount;

                                                // put GL posting for immediately removed deposited amnt here

                                                gLPostingForLoanAmountRemoval.Amount = tellerPosting.Amount;
                                                gLPostingForLoanAmountRemoval.CreditNarration = String.Format("Deposit to {0}", loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                                gLPostingForLoanAmountRemoval.DebitNarration = String.Format("Withdrawal From {0}", updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                                                gLPostingForLoanAmountRemoval.GlAccountToCredit = new GlAccount();
                                                gLPostingForLoanAmountRemoval.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                                gLPostingForLoanAmountRemoval.GlAccountToDebit = new GlAccount();
                                                gLPostingForLoanAmountRemoval.GlAccountToDebit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                                                gLPostingForLoanAmountRemoval.TransactionDate = eod[0].FinancialDate;
                                                gLPostingForLoanAmountRemoval.DateAdded = DateTime.Now;
                                                gLPostingForLoanAmountRemoval.DateUpdated = DateTime.Now;

                                            }
                                            else if (tellerPosting.Amount > HasTheCustomerTakenALoan[0].Balance)
                                            {
                                                glAccounts.Balance =
                                                tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                    "Withdrawal", HasTheCustomerTakenALoan[0].Balance);
                                                updateSavingsGlBalance.SavingsAccountGL.Balance =
                                            tellerPostingLogic.GlAccountTransaction(updateSavingsGlBalance.SavingsAccountGL,
                                                "Withdrawal", HasTheCustomerTakenALoan[0].Balance);
                                                loanConfig[0].LoanPrincipalGlAccount.Balance = tellerPostingLogic.GlAccountTransaction(loanConfig[0].LoanPrincipalGlAccount,
                                                DropDownListPostingType.SelectedValue, HasTheCustomerTakenALoan[0].Balance);

                                                //reduce the money left to be paid in loan acct
                                                HasTheCustomerTakenALoan[0].Balance = HasTheCustomerTakenALoan[0].Balance -
                                                                                      HasTheCustomerTakenALoan[0].Balance;

                                                // put GL posting for immediately removed deposited amnt here

                                                gLPostingForLoanAmountRemoval.Amount = HasTheCustomerTakenALoan[0].Balance;
                                                gLPostingForLoanAmountRemoval.CreditNarration = String.Format("Deposit to {0}", loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                                gLPostingForLoanAmountRemoval.DebitNarration = String.Format("Withdrawal From {0}", updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                                                gLPostingForLoanAmountRemoval.GlAccountToCredit = new GlAccount();
                                                gLPostingForLoanAmountRemoval.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                                gLPostingForLoanAmountRemoval.GlAccountToDebit = new GlAccount();
                                                gLPostingForLoanAmountRemoval.GlAccountToDebit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                                                gLPostingForLoanAmountRemoval.TransactionDate = eod[0].FinancialDate;
                                                gLPostingForLoanAmountRemoval.DateAdded = DateTime.Now;
                                                gLPostingForLoanAmountRemoval.DateUpdated = DateTime.Now;
                                            }
                                            
                                        }
                                        // Jump out after doing for first overdue account, as same customer shouldn't have unpaid loans more than once
                                        break;
                                    }
                                }
                                    
                                
                                }
                            
                            
                        }
                       

                        else if (customerAccounts.AccountType == AccountType.Current)
                        {
                            if (DropDownListPostingType.SelectedValue == "Withdrawal")
                            {
                                try
                                {
                                    if (
                                    tellerPostingLogic.CustomerCurrentAccountBalanceConfirmed(customerAccounts, currentConfig[0],
                                        DropDownListPostingType.SelectedValue,
                                        tellerPosting.Amount) == false)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                                            "<script type='text/javascript'>alertify.alert('Message', '" +
                                            "Insufficient Funds. Deposit into Customer Account" +
                                            "', function(){});</script>",
                                            false);
                                    }
                                    else if (
                                tellerPostingLogic.GLAccountBalanceConfirmed(glAccount,
                                    DropDownListPostingType.SelectedValue,
                                    tellerPosting.Amount) == false)
                                    {
                                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                                            "<script type='text/javascript'>alertify.alert('Message', '" +
                                            "Insufficient Funds in Till Account" +
                                            "', function(){});</script>",
                                            false);
                                    }
                                    else if (tellerPostingLogic.CustomerCurrentAccountBalanceConfirmed(customerAccounts, currentConfig[0],
                                            DropDownListPostingType.SelectedValue,
                                            tellerPosting.Amount) &&
                                        tellerPostingLogic.GLAccountBalanceConfirmed(glAccount,
                                            DropDownListPostingType.SelectedValue,
                                            tellerPosting.Amount))
                                    {
                                        glAccounts.Balance = tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        Account.Balance = tellerPostingLogic.GlAccountTransaction(glAccount,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        updateCurrentGlBalance.currentAccountGL.Balance =
                                    tellerPostingLogic.GlAccountTransaction(updateCurrentGlBalance.currentAccountGL,
                                        DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                    }
                                }
                                catch (Exception)
                                {
                                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                                            "<script type='text/javascript'>alertify.alert('Message', '" +
                                            "Insufficient Funds. Confirm Available Funds in till And Customer Accounts" +
                                            "', function(){});</script>",
                                            false);
                                }
                            }

                            
                            
                            else if (DropDownListPostingType.SelectedValue == "Deposit")
                                {
                                    if (HasTheCustomerTakenALoan.Count == 0)
                                    {
                                        glAccounts.Balance = tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                               DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        Account.Balance = tellerPostingLogic.GlAccountTransaction(glAccount,
                                            DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                        updateCurrentGlBalance.currentAccountGL.Balance =
                                    tellerPostingLogic.GlAccountTransaction(updateCurrentGlBalance.currentAccountGL,
                                        DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                    }

                                    else if (HasTheCustomerTakenALoan.Count != 0)
                                    {
                                        foreach (var customerLoanAccount in HasTheCustomerTakenALoan)
                                        {
                                            if (customerLoanAccount.LoanStatus == LoanStatus.Overdue)
                                            {
                                                glAccounts.Balance = tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                                Account.Balance = tellerPostingLogic.GlAccountTransaction(glAccount,
                                                    DropDownListPostingType.SelectedValue, tellerPosting.Amount);
                                                updateCurrentGlBalance.currentAccountGL.Balance =
                                            tellerPostingLogic.GlAccountTransaction(updateCurrentGlBalance.currentAccountGL,
                                                DropDownListPostingType.SelectedValue, tellerPosting.Amount);

                                                //Put GLposting for deposit here
                                                //Code to Save savings Transaction in GL Posting:

                                                currentdepositGlPosting.Amount = tellerPosting.Amount;
                                                currentdepositGlPosting.CreditNarration = String.Format("Deposit to {0}", glAccountForGlPostings.GlAccountName);
                                                currentdepositGlPosting.DebitNarration = String.Format("Deposit to {0}", updateCurrentGlBalance.currentAccountGL.GlAccountName);
                                                currentdepositGlPosting.GlAccountToCredit = new GlAccount();
                                                currentdepositGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                                                currentdepositGlPosting.GlAccountToDebit = new GlAccount();
                                                currentdepositGlPosting.GlAccountToDebit.Id = glAccount.Id;
                                                currentdepositGlPosting.TransactionDate = eod[0].FinancialDate;
                                                currentdepositGlPosting.DateAdded = DateTime.Now;
                                                currentdepositGlPosting.DateUpdated = DateTime.Now;
                                                tellerPosting.DateAdded = DateTime.Now;
                                                tellerPosting.DateUpdated = DateTime.Now;

                                                if (tellerPosting.Amount <= HasTheCustomerTakenALoan[0].Balance)
                                                {
                                                    glAccounts.Balance =
                                                    tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                        "Withdrawal", tellerPosting.Amount);
                                                    updateCurrentGlBalance.currentAccountGL.Balance =
                                                tellerPostingLogic.GlAccountTransaction(updateCurrentGlBalance.currentAccountGL,
                                                    "Withdrawal", tellerPosting.Amount);
                                                    loanConfig[0].LoanPrincipalGlAccount.Balance = tellerPostingLogic.GlAccountTransaction(loanConfig[0].LoanPrincipalGlAccount,
                                                    DropDownListPostingType.SelectedValue, tellerPosting.Amount);

                                                    //reduce the money left to be paid in loan acct
                                                    HasTheCustomerTakenALoan[0].Balance = HasTheCustomerTakenALoan[0].Balance -
                                                                                          tellerPosting.Amount;

                                                    // put GL posting for immediately removed deposited amnt here

                                                    gLPostingForLoanAmountRemoval.Amount = tellerPosting.Amount;
                                                    gLPostingForLoanAmountRemoval.CreditNarration = String.Format("Deposit to {0}", loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                                    gLPostingForLoanAmountRemoval.DebitNarration = String.Format("Withdrawal From {0}", updateCurrentGlBalance.currentAccountGL.GlAccountName);
                                                    gLPostingForLoanAmountRemoval.GlAccountToCredit = new GlAccount();
                                                    gLPostingForLoanAmountRemoval.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                                    gLPostingForLoanAmountRemoval.GlAccountToDebit = new GlAccount();
                                                    gLPostingForLoanAmountRemoval.GlAccountToDebit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                                                    gLPostingForLoanAmountRemoval.TransactionDate = eod[0].FinancialDate;
                                                    gLPostingForLoanAmountRemoval.DateAdded = DateTime.Now;
                                                    gLPostingForLoanAmountRemoval.DateUpdated = DateTime.Now;

                                                }
                                                else if (tellerPosting.Amount > HasTheCustomerTakenALoan[0].Balance)
                                                {
                                                    glAccounts.Balance =
                                                    tellerPostingLogic.CustomerAccountTransaction(customerAccounts,
                                                        "Withdrawal", HasTheCustomerTakenALoan[0].Balance);
                                                    updateCurrentGlBalance.currentAccountGL.Balance =
                                                tellerPostingLogic.GlAccountTransaction(updateCurrentGlBalance.currentAccountGL,
                                                    "Withdrawal", HasTheCustomerTakenALoan[0].Balance);
                                                    loanConfig[0].LoanPrincipalGlAccount.Balance = tellerPostingLogic.GlAccountTransaction(loanConfig[0].LoanPrincipalGlAccount,
                                                    DropDownListPostingType.SelectedValue, HasTheCustomerTakenALoan[0].Balance);

                                                    //reduce the money left to be paid in loan acct
                                                    HasTheCustomerTakenALoan[0].Balance = HasTheCustomerTakenALoan[0].Balance -
                                                                                          HasTheCustomerTakenALoan[0].Balance;

                                                    // put GL posting for immediately removed deposited amnt here

                                                    gLPostingForLoanAmountRemoval.Amount = HasTheCustomerTakenALoan[0].Balance;
                                                    gLPostingForLoanAmountRemoval.CreditNarration = String.Format("Deposit to {0}", loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                                    gLPostingForLoanAmountRemoval.DebitNarration = String.Format("Withdrawal From {0}", updateCurrentGlBalance.currentAccountGL.GlAccountName);
                                                    gLPostingForLoanAmountRemoval.GlAccountToCredit = new GlAccount();
                                                    gLPostingForLoanAmountRemoval.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                                    gLPostingForLoanAmountRemoval.GlAccountToDebit = new GlAccount();
                                                    gLPostingForLoanAmountRemoval.GlAccountToDebit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                                                    gLPostingForLoanAmountRemoval.TransactionDate = eod[0].FinancialDate;
                                                    gLPostingForLoanAmountRemoval.DateAdded = DateTime.Now;
                                                    gLPostingForLoanAmountRemoval.DateUpdated = DateTime.Now;
                                                }

                                            }
                                            // Jump out after doing for first overdue account, as same customer shouldn't have unpaid loans more than once
                                            break;
                                        }
                                    }
                                    
                                }
                            
                            
                        }

                        

                        //Added the if statement cos of GL postings for overdue accts

                        if (HasTheCustomerTakenALoan.Count == 0)
                        {
                            //Code to Save savings Transaction in GL Posting:
                            //GlPosting savingsdepositGlPosting = new GlPosting();
                            savingsdepositGlPosting.Amount = tellerPosting.Amount;
                            savingsdepositGlPosting.CreditNarration = String.Format("Deposit to {0}", glAccountForGlPostings.GlAccountName);
                            savingsdepositGlPosting.DebitNarration = String.Format("Deposit to {0}", updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                            savingsdepositGlPosting.GlAccountToCredit = new GlAccount();
                            savingsdepositGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                            savingsdepositGlPosting.GlAccountToDebit = new GlAccount();
                            savingsdepositGlPosting.GlAccountToDebit.Id = glAccount.Id;
                            savingsdepositGlPosting.TransactionDate = eod[0].FinancialDate;
                            savingsdepositGlPosting.DateAdded = DateTime.Now;
                            savingsdepositGlPosting.DateUpdated = DateTime.Now;
                            tellerPosting.DateAdded = DateTime.Now;
                            tellerPosting.DateUpdated = DateTime.Now;

                            //GlPosting savingswithdrawalGlPosting = new GlPosting();
                            if (
                                customerAccounts.AccountType == AccountType.Savings && tellerPostingLogic.GLAccountBalanceConfirmed(glAccountForGlPostings, DropDownListPostingType.SelectedValue,
                                    tellerPosting.Amount) &&
                                tellerPostingLogic.CustomerSavingsAccountBalanceConfirmed(updatebalforglposting, savingsConfig[0],
                                    DropDownListPostingType.SelectedValue, tellerPosting.Amount))
                            {

                                savingswithdrawalGlPosting.Amount = tellerPosting.Amount;
                                savingswithdrawalGlPosting.CreditNarration = String.Format("Withdrawal From {0}",
                                    glAccountForGlPostings.GlAccountName);
                                savingswithdrawalGlPosting.DebitNarration = String.Format("Withdrawal From {0}",
                                    updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                                savingswithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                                savingswithdrawalGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                                savingswithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                                savingswithdrawalGlPosting.GlAccountToDebit.Id = glAccount.Id;
                                savingswithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                                savingswithdrawalGlPosting.DateAdded = DateTime.Now;
                                savingswithdrawalGlPosting.DateUpdated = DateTime.Now;
                                tellerPosting.DateAdded = DateTime.Now;
                                tellerPosting.DateUpdated = DateTime.Now;
                            }

                            //Code to Save Current A/C Transaction in GL Posting:
                            //GlPosting currentdepositGlPosting = new GlPosting();
                            currentdepositGlPosting.Amount = tellerPosting.Amount;
                            currentdepositGlPosting.CreditNarration = String.Format("Deposit to {0}", glAccountForGlPostings.GlAccountName);
                            currentdepositGlPosting.DebitNarration = String.Format("Deposit to {0}", updateCurrentGlBalance.currentAccountGL.GlAccountName);
                            currentdepositGlPosting.GlAccountToCredit = new GlAccount();
                            currentdepositGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                            currentdepositGlPosting.GlAccountToDebit = new GlAccount();
                            currentdepositGlPosting.GlAccountToDebit.Id = glAccount.Id;
                            currentdepositGlPosting.TransactionDate = eod[0].FinancialDate;
                            currentdepositGlPosting.DateAdded = DateTime.Now;
                            currentdepositGlPosting.DateUpdated = DateTime.Now;
                            tellerPosting.DateAdded = DateTime.Now;
                            tellerPosting.DateUpdated = DateTime.Now;

                            //GlPosting currentwithdrawalGlPosting = new GlPosting();
                            if (
                                customerAccounts.AccountType == AccountType.Current && tellerPostingLogic.GLAccountBalanceConfirmed(glAccountForGlPostings, DropDownListPostingType.SelectedValue,
                                    tellerPosting.Amount) &&
                                tellerPostingLogic.CustomerCurrentAccountBalanceConfirmed(updatebalforglposting, currentConfig[0],
                                    DropDownListPostingType.SelectedValue, tellerPosting.Amount))
                            {

                                currentwithdrawalGlPosting.Amount = tellerPosting.Amount;
                                currentwithdrawalGlPosting.CreditNarration = String.Format("Withdrawal From {0} ",
                                    glAccountForGlPostings.GlAccountName);
                                currentwithdrawalGlPosting.DebitNarration = String.Format("Withdrawal From {0}",
                                    updateCurrentGlBalance.currentAccountGL.GlAccountName);
                                currentwithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                                currentwithdrawalGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                                currentwithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                                currentwithdrawalGlPosting.GlAccountToDebit.Id = glAccount.Id;
                                currentwithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                                currentwithdrawalGlPosting.DateAdded = DateTime.Now;
                                currentwithdrawalGlPosting.DateUpdated = DateTime.Now;
                                tellerPosting.DateAdded = DateTime.Now;
                                tellerPosting.DateUpdated = DateTime.Now;
                            }
                        }
                       
                       

                        if (DropDownListPostingType.SelectedValue == "Deposit")
                        {
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerPostingDb>().InsertData(tellerPosting);
                        }
                        //Validate acct balance before doing teller posting for withdrawals
                        if (DropDownListPostingType.SelectedValue == "Withdrawal")
                        {
                            if (customerAccounts.AccountType == AccountType.Current && tellerPostingLogic.GLAccountBalanceConfirmed(glAccountForGlPostings,
                                    DropDownListPostingType.SelectedValue,
                                    tellerPosting.Amount) &&
                                tellerPostingLogic.CustomerCurrentAccountBalanceConfirmed(updatebalforglposting,
                                    currentConfig[0],
                                    DropDownListPostingType.SelectedValue, tellerPosting.Amount))
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerPostingDb>
                                    ().InsertData(tellerPosting);
                            }
                            else if (customerAccounts.AccountType == AccountType.Savings && tellerPostingLogic.GLAccountBalanceConfirmed(glAccountForGlPostings,
                                    DropDownListPostingType.SelectedValue,
                                    tellerPosting.Amount) &&
                                tellerPostingLogic.CustomerSavingsAccountBalanceConfirmed(updatebalforglposting,
                                    savingsConfig[0],
                                    DropDownListPostingType.SelectedValue, tellerPosting.Amount))
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerPostingDb>
                                    ().InsertData(tellerPosting);
                            }
                        }
                        
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().UpdateData(Account);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().UpdateData(glAccounts);
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
                            if (DropDownListPostingType.SelectedValue == "Deposit" && HasTheCustomerTakenALoan.Count == 0)
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(savingsdepositGlPosting);
                            }
                            else if (DropDownListPostingType.SelectedValue == "Deposit" &&
                                     HasTheCustomerTakenALoan.Count != 0)
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                             .UpdateData(HasTheCustomerTakenALoan[0]);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(savingsdepositGlPosting);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(gLPostingForLoanAmountRemoval);
                            }

                        }
                        if (customerAccounts.AccountType == AccountType.Savings)
                        {
                            if (DropDownListPostingType.SelectedValue == "Withdrawal")
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(savingswithdrawalGlPosting);
                            }

                        }
                        if (customerAccounts.AccountType == AccountType.Current )
                        {
                            if (DropDownListPostingType.SelectedValue == "Deposit" && HasTheCustomerTakenALoan.Count == 0)
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .InsertData(currentdepositGlPosting);
                            }

                            else if (DropDownListPostingType.SelectedValue == "Deposit" &&
                                    HasTheCustomerTakenALoan.Count != 0)
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                             .UpdateData(HasTheCustomerTakenALoan[0]);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(currentdepositGlPosting);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                             .InsertData(gLPostingForLoanAmountRemoval);
                            }

                        }
                        if (customerAccounts.AccountType == AccountType.Current)
                        {
                            if (DropDownListPostingType.SelectedValue == "Withdrawal")
                            {
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .InsertData(currentwithdrawalGlPosting);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                            .UpdateData(customerAccounts);
                            }
                        }


                    }

                    


                    TextBoxNameCustAcctNo.Value = String.Empty;
                    TextBoxNameCustAcctName.Value = String.Empty;
                    TextBoxNameTillAcctName.Value = String.Empty;
                    TextBoxNameAmount.Value = String.Empty;
                    TextBoxNameNarration.Value = String.Empty;


                    if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Teller Posting Saved Successfully" + "', function(){location = '/TellerPostingMgt/PostTransactionsIntoCustomerAcct.aspx';});</script>", false);
                    }
                }

                else if (checkclosedoropen.IsClosed)
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message",
                        "<script type='text/javascript'>alertify.alert('Message', '" +
                        "Customer Account is closed. Open Account" +
                        "', function(){location = '/TellerPostingMgt/PostTransactionsIntoCustomerAcct.aspx';});</script>",
                        false);
                }
                

            }
            catch (Exception ex)
            {
                if (DropDownListPostingType.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Posting Type Not Selected. Please Select a Posting Type " + "', function(){});</script>", false);
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