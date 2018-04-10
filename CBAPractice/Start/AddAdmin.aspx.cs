using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.Start
{
    public partial class AddAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropDownRole.DataSource = Enum.GetNames(typeof(Role));
                DropDownRole.DataBind();
            }
           
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {

            //var userDB = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>();

            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameFName.Value)) throw new Exception("First Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameLName.Value)) throw new Exception("Last Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameEmail.Value)) throw new Exception("Email field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameUName.Value)) throw new Exception("User Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNamePhone.Value)) throw new Exception("Phone No. field is required");

                IList<Core.User> AdminList = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrievebyAdminRole();

                if (AdminList.Count == 0)
                {
                    //create headquarters branch for user
                    Branch branch = new Branch
                    {
                        Address = "Coconut Bus Stop",
                        BranchName = "HeadQuarters",
                        DateAdded = DateTime.Now,
                        DateUpdated = DateTime.Now,
                        RcNumber = "00000"
                    };
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().InsertData(branch);
                    //User user =Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>();
                    User user = new User();
                    user.FirstName = TextBoxNameFName.Value;
                    user.LastName = TextBoxNameLName.Value;
                    user.OtherNames = TextBoxNameONames.Value;
                    user.FullName = user.FirstName + "" + user.LastName + "" + user.OtherNames;
                    //user.Branch = BranchTextbox.Text;            should inherit from type Branch
                    user.Email = TextBoxNameEmail.Value;
                    //user.Role = RoleTextBox.Text;                should be an enum
                    user.UserName = TextBoxNameUName.Value;
                    user.PhoneNumber = TextBoxNamePhone.Value;
                    user.UserRole = (Role)Enum.Parse(typeof(Role), DropDownRole.SelectedValue);
                    user.Branch = new Branch();
                    //string userBranchId = BranchDropDown.SelectedValue;
                    //string userbranchname = BranchDropDown.Text;
                    user.Branch.Id = branch.Id;
                    user.Branch.BranchName = branch.BranchName;


                    UserLogic userLogic = new UserLogic();
                    user.Password = userLogic.EncryptPassword(userLogic.CreatePassword());
                    

                    user.DateAdded = DateTime.Now;
                    user.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().InsertData(user);
                    userLogic.SendMail(user.Email, user.Password);
                    //Response.Redirect("../Start/Login.aspx");
                }

                else
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Admin User Created Already. Only One Admin Can Be Created Through This Page" + "', function(){});</script>", false);
                }


                TextBoxNameFName.Value = String.Empty;
                TextBoxNameLName.Value = String.Empty;
                TextBoxNameONames.Value = String.Empty;
                //BranchTextbox.Text = String.Empty;
                TextBoxNameEmail.Value = String.Empty;
                //RoleTextBox.Text = String.Empty;
                TextBoxNameUName.Value = String.Empty;
                TextBoxNamePhone.Value = String.Empty;

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Admin Saved Successfully" + "', function(){location = '/Start/Login.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }

            }

        }
    }
}