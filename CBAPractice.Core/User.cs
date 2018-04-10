using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class User : Entity
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string OtherNames { get; set; }
        public virtual Branch Branch { get; set; }
        public virtual string Email { get; set; }
        public virtual Role UserRole { get; set; }
        public virtual string UserName { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual string Password { get; set; }
        public virtual string FullName { get; set; }
      
    }

    public class UserDetail : User
    {
        public virtual string RoleString { get; set; }
        public UserDetail(User user)
        {
            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.OtherNames = user.OtherNames;
            this.Branch = user.Branch;
            this.Email = user.Email;
            this.UserName = user.UserName;
            this.PhoneNumber = user.PhoneNumber;
            this.RoleString = Enum.GetName(typeof(Role), user.UserRole);
            this.Id = user.Id;
        }
    }

    public enum Role
    {
        Admin,
        Teller
    }


}
