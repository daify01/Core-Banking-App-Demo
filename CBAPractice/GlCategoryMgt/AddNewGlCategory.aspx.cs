using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CBAPractice.Core;
using CBAPractice.Data;

namespace CBAPractice.GlCategoryMgt
{
    public partial class AddNewGlCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DropDownMainAccountCategory.DataSource = Enum.GetNames(typeof(MainAccountCategory));
            DropDownMainAccountCategory.DataBind();

            if (!IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    int id = Convert.ToInt32(Request.QueryString["id"]);
                    GlCategory glCategory =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>()
                            .RetrieveById(id);
                    TextBoxNameGlCategoryName.Value = glCategory.GlCategoryName;
                    TextBoxNameDescription.Value = glCategory.Description;
                    DropDownMainAccountCategory.SelectedValue = glCategory.MainAccountCategory.ToString();
                    TextBoxId.Value = glCategory.Id.ToString();

                }  
            }

        }

        protected void searchsubmit_OnServerClick(object sender, EventArgs e)
        {
           try
            {
                if (string.IsNullOrWhiteSpace(TextBoxNameGlCategoryName.Value)) throw new Exception("Gl Category Name field is required");
                if (string.IsNullOrWhiteSpace(TextBoxNameDescription.Value)) throw new Exception("Description field is required");

                if (string.IsNullOrWhiteSpace(TextBoxId.Value))
                {
                    
                    GlCategory glCategory = new GlCategory();
                    glCategory.GlCategoryName = TextBoxNameGlCategoryName.Value;
                    glCategory.Description = TextBoxNameDescription.Value;
                    glCategory.MainAccountCategory = (MainAccountCategory)Enum.Parse(typeof(MainAccountCategory),DropDownMainAccountCategory.SelectedValue);
                   
                   glCategory.DateAdded = DateTime.Now;
                    glCategory.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>().InsertData(glCategory);
                }

                else
                {
                    GlCategory glCategory =
                        Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>()
                            .RetrieveById(Convert.ToInt32(TextBoxId.Value));
                    glCategory.GlCategoryName = TextBoxNameGlCategoryName.Value;
                    glCategory.Description = TextBoxNameDescription.Value;
                    glCategory.MainAccountCategory = (MainAccountCategory)Enum.Parse(typeof(MainAccountCategory), DropDownMainAccountCategory.SelectedValue);
                    //DropDownMainAccountCategory.Enabled = false;
                    
                   
                   glCategory.DateUpdated = DateTime.Now;
                    Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>()
                        .UpdateData(glCategory);


                }


                TextBoxNameGlCategoryName.Value = String.Empty;
                TextBoxNameDescription.Value = String.Empty;
               

                if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "message"))
                {
                    Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gl Category Saved Successfully" + "', function(){location = '/GlCategoryMgt/AddNewGlCategory.aspx';});</script>", false);
                }

            }
           catch (Exception ex)
           {
               IList<GlCategory> glcategory = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<IGlCategoryDb>().RetrieveAll();

               foreach (var glcategorytest in glcategory)
               {
                   if (TextBoxNameGlCategoryName.Value == glcategorytest.GlCategoryName)
                   {
                       Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Gl Catgeory Name Already Exists. Change it" + "', function(){});</script>", false);
                   }
               }
               if (DropDownMainAccountCategory.SelectedValue == "0")
               {
                   Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "message", "<script type='text/javascript'>alertify.alert('Message', '" + "Main Account Category Not Selected. Please Select One " + "', function(){});</script>", false);
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