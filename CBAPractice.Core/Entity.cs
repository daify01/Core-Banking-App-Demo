using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBAPractice.Core
{
    public class Entity : IEntity
    {
        public virtual int Id { get; set; }
        

        public virtual DateTime DateAdded { get; set; }

        public virtual DateTime DateUpdated { get; set; }
    }

    public interface IEntity
    {
        int Id { get; set; }
    }
}
