using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
   public class TellerPosting : Entity
    {
        public virtual CustomerAccounts CustomerAccounts { get; set; }
        public virtual GlAccount GlAccount { get; set; }
        public virtual double Amount { get; set; }
        public virtual string Narration { get; set; }
        public virtual PostingType PostingType { get; set; }
        public virtual DateTime TransactionDate { get; set; }
    }

    public enum PostingType
    {
        Deposit = 1,
        Withdrawal
    }
}
