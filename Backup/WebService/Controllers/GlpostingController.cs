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
    public class GlpostingController : ApiController
    {
        // GET api/glposting
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/glposting/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/glposting
        public void Post([FromBody]string value)
        {
        }

        // PUT api/glposting/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/glposting/5
        public void Delete(int id)
        {
        }

        static readonly IGlPostingDb GlPostingDb = new GlPostingDb();

        public void PostGlPostings(GlPosting item)
        {
            GlPostingDb.InsertData(item);
        }

        
        public IList<GlPosting> GetByOriginalDataElement(string originaldataelement)
        {
            //return GlPostingDb.RetrieveAll().Where(x => x.CreditNarration.Substring(0,20) == originaldataelement).ToList();
            return GlPostingDb.RetrieveAll().Where(x => x.CreditNarration.StartsWith(originaldataelement)).ToList();
        }

        public void PutGlPostings(int glpostingsid, GlPosting glpostingstoUpdate)
        {
            IGlPostingDb glPostingDb = new GlPostingDb();
            glPostingDb.RetrieveById(glpostingsid);
            glPostingDb.UpdateData(glpostingstoUpdate);
        }
    }
}
