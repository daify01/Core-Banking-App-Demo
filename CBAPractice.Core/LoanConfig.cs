using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class LoanConfig : Entity
    {
        public virtual int debitInterestRate { get; set; }
        public virtual GlAccount InterestIncomeGlAccount { get; set; }
        public virtual GlAccount LoanPrincipalGlAccount { get; set; }
    }
}
