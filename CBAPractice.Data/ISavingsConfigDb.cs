using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBAPractice.Core;

namespace CBAPractice.Data
{
    public interface ISavingsConfigDb
    {
        int InsertData(SavingsConfig item);
        SavingsConfig RetrieveById(int id);
        IList<SavingsConfig> RetrieveAll();
        void UpdateData(SavingsConfig item);

        void DeleteData(SavingsConfig item);
        SavingsConfig RetrieveByBranch(int id);
    }
}
