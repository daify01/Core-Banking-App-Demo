using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBA.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Generate_Password Gen = new Generate_Password();
            
            Gen.SendPassword();

            Console.WriteLine(Gen.GeneratePassword());
            Console.ReadLine();

           
        }
    }
}
