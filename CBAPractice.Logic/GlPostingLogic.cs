using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core; 

namespace CBAPractice.Logic
{
   public class GlPostingLogic
    {
       public double DebitGlAccount(GlAccount glAccount, double amount )
       {
           if (glAccount.GlAccountCodes.StartsWith("1") || glAccount.GlAccountCodes.StartsWith("5"))
           {
               glAccount.Balance = glAccount.Balance + amount;
               
           }

           else if (glAccount.GlAccountCodes.StartsWith("2") || glAccount.GlAccountCodes.StartsWith("3") || glAccount.GlAccountCodes.StartsWith("4"))
           {
               glAccount.Balance = glAccount.Balance - amount;
               
           }

           return glAccount.Balance;
       }

       public double CreditGlAccount(GlAccount glAccount, double amount)
       {
           
           if (glAccount.GlAccountCodes.StartsWith("1") || glAccount.GlAccountCodes.StartsWith("5"))
           {
               glAccount.Balance = glAccount.Balance - amount;

           }

           else if (glAccount.GlAccountCodes.StartsWith("2") || glAccount.GlAccountCodes.StartsWith("3") || glAccount.GlAccountCodes.StartsWith("4"))
           {
               glAccount.Balance = glAccount.Balance + amount;

           }

           return glAccount.Balance;
       }
    }
}
