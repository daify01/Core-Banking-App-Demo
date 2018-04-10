using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CBAPractice.TellerPostingMgt
{
    public partial class ViewAllTellerPostings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                dateValue.Value = DateTime.Now.Date.ToString("yyyy-MM-dd");
                //if (date.Value == "yyyy-mm-dd")
                //{
                //    date.Value = DateTime.Now.ToShortDateString();
                //}
            }

        }
    }
}