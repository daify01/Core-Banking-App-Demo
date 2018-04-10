using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class GlAccount : Entity
    {
        public virtual GlCategory GlCategory { get; set; }
        public virtual string GlAccountName { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual string GlAccountCodes { get; set; }
        public virtual double Balance { get; set; }
    }
}
