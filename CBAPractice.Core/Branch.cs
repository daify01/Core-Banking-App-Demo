using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class Branch : Entity
    {
        public virtual string BranchName { get; set; }

        public virtual string RcNumber { get; set; }

        public virtual string Address { get; set; }
    }
}
