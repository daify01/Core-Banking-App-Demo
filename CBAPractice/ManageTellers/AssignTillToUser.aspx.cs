using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.ManageTellers
{
    public partial class AssignTillToUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                User users = new User();
                var user =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrieveAll();
                DropDownUser.DataSource = user;
                DropDownUser.DataValueField = "Id";
                DropDownUser.DataTextField = "FullName";
                DropDownUser.DataBind();

                var glAccount =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlAccountDb>().RetrieveCashAssettypes();
                DropDownTillAccount.DataSource = glAccount;
                DropDownTillAccount.DataValueField = "Id";
                DropDownTillAccount.DataTextField = "GlAccountName";
                DropDownTillAccount.DataBind();

                

                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    Teller teller =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                            .RetrieveById(id);
                    DropDownTillAccount.SelectedValue = teller.GlAccount.Id.ToString();
                    DropDownUser.SelectedValue = teller.User.Id.ToString();
                    TextBoxId.Value = teller.Id.ToString();

                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    
                    Core.User user = new User();
                    user = (User)Session["User"];
                   
                    Teller userInTellerDB = new Teller();
                    userInTellerDB =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                            .RetrievebyUserId(Int32.Parse(DropDownUser.SelectedValue));

                    

                    if (userInTellerDB != null)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "User Already Assigned to Till. Choose Another" + "', function(){});</script>", false);

                    }
                   
                    else 
                    {
                        Teller teller = new Teller();
                        teller.GlAccount = new GlAccount();
                        teller.GlAccount.Id = int.Parse(DropDownTillAccount.SelectedValue);
                        teller.User = new User();
                        teller.User.Id = int.Parse(DropDownUser.SelectedValue);
                        teller.DateAdded = DateTime.Now;
                        teller.DateUpdated = DateTime.Now;
                       Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>().InsertData(teller);
      }
                

                }

                else
                {
                    Teller userInTellerDB = new Teller();
                    userInTellerDB =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                            .RetrievebyUserId(Int32.Parse(DropDownUser.SelectedValue));



                    if (userInTellerDB != null)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "User Already Assigned to Till. Choose Another" + "', function(){});</script>", false);

                    }

                    else
                    {
                        Teller teller = new Teller();
                        teller.GlAccount = new GlAccount();
                        teller.GlAccount.Id = int.Parse(DropDownTillAccount.SelectedValue);
                        teller.User = new User();
                        teller.User.Id = int.Parse(DropDownUser.SelectedValue);
                        teller.DateAdded = DateTime.Now;
                        teller.DateUpdated = DateTime.Now;
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>().InsertData(teller);
                    }


                }


                

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Till Assigned to User Successfully" + "', function(){location = '/ManageTellers/AssignTillToUser.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                Teller tellerinTellerDB = new Teller();
                tellerinTellerDB =
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ITellerDb>()
                        .RetrievebyTillId(Int32.Parse(DropDownTillAccount.SelectedValue));
                 if (tellerinTellerDB != null)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Till Already Assigned to User. Choose Another" + "', function(){});</script>", false);

                    }
               if (DropDownUser.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "User Not Selected. Please Select a User " + "', function(){});</script>", false);
                }
                if (DropDownTillAccount.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Till Not Selected. Please Select a Till " + "', function(){});</script>", false);
                }
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }

            }
        }
    }
}