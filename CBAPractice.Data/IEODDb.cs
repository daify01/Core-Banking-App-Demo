using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface IEODDb
    {
        int InsertData(EOD item);
        EOD RetrieveById(int id);
        IList<EOD> RetrieveAll();
        void UpdateData(EOD item);
        void DeleteData(EOD item);
    }
}
