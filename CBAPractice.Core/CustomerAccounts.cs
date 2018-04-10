using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
   public class CustomerAccounts : Entity
    {
        public virtual Customer Customer { get; set; }

        public virtual Branch Branch { get; set; }

        public virtual string AccountName { get; set; }

       public virtual string AccountNumber { get; set; }

       public virtual AccountType AccountType { get; set; }

       public virtual double Balance { get; set; }

       public virtual int CoTCharge { get; set; }

       public virtual bool IsClosed { get; set; }


    }

   public class CustomerAccountDetail : CustomerAccounts
   {
       public virtual string AccountTypeString { get; set; }
       public CustomerAccountDetail(CustomerAccounts customerAccounts)
       {
           this.Branch = customerAccounts.Branch;
           this.Customer = customerAccounts.Customer;
           this.AccountName = customerAccounts.AccountName;
           this.AccountNumber = customerAccounts.AccountNumber;
           this.CoTCharge = customerAccounts.CoTCharge;
           this.IsClosed = customerAccounts.IsClosed;

           this.Balance = customerAccounts.Balance;
           
           this.AccountTypeString = Enum.GetName(typeof(AccountType), customerAccounts.AccountType);

           this.Id = customerAccounts.Id;
       }
   }

    public enum AccountType
    {
        Savings,
        Current,
        //Loan
    }
}
