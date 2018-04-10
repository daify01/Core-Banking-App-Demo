using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CBA.Data.Nhibernate;
using CBA.Data.NHibernate;
using CBAPractice.Core;
using CBAPractice.Data;
using CuttingEdge.ServiceLocation;

namespace CBA.DependencyInitializer
{
    public class AppInitializer
    {
        public static void InitNhibernate(SimpleServiceLocator ssl = null)
        {
            if (ssl == null) ssl = new SimpleServiceLocator();
            
            ssl.RegisterSingle<IUserDb>(new UserDb());
            ssl.RegisterSingle<IBranchDb>(new BranchDb());
            ssl.RegisterSingle<ICustomerDb>(new CustomerDb());
            ssl.RegisterSingle<ICustomerAccountsDb>(new CustomerAccountsDb());
            ssl.RegisterSingle<IGlCategoryDb>(new GlCategoryDb());
            ssl.RegisterSingle<IGlAccountDb>(new GlAccountDb());
            ssl.RegisterSingle<ISavingsConfigDb>(new SavingsConfigDb());
            ssl.RegisterSingle<ICurrentConfigDb>(new CurrentConfigDb());
            ssl.RegisterSingle<ILoanConfigDb>(new LoanConfigDb());
            ssl.RegisterSingle<ITellerDb>(new TellerDb());
            ssl.RegisterSingle<IGlPostingDb>(new GlPostingDb());
            ssl.RegisterSingle<ITellerPostingDb>(new TellerPostingDb());
            ssl.RegisterSingle<ILoanAccountDb>(new LoanAccountDb());
            ssl.RegisterSingle<IEODDb>(new EODDb());
            ssl.RegisterSingle<IOnUsWithdrawalDb>(new OnUsWithdrawalDb());


            Microsoft.Practices.ServiceLocation.ServiceLocator.SetLocatorProvider(() => ssl);
        }
    }
}
