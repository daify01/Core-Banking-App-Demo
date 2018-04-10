using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class CurrentConfig : Entity
    {
        public virtual int creditInterestRate { get; set; }
        public virtual int minimumBalance { get; set; }
        public virtual GlAccount InterestExpenseGlAccount { get; set; }
        public virtual double coT { get; set; }
        public virtual GlAccount coTIncomeGl { get; set; }
        public virtual GlAccount currentAccountGL { get; set; }
        public virtual Branch Branch { get; set; }
    }
}
