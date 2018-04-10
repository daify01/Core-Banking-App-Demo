using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class LoanAccount : Entity
    {
        public virtual CustomerAccounts LinkedAccount { get; set; }
        public virtual LoanConfig LoanConfig { get; set; }
        public virtual double LoanAmount { get; set; }
        public virtual double LoanDuration { get; set; }
        public virtual double LoanInterest { get; set; }
        public virtual double Balance { get; set; }
        public virtual string AccountNumber { get; set; }
        public virtual string AccountName { get; set; }
        public virtual DateTime LoanStartDate { get; set; }
        public virtual DateTime LoanDueDate { get; set; }
        public virtual PaymentSchedule PaymentSchedule { get; set; }
        public virtual int NumberOfDays { get; set; }
        public virtual LoanStatus LoanStatus { get; set; }
        public virtual DateTime TransactionDate { get; set; }
    }

    public enum PaymentSchedule
    {
       Days,
       Monthly
    }

    public enum LoanStatus
    {
        BeingPaid,
        FullyPaid,
        Overdue
    }
}
