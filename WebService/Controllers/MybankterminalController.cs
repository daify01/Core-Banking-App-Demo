using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CBA.Data.NHibernate;
using CBAPractice.Core;
using CBAPractice.Data;

namespace WebService.Controllers
{
    public class MybankterminalController : ApiController
    {
        // GET api/mybankterminal
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/mybankterminal/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/mybankterminal
        public void Post([FromBody]string value)
        {
        }

        // PUT api/mybankterminal/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/mybankterminal/5
        public void Delete(int id)
        {
        }

        IOnUsWithdrawalDb onUsWithdrawalDb = new OnUsWithdrawalDb();

        public bool GetATMTerminal(string terminalfromSWITCH)
        {
            IList<OnUsWithdrawal> terminalList = onUsWithdrawalDb.RetrieveAll();

            foreach (var terminal in terminalList)
            {
                if (terminal.TerminalID.Substring(0,1) == terminalfromSWITCH)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
