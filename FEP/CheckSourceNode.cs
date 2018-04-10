using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;
using Switch.Core;
using Trx.Messaging.Iso8583;



namespace FEP
{
    public class CheckSourceNode
    {
        public void SourceNode(Iso8583Message message)
        {
            if (IsExpired(message) == true)
            {
                message.Fields.Add(39, "54"); //Invalid Transaction
                message.SetResponseMessageTypeIdentifier();
            }
            else if (IsCardNumberValid(message) == false)
            {
                message.Fields.Add(39, "14"); //Invalid Transaction
                message.SetResponseMessageTypeIdentifier();
            }
            else if (IsTransactionAmountValid(message) == false)
            {
                message.Fields.Add(39, "13"); //Invalid Transaction
                message.SetResponseMessageTypeIdentifier();
            }
            else if (IsAccountNumberValid(message) == false)
            {
                message.Fields.Add(39, "42");
            }
            else if (IsInterBankPayment(message))
            {
                if (IsInterBankMakePayment(message))
                {
                    PerformInterBankMakePayment(message);
                }
                else if (IsInterBankReceivePayment(message))
                {
                    PerformInterBankReceivePayment(message);
                }
            }
            else if (IsTerminal(message) == false)
            {
                message.Fields.Add(39, "56");
                //Code to redirect wrong card PAN to switch here
            }
            else if (IsTerminal(message))
            {
                if (message.MessageTypeIdentifier == 200)
                {
                    PerformTransactionOnATMRequest(message);
                }
                if (message.MessageTypeIdentifier == 421)
                {
                    if (ISReversed(message) == false)
                    {
                        PerformReversal(message);
                    }
                    if (IsreversalODEinDatabase(message) == false)
                    {
                        message.Fields.Add(39, "25");
                    }
                    else if (ISReversed(message))
                    {
                        message.Fields.Add(39, "21");
                    }
                }
            }
            

        }

        public bool IsExpired(Iso8583Message message)
        {
            int expirydate = int.Parse(message.Fields[14].ToString());
            int today = int.Parse(DateTime.Today.ToString("yyMM"));

            if (today < expirydate)
            {
                return false;
            }
            return true;
        }

        public bool IsCardNumberValid(Iso8583Message message)
        {
            string cardnumber = message.Fields[2].ToString();
            if (cardnumber.Length == 16)
            {
                return true;
            }
            return false;
        }

        public bool IsTransactionAmountValid(Iso8583Message message)
        {
            string amount = message.Fields[4].ToString();
            int amounts = int.Parse(amount);
            if (amount.Length > 12 || amounts < 0)
            {
                return false;
            }
            return true;
        }

