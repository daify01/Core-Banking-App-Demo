using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Logic
{
    public class EnumGetter 
    {
        public static IList<NameAndValue> GetNameValues(Type theEnumType)
        {
            List<NameAndValue> result = new List<NameAndValue>();
            //get an array collection of the string format of our enum
            string[] names = Enum.GetNames(theEnumType);
            foreach (var name in names)
            {
                result.Add(new NameAndValue()
                {
                    Name = name,
                    Value = (int)Enum.Parse(theEnumType, name) //get the interger representation of this name string in the enum
                });
            }
            return result;
        }
    }
}
