using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class GlCategory : Entity
    {
        public virtual string GlCategoryName { get; set; }
        public virtual MainAccountCategory MainAccountCategory { get; set; }
        public virtual string Description { get; set; }
    }

    public class GlCategoryDetail : GlCategory
    {
        public virtual string MainAccountCategoryString { get; set; }
        public GlCategoryDetail(GlCategory glcategory)
        {
            this.GlCategoryName = glcategory.GlCategoryName;
            this.Description = glcategory.Description;
            this.MainAccountCategoryString = Enum.GetName(typeof (MainAccountCategory), glcategory.MainAccountCategory);
            this.Id = glcategory.Id;
        }  
    }

   public enum MainAccountCategory
    {
       Asset = 1,
       Liability,
       Capital,
       Income,
       Expense
    }
}
