using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Providers.Entities;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using User = CBAPractice.Core.User;

namespace CBAPractice
{
    public partial class CBA : System.Web.UI.MasterPage
    {
        private User _user;
        protected void Page_Init(object sender, EventArgs e)
        {
            //Core.User user = new Core.User();
            _user = new User();
            _user = (User)Session["User"];
            if (_user == null)
            {
               Response.Redirect("~/Start/Login.aspx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["id"]=="1") Logout();
            //else if (Request.QueryString["id"]=="2") ChangePassword();
        }

        protected void Logout()
        {
            Session.RemoveAll();
            Response.Redirect("~/Start/Login.aspx");
        }

        protected void ChangePassword()
        {

            Response.Redirect("~/ManageUsers/ChangeUserPassword.aspx");
        }

        string GetNodeString(SiteMapNode node)
        {
            string x = string.Empty;
            if ((_user.UserRole == Role.Admin && node.Roles != null &&
                 (node.Roles.Contains("*") || node.Roles.Contains("Admin"))) ||
                (_user.UserRole == Role.Teller && node.Roles != null &&
                 (node.Roles.Contains("*") || node.Roles.Contains("Teller"))))
            {
                x = string.Format(@"<a href='{0}'>
                <i class='fa {1}'></i> <span>{2}</span>", node.Url, node.ResourceKey, node.Title);

                if (node.HasChildNodes)
                {
                    x += string.Format(@"<i class='fa fa-angle-left pull-right'></i>");
                }

                x += string.Format(@"</a>");
            }
            return x;
        }

        protected string GenerateMenuLinks(SiteMapNode node)
        {
            StringBuilder sideMenuText = new StringBuilder("");

            foreach ( SiteMapNode childNode in node.ChildNodes)
            {
                sideMenuText.Append("<li class='treeview'>");
                sideMenuText.Append(GetNodeString(childNode));
                sideMenuText.Append("<ul class='treeview-menu'>");
                foreach (SiteMapNode child in childNode.ChildNodes)
                {
                    if ((_user.UserRole == Role.Admin && child.Roles != null &&
                         (child.Roles.Contains("*") || child.Roles.Contains("Admin"))) ||
                        (_user.UserRole == Role.Teller && child.Roles != null &&
                         (child.Roles.Contains("*") || child.Roles.Contains("Teller"))))
                    {
                        sideMenuText.Append("<li>");
                        sideMenuText.Append(GetNodeString(child));
                        sideMenuText.Append("</li>");
                    }
                }
                sideMenuText.Append("</ul>");
                sideMenuText.Append("</li>");
            }
            return sideMenuText.ToString();
        }
       
        
    }
}