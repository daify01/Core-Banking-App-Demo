using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;


namespace CBAPractice.ManageCustomers
{
    public partial class AddNewCustomer : System.Web.UI.Page
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                DropDownGender.DataSource = Enum.GetNames(typeof(Gender));
                DropDownGender.DataBind();
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    Customer customer = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>().RetrieveById(id);
                    TextBoxNameFName.Value = customer.FirstName;
                    TextBoxNameLName.Value = customer.LastName;
                    TextBoxNameONames.Value = customer.OtherNames;
                    TextBoxNameAddress.Value = customer.Address;
                    TextBoxNameEmail.Value = customer.Email;
                    TextBoxNamePhone.Value = customer.PhoneNumber; 
                    DropDownGender.SelectedValue = customer.Gender.ToString();
                   
                    TextBoxId.Value = customer.Id.ToString();


                   
                }
            }
        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
           
            try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameFName.Value)) throw new Exception("First Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameLName.Value)) throw new Exception("Last Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameEmail.Value)) throw new Exception("Email field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameAddress.Value)) throw new Exception("User Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameEmail.Value)) throw new Exception("User Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNamePhone.Value)) throw new Exception("Phone No. field is required");
                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    // Code that Adds new customer with necessary info
                   
                    Customer customer = new Customer();
                    customer.FirstName = TextBoxNameFName.Value;
                    customer.LastName = TextBoxNameLName.Value;
                    customer.OtherNames = TextBoxNameONames.Value;
                   
                    customer.Email = TextBoxNameEmail.Value;
                    
                    customer.PhoneNumber = TextBoxNamePhone.Value;
                    customer.Address = TextBoxNameAddress.Value;
                    customer.Gender = (Gender)Enum.Parse(typeof(Gender), DropDownGender.SelectedValue);

                    customer.DateAdded = DateTime.Now;
                    customer.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>().InsertData(customer);
                }

                else
                {
                    // Code that Updates the Customer Info, upon editing

                    Customer customer =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>()
                            .RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    customer.FirstName = TextBoxNameFName.Value;
                    customer.LastName = TextBoxNameLName.Value;
                    customer.OtherNames = TextBoxNameONames.Value;
                    customer.Address = TextBoxNameAddress.Value;
                    
                    customer.Email = TextBoxNameEmail.Value;
                   
                    customer.Gender = (Gender)Enum.Parse(typeof(Gender), DropDownGender.SelectedValue);

                    
                    //customer.Id = int.Parse(TextBoxId.Value);
                    
                   
                    customer.PhoneNumber = TextBoxNamePhone.Value;
                    customer.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>().UpdateData(customer);
                }


                TextBoxNameFName.Value = String.Empty;
                TextBoxNameLName.Value = String.Empty;
                TextBoxNameONames.Value = String.Empty;
                TextBoxNameAddress.Value = String.Empty;
                //BranchTextbox.Text = String.Empty;
                TextBoxNameEmail.Value = String.Empty;
                //RoleTextBox.Text = String.Empty;
                TextBoxNamePhone.Value = String.Empty;

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Customer Saved Successfully" + "', function(){location = '/ManageCustomers/AddNewCustomer.aspx';});</script>", false);
                }

            }
            catch (Exception ex)
            {
                IList<Customer> customers = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ICustomerDb>().RetrieveAll();

                foreach (var customertest in customers)
                {
                    if (TextBoxNamePhone.Value == customertest.PhoneNumber)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Phone Number Already Exists. Change Phone No" + "', function(){});</script>", false);
                    }
                    else if (TextBoxNameEmail.Value == customertest.Email)
                    {
                        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Email Already Exists. Change Email" + "', function(){});</script>", false);
                    }
                }
                //if (DropDownGender.Text == "Select")
                //{
                //    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gender Not Selected. Please Select a Gender " + "', function(){});</script>", false);
                //}
                string errorMessage = ex.InnerException == null ? ex.Message : ex.InnerException.Message;
                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", @"<script type='text/javascript'>alertify.alert('Message', """ + errorMessage.Replace("\n", "").Replace("\r", "") + @""", function(){});</script>", false);
                }

            }
        }
    }
}