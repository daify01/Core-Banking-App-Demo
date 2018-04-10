using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Data;
using System.Net.Mail;
using CBAPractice.Core;
using CBAPractice.Data;
using CBAPractice.Logic;

namespace CBAPractice.ManageUsers
{
    public partial class AddNewUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            
            if (!IsPostBack)
            {
                var branch = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>().RetrieveAll();
                BranchDropDown.DataSource = branch;
                BranchDropDown.DataBind();
                DropDownRole.DataSource = Enum.GetNames(typeof(Role));
                DropDownRole.DataBind();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    User user = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrieveById(id);
                    TextBoxNameFName.Value = user.FirstName;
                    TextBoxNameLName.Value = user.LastName;
                    TextBoxNameONames.Value = user.OtherNames;
                    TextBoxNameEmail.Value = user.Email;
                    TextBoxNamePhone.Value = user.PhoneNumber;
                    TextBoxNameUName.Value = user.UserName;
                    DropDownRole.SelectedValue = user.UserRole.ToString();
                    
                    
                    BranchDropDown.SelectedValue = user.Branch.Id.ToString();
                    
                    TextBoxId.Value = user.Id.ToString();


                }
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
                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    //var branches = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IBranchDb>();
                    Branch branch = new Branch();
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
                    user.UserRole = (Role)Enum.Parse(typeof(Role),DropDownRole.SelectedValue);
                    user.Branch = new Branch();
                   string userBranchId = BranchDropDown.SelectedValue;
                   string userbranchname = BranchDropDown.Text;
                   user.Branch.Id = int.Parse(userBranchId);
                   user.Branch.BranchName = Convert.ToString(user.Branch.Id);
                    
                    
                    UserLogic userLogic = new UserLogic();
                    user.Password = userLogic.EncryptPassword(userLogic.CreatePassword());

                    
                    user.DateAdded = DateTime.Now;
                    user.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().InsertData(user);
                    userLogic.SendMail(user.Email, user.Password);
                }

                else
                {
                    User user =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>()
                            .RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    user.FirstName = TextBoxNameFName.Value;
                    user.LastName = TextBoxNameLName.Value;
                    user.OtherNames = TextBoxNameONames.Value;
                    user.FullName = user.FirstName + "" + user.LastName + "" + user.OtherNames;
                    //user.Branch = BranchTextbox.Text;            should inherit from type Branch
                    user.Email = TextBoxNameEmail.Value;
                    //user.Role = RoleTextBox.Text;                should be an enum
                    user.UserRole = (Role)Enum.Parse(typeof(Role), DropDownRole.SelectedValue);
                    user.Branch = new Branch();

                    user.Branch.Id = int.Parse(BranchDropDown.SelectedValue);
                    user.Branch.BranchName = Convert.ToString(user.Branch.Id);
                    //{ BranchName = this.BranchDropDown.SelectedValue };
                    //user.Password = "12344";                         // Password test
                    user.UserName = TextBoxNameUName.Value;
                    user.PhoneNumber = TextBoxNamePhone.Value;
                    user.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().UpdateData(user);
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
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "User Saved Successfully" + "', function(){location = '/ManageUsers/AddNewUser.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                IList<User> users = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IUserDb>().RetrieveAll();
                foreach (var usertest in users)
                {
                    if (TextBoxNameEmail.Value == usertest.Email)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Email Already Exists. Change Email" + "', function(){});</script>", false);
                    }

                    if (TextBoxNamePhone.Value == usertest.PhoneNumber)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Phone Number Already Exists. Change Phone No" + "', function(){});</script>", false);
                    }
                    if (TextBoxNameUName.Value == usertest.UserName)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Username Already Exists. Change Username" + "', function(){});</script>", false);
                    }
                }
                if (BranchDropDown.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Branch Not Selected. Please Select a Branch " + "', function(){});</script>", false);
                }
                if (DropDownRole.SelectedValue == "0")
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Role Not Selected. Please Select a User Role " + "', function(){});</script>", false);
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