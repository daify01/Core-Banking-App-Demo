using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
   public class Teller : Entity
    {
       public virtual User User { get; set; }
       public virtual GlAccount GlAccount { get; set; }
    }
}
