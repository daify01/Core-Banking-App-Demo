using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class OnUsWithdrawal : Entity
    {
        public virtual string Name { get; set; }
        public virtual string TerminalID { get; set; }
        public virtual string Location { get; set; }
    }
}