        public bool IsTerminal(Iso8583Message MessageWithSourceNodeDetails)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string Terminal = MessageWithSourceNodeDetails.Fields[41].ToString().Substring(0, 1);
            if (apIinstantiator.IsMyTerminal(Terminal))
            {
                return true;
            }
            return false;
        }
        public bool IsAccountNumberValid(Iso8583Message message)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string accountnumber = message.Fields[102].ToString();
            if (apIinstantiator.GetCustomerAccountsByCategory(accountnumber) == null)
            {
                return false;
            }
            return true;
        }

        public bool IsInterBankPayment(Iso8583Message message)
        {
            string cardPAN = message.Fields[2].ToString().Substring(0, 6);
            string ReceivingInstitutionCardPan = message.Fields[100].ToString();
            string PaymentTransactnCode = message.Fields[3].ToString().Substring(0, 2);
            if (PaymentTransactnCode == "50" && cardPAN != ReceivingInstitutionCardPan)
            {
                return true;
            }
            return false;
        }

        public bool IsInterBankMakePayment(Iso8583Message message)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string acctNumber1 = message.Fields[102].ToString();

            CustomerAccounts debitCustomerAccounts = apIinstantiator.GetCustomerAccountsByCategory(acctNumber1);

            if (debitCustomerAccounts != null)
            {
                return true;
            }
            return false;
        }

        public bool IsInterBankReceivePayment(Iso8583Message message)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string acctNumber2 = message.Fields[103].ToString();
            CustomerAccounts creditCustomerAccounts = apIinstantiator.GetCustomerAccountsByCategory(acctNumber2);
            if (creditCustomerAccounts != null)
            {
                return true;
            }
            return false;
        }
        //public bool ConfirmTransactionChannelAndCardPanInRoute(SourceNode sourceNode,
        //    Iso8583Message MessageWithSourceNodeDetails) //out Route route
        //{
            
        //    string channelMessage = MessageWithSourceNodeDetails.Fields[41].ToString().Substring(0, 1);
        //    string cardPAN = MessageWithSourceNodeDetails.Fields[2].ToString().Substring(0, 6);

            
        //    string ChannelID = ConfigurationSettings.AppSettings["ATMChannelID"];
        //    string RouteCardPan = ConfigurationSettings.AppSettings["CardPAN"];

        //    //if(ChannelID == channelMessage && cardPAN ==)

            
        //            //if (typeChannelFeeCombo.Channel.Code.Substring(0, 1) == channelMessage) //Channel will always be correct, so the 'else' only caters for wwong route
        //            if (ChannelID == channelMessage) 
        //            {
        //                //Route route = scheme.Route;

        //                if (RouteCardPan == cardPAN)
        //                {
                            
        //                    return true;
        //                }

        //            }
        //            else
        //            {

        //                return false;
        //            }
                

            

        //    return false;
        //}

        public bool IsreversalODEinDatabase(Iso8583Message message)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string originalDataElement = message.Fields[90].ToString().Substring(22);
            IList<GlPosting> getPostingsbyODE = apIinstantiator.GetByOriginalDataElement(originalDataElement);
            if (getPostingsbyODE == null)
            {
                return false;
            }
            return true;
        }

        public bool ISReversed(Iso8583Message message)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string originalDataElement = message.Fields[90].ToString().Substring(22);
            IList<GlPosting> getPostingsbyODE = apIinstantiator.GetByOriginalDataElement(originalDataElement);
            if (IsreversalODEinDatabase(message))
            {
                foreach (var glPosting in getPostingsbyODE)
                {
                    if (glPosting.IsReversed == false)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public void PerformReversal(Iso8583Message message)
        {
            APIinstantiator apIinstantiator = new APIinstantiator();
            string originalDataElement = message.Fields[90].ToString().Substring(22);
            //string CustomerAccountNumber = ConfigurationSettings.AppSettings["CustomerLinkedAccount"];
            string ATMAccountCode = ConfigurationSettings.AppSettings["OnUsATMLinkedAccount"];
            string ATMIncomeAccountCode = ConfigurationSettings.AppSettings["ATMIncomeAccount"];

            IList<GlPosting> getPostingsbyODE = apIinstantiator.GetByOriginalDataElement(originalDataElement);

            string [] glNarrationAcctNumPart = getPostingsbyODE[0].CreditNarration.Split(new char[] {'&'}, 2);

            string CustomerAccountNumber = glNarrationAcctNumPart[1].Substring(0, 10);

            CustomerAccounts customerAccounts = apIinstantiator.GetCustomerAccountsByCategory(CustomerAccountNumber);

            IList<CurrentConfig> currentConfig = apIinstantiator.GetAllCurrentConfigs();

            IList<SavingsConfig> savingsConfig = apIinstantiator.GetAllSavingsConfigsConfigs();

            //Code to update balance in savings account GL
            var updateSavingsGlBalance = apIinstantiator.GetBySavingsConfigBranch(savingsConfig[0].Branch.Id);


            //Put Code to update balance in current account GL here:
            var updateCurrentGlBalance = apIinstantiator.GetByCurrentConfigBranch(currentConfig[0].Branch.Id);
            

            GlAccount OnUsAccount = apIinstantiator.GetByGlAccountCode(ATMAccountCode);

            //Account to use When Tramsaction Fee is applied
            GlAccount ATMIncomeAccount = apIinstantiator.GetByGlAccountCode(ATMIncomeAccountCode);



            IList<EOD> eod = apIinstantiator.GetAllEods();

            foreach (var glPosting in getPostingsbyODE)
            {
                customerAccounts.Balance = customerAccounts.Balance + glPosting.Amount;
                if (glPosting.GlAccountToDebit.Id == updateSavingsGlBalance.SavingsAccountGL.Id)
                {
                    updateSavingsGlBalance.SavingsAccountGL.Balance = updateSavingsGlBalance.SavingsAccountGL.Balance +
                                                                      glPosting.Amount;
                }
                if (glPosting.GlAccountToDebit.Id == updateCurrentGlBalance.currentAccountGL.Id)
                {
                    updateCurrentGlBalance.currentAccountGL.Balance = updateCurrentGlBalance.currentAccountGL.Balance +
                                                                      glPosting.Amount;
                }
                if (glPosting.GlAccountToCredit.Id == OnUsAccount.Id)
                {
                    OnUsAccount.Balance = OnUsAccount.Balance +glPosting.Amount;
                }
                if (glPosting.GlAccountToCredit.Id == ATMIncomeAccount.Id)
                {
                    ATMIncomeAccount.Balance = ATMIncomeAccount.Balance + glPosting.Amount;
                }
            }

            if (getPostingsbyODE[0].GlAccountToDebit.Id == updateSavingsGlBalance.SavingsAccountGL.Id)
            {
                apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + OnUsAccount.Id, OnUsAccount);
                apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + ATMIncomeAccount.Id, ATMIncomeAccount);
                apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateSavingsGlBalance.SavingsAccountGL.Id, updateSavingsGlBalance.SavingsAccountGL);
            }
            if (getPostingsbyODE[0].GlAccountToDebit.Id == updateCurrentGlBalance.currentAccountGL.Id)
            {
                apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + OnUsAccount.Id, OnUsAccount);
                apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + ATMIncomeAccount.Id, ATMIncomeAccount);
                apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateCurrentGlBalance.currentAccountGL.Id, updateCurrentGlBalance.currentAccountGL);
            }
            string reversedODE = originalDataElement.
                Replace("0200", "0421");
            string accountbalance = string.Concat(customerAccounts.Balance);

            if (getPostingsbyODE[0].GlAccountToDebit.Id == updateSavingsGlBalance.SavingsAccountGL.Id)
            {
                GlPosting reversedsavingswithdrawalGlPosting = new GlPosting();
                reversedsavingswithdrawalGlPosting.Amount = getPostingsbyODE[0].Amount;
                reversedsavingswithdrawalGlPosting.CreditNarration = String.Format(reversedODE + "Withdrawal From {0}",
                   updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                reversedsavingswithdrawalGlPosting.DebitNarration = String.Format(reversedODE + "Withdrawal From {0}",
                    OnUsAccount.GlAccountName);
                reversedsavingswithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                reversedsavingswithdrawalGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                reversedsavingswithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                reversedsavingswithdrawalGlPosting.GlAccountToDebit.Id = OnUsAccount.Id;
                reversedsavingswithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                reversedsavingswithdrawalGlPosting.IsReversed = true;
                reversedsavingswithdrawalGlPosting.DateAdded = DateTime.Now;
                reversedsavingswithdrawalGlPosting.DateUpdated = DateTime.Now;

                GlPosting reversedFeeGlPosting = new GlPosting();
                reversedFeeGlPosting.Amount = getPostingsbyODE[1].Amount;
                reversedFeeGlPosting.CreditNarration = String.Format(reversedODE + "Fee Credited To {0}",
                    updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                reversedFeeGlPosting.DebitNarration = String.Format(reversedODE + "Fee Deducted From {0}",
                    ATMIncomeAccount.GlAccountName);
                reversedFeeGlPosting.GlAccountToCredit = new GlAccount();
                reversedFeeGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                reversedFeeGlPosting.GlAccountToDebit = new GlAccount();
                reversedFeeGlPosting.GlAccountToDebit.Id = ATMIncomeAccount.Id;
                reversedFeeGlPosting.TransactionDate = eod[0].FinancialDate;
                reversedFeeGlPosting.IsReversed = true;
                reversedFeeGlPosting.DateAdded = DateTime.Now;
                reversedFeeGlPosting.DateUpdated = DateTime.Now;

                apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", reversedsavingswithdrawalGlPosting);
                apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", reversedFeeGlPosting);
                message.Fields.Add(54, accountbalance);
                message.Fields.Add(39, "99");
            }

            else if (getPostingsbyODE[0].GlAccountToDebit.Id == updateCurrentGlBalance.currentAccountGL.Id)
            {
                GlPosting reversedcurrentwithdrawalGlPosting = new GlPosting();
                reversedcurrentwithdrawalGlPosting.Amount = getPostingsbyODE[0].Amount;
                reversedcurrentwithdrawalGlPosting.CreditNarration = String.Format(reversedODE + "Withdrawal From {0} ",
                   updateCurrentGlBalance.currentAccountGL.GlAccountName);
                reversedcurrentwithdrawalGlPosting.DebitNarration = String.Format(reversedODE + "Withdrawal From {0}",
                    OnUsAccount.GlAccountName);
                reversedcurrentwithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                reversedcurrentwithdrawalGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                reversedcurrentwithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                reversedcurrentwithdrawalGlPosting.GlAccountToDebit.Id = OnUsAccount.Id;
                reversedcurrentwithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                reversedcurrentwithdrawalGlPosting.IsReversed = true;
                reversedcurrentwithdrawalGlPosting.DateAdded = DateTime.Now;
                reversedcurrentwithdrawalGlPosting.DateUpdated = DateTime.Now;

                GlPosting reversedFeeGlPosting = new GlPosting();
                reversedFeeGlPosting.Amount = getPostingsbyODE[1].Amount;
                reversedFeeGlPosting.CreditNarration = String.Format(reversedODE + "Fee Credited To {0}",
                    updateCurrentGlBalance.currentAccountGL.GlAccountName);
                reversedFeeGlPosting.DebitNarration = String.Format(reversedODE + "Fee Deducted From {0}",
                    ATMIncomeAccount.GlAccountName);
                reversedFeeGlPosting.GlAccountToCredit = new GlAccount();
                reversedFeeGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                reversedFeeGlPosting.GlAccountToDebit = new GlAccount();
                reversedFeeGlPosting.GlAccountToDebit.Id = ATMIncomeAccount.Id;
                reversedFeeGlPosting.TransactionDate = eod[0].FinancialDate;
                reversedFeeGlPosting.IsReversed = true;
                reversedFeeGlPosting.DateAdded = DateTime.Now;
                reversedFeeGlPosting.DateUpdated = DateTime.Now;

                apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", reversedcurrentwithdrawalGlPosting);
                apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", reversedFeeGlPosting);
                message.Fields.Add(54, accountbalance);
                message.Fields.Add(39, "99");
            }

            foreach (var updateIsReversed in getPostingsbyODE)
            {
                updateIsReversed.IsReversed = true;
                apIinstantiator.UpdatePostings("api/Glposting/PutGlPostings/?glpostingsid=" + updateIsReversed.Id, updateIsReversed);
            }

            

       
            

        }

        public void PerformTransactionOnATMRequest(Iso8583Message message)
        {
            string transactionMessage = message.Fields[3].ToString().Substring(0, 2);
            string originalDataElement = message.Fields[90].ToString().Substring(22);
            double Fee = double.Parse(message.Fields[28].ToString());
            string Withdrawal = ConfigurationSettings.AppSettings["CashWithdrawal"];
            string BalanceEnquiry = ConfigurationSettings.AppSettings["BalanceEnquiry"];
            //string CustomerAccountNumber = ConfigurationSettings.AppSettings["CustomerLinkedAccount"];
            string CustomerAccountNumber = message.Fields[102].ToString();
            string ATMAccountCode = ConfigurationSettings.AppSettings["OnUsATMLinkedAccount"];
            string ATMIncomeAccountCode = ConfigurationSettings.AppSettings["ATMIncomeAccount"];
            int amount = int.Parse(message.Fields[4].ToString())/100;

            if (transactionMessage == Withdrawal)
            {
                APIinstantiator apIinstantiator = new APIinstantiator();

                CustomerAccounts customerAccounts = apIinstantiator.GetCustomerAccountsByCategory(CustomerAccountNumber);
                
                IList<CurrentConfig> currentConfig = apIinstantiator.GetAllCurrentConfigs();

                IList<SavingsConfig> savingsConfig = apIinstantiator.GetAllSavingsConfigsConfigs();
                
                //Code to update balance in savings account GL
                var updateSavingsGlBalance = apIinstantiator.GetBySavingsConfigBranch(savingsConfig[0].Branch.Id);
                

                //Put Code to update balance in current account GL here:
                var updateCurrentGlBalance = apIinstantiator.GetByCurrentConfigBranch(currentConfig[0].Branch.Id);
                
                
                GlAccount OnUsAccount = apIinstantiator.GetByGlAccountCode(ATMAccountCode);
                
                //Account to use When Tramsaction Fee is applied
                GlAccount ATMIncomeAccount = apIinstantiator.GetByGlAccountCode(ATMIncomeAccountCode);

               
                
                IList<EOD> eod = apIinstantiator.GetAllEods();

                
                if (customerAccounts.AccountType == AccountType.Savings)
                {
                    try
                    {
                        if (
                        apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,
                            amount) == false)
                        {
                            message.Fields.Add(39, "51");
                        }
                        
                        else if (apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,amount) &&
                            apIinstantiator.GLAccountBalanceConfirmed(OnUsAccount.Id,
                                amount))
                        {
                            //Apply Transaction amount on appropriate accounts
                            customerAccounts.Balance = apIinstantiator.CustomerAccountWithdrawal(customerAccounts.Id,
                                 amount);
                            OnUsAccount.Balance = apIinstantiator.GlAccountWithdrawal(OnUsAccount.Id,
                                  amount);
                            updateSavingsGlBalance.SavingsAccountGL.Balance =
                        apIinstantiator.GlAccountWithdrawal(updateSavingsGlBalance.SavingsAccountGL.Id,
                           amount);

                            //Apply Transaction fee on appropriate accounts
                            customerAccounts.Balance = customerAccounts.Balance - Fee;
                            updateSavingsGlBalance.SavingsAccountGL.Balance =
                                updateSavingsGlBalance.SavingsAccountGL.Balance - Fee;
                            ATMIncomeAccount.Balance = ATMIncomeAccount.Balance + Fee;
                            
                            //customerAccountsforFee.Balance = apIinstantiator.CustomerAccountWithdrawal(customerAccountsforFee.Id,
                            //    Fee);
                            //ATMIncomeAccount.Balance = apIinstantiator.GlAccountWithdrawal(ATMIncomeAccount.Id,
                            //      Fee);
                          

                            //GL Posting for savings withrawal below:
                          GlPosting savingswithdrawalGlPosting = new GlPosting();
                          savingswithdrawalGlPosting.Amount = amount;
                          savingswithdrawalGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                              OnUsAccount.GlAccountName);
                          savingswithdrawalGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                              updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                          savingswithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                          savingswithdrawalGlPosting.GlAccountToCredit.Id = OnUsAccount.Id;
                          savingswithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                          savingswithdrawalGlPosting.GlAccountToDebit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                          savingswithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                            savingswithdrawalGlPosting.IsReversed = false;
                          savingswithdrawalGlPosting.DateAdded = DateTime.Now;
                          savingswithdrawalGlPosting.DateUpdated = DateTime.Now;

                          GlPosting FeeGlPosting = new GlPosting();
                          FeeGlPosting.Amount = Fee;
                          FeeGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Credited To {0}",
                              ATMIncomeAccount.GlAccountName);
                          FeeGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Deducted From {0}",
                              updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                          FeeGlPosting.GlAccountToCredit = new GlAccount();
                          FeeGlPosting.GlAccountToCredit.Id = ATMIncomeAccount.Id;
                          FeeGlPosting.GlAccountToDebit = new GlAccount();
                          FeeGlPosting.GlAccountToDebit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                          FeeGlPosting.TransactionDate = eod[0].FinancialDate;
                            FeeGlPosting.IsReversed = false;
                          FeeGlPosting.DateAdded = DateTime.Now;
                          FeeGlPosting.DateUpdated = DateTime.Now;

                          string accountbalance = string.Concat(customerAccounts.Balance);
                          

                            
                          apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                          apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + OnUsAccount.Id, OnUsAccount);
                          apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + ATMIncomeAccount.Id, ATMIncomeAccount);
                          apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateSavingsGlBalance.SavingsAccountGL.Id, updateSavingsGlBalance.SavingsAccountGL);
                            apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", savingswithdrawalGlPosting);
                            apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", FeeGlPosting);
                            message.Fields.Add(54, accountbalance);
                            message.Fields.Add(39, "00");
                        }
                    }
                    catch (Exception)
                    {
                        if (
                       apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,
                           amount) == false)
                        {
                            message.Fields.Add(39, "51");
                        }
                    }


                }


                else if (customerAccounts.AccountType == AccountType.Current)
                {
                    try
                    {
                        if (
                        apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id, 
                            amount) == false)
                        {
                            message.Fields.Add(39, "51");
                        }
                        
                        else if (apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                                amount) &&
                            apIinstantiator.GLAccountBalanceConfirmed(OnUsAccount.Id,
                                amount))
                        {
                            customerAccounts.Balance = apIinstantiator.CustomerAccountWithdrawal(customerAccounts.Id,
                                amount);
                            OnUsAccount.Balance = apIinstantiator.GlAccountWithdrawal(OnUsAccount.Id,
                                amount);
                            updateCurrentGlBalance.currentAccountGL.Balance =
                        apIinstantiator.GlAccountWithdrawal(updateCurrentGlBalance.currentAccountGL.Id,
                            amount);
                            //Apply Transaction fee on appropriate accounts
                            customerAccounts.Balance = customerAccounts.Balance - Fee;
                            updateCurrentGlBalance.currentAccountGL.Balance =
                               updateCurrentGlBalance.currentAccountGL.Balance - Fee;
                            ATMIncomeAccount.Balance = ATMIncomeAccount.Balance + Fee;

                            //Calculate COT for current A/C withdrawal
                            int chargeOnCoT = Convert.ToInt32(currentConfig[0].coT * amount) / 1000;
                            customerAccounts.CoTCharge = chargeOnCoT + customerAccounts.CoTCharge;
                            

                            //GL Posting for savings withrawal below:
                            GlPosting currentwithdrawalGlPosting = new GlPosting();
                            currentwithdrawalGlPosting.Amount = amount;
                            currentwithdrawalGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0} ",
                                OnUsAccount.GlAccountName);
                            currentwithdrawalGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                                updateCurrentGlBalance.currentAccountGL.GlAccountName);
                            currentwithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                            currentwithdrawalGlPosting.GlAccountToCredit.Id = OnUsAccount.Id;
                            currentwithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                            currentwithdrawalGlPosting.GlAccountToDebit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                            currentwithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                            currentwithdrawalGlPosting.IsReversed = false;
                            currentwithdrawalGlPosting.DateAdded = DateTime.Now;
                            currentwithdrawalGlPosting.DateUpdated = DateTime.Now;

                            GlPosting FeeGlPosting = new GlPosting();
                            FeeGlPosting.Amount = Fee;
                            FeeGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Credited To {0}",
                                ATMIncomeAccount.GlAccountName);
                            FeeGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Deducted From {0}",
                                updateCurrentGlBalance.currentAccountGL.GlAccountName);
                            FeeGlPosting.GlAccountToCredit = new GlAccount();
                            FeeGlPosting.GlAccountToCredit.Id = ATMIncomeAccount.Id;
                            FeeGlPosting.GlAccountToDebit = new GlAccount();
                            FeeGlPosting.GlAccountToDebit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                            FeeGlPosting.TransactionDate = eod[0].FinancialDate;
                            FeeGlPosting.IsReversed = false;
                            FeeGlPosting.DateAdded = DateTime.Now;
                            FeeGlPosting.DateUpdated = DateTime.Now;

                            string accountbalance = string.Concat(customerAccounts.Balance);

                            
                            apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                            apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + OnUsAccount.Id, OnUsAccount);
                            apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + ATMIncomeAccount.Id, ATMIncomeAccount);
                            apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateCurrentGlBalance.currentAccountGL.Id, updateCurrentGlBalance.currentAccountGL);
                            apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", currentwithdrawalGlPosting);
                            apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", FeeGlPosting);
                            message.Fields.Add(54, accountbalance);
                            message.Fields.Add(39, "00");
                        }
                    }
                    catch (Exception)
                    {
                        if (
                        apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                            amount) == false)
                        {
                            message.Fields.Add(39, "51");
                        }
                    }

                }

            }

            else if (transactionMessage == BalanceEnquiry)
            {
                APIinstantiator apIinstantiator = new APIinstantiator();
                CustomerAccounts customerAccounts = apIinstantiator.GetCustomerAccountsByCategory(CustomerAccountNumber);
                string accountbalance = string.Concat(customerAccounts.Balance);
                message.Fields.Add(54, accountbalance);
                message.Fields.Add(39, "00");
                
            }
        }

        public void PerformInterBankMakePayment(Iso8583Message message)
        {
            string originalDataElement = message.Fields[90].ToString().Substring(22);
            double Fee = double.Parse(message.Fields[28].ToString());
            string Withdrawal = ConfigurationSettings.AppSettings["CashWithdrawal"];
            string BalanceEnquiry = ConfigurationSettings.AppSettings["BalanceEnquiry"];
            //string CustomerAccountNumber = ConfigurationSettings.AppSettings["CustomerLinkedAccount"];
            string CustomerAccountNumber = message.Fields[102].ToString();
            string ATMAccountCode = ConfigurationSettings.AppSettings["TSSLinkedAccount"];
            string ATMIncomeAccountCode = ConfigurationSettings.AppSettings["ATMIncomeAccount"];
            int amount = int.Parse(message.Fields[4].ToString()) / 100;


            APIinstantiator apIinstantiator = new APIinstantiator();

            CustomerAccounts customerAccounts = apIinstantiator.GetCustomerAccountsByCategory(CustomerAccountNumber);

            IList<CurrentConfig> currentConfig = apIinstantiator.GetAllCurrentConfigs();

            IList<SavingsConfig> savingsConfig = apIinstantiator.GetAllSavingsConfigsConfigs();

            //Code to update balance in savings account GL
            var updateSavingsGlBalance = apIinstantiator.GetBySavingsConfigBranch(savingsConfig[0].Branch.Id);


            //Put Code to update balance in current account GL here:
            var updateCurrentGlBalance = apIinstantiator.GetByCurrentConfigBranch(currentConfig[0].Branch.Id);


            GlAccount TSSAccount = apIinstantiator.GetByGlAccountCode(ATMAccountCode);

            //Account to use When Tramsaction Fee is applied
            //GlAccount ATMIncomeAccount = apIinstantiator.GetByGlAccountCode(ATMIncomeAccountCode);



            IList<EOD> eod = apIinstantiator.GetAllEods();


            if (customerAccounts.AccountType == AccountType.Savings)
            {
                try
                {
                    if (
                    apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,
                        amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }

                    else if (apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id, amount))
                    {
                        //Apply Transaction amount on appropriate accounts
                        customerAccounts.Balance = apIinstantiator.CustomerAccountWithdrawal(customerAccounts.Id,
                             amount);
                        TSSAccount.Balance = apIinstantiator.GlAccountWithdrawal(TSSAccount.Id,
                              amount);
                        updateSavingsGlBalance.SavingsAccountGL.Balance =
                            updateSavingsGlBalance.SavingsAccountGL.Balance - amount;


                        //Apply Transaction fee on appropriate accounts
                        customerAccounts.Balance = customerAccounts.Balance - Fee;
                        updateSavingsGlBalance.SavingsAccountGL.Balance =
                            updateSavingsGlBalance.SavingsAccountGL.Balance - Fee;
                        TSSAccount.Balance = TSSAccount.Balance + Fee;

                        //customerAccountsforFee.Balance = apIinstantiator.CustomerAccountWithdrawal(customerAccountsforFee.Id,
                        //    Fee);
                        //ATMIncomeAccount.Balance = apIinstantiator.GlAccountWithdrawal(ATMIncomeAccount.Id,
                        //      Fee);


                        //GL Posting for savings withrawal below:
                        GlPosting savingswithdrawalGlPosting = new GlPosting();
                        savingswithdrawalGlPosting.Amount = amount;
                        savingswithdrawalGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                            TSSAccount.GlAccountName);
                        savingswithdrawalGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                            updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                        savingswithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                        savingswithdrawalGlPosting.GlAccountToCredit.Id = TSSAccount.Id;
                        savingswithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                        savingswithdrawalGlPosting.GlAccountToDebit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                        savingswithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                        savingswithdrawalGlPosting.IsReversed = false;
                        savingswithdrawalGlPosting.DateAdded = DateTime.Now;
                        savingswithdrawalGlPosting.DateUpdated = DateTime.Now;

                        GlPosting FeeGlPosting = new GlPosting();
                        FeeGlPosting.Amount = Fee;
                        FeeGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Credited To {0}",
                            TSSAccount.GlAccountName);
                        FeeGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Deducted From {0}",
                            updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                        FeeGlPosting.GlAccountToCredit = new GlAccount();
                        FeeGlPosting.GlAccountToCredit.Id = TSSAccount.Id;
                        FeeGlPosting.GlAccountToDebit = new GlAccount();
                        FeeGlPosting.GlAccountToDebit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                        FeeGlPosting.TransactionDate = eod[0].FinancialDate;
                        FeeGlPosting.IsReversed = false;
                        FeeGlPosting.DateAdded = DateTime.Now;
                        FeeGlPosting.DateUpdated = DateTime.Now;

                        string accountbalance = string.Concat(customerAccounts.Balance);



                        apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + TSSAccount.Id, TSSAccount);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateSavingsGlBalance.SavingsAccountGL.Id, updateSavingsGlBalance.SavingsAccountGL);
                        apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", savingswithdrawalGlPosting);
                        apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", FeeGlPosting);
                        message.Fields.Add(54, accountbalance);
                        message.Fields.Add(39, "00");
                        //Modified Field to Differentiate Make Payment From Recieve Payment in switch
                        message.Fields.Add(78, "1");
                    }
                }
                catch (Exception)
                {
                    if (
                   apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,
                       amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }
                }


            }


            else if (customerAccounts.AccountType == AccountType.Current)
            {
                try
                {
                    if (
                    apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                        amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }

                    else if (apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                            amount))
                    {
                        customerAccounts.Balance = apIinstantiator.CustomerAccountWithdrawal(customerAccounts.Id,
                            amount);
                        TSSAccount.Balance = apIinstantiator.GlAccountWithdrawal(TSSAccount.Id,
                            amount);
                        updateCurrentGlBalance.currentAccountGL.Balance =
                            updateCurrentGlBalance.currentAccountGL.Balance - amount;

                        //Apply Transaction fee on appropriate accounts
                        customerAccounts.Balance = customerAccounts.Balance - Fee;
                        updateCurrentGlBalance.currentAccountGL.Balance =
                           updateCurrentGlBalance.currentAccountGL.Balance - Fee;
                        TSSAccount.Balance = TSSAccount.Balance + Fee;

                        //Calculate COT for current A/C withdrawal
                        int chargeOnCoT = Convert.ToInt32(currentConfig[0].coT * amount) / 1000;
                        customerAccounts.CoTCharge = chargeOnCoT + customerAccounts.CoTCharge;


                        //GL Posting for savings withrawal below:
                        GlPosting currentwithdrawalGlPosting = new GlPosting();
                        currentwithdrawalGlPosting.Amount = amount;
                        currentwithdrawalGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0} ",
                            TSSAccount.GlAccountName);
                        currentwithdrawalGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                            updateCurrentGlBalance.currentAccountGL.GlAccountName);
                        currentwithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                        currentwithdrawalGlPosting.GlAccountToCredit.Id = TSSAccount.Id;
                        currentwithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                        currentwithdrawalGlPosting.GlAccountToDebit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                        currentwithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                        currentwithdrawalGlPosting.IsReversed = false;
                        currentwithdrawalGlPosting.DateAdded = DateTime.Now;
                        currentwithdrawalGlPosting.DateUpdated = DateTime.Now;

                        GlPosting FeeGlPosting = new GlPosting();
                        FeeGlPosting.Amount = Fee;
                        FeeGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Credited To {0}",
                            TSSAccount.GlAccountName);
                        FeeGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Fee Deducted From {0}",
                            updateCurrentGlBalance.currentAccountGL.GlAccountName);
                        FeeGlPosting.GlAccountToCredit = new GlAccount();
                        FeeGlPosting.GlAccountToCredit.Id = TSSAccount.Id;
                        FeeGlPosting.GlAccountToDebit = new GlAccount();
                        FeeGlPosting.GlAccountToDebit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                        FeeGlPosting.TransactionDate = eod[0].FinancialDate;
                        FeeGlPosting.IsReversed = false;
                        FeeGlPosting.DateAdded = DateTime.Now;
                        FeeGlPosting.DateUpdated = DateTime.Now;

                        string accountbalance = string.Concat(customerAccounts.Balance);


                        apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + TSSAccount.Id, TSSAccount);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateCurrentGlBalance.currentAccountGL.Id, updateCurrentGlBalance.currentAccountGL);
                        apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", currentwithdrawalGlPosting);
                        apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", FeeGlPosting);
                        message.Fields.Add(54, accountbalance);
                        message.Fields.Add(39, "00");
                        //Modified Field to Differentiate Make Payment From Recieve Payment in switch
                        message.Fields.Add(78, "1");
                    }
                }
                catch (Exception)
                {
                    if (
                    apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                        amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }
                }

            }
        }

        public void PerformInterBankReceivePayment(Iso8583Message message)
        {
            string originalDataElement = message.Fields[90].ToString().Substring(22);
            //double Fee = double.Parse(message.Fields[28].ToString());
            //string CustomerAccountNumber = ConfigurationSettings.AppSettings["CustomerLinkedAccount"];
            string CustomerAccountNumber = message.Fields[103].ToString();
            string ATMAccountCode = ConfigurationSettings.AppSettings["TSSLinkedAccount"];
            int amount = int.Parse(message.Fields[4].ToString()) / 100;


            APIinstantiator apIinstantiator = new APIinstantiator();

            CustomerAccounts customerAccounts = apIinstantiator.GetCustomerAccountsByCategory(CustomerAccountNumber);

            IList<CurrentConfig> currentConfig = apIinstantiator.GetAllCurrentConfigs();

            IList<SavingsConfig> savingsConfig = apIinstantiator.GetAllSavingsConfigsConfigs();

            //Code to update balance in savings account GL
            var updateSavingsGlBalance = apIinstantiator.GetBySavingsConfigBranch(savingsConfig[0].Branch.Id);


            //Put Code to update balance in current account GL here:
            var updateCurrentGlBalance = apIinstantiator.GetByCurrentConfigBranch(currentConfig[0].Branch.Id);


            GlAccount TSSAccount = apIinstantiator.GetByGlAccountCode(ATMAccountCode);

            //Account to use When Tramsaction Fee is applied
            //GlAccount ATMIncomeAccount = apIinstantiator.GetByGlAccountCode(ATMIncomeAccountCode);



            IList<EOD> eod = apIinstantiator.GetAllEods();


            if (customerAccounts.AccountType == AccountType.Savings)
            {
                try
                {
                    if (
                    apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,
                        amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }

                    else if (apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id, amount))
                    {
                        //Apply Transaction amount on appropriate accounts
                        customerAccounts.Balance = customerAccounts.Balance + amount;
                        TSSAccount.Balance = TSSAccount.Balance + amount;
                        TSSAccount.Balance = TSSAccount.Balance - amount;
                        updateSavingsGlBalance.SavingsAccountGL.Balance =
                            updateSavingsGlBalance.SavingsAccountGL.Balance + amount;




                        //GL Posting for savings withrawal below:
                        GlPosting savingswithdrawalGlPosting = new GlPosting();
                        savingswithdrawalGlPosting.Amount = amount;
                        savingswithdrawalGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                             updateSavingsGlBalance.SavingsAccountGL.GlAccountName);
                        savingswithdrawalGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                            TSSAccount.GlAccountName);
                        savingswithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                        savingswithdrawalGlPosting.GlAccountToCredit.Id = updateSavingsGlBalance.SavingsAccountGL.Id;
                        savingswithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                        savingswithdrawalGlPosting.GlAccountToDebit.Id = TSSAccount.Id;
                        savingswithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                        savingswithdrawalGlPosting.IsReversed = false;
                        savingswithdrawalGlPosting.DateAdded = DateTime.Now;
                        savingswithdrawalGlPosting.DateUpdated = DateTime.Now;


                        string accountbalance = string.Concat(customerAccounts.Balance);



                        apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + TSSAccount.Id, TSSAccount);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateSavingsGlBalance.SavingsAccountGL.Id, updateSavingsGlBalance.SavingsAccountGL);
                        apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", savingswithdrawalGlPosting);
                        message.Fields.Add(54, accountbalance);
                        message.Fields.Add(39, "00");
                    }
                }
                catch (Exception)
                {
                    if (
                   apIinstantiator.CustomerSavingsAccountBalanceConfirmed(customerAccounts.Id,
                       amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }
                }


            }


            else if (customerAccounts.AccountType == AccountType.Current)
            {
                try
                {
                    if (
                    apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                        amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }

                    else if (apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                            amount))
                    {
                        customerAccounts.Balance = customerAccounts.Balance + amount;
                        TSSAccount.Balance = TSSAccount.Balance + amount;
                        TSSAccount.Balance = TSSAccount.Balance - amount;
                        updateCurrentGlBalance.currentAccountGL.Balance =
                            updateCurrentGlBalance.currentAccountGL.Balance + amount;


                        //Calculate COT for current A/C withdrawal
                        int chargeOnCoT = Convert.ToInt32(currentConfig[0].coT * amount) / 1000;
                        customerAccounts.CoTCharge = chargeOnCoT + customerAccounts.CoTCharge;


                        //GL Posting for savings withrawal below:
                        GlPosting currentwithdrawalGlPosting = new GlPosting();
                        currentwithdrawalGlPosting.Amount = amount;
                        currentwithdrawalGlPosting.CreditNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0} ",
                            updateCurrentGlBalance.currentAccountGL.GlAccountName);
                        currentwithdrawalGlPosting.DebitNarration = String.Format(originalDataElement + "&" + customerAccounts.AccountNumber + "Withdrawal From {0}",
                            TSSAccount.GlAccountName);
                        currentwithdrawalGlPosting.GlAccountToCredit = new GlAccount();
                        currentwithdrawalGlPosting.GlAccountToCredit.Id = updateCurrentGlBalance.currentAccountGL.Id;
                        currentwithdrawalGlPosting.GlAccountToDebit = new GlAccount();
                        currentwithdrawalGlPosting.GlAccountToDebit.Id = TSSAccount.Id;
                        currentwithdrawalGlPosting.TransactionDate = eod[0].FinancialDate;
                        currentwithdrawalGlPosting.IsReversed = false;
                        currentwithdrawalGlPosting.DateAdded = DateTime.Now;
                        currentwithdrawalGlPosting.DateUpdated = DateTime.Now;


                        string accountbalance = string.Concat(customerAccounts.Balance);


                        apIinstantiator.UpdateCustomerAccounts("api/Customeraccounts/PutCustomerAccounts/?customeraccountsid=" + customerAccounts.Id, customerAccounts);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + TSSAccount.Id, TSSAccount);
                        apIinstantiator.UpdateGlAccounts("api/Glaccount/PutGlAccount/?glaccountsid=" + updateCurrentGlBalance.currentAccountGL.Id, updateCurrentGlBalance.currentAccountGL);
                        apIinstantiator.InsertGlPostings("api/Glposting/PostGlPostings", currentwithdrawalGlPosting);
                        message.Fields.Add(54, accountbalance);
                        message.Fields.Add(39, "00");

                    }
                }
                catch (Exception)
                {
                    if (
                    apIinstantiator.CustomerCurrentAccountBalanceConfirmed(customerAccounts.Id,
                        amount) == false)
                    {
                        message.Fields.Add(39, "51");
                    }
                }

            }
        }
        
    }
}
