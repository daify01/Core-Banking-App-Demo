using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.ManageBranches
{
    public partial class ViewAllBranches : System.Web.UI.Page
    {

        public IList<Branch> Branches;

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                var branchDb = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>();
                Branches = branchDb.RetrieveAll();
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {

        }
    }
}