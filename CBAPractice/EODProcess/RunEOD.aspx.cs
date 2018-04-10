using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.EODProcess
{
    public partial class RunEOD : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void closesubmit_OnServerClick(object sender, EventArgs e)
        {
            //IList<EOD> eods = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            //if (eods[0].IsClosed)
            //{
            //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Business has been closed already. You must Open Business before you can close again" +
            //   "', function(){location = '/EODProcess/RunEOD.aspx';});</script>", false);
            //}
            EODLogic eodLogic = new EODLogic();
            eodLogic.CloseBusiness();
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            eodLogic.RunEOD();
            
            if (eod[0].IsClosed)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Business is closed" +
               "', function(){location = '/EODProcess/RunEOD.aspx';});</script>", false);
            }
        }

        protected void opensubmit_OnServerClick(object sender, EventArgs e)
        {
            EODLogic eodLogic = new EODLogic();
            eodLogic.OpenBusiness();
            IList<EOD> eod = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IEODDb>().RetrieveAll();
            if (eod[0].IsClosed == false)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Business is open" +
               "', function(){location = '/EODProcess/RunEOD.aspx';});</script>", false);
                
            }
        }
    }
}