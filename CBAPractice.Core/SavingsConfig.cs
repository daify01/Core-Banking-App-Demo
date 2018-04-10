using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class SavingsConfig : Entity
    {
        public virtual int creditInterestRate { get; set; }
        public virtual int minimumBalance { get; set; }
        public virtual GlAccount InterestExpenseGlAccount { get; set; }
        public virtual GlAccount SavingsAccountGL { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
