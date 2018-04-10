using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class EOD : Entity
    {
        public virtual bool IsClosed { get; set;}
        public virtual DateTime FinancialDate { get; set; }
    }
}
