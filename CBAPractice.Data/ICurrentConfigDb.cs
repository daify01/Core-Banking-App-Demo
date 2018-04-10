using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface ICurrentConfigDb
    {
        int InsertData(CurrentConfig item);
        CurrentConfig RetrieveById(int id);
        IList<CurrentConfig> RetrieveAll();
        void UpdateData(CurrentConfig item);

        void DeleteData(CurrentConfig item);
        CurrentConfig RetrieveByBranch(int id);
    }
}
