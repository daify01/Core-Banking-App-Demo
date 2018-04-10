using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Logic
{
   public class TellerPostingLogic
    {
       public bool GLAccountBalanceConfirmed(GlAccount glAccount, string accountType, double amount)
       {
           if (accountType == "Withdrawal" && glAccount.Balance < amount)
           {
               return false;
           }
           else return true;
       }

       public bool CustomerSavingsAccountBalanceConfirmed(CustomerAccounts customerAccounts, SavingsConfig savingsConfig, string accountType, double amount)
       {
           if (accountType == "Withdrawal" && (customerAccounts.Balance < amount + savingsConfig.minimumBalance))
           {
               return false;
           }
           else return true;
       }

       public bool CustomerCurrentAccountBalanceConfirmed(CustomerAccounts customerAccounts, CurrentConfig currentConfig, string accountType, double amount)
       {
           if (accountType == "Withdrawal" && (customerAccounts.Balance < amount + currentConfig.minimumBalance + customerAccounts.CoTCharge))
           {
               return false;
           }
           else return true;
       }
        public double GlAccountTransaction(GlAccount glAccount, string accountType, double amount)
        {
            if (accountType == "Deposit")
            {
                glAccount.Balance = glAccount.Balance + amount;

            }

            else if (accountType == "Withdrawal" && glAccount.Balance >= amount)
            {
                glAccount.Balance = glAccount.Balance - amount;

            }
            //else if (accountType == "Withdrawal" && glAccount.Balance < amount)
            //{
            //    throw new Exception("Insufficient Money in Till Account. Receive from Vault");
            //}

            return glAccount.Balance;
        }

        public double CustomerAccountTransaction(CustomerAccounts customerAccounts, string accountType, double amount)
        {

            if (accountType == "Deposit")
            {
                customerAccounts.Balance = customerAccounts.Balance + amount;

            }

            else if (accountType == "Withdrawal" && customerAccounts.Balance >= amount)
            {
                customerAccounts.Balance = customerAccounts.Balance - amount;

            }
            //else if (accountType == "Withdrawal" && customerAccounts.Balance < amount)
            //{
            //    throw new Exception("Account Balance is insufficient");
            //}
            return customerAccounts.Balance;
        }

        public double CreditCustomerAccounts(CustomerAccounts customerAccounts, double amount)
       {
           customerAccounts.Balance = customerAccounts.Balance + amount;
            return customerAccounts.Balance;
       }

        public double DebitCustomerAccounts(CustomerAccounts customerAccounts, double amount)
        {
            customerAccounts.Balance = customerAccounts.Balance - amount;
            return customerAccounts.Balance;
        }

       public bool unsassignedTill(Teller teller, User user)
       {
           if (teller.User.UserName == null || teller.User.UserName != user.UserName)
           {
               return true;
           }
           else return false;
       }
    }
}
