using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class Customer : Entity
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string OtherNames { get; set; }

        public virtual string Address { get; set; }

        public virtual string Email { get; set; }

        public virtual Gender Gender { get; set; }

        public virtual string  PhoneNumber { get; set; }

        
    }

    public class CustomerDetail : Customer
    {
        public virtual string GenderString { get; set; }
        public CustomerDetail(Customer customer)
        {
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.OtherNames = customer.OtherNames;
            this.Address = customer.Address;
            this.Email = customer.Email;
            
            this.PhoneNumber = customer.PhoneNumber;
            //finish it up
            this.GenderString = Enum.GetName(typeof(Gender), customer.Gender);

            this.Id = customer.Id;
        }
    }

    public enum Gender
    {
        male,
        female
    }
}
