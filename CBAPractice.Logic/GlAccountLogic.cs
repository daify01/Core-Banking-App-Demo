using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Logic
{
    public class GlAccountLogic
    {


        public string GetGlAccountCode(GlAccount glAccount)
        {
            //GlAccount glAccount = new GlAccount();
        GlCategory glCategory = new GlCategory();
        Random Rand = new Random();
        string RandString = Convert.ToString(Rand.Next(10000, 99999));
            string GlAccountCode = "";

        //var tr = Enum.GetValues(typeof(MainAccountCategory));
        //    int enumId = Convert.ToInt32(tr);

        switch (glAccount.GlCategory.MainAccountCategory)
                {
                    case MainAccountCategory.Asset:
                        GlAccountCode = '1' + RandString;
                        break;

                    case MainAccountCategory.Liability:
                        GlAccountCode = '2' + RandString;
                        break;

                    case MainAccountCategory.Capital:
                        GlAccountCode = '3' + RandString;
                        break;

                    case MainAccountCategory.Income:
                        GlAccountCode = '4' + RandString;
                        break;

                    case MainAccountCategory.Expense:
                        GlAccountCode = '5' + RandString;
                        break;

                }

            glAccount.GlAccountCodes = GlAccountCode;
            return glAccount.GlAccountCodes;

        }
    }

}
