using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.Logic
{
    public class EODLogic
    {
        public bool CloseBusiness()
        {
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            if (eod.Count == 0)
            {
                EOD eodInstance = new EOD();
                eodInstance.IsClosed = true;
                eodInstance.DateAdded = DateTime.Now;
                eodInstance.DateUpdated = DateTime.Now;
                DateTime FinanceDate = DateTime.Now.Date;
                eodInstance.FinancialDate = FinanceDate;
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().InsertData(eodInstance);
                IList<EOD> eodhere = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                return eodhere[0].IsClosed;
            }
            else if (eod.Count != 0)
            {
                eod[0].IsClosed = true;
                eod[0].DateAdded = DateTime.Now;
                eod[0].DateUpdated = DateTime.Now;
                //DateTime FinanceDate = DateTime.Now.Date;
                //eod[0].FinancialDate = FinanceDate;
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().UpdateData(eod[0]);
               
            }
            return eod[0].IsClosed;
        }

        public void IncrementFinancialdate()
        {
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            eod[0].FinancialDate = eod[0].FinancialDate.AddDays(1);
            eod[0].DateAdded = DateTime.Now;
            eod[0].DateUpdated = DateTime.Now;
            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().UpdateData(eod[0]);
        }

        public void IncrementPresentLoanDate()
        {
            IList<LoanAccount> GetLoanProperties =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveAll();
            foreach (var loanAccount in GetLoanProperties)
            {
                loanAccount.DateUpdated = loanAccount.DateUpdated.AddDays(1);
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().UpdateData(loanAccount);
            }
           
        }
        public bool OpenBusiness()
        {
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            eod[0].IsClosed = false;
            eod[0].DateAdded = DateTime.Now;
            eod[0].DateUpdated = DateTime.Now;
            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().UpdateData(eod[0]);
            return eod[0].IsClosed;
        }

        public void RunEOD()
        {
            PayInterests();
            ReceiveInterests();
            ReceivePrincipalPayment();
            ReceivePrincipalAndInterestOnOverdueAccts();
            GetCOT();
            IncrementFinancialdate();
            IncrementPresentLoanDate();
        }

        public void PayInterests()
        {

            IList<CurrentConfig> CurrentConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().RetrieveAll();
            IList<SavingsConfig> SavingsConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().RetrieveAll();
            IList<CustomerAccounts> GetSavingsorCurrentProperties=
                 Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveAll();

            foreach (var customeraccount in GetSavingsorCurrentProperties)
            {
                IList<EOD> eod =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                if (customeraccount.AccountType == AccountType.Savings)
                {
                    if (customeraccount.IsClosed == false)
                    {
                        double savingsInterest = CalculateInterest(customeraccount.Balance,
                            SavingsConfig[0].creditInterestRate, 1);
                        customeraccount.Balance = customeraccount.Balance + savingsInterest;
                        GlPostingLogic glPostingLogic = new GlPostingLogic();
                        SavingsConfig[0].InterestExpenseGlAccount.Balance =
                            glPostingLogic.DebitGlAccount(SavingsConfig[0].InterestExpenseGlAccount, savingsInterest);
                        SavingsConfig[0].SavingsAccountGL.Balance =
                            glPostingLogic.CreditGlAccount(SavingsConfig[0].SavingsAccountGL, savingsInterest);
                        GlPosting savingsGlPosting = new GlPosting();
                        savingsGlPosting.Amount = savingsInterest;
                        savingsGlPosting.CreditNarration = String.Format("{0} Received Interest on Savings at EOD",
                            customeraccount.AccountName);
                        savingsGlPosting.DebitNarration = String.Format("{0} Paid Interest on Savings at EOD",
                            SavingsConfig[0].InterestExpenseGlAccount.GlAccountName);
                        savingsGlPosting.GlAccountToCredit = new GlAccount();
                        savingsGlPosting.GlAccountToCredit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                        savingsGlPosting.GlAccountToDebit = new GlAccount();
                        savingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].InterestExpenseGlAccount.Id;
                        savingsGlPosting.TransactionDate = eod[0].FinancialDate;
                        savingsGlPosting.DateAdded = DateTime.Now;
                        savingsGlPosting.DateUpdated = DateTime.Now;
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                            .UpdateData(customeraccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(SavingsConfig[0].InterestExpenseGlAccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(SavingsConfig[0].SavingsAccountGL);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .InsertData(savingsGlPosting);
                    }
                }
                else if (customeraccount.AccountType == AccountType.Current)
                {
                    if (customeraccount.IsClosed == false)
                    {
                        double currentInterest = CalculateInterest(customeraccount.Balance,
                            CurrentConfig[0].creditInterestRate, 1);
                        customeraccount.Balance = customeraccount.Balance + currentInterest;
                        GlPostingLogic glPostingLogic = new GlPostingLogic();
                        CurrentConfig[0].InterestExpenseGlAccount.Balance =
                            glPostingLogic.DebitGlAccount(CurrentConfig[0].InterestExpenseGlAccount, currentInterest);
                        CurrentConfig[0].currentAccountGL.Balance =
                            glPostingLogic.CreditGlAccount(CurrentConfig[0].currentAccountGL, currentInterest);
                        GlPosting currentGlPosting = new GlPosting();
                        currentGlPosting.Amount = currentInterest;
                        currentGlPosting.CreditNarration = String.Format("{0} Received Interest on Savings at EOD",
                            customeraccount.AccountName);
                        currentGlPosting.DebitNarration = String.Format("{0} Paid Interest on Savings at EOD",
                            CurrentConfig[0].InterestExpenseGlAccount.GlAccountName);
                        currentGlPosting.GlAccountToCredit = new GlAccount();
                        currentGlPosting.GlAccountToCredit.Id = CurrentConfig[0].currentAccountGL.Id;
                        currentGlPosting.GlAccountToDebit = new GlAccount();
                        currentGlPosting.GlAccountToDebit.Id = CurrentConfig[0].InterestExpenseGlAccount.Id;
                        currentGlPosting.TransactionDate = eod[0].FinancialDate;
                        currentGlPosting.DateAdded = DateTime.Now;
                        currentGlPosting.DateUpdated = DateTime.Now;
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                            .UpdateData(customeraccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(CurrentConfig[0].InterestExpenseGlAccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(CurrentConfig[0].currentAccountGL);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .InsertData(currentGlPosting);
                    }
                }
            }

        }

        public void ReceivePrincipalAndInterestOnOverdueAccts()
        {
            IList<CurrentConfig> CurrentConfig =
               Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().RetrieveAll();
            IList<SavingsConfig> SavingsConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().RetrieveAll();
            IList<LoanConfig> loanConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>().RetrieveAll();

            IList<LoanAccount> GetOverdueLoans =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveByOverdueStatus();
            IList<EOD> eod =
                       Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            GlPostingLogic glPostingLogic = new GlPostingLogic();
            foreach (var loanAccount in GetOverdueLoans)
            {
                if (loanAccount.LinkedAccount.AccountType == AccountType.Savings)
                {
                    if (loanAccount.LinkedAccount.IsClosed == false && loanAccount.LinkedAccount.Balance > 0)
                    {
                        if (loanAccount.LinkedAccount.Balance > loanAccount.Balance)
                        {
                            loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - loanAccount.Balance;
                            loanAccount.Balance = loanAccount.Balance - loanAccount.Balance;

                            SavingsConfig[0].SavingsAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(SavingsConfig[0].SavingsAccountGL, loanAccount.Balance);
                            loanConfig[0].LoanPrincipalGlAccount.Balance =
                                glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, loanAccount.Balance);

                            GlPosting loansavingsGlPosting = new GlPosting();
                            loansavingsGlPosting.Amount = loanAccount.Balance;
                            loansavingsGlPosting.CreditNarration = String.Format("{0} Loan Balance Received at EOD",
                                loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                            loansavingsGlPosting.DebitNarration = String.Format("{0} Loan balance taken at EOD",
                                loanAccount.LinkedAccount.AccountName);
                            loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                            loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                            loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                            loansavingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                            loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                            loansavingsGlPosting.DateAdded = DateTime.Now;
                            loansavingsGlPosting.DateUpdated = DateTime.Now;
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .UpdateData(loanAccount.LinkedAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                .UpdateData(loanAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(SavingsConfig[0].SavingsAccountGL);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                .InsertData(loansavingsGlPosting);
                        }

                        else if (loanAccount.LinkedAccount.Balance <= loanAccount.Balance)
                        {
                            loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - loanAccount.LinkedAccount.Balance;
                            loanAccount.Balance = loanAccount.Balance - loanAccount.LinkedAccount.Balance;

                            SavingsConfig[0].SavingsAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(SavingsConfig[0].SavingsAccountGL, loanAccount.LinkedAccount.Balance);
                            loanConfig[0].LoanPrincipalGlAccount.Balance =
                                glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, loanAccount.LinkedAccount.Balance);

                            GlPosting loansavingsGlPosting = new GlPosting();
                            loansavingsGlPosting.Amount = loanAccount.LinkedAccount.Balance;
                            loansavingsGlPosting.CreditNarration = String.Format("{0} Loan Balance Received at EOD",
                                loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                            loansavingsGlPosting.DebitNarration = String.Format("{0} Loan balance taken at EOD",
                                loanAccount.LinkedAccount.AccountName);
                            loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                            loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                            loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                            loansavingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                            loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                            loansavingsGlPosting.DateAdded = DateTime.Now;
                            loansavingsGlPosting.DateUpdated = DateTime.Now;
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .UpdateData(loanAccount.LinkedAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                .UpdateData(loanAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(SavingsConfig[0].SavingsAccountGL);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                .InsertData(loansavingsGlPosting);
                        }
                    }
                }

                if (loanAccount.LinkedAccount.AccountType == AccountType.Current)
                {
                    if (loanAccount.LinkedAccount.IsClosed == false && loanAccount.LinkedAccount.Balance > 0)
                    {
                        if (loanAccount.LinkedAccount.Balance > loanAccount.Balance)
                        {
                            loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - loanAccount.Balance;
                            loanAccount.Balance = loanAccount.Balance - loanAccount.Balance;

                            CurrentConfig[0].currentAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, loanAccount.Balance);
                            loanConfig[0].LoanPrincipalGlAccount.Balance =
                                glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, loanAccount.Balance);

                            GlPosting loansavingsGlPosting = new GlPosting();
                            loansavingsGlPosting.Amount = loanAccount.Balance;
                            loansavingsGlPosting.CreditNarration = String.Format("{0} Loan Balance Received at EOD",
                                loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                            loansavingsGlPosting.DebitNarration = String.Format("{0} Loan balance taken at EOD",
                                loanAccount.LinkedAccount.AccountName);
                            loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                            loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                            loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                            loansavingsGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                            loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                            loansavingsGlPosting.DateAdded = DateTime.Now;
                            loansavingsGlPosting.DateUpdated = DateTime.Now;
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .UpdateData(loanAccount.LinkedAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                .UpdateData(loanAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(CurrentConfig[0].currentAccountGL);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                .InsertData(loansavingsGlPosting);
                        }

                        else if (loanAccount.LinkedAccount.Balance <= loanAccount.Balance)
                        {
                            loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - loanAccount.Balance;
                            loanAccount.Balance = loanAccount.Balance - loanAccount.Balance;

                            CurrentConfig[0].currentAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, loanAccount.Balance);
                            loanConfig[0].LoanPrincipalGlAccount.Balance =
                                glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, loanAccount.Balance);

                            GlPosting loansavingsGlPosting = new GlPosting();
                            loansavingsGlPosting.Amount = loanAccount.Balance;
                            loansavingsGlPosting.CreditNarration = String.Format("{0} Loan Balance Received at EOD",
                                loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                            loansavingsGlPosting.DebitNarration = String.Format("{0} Loan balance taken at EOD",
                                loanAccount.LinkedAccount.AccountName);
                            loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                            loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                            loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                            loansavingsGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                            loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                            loansavingsGlPosting.DateAdded = DateTime.Now;
                            loansavingsGlPosting.DateUpdated = DateTime.Now;
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                .UpdateData(loanAccount.LinkedAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                .UpdateData(loanAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(CurrentConfig[0].currentAccountGL);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                            Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                .InsertData(loansavingsGlPosting);
                        }
                    }
                }
            }
        }

        public void ReceiveInterests()
        {
            IList<CurrentConfig> CurrentConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().RetrieveAll();
            IList<SavingsConfig> SavingsConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().RetrieveAll();
            IList<LoanConfig> loanConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>().RetrieveAll();
            
            IList<LoanAccount> GetLoanProperties =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveAll();

            foreach (var loanAccount in GetLoanProperties)
            {
                IList<EOD> eod =
                       Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                if (loanAccount.LinkedAccount.AccountType == AccountType.Savings)
                {
                    if (loanAccount.LinkedAccount.IsClosed == false && loanAccount.LinkedAccount.Balance > 0)
                    {
                        if (loanAccount.PaymentSchedule == PaymentSchedule.Days)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays))
                            {
                                double loanInterest = CalculateInterest(loanAccount.LoanAmount, loanConfig[0].debitInterestRate, 1);
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - (loanInterest*loanAccount.NumberOfDays);
                                loanAccount.Balance = loanAccount.Balance - (loanInterest*loanAccount.NumberOfDays);
                                loanAccount.DateAdded = loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                SavingsConfig[0].SavingsAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(SavingsConfig[0].SavingsAccountGL, loanInterest);
                                loanConfig[0].InterestIncomeGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].InterestIncomeGlAccount, loanInterest);
                                GlPosting loansavingsGlPosting = new GlPosting();
                                loansavingsGlPosting.Amount = loanInterest;
                                loansavingsGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].InterestIncomeGlAccount.GlAccountName);
                                loansavingsGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                                loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].InterestIncomeGlAccount.Id;
                                loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                                loansavingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                                loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                                loansavingsGlPosting.DateAdded = DateTime.Now;
                                loansavingsGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(SavingsConfig[0].SavingsAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].InterestIncomeGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loansavingsGlPosting);
                            }
                        }

                        if (loanAccount.PaymentSchedule == PaymentSchedule.Monthly)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddMonths(1))
                            {
                                double loanInterest = CalculateInterest(loanAccount.LoanAmount, loanConfig[0].debitInterestRate, 1);
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - loanInterest*(loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.Balance = loanAccount.Balance - loanInterest * (loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.DateAdded = loanAccount.DateAdded.AddMonths(1);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                SavingsConfig[0].SavingsAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(SavingsConfig[0].SavingsAccountGL, loanInterest);
                                loanConfig[0].InterestIncomeGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].InterestIncomeGlAccount, loanInterest);
                                GlPosting loansavingsGlPosting = new GlPosting();
                                loansavingsGlPosting.Amount = loanInterest;
                                loansavingsGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].InterestIncomeGlAccount.GlAccountName);
                                loansavingsGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                                loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].InterestIncomeGlAccount.Id;
                                loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                                loansavingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                                loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                                loansavingsGlPosting.DateAdded = DateTime.Now;
                                loansavingsGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(SavingsConfig[0].SavingsAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].InterestIncomeGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loansavingsGlPosting);
                            }
                        }
                       
                    }
                }

                else if (loanAccount.LinkedAccount.AccountType == AccountType.Current)
                {
                    if (loanAccount.LinkedAccount.IsClosed == false && loanAccount.LinkedAccount.Balance > 0)
                    {
                        if (loanAccount.PaymentSchedule == PaymentSchedule.Days)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays))
                            {
                                double loanInterest = CalculateInterest(loanAccount.LoanAmount, loanConfig[0].debitInterestRate, 1);
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - (loanInterest * loanAccount.NumberOfDays);
                                loanAccount.Balance = loanAccount.Balance - (loanInterest * loanAccount.NumberOfDays);
                                loanAccount.DateAdded = loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                CurrentConfig[0].currentAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, loanInterest);
                                loanConfig[0].InterestIncomeGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].InterestIncomeGlAccount, loanInterest);
                                GlPosting loancurrentGlPosting = new GlPosting();
                                loancurrentGlPosting.Amount = loanInterest;
                                loancurrentGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].InterestIncomeGlAccount.GlAccountName);
                                loancurrentGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loancurrentGlPosting.GlAccountToCredit = new GlAccount();
                                loancurrentGlPosting.GlAccountToCredit.Id = loanConfig[0].InterestIncomeGlAccount.Id;
                                loancurrentGlPosting.GlAccountToDebit = new GlAccount();
                                loancurrentGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                                loancurrentGlPosting.TransactionDate = eod[0].FinancialDate;
                                loancurrentGlPosting.DateAdded = DateTime.Now;
                                loancurrentGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(CurrentConfig[0].currentAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].InterestIncomeGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loancurrentGlPosting);
                            }
                        }

                        if (loanAccount.PaymentSchedule == PaymentSchedule.Monthly)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddMonths(1))
                            {
                                double loanInterest = CalculateInterest(loanAccount.LoanAmount, loanConfig[0].debitInterestRate, 1);
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - loanInterest*(loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.Balance = loanAccount.Balance - loanInterest * (loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.DateAdded = loanAccount.DateAdded.AddMonths(1);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                CurrentConfig[0].currentAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, loanInterest);
                                loanConfig[0].InterestIncomeGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].InterestIncomeGlAccount, loanInterest);
                                GlPosting loancurrentGlPosting = new GlPosting();
                                loancurrentGlPosting.Amount = loanInterest;
                                loancurrentGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].InterestIncomeGlAccount.GlAccountName);
                                loancurrentGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loancurrentGlPosting.GlAccountToCredit = new GlAccount();
                                loancurrentGlPosting.GlAccountToCredit.Id = loanConfig[0].InterestIncomeGlAccount.Id;
                                loancurrentGlPosting.GlAccountToDebit = new GlAccount();
                                loancurrentGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                                loancurrentGlPosting.TransactionDate = eod[0].FinancialDate;
                                loancurrentGlPosting.DateAdded = DateTime.Now;
                                loancurrentGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(CurrentConfig[0].currentAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].InterestIncomeGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loancurrentGlPosting);
                            }
                        }
                    }
                }
            }
        }

        public double CalculateInterest(double Principal, int Rate, double Time)
        {
            double Interest = (Principal*Rate*Time)/(100*365);
            return Interest;
        }

        public void ReceivePrincipalPayment()
        {
            IList<CurrentConfig> CurrentConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().RetrieveAll();
            IList<SavingsConfig> SavingsConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().RetrieveAll();
            IList<LoanConfig> loanConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanConfigDb>().RetrieveAll();
            IList<LoanAccount> GetLoanProperties =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>().RetrieveAll();
            

            foreach (var loanAccount in GetLoanProperties)
            {
                IList<EOD> eod =
                       Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                if (loanAccount.LinkedAccount.AccountType == AccountType.Savings)
                {
                    if (loanAccount.LinkedAccount.IsClosed == false && loanAccount.LinkedAccount.Balance > 0)
                    {
                        if (loanAccount.PaymentSchedule == PaymentSchedule.Days)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays))
                            {
                                double PrincipalPerDay = loanAccount.LoanAmount / loanAccount.LoanDuration;
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - PrincipalPerDay*loanAccount.NumberOfDays;
                                loanAccount.Balance = loanAccount.Balance - PrincipalPerDay*loanAccount.NumberOfDays;
                                loanAccount.DateAdded = loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                SavingsConfig[0].SavingsAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(SavingsConfig[0].SavingsAccountGL, PrincipalPerDay);
                                loanConfig[0].LoanPrincipalGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, PrincipalPerDay);
                                GlPosting loansavingsGlPosting = new GlPosting();
                                loansavingsGlPosting.Amount = PrincipalPerDay;
                                loansavingsGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                loansavingsGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                                loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                                loansavingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                                loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                                loansavingsGlPosting.DateAdded = DateTime.Now;
                                loansavingsGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(SavingsConfig[0].SavingsAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loansavingsGlPosting);
                            }
                        }

                        if (loanAccount.PaymentSchedule == PaymentSchedule.Monthly)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddMonths(1))
                            {
                                double PrincipalPerDay = loanAccount.LoanAmount / loanAccount.LoanDuration;
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - PrincipalPerDay*(loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.Balance = loanAccount.Balance - PrincipalPerDay * (loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.DateAdded = loanAccount.DateAdded.AddMonths(1);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                SavingsConfig[0].SavingsAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(SavingsConfig[0].SavingsAccountGL, PrincipalPerDay);
                                loanConfig[0].LoanPrincipalGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, PrincipalPerDay);
                                GlPosting loansavingsGlPosting = new GlPosting();
                                loansavingsGlPosting.Amount = PrincipalPerDay;
                                loansavingsGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                loansavingsGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loansavingsGlPosting.GlAccountToCredit = new GlAccount();
                                loansavingsGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                loansavingsGlPosting.GlAccountToDebit = new GlAccount();
                                loansavingsGlPosting.GlAccountToDebit.Id = SavingsConfig[0].SavingsAccountGL.Id;
                                loansavingsGlPosting.TransactionDate = eod[0].FinancialDate;
                                loansavingsGlPosting.DateAdded = DateTime.Now;
                                loansavingsGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(SavingsConfig[0].SavingsAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loansavingsGlPosting);
                            }
                        }
                    }
                }

                else if (loanAccount.LinkedAccount.AccountType == AccountType.Current)
                {
                    if (loanAccount.LinkedAccount.IsClosed == false && loanAccount.LinkedAccount.Balance > 0)
                    {
                        if (loanAccount.PaymentSchedule == PaymentSchedule.Days)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays))
                            {
                                double PrincipalPerDay = loanAccount.LoanAmount / loanAccount.LoanDuration;
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - PrincipalPerDay * loanAccount.NumberOfDays;
                                loanAccount.Balance = loanAccount.Balance - PrincipalPerDay * loanAccount.NumberOfDays;
                                loanAccount.DateAdded = loanAccount.DateAdded.AddDays(loanAccount.NumberOfDays);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                CurrentConfig[0].currentAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, PrincipalPerDay);
                                loanConfig[0].LoanPrincipalGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, PrincipalPerDay);
                                GlPosting loancurrentGlPosting = new GlPosting();
                                loancurrentGlPosting.Amount = PrincipalPerDay;
                                loancurrentGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                loancurrentGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loancurrentGlPosting.GlAccountToCredit = new GlAccount();
                                loancurrentGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                loancurrentGlPosting.GlAccountToDebit = new GlAccount();
                                loancurrentGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                                loancurrentGlPosting.TransactionDate = eod[0].FinancialDate;
                                loancurrentGlPosting.DateAdded = DateTime.Now;
                                loancurrentGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(CurrentConfig[0].currentAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loancurrentGlPosting);
                            }
                        }

                        if (loanAccount.PaymentSchedule == PaymentSchedule.Monthly)
                        {
                            if (loanAccount.DateUpdated == loanAccount.DateAdded.AddMonths(1))
                            {
                                double PrincipalPerDay = loanAccount.LoanAmount / loanAccount.LoanDuration;
                                loanAccount.LinkedAccount.Balance = loanAccount.LinkedAccount.Balance - PrincipalPerDay * (loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.Balance = loanAccount.Balance - PrincipalPerDay * (loanAccount.DateUpdated.Day - loanAccount.DateAdded.Day);
                                loanAccount.DateAdded = loanAccount.DateAdded.AddMonths(1);
                                GlPostingLogic glPostingLogic = new GlPostingLogic();
                                CurrentConfig[0].currentAccountGL.Balance =
                                    glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, PrincipalPerDay);
                                loanConfig[0].LoanPrincipalGlAccount.Balance =
                                    glPostingLogic.CreditGlAccount(loanConfig[0].LoanPrincipalGlAccount, PrincipalPerDay);
                                GlPosting loancurrentGlPosting = new GlPosting();
                                loancurrentGlPosting.Amount = PrincipalPerDay;
                                loancurrentGlPosting.CreditNarration = String.Format("{0} Received Interest on Loan at EOD",
                                    loanConfig[0].LoanPrincipalGlAccount.GlAccountName);
                                loancurrentGlPosting.DebitNarration = String.Format("{0} Paid Interest on Loan at EOD",
                                    loanAccount.LinkedAccount.AccountName);
                                loancurrentGlPosting.GlAccountToCredit = new GlAccount();
                                loancurrentGlPosting.GlAccountToCredit.Id = loanConfig[0].LoanPrincipalGlAccount.Id;
                                loancurrentGlPosting.GlAccountToDebit = new GlAccount();
                                loancurrentGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                                loancurrentGlPosting.TransactionDate = eod[0].FinancialDate;
                                loancurrentGlPosting.DateAdded = DateTime.Now;
                                loancurrentGlPosting.DateUpdated = DateTime.Now;
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                                    .UpdateData(loanAccount.LinkedAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILoanAccountDb>()
                                    .UpdateData(loanAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(CurrentConfig[0].currentAccountGL);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                                    .UpdateData(loanConfig[0].LoanPrincipalGlAccount);
                                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                                    .InsertData(loancurrentGlPosting);
                            }
                        }
                        
                    }
                }

            }

            
        }

        public void GetCOT()
        {
            IList<CurrentConfig> CurrentConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICurrentConfigDb>().RetrieveAll();
            IList<SavingsConfig> SavingsConfig =
                Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ISavingsConfigDb>().RetrieveAll();
            IList<CustomerAccounts> GetCurrentAccountCoT =
                 Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>().RetrieveAll();

            foreach (var customeraccount in GetCurrentAccountCoT)
            {
                IList<EOD> eod =
                       Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
                if (customeraccount.AccountType == AccountType.Current)
                {
                    if (customeraccount.IsClosed == false)
                    {
                        customeraccount.Balance = customeraccount.Balance - customeraccount.CoTCharge;
                        GlPostingLogic glPostingLogic = new GlPostingLogic();
                        CurrentConfig[0].currentAccountGL.Balance =
                            glPostingLogic.DebitGlAccount(CurrentConfig[0].currentAccountGL, customeraccount.CoTCharge);
                        CurrentConfig[0].coTIncomeGl.Balance =
                            glPostingLogic.CreditGlAccount(CurrentConfig[0].coTIncomeGl, customeraccount.CoTCharge);
                        GlPosting currentGlPosting = new GlPosting();
                        currentGlPosting.Amount = customeraccount.CoTCharge;
                        currentGlPosting.CreditNarration = String.Format("{0} Received COT at EOD",
                            CurrentConfig[0].coTIncomeGl.GlAccountName);
                        currentGlPosting.DebitNarration = String.Format("{0} Paid COT at EOD",
                            CurrentConfig[0].currentAccountGL.GlAccountName);
                        currentGlPosting.GlAccountToCredit = new GlAccount();
                        currentGlPosting.GlAccountToCredit.Id = CurrentConfig[0].coTIncomeGl.Id;
                        currentGlPosting.GlAccountToDebit = new GlAccount();
                        currentGlPosting.GlAccountToDebit.Id = CurrentConfig[0].currentAccountGL.Id;
                        currentGlPosting.TransactionDate = eod[0].FinancialDate;
                        currentGlPosting.DateAdded = DateTime.Now;
                        currentGlPosting.DateUpdated = DateTime.Now;
                        customeraccount.CoTCharge = 0;
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerAccountsDb>()
                            .UpdateData(customeraccount);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(CurrentConfig[0].currentAccountGL);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>()
                            .UpdateData(CurrentConfig[0].coTIncomeGl);
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlPostingDb>()
                            .InsertData(currentGlPosting);
                    }
                }
            }


        }

    }
}
