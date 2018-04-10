using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class GlPosting : Entity
    {
        public virtual GlAccount GlAccountToDebit { get; set; }
        public virtual string DebitNarration { get; set; }
        public virtual GlAccount GlAccountToCredit { get; set; }
        public virtual string CreditNarration { get; set; }
        public virtual double Amount { get; set; }
        public virtual DateTime TransactionDate { get; set; }
        public virtual bool IsReversed { get; set; }
    }
}
